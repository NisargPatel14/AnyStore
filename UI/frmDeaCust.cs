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
    public partial class frmDeaCust : Form
    {
        public frmDeaCust()
        {
            InitializeComponent();
        }
        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        deacustBLL dc = new deacustBLL();
        deacustDAL dcdal = new deacustDAL();
        userDAL udal = new userDAL();
        private void btnAdd_Click(object sender, EventArgs e)
        {
            dc.type = cmbType.Text;
            dc.name = txtName.Text;
            dc.email = txtEmail.Text;
            dc.contact=txtContact.Text;
            dc.address = txtAddress.Text;
            dc.added_date = DateTime.Now;

            string loggedUser = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUser);
            dc.added_by = usr.id;
            bool success = dcdal.Insert(dc);
            if (success == true)
            {
                MessageBox.Show("New Category Inserted Successfully !");
                clear();
                DataTable dt = dcdal.Select();
                dgvDeaCust.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Failed to Insert new Category! ");
            }
        }
        public void clear()
        {
            cmbType.Text = "";
            txtName.Text = "";
            txtEmail.Text = "";
            txtContact.Text = "";
            txtAddress.Text = "";
            txtSearch.Text = "";
        }

        private void frmDeaCust_Load(object sender, EventArgs e)
        {
            DataTable dt = dcdal.Select();
            dgvDeaCust.DataSource = dt;
        }

        private void dgvDeaCust_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int RowIndex = e.RowIndex;
            txtDeaCustID.Text = dgvDeaCust.Rows[RowIndex].Cells[0].Value.ToString();
            cmbType.Text = dgvDeaCust.Rows[RowIndex].Cells[1].Value.ToString();
            txtName.Text = dgvDeaCust.Rows[RowIndex].Cells[2].Value.ToString();
            txtEmail.Text = dgvDeaCust.Rows[RowIndex].Cells[3].Value.ToString();
            txtContact.Text = dgvDeaCust.Rows[RowIndex].Cells[4].Value.ToString();
            txtAddress.Text = dgvDeaCust.Rows[RowIndex].Cells[5].Value.ToString();
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            dc.id = Convert.ToInt32(txtDeaCustID.Text);
            dc.type = cmbType.Text;
            dc.name = txtName.Text;
            dc.email = txtEmail.Text;
            dc.contact = txtContact.Text;
            dc.address = txtAddress.Text;
            dc.added_date = DateTime.Now;

            string loggedUser = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUser);
            dc.added_by = usr.id;

            bool success = dcdal.Update(dc);
            if (success == true)
            {
                MessageBox.Show("Dealer or Customer Upated Successfully !");
                clear();
                DataTable dt = dcdal.Select();
                dgvDeaCust.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Failed to update the Dealer or Customer! ");
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            dc.id = Convert.ToInt32(txtDeaCustID.Text);
            bool success = dcdal.Delete(dc);
            if (success == true)
            {
                MessageBox.Show("Dealer or Customer Deleted Successfully !");
                clear();
                DataTable dt = dcdal.Select();
                dgvDeaCust.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Failed to Delete Dealer or Customer! ");
            }
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keywords = txtSearch.Text;
            if (keywords != null)
            {
                DataTable dt = dcdal.Search(keywords);
                dgvDeaCust.DataSource = dt;
            }
            else
            {
                DataTable dt = dcdal.Select();
                dgvDeaCust.DataSource = dt;
            }
        }
    }
}
