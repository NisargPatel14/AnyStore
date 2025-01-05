using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ADT_PROJECT.BussinessLogicLayer;

namespace ADT_PROJECT.DataAccessLayer
{
    internal class productDAL
    {
        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
        #region Select Method for Product Module
        public DataTable Select()
        {
            SqlConnection con = new SqlConnection(myconnstrng);
            DataTable dt = new DataTable();
            try
            {
                String g = "SELECT * from tbl_products";
                SqlCommand cmd = new SqlCommand(g, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                con.Open();
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return dt;
        }
        #endregion
        #region Insert new Product
        public bool Insert(productBLL p)
        {
            bool isSuccess = false;
            SqlConnection con = new SqlConnection(myconnstrng);
            try
            {
                String g = "INSERT into tbl_products (name,category,description,rate,qty,added_date,added_by) VALUES (@name,@category,@description,@rate,@qty,@added_date,@added_by)";

                SqlCommand cmd = new SqlCommand(g, con);
                cmd.Parameters.AddWithValue("@name", p.name);
                cmd.Parameters.AddWithValue("@category", p.category);
                cmd.Parameters.AddWithValue("@description", p.description);
                cmd.Parameters.AddWithValue("@rate", p.rate);
                cmd.Parameters.AddWithValue("@qty", p.qty);
                cmd.Parameters.AddWithValue("@added_date", p.added_date);
                cmd.Parameters.AddWithValue("@added_by", p.added_by);

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
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
        #region Update Method
        public bool Update(productBLL p)
        {
            bool isSuccess = false;
            SqlConnection con = new SqlConnection(myconnstrng);
            try
            {
                string h = "UPDATE tbl_products SET name=@name,category=@category,description=@description,rate=@rate,added_date=@added_date,added_by=@added_by WHERE id=@id";
                SqlCommand cmd = new SqlCommand(h, con);

                cmd.Parameters.AddWithValue("@name", p.name);
                cmd.Parameters.AddWithValue("@category", p.category);
                cmd.Parameters.AddWithValue("@description", p.description);
                cmd.Parameters.AddWithValue("@rate", p.rate);
                cmd.Parameters.AddWithValue("@qty", p.qty);
                cmd.Parameters.AddWithValue("@added_date", p.added_date);
                cmd.Parameters.AddWithValue("@added_by", p.added_by);
                cmd.Parameters.AddWithValue("@id", p.id);

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
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
        #region Delete Product
        public bool Delete(productBLL p)
        {
            bool isSuccess = false;
            SqlConnection con = new SqlConnection(myconnstrng);
            try
            {
                string i = "DELETE from tbl_products WHERE id=@id";
                SqlCommand cmd = new SqlCommand(i, con);

                cmd.Parameters.AddWithValue("@id", p.id);

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
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
        #region Method for Search Functionality
        public DataTable Search(string keywords)
        {
            SqlConnection con = new SqlConnection(myconnstrng);
            DataTable dt = new DataTable();
            try
            {
                String j = "SELECT * from tbl_products WHERE id LIKE '%" + keywords + "%' OR name LIKE '%" + keywords + "%' OR category LIKE '%" + keywords + "%'";
                SqlCommand cmd = new SqlCommand(j, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                con.Open();
                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return dt;
        }
        #endregion
        #region Method to search Product in Transaction Module
        public productBLL GetProductsForTransactions(string keyword) 
        {
            productBLL p = new productBLL();
            SqlConnection con = new SqlConnection(myconnstrng);
            DataTable dt = new DataTable();
            try
            {
                String j = "SELECT name , rate , qty from tbl_products WHERE id LIKE '%" + keyword + "%' OR name LIKE '%" + keyword + "%'";
                SqlDataAdapter sda = new SqlDataAdapter(j,con);
                con.Open();
                sda.Fill(dt);
                if(dt.Rows.Count>0)
                {
                    p.name = dt.Rows[0]["name"].ToString();
                    p.rate = Convert.ToDecimal(dt.Rows[0]["rate"].ToString());
                    p.qty = Convert.ToDecimal(dt.Rows[0]["qty"].ToString());
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
            return p;
        }
        #endregion
        #region Method to get product ID based on product name
        public productBLL GetDeaProdcutIDFromName(string ProductName)
        {
            productBLL p = new productBLL();
            SqlConnection con = new SqlConnection(myconnstrng);
            DataTable dt = new DataTable();

            try
            {
                String j = "SELECT id from tbl_products WHERE name='" + ProductName + "'";
                SqlDataAdapter sda = new SqlDataAdapter(j, con);
                con.Open();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    p.id = Convert.ToInt32(dt.Rows[0]["id"].ToString());
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
            return p;
        }
        #endregion
        #region Method to get current quantity from database based on product ID
        public decimal GetProductQty(int ProductID)
        {
            SqlConnection con = new SqlConnection(myconnstrng);
            decimal qty = 0;
            DataTable dt = new DataTable();
            try
            {
                String j = "SELECT qty from tbl_products WHERE id='" + ProductID + "'";
                SqlCommand cmd = new SqlCommand(j, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                con.Open();
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    qty = Convert.ToDecimal(dt.Rows[0]["qty"].ToString());
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
            return qty;
        }
        #endregion
        #region Method to update quantity
        public bool UpdateQuantity(int ProductID, decimal qty)
        {
            bool success = false;
            SqlConnection con = new SqlConnection(myconnstrng);
            try 
            {
                string sql = "UPDATE tbl_products set qty=@qty where id=@id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@qty", qty);
                cmd.Parameters.AddWithValue("@id", ProductID);
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                if(rows>0)
                {
                    success = true;
                }
                else 
                {
                    success = false;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return success;
        }
        #endregion
        #region Method to Increase Product
        public bool IncreaseProduct(int ProductID, decimal Increaseqty) 
        {
            bool success = false;
            SqlConnection con = new SqlConnection(myconnstrng);
            try
            {
                decimal currentQty = GetProductQty(ProductID);
                decimal newQty = currentQty + Increaseqty;
                success=UpdateQuantity(ProductID, newQty);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return success;
        }
        #endregion
        #region Method to Decrease Product
        public bool DecreaseProduct(int ProductID, decimal Decreaseqty) 
        {
            bool success = false;
            SqlConnection con = new SqlConnection(myconnstrng);
            try
            {
                decimal currentQty = GetProductQty(ProductID);
                decimal newQty = currentQty - Decreaseqty;
                success = UpdateQuantity(ProductID, newQty);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return success;
        }
        #endregion
        #region Display products based on categories
        public DataTable DisplayProductsByCategory(string category)
        {
            SqlConnection con = new SqlConnection(myconnstrng);
            DataTable dt = new DataTable();
            try 
            {
                string sql = "Select * from tbl_products WHERE category='"+category+"'";
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                con.Open();
                adapter.Fill(dt);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
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
