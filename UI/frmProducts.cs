using ADT_PROJECT.BussinessLogicLayer;
using ADT_PROJECT.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADT_PROJECT.UI
{
    public partial class frmProducts : Form
    {
        public frmProducts()
        {
            InitializeComponent();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void lblDescription_Click(object sender, EventArgs e)
        {

        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {

        }
        categoriesDAL cdal = new categoriesDAL();
        productBLL p = new productBLL();
        productDAL pdal = new productDAL();
        userDAL udal = new userDAL();
        private void frmProducts_Load(object sender, EventArgs e)
        {
            DataTable categoriesDT = cdal.Select();
            cmbCategory.DataSource = categoriesDT;
            cmbCategory.DisplayMember = "title";
            cmbCategory.ValueMember = "title";

            DataTable dt = pdal.Select();
            dgvProducts.DataSource = dt;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            p.name = txtName.Text;
            p.category = cmbCategory.Text;
            p.description = txtDescription.Text;
            p.rate = Convert.ToDecimal(txtRate.Text);
            p.qty = 0;
            p.added_date = DateTime.Now;

            string loggedUser= frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUser);
            p.added_by = usr.id;

            bool success = pdal.Insert(p);
            if (success == true)
            {
                MessageBox.Show("New Product Inserted Successfully !");
                clear();
                DataTable dt = pdal.Select();
                dgvProducts.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Failed to Insert new Product! ");
            }
        }
        public void clear()
        {
            txtProductID.Text = ""; 
            txtName.Text = "";
            cmbCategory.Text = "";
            txtDescription.Text = "";
            txtRate.Text = "";
            txtSearch.Text = "";
        }

        private void dgvProducts_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int RowIndex = e.RowIndex;
            txtProductID.Text = dgvProducts.Rows[RowIndex].Cells[0].Value.ToString();
            txtName.Text = dgvProducts.Rows[RowIndex].Cells[1].Value.ToString();
            cmbCategory.Text = dgvProducts.Rows[RowIndex].Cells[2].Value.ToString();
            txtDescription.Text = dgvProducts.Rows[RowIndex].Cells[3].Value.ToString();
            txtRate.Text = dgvProducts.Rows[RowIndex].Cells[4].Value.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            p.id = Convert.ToInt32(txtProductID.Text);
            p.name = txtName.Text;
            p.category = cmbCategory.Text;
            p.description = txtDescription.Text;
            p.rate = Convert.ToDecimal(txtRate.Text);
            p.added_date = DateTime.Now;
            string loggedUser = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUser);
            p.added_by = usr.id;

            bool success = pdal.Update(p);
            if (success == true)
            {
                MessageBox.Show("Product Upated Successfully !");
                clear();
                DataTable dt = pdal.Select();
                dgvProducts.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Failed to update the Product! ");
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            p.id = Convert.ToInt32(txtProductID.Text);
            bool success = pdal.Delete(p);
            if (success == true)
            {
                MessageBox.Show("Product Deleted Successfully !");
                clear();
                DataTable dt = pdal.Select();
                dgvProducts.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Failed to Delete the Product! ");
            }
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keywords = txtSearch.Text;
            if (keywords != null)
            {
                DataTable dt = pdal.Search(keywords);
                dgvProducts.DataSource = dt;
            }
            else
            {
                DataTable dt = pdal.Select();
                dgvProducts.DataSource = dt;
            }
        }
    }
}

