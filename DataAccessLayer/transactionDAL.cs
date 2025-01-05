using ADT_PROJECT.BussinessLogicLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADT_PROJECT.DataAccessLayer
{
    internal class transactionDAL
    {
        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
        #region INSERT Method
        public bool Insert(transactionBLL t, out int transactionID)
        {
            bool isSuccess = false;
            transactionID = -1;
            SqlConnection con = new SqlConnection(myconnstrng);
            try
            {
                String g = "INSERT into tbl_transactions (type,dea_cust_id,grandTotal,transaction_date,tax,discount,added_by) VALUES (@type,@dea_cust_id,@grandTotal,@transaction_date,@tax,@discount,@added_by); SELECT @@IDENTITY;";

                SqlCommand cmd = new SqlCommand(g, con);
                cmd.Parameters.AddWithValue("@type", t.type);
                cmd.Parameters.AddWithValue("@dea_cust_id", t.dea_cust_id);
                cmd.Parameters.AddWithValue("@grandTotal", t.grandTotal);
                cmd.Parameters.AddWithValue("@transaction_date", t.transaction_date);
                cmd.Parameters.AddWithValue("@tax", t.tax);
                cmd.Parameters.AddWithValue("@discount", t.discount);
                cmd.Parameters.AddWithValue("@added_by", t.added_by);

                con.Open();
                object o = cmd.ExecuteScalar();
                if (o != null)
                {
                    transactionID = Convert.ToInt32(o.ToString());
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return isSuccess;
        }
        #endregion
        #region Method to display all the transactions
        public DataTable DisplayAllTransactions()
        {
            SqlConnection con = new SqlConnection(myconnstrng);
            DataTable dt = new DataTable();
            try 
            {
                string sql = "Select * from tbl_transactions";
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                con.Open();
                adapter.Fill(dt);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally 
            {
                con.Close();
            }
            return dt;
        }
        #endregion
        #region Method to display transaction based on transaction type
        public DataTable DisplayTransactionByType(string type)
        {
            SqlConnection con = new SqlConnection(myconnstrng);
            DataTable dt = new DataTable();
            try
            {
                string sql = "Select * from tbl_transactions where type='"+type+"'";
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                con.Open();
                adapter.Fill(dt);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                con.Close();
            }
            return dt;
        }
        #endregion
    }
}
