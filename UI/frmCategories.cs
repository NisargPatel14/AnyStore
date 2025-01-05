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
    public partial class frmCategories : Form
    {
        public frmCategories()
        {
            InitializeComponent();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        categoriesBLL c = new categoriesBLL();
        categoriesDAL dal = new categoriesDAL();
        userDAL udal = new userDAL();
        private void btnAdd_Click(object sender, EventArgs e)
        {
            c.title = txtTitle.Text;
            c.description = txtDescription.Text;
            c.added_date = DateTime.Now;

            string loggedUser = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUser);
            c.added_by = usr.id;

            bool success = dal.Insert(c);
            if(success==true)
            {
                MessageBox.Show("New Category Inserted Successfully !");
                clear();
                DataTable dt = dal.Select();
                dgvCategories.DataSource = dt;
            }
            else 
            {
                MessageBox.Show("Failed to Insert new Category! ");
            }
        }
        public void clear() 
        {
            txtCategories.Text = "";
            txtTitle.Text = "";
            txtDescription.Text = "";
            txtSearch.Text = "";
        }
        private void frmCategories_Load(object sender, EventArgs e)
        {
            DataTable dt = dal.Select();
            dgvCategories.DataSource = dt;

        }

        private void dgvCategories_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int RowIndex = e.RowIndex;
            txtCategories.Text = dgvCategories.Rows[RowIndex].Cells[0].Value.ToString();
            txtTitle.Text = dgvCategories.Rows[RowIndex].Cells[1].Value.ToString();
            txtDescription.Text = dgvCategories.Rows[RowIndex].Cells[2].Value.ToString();
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            c.id = Convert.ToInt32(txtCategories.Text);
            c.title=txtTitle.Text;
            c.description=txtDescription.Text;
            c.added_date=DateTime.Now;
            string loggedUser = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUser);
            c.added_by = usr.id;

            bool success = dal.Update(c);
            if (success == true)
            {
                MessageBox.Show("Category Upated Successfully !");
                clear();
                DataTable dt = dal.Select();
                dgvCategories.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Failed to update the Category! ");
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            c.id = Convert.ToInt32(txtCategories.Text);
            bool success = dal.Delete(c);
            if (success == true)
            {
                MessageBox.Show("Category Deleted Successfully !");
                clear();
                DataTable dt = dal.Select();
                dgvCategories.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Failed to Delete the Category! ");
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keywords = txtSearch.Text;
            if(keywords!=null)
            {
                DataTable dt = dal.Search(keywords);
                dgvCategories.DataSource = dt;
            }
            else 
            {
                DataTable dt = dal.Select();
                dgvCategories.DataSource = dt;
            }
        }
    }
}
