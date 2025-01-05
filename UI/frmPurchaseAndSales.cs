using ADT_PROJECT.BussinessLogicLayer;
using ADT_PROJECT.DataAccessLayer;
using DGVPrinterHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace ADT_PROJECT.UI
{
    public partial class frmPurchaseAndSales : Form
    {
        public frmPurchaseAndSales()
        {
            InitializeComponent();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        deacustDAL dcdal = new deacustDAL();
        productDAL pdal = new productDAL();
        userDAL udal = new userDAL();
        DataTable transactionDT = new DataTable();
        transactionDAL tdal = new transactionDAL();
        transactionDetailsDAL tddal = new transactionDetailsDAL();
        private void frmPurchaseAndSales_Load(object sender, EventArgs e)
        {
            string type = UserDashboard.TransactionType;
            lblTop.Text = type;

            //specify coloumns for Transaction Data Table
            transactionDT.Columns.Add("Product Name");
            transactionDT.Columns.Add("Rate");
            transactionDT.Columns.Add("Quantity");
            transactionDT.Columns.Add("Total");

        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keywords = txtSearch.Text;

            if(keywords=="")
            {
                txtName.Text = "";
                txtEmail.Text = "";
                txtContact.Text = "";
                txtAddress.Text = "";
                return;
            }
            //to get the details and set the value on text boxes
            deacustBLL dc = dcdal.AdvancedSrch(keywords);

            //to set the vales from deacustBLL to textboxes
            txtName.Text = dc.name;
            txtEmail.Text = dc.email;
            txtContact.Text = dc.contact;
            txtAddress.Text = dc.address;

        }
        private void txtSearchPD_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearchPD.Text;
            if (keyword == "")
            {
                txtNamePD.Text = "";
                txtInventory.Text = "";
                txtRate.Text = "";
                txtqty.Text = "";
                return;
            }
            //to get the details and set the value on text boxes
            productBLL p = pdal.GetProductsForTransactions(keyword);

            //to set the vales from deacustBLL to textboxes
            txtNamePD.Text = p.name;
            txtInventory.Text = p.qty.ToString();
            txtRate.Text = p.rate.ToString();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string productName = txtNamePD.Text;
            decimal Rate = Convert.ToDecimal(txtRate.Text);
            decimal qty = Convert.ToDecimal(txtqty.Text);

            decimal total = Rate * qty;

            decimal subtotal = Convert.ToDecimal(txtSubtotal.Text);
            subtotal = subtotal + total;

            if(productName=="")
            {
                MessageBox.Show("Select he product first ! Try again!");
            }
            else
            {
                transactionDT.Rows.Add(productName,Rate,qty,total);

                dgvAddedProducts.DataSource = transactionDT;

                txtSubtotal.Text = subtotal.ToString();
                txtSearchPD.Text = "";
                txtNamePD.Text = "";
                txtInventory.Text = "0.00";
                txtRate.Text = "0.00";
                txtqty.Text = "0.00";
            }
        }
        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            string value = txtDiscount.Text;
            if(value =="")
            {
                MessageBox.Show("Please ADD Discount First!");
            }
            else
            {
                decimal subtotal = Convert.ToDecimal(txtSubtotal.Text);
                decimal discount = Convert.ToDecimal(txtDiscount.Text);
                decimal grandTotal = ((100-discount)/100) * subtotal;
                txtGrandTotal.Text = grandTotal.ToString();
            }
        }
        private void txtVat_TextChanged(object sender, EventArgs e)
        {
            string check = txtGrandTotal.Text;
            if(check=="")
            {
                MessageBox.Show("Calculate the discount and set the grand total first!");
            }
            else
            {
                decimal previousGT = Convert.ToDecimal(txtGrandTotal.Text);
                decimal vat = Convert.ToDecimal(txtVat.Text);
                decimal grandTotal = ((100-vat)/100)*previousGT;
                txtGrandTotal.Text = grandTotal.ToString();

            }
        }
        private void txtPaidAmount_TextChanged(object sender, EventArgs e)
        {
            decimal grandTotal = Convert.ToDecimal(txtGrandTotal.Text);
            decimal paidAmount = Convert.ToDecimal(txtPaidAmount.Text);

            decimal returnAmount = paidAmount - grandTotal;
            txtReturnAmount.Text = returnAmount.ToString(); 
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            transactionBLL t = new transactionBLL();
            t.type = lblTop.Text;
            string deaCustName = txtName.Text;
            deacustBLL dc = dcdal.GetDeaCustIDFromName(deaCustName);

            t.dea_cust_id = dc.id;
            t.grandTotal = Math.Round(Convert.ToDecimal(txtGrandTotal.Text),2);
            t.transaction_date = DateTime.Now;
            t.tax = Convert.ToDecimal(txtVat.Text);
            t.discount = Convert.ToDecimal(txtDiscount.Text);

            string username = frmLogin.loggedIn;
            userBLL u = udal.GetIDFromUsername(username);
            t.added_by = u.id;
            t.transasctionDetails = transactionDT;
            bool success = false;
            
            using(TransactionScope scope = new TransactionScope())
            {
                int transactionID = -1;
                bool w = tdal.Insert(t, out transactionID);

                for(int i=0;i<transactionDT.Rows.Count; i++)
                {
                    transactionDetailsBLL transactionDetail = new transactionDetailsBLL();
                    string productName = transactionDT.Rows[i][0].ToString();
                    productBLL p = pdal.GetDeaProdcutIDFromName(productName);

                    transactionDetail.product_id = p.id;
                    transactionDetail.rate = Convert.ToDecimal(transactionDT.Rows[i][1].ToString());
                    transactionDetail.qty = Convert.ToDecimal(transactionDT.Rows[i][2].ToString());
                    transactionDetail.total = Math.Round(Convert.ToDecimal(transactionDT.Rows[i][3].ToString()),2);
                    transactionDetail.dea_cust_id = dc.id;
                    transactionDetail.added_date = DateTime.Now;
                    transactionDetail.added_by = u.id;

                    string transactionType = lblTop.Text;
                    bool x = false;
                    if(transactionType=="Purchase")
                    {
                         x = pdal.IncreaseProduct(transactionDetail.product_id,transactionDetail.qty);
                    }
                    else if(transactionType=="Sales")
                    {
                         x = pdal.DecreaseProduct(transactionDetail.product_id, transactionDetail.qty);
                    }
                    bool y = tddal.Insert(transactionDetail);
                    success = w && x && y;
                }
                if (success == true)
                {
                    scope.Complete();

                    //To print the bill 
                    DGVPrinter printer = new DGVPrinter();
                    printer.Title = "\r\n\r\n\r\n ANYSTORE PVT. LTD\r\n\r\n";
                    printer.SubTitle = "Mahesana,Gujarat \r\n Phone: 01-45XXXX \r\n\r\n";
                    printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                    printer.PageNumbers = true;
                    printer.PageNumberInHeader = false;
                    printer.PorportionalColumns = true;
                    printer.HeaderCellAlignment = StringAlignment.Near;
                    printer.Footer="Discount: "+txtDiscount.Text + "% \r\n" + "VAT: " + txtVat.Text + "% \r\n" + "Grand Total: "+txtGrandTotal.Text + "\r\n\r\n" + "THANK YOU FOR DOING BUSSINESS WITH US :)";
                    printer.FooterSpacing = 15;
                    printer.PrintDataGridView(dgvAddedProducts);

                    MessageBox.Show("Transaction Complted Successfuly!");
                    dgvAddedProducts.DataSource = null;
                    dgvAddedProducts.Rows.Clear();
                    txtSearch.Text = "";
                    txtName.Text = "";
                    txtEmail.Text = "";
                    txtAddress.Text = "";
                    txtSearchPD.Text = "";
                    txtNamePD.Text = "";
                    txtInventory.Text = "0";
                    txtRate.Text = "0";
                    txtqty.Text = "0";
                    txtSubtotal.Text = "0";
                    txtDiscount.Text = "0";
                    txtVat.Text = "0";
                    txtGrandTotal.Text="0";
                    txtPaidAmount.Text="0";
                    txtReturnAmount.Text = "0";
                }
                else
                {
                    MessageBox.Show("Transaction Failed!");
                }
            }
        }
    }
}
