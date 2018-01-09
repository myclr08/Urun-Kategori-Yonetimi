using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Odev
{
    public partial class frmCategory : Form
    {
        public frmCategory()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection();
        string connectionString = "Data Source=ASD\\SQLEXPRESS;Initial Catalog=NORTHWND;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rd;
        Categories seciliCategory = new Categories();
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (lstCategories.Items.Count > 0)
                    lstCategories.Items.Clear();
                con.Open();
                cmd.CommandText = "select * from Categories where CategoryName like @name ";
                cmd.Parameters.AddWithValue("@name", "%" + txtSearch.Text + "%");
                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    Categories ctg = new Categories()
                    {
                        ID = Convert.ToInt32(rd["CategoryID"]),
                        CategoryName = rd["CategoryName"].ToString(),
                        Description = rd["Description"].ToString()
                    };
                    lstCategories.Items.Add(ctg);
                }
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        private void KategorileriGetir()
        {
            try
            {
                if (lstCategories.Items.Count > 0)
                    lstCategories.Items.Clear();
                con.Open();
                cmd.CommandText = "select * from Categories ";
                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    Categories ctg = new Categories()
                    {
                        ID = Convert.ToInt32(rd["CategoryID"]),
                        CategoryName = rd["CategoryName"].ToString(),
                        Description = rd["Description"].ToString()
                    };
                    lstCategories.Items.Add(ctg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

        }

        private void frmCategory_Load(object sender, EventArgs e)
        {
            con.ConnectionString = connectionString;
            cmd.Connection = con;
            KategorileriGetir();
        }

        private void lstCategories_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (lstCategories.SelectedIndex == -1)
                return;
            seciliCategory = lstCategories.SelectedItem as Categories;
            txtCategoryName.Text = seciliCategory.CategoryName;
            txtDescription.Text = seciliCategory.Description;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            try
            {
                con.Open();
                cmd.CommandText = "INSERT INTO Categories (CategoryName, Description) VALUES(@name, @des)";
                cmd.Parameters.AddWithValue("@name", txtCategoryName.Text);
                cmd.Parameters.AddWithValue("@des", txtDescription.Text);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                KategorileriGetir();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            try
            {
                con.Open();
                cmd.CommandText = "UPDATE Categories SET CategoryName = @name, Description = @des WHERE(CategoryID = @id)";
                cmd.Parameters.AddWithValue("@name", txtCategoryName.Text);
                cmd.Parameters.AddWithValue("@id", seciliCategory.ID);
                cmd.Parameters.AddWithValue("@des", txtDescription.Text);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                KategorileriGetir();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                cmd.CommandText = "DELETE FROM Categories WHERE(CategoryID = @id)";
                cmd.Parameters.AddWithValue("@id", seciliCategory.ID);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                KategorileriGetir();
            }
        }
    }
}
