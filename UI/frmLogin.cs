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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }
        loginBLL l = new loginBLL();
        loginDAL dal = new loginDAL();

        public static string loggedIn;
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            l.username = txtUsername.Text;
            l.password = txtPassword.Text;
            l.user_type = cmbUserType.Text;

            bool success = dal.loginCheck(l);
            if (success == true)
            {
                MessageBox.Show("Login Successful");
                loggedIn = l.username;
                switch(l.user_type)
                {
                    case "Admin":
                        {
                            AdminForm admin = new AdminForm();
                            admin.Show();
                            this.Hide();
                            break;
                        }
                    case "User":
                        {
                            UserDashboard user = new UserDashboard();
                            user.Show();
                            this.Hide();
                            break;
                        }
                    default:
                        {
                            MessageBox.Show("Invalid User Type !");
                            break;
                        }
                }
            }
            else
            {
                MessageBox.Show("Login Failed");
            }
        }
    }
}
