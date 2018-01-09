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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection();
        string connectionString = "Data Source=ASD\\SQLEXPRESS;Initial Catalog=NORTHWND;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rd;
        Products seciliProduct = new Products();

        private void btnCatForm_Click(object sender, EventArgs e)
        {
            frmCategory frm = new frmCategory();
            frm.ShowDialog();
            cmbListeKategorileri.Items.Clear();
            cmbCategory.Items.Clear();
            KategorileriGetir(cmbListeKategorileri);
            KategorileriGetir(cmbCategory);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            con.ConnectionString = connectionString;
            cmd.Connection = con;
            KategorileriGetir(cmbListeKategorileri);
            KategorileriGetir(cmbCategory);
        }

        private void UrunListesiniDoldur(Categories ctg)
        {
            try
            {
                if (cmbListeKategorileri.SelectedIndex == -1)
                    return;
                if (lstProduct.Items.Count > 0)
                    lstProduct.Items.Clear();
                con.Open();
                cmd.CommandText = "select * from Products where CategoryID=@id";
                cmd.Parameters.AddWithValue("@id", ctg.ID);
                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    Products prd = new Products()
                    {
                        ID = Convert.ToInt32(rd["ProductID"]),
                        ProductName = rd["ProductName"].ToString(),
                        CategoryID = Convert.ToInt32(rd["CategoryID"]),
                        Discontinued = Convert.ToBoolean(rd["Discontinued"]),
                        UnitPrice = Convert.ToDecimal(rd["UnitPrice"]),
                        UnitsInStock = Convert.ToInt16(rd["UnitsInStock"])
                    };
                    lstProduct.Items.Add(prd);
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

        private void KategorileriGetir(ComboBox cmb)
        {
            try
            {
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
                    cmb.Items.Add(ctg);

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

        private void cmbListeKategorileri_SelectedIndexChanged(object sender, EventArgs e)
        {
            UrunListesiniDoldur(cmbListeKategorileri.SelectedItem as Categories);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                cmd.CommandText = "INSERT INTO Products(ProductName, CategoryID, UnitPrice, UnitsInStock, Discontinued) VALUES(@prname, @ctgid, @unitprice, @stock, @discontinued)";
                cmd.Parameters.AddWithValue("@prname", txtProductName.Text);
                cmd.Parameters.AddWithValue("@ctgid", (cmbCategory.SelectedItem as Categories).ID);
                cmd.Parameters.AddWithValue("@unitprice", nudUnitPrice.Value);
                cmd.Parameters.AddWithValue("@stock", Convert.ToInt16(nudUnitsInStock.Value));
                cmd.Parameters.AddWithValue("@discontinued", cbDiscontinued.Checked);
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
                if (cmbListeKategorileri.SelectedIndex != cmbCategory.SelectedIndex)
                    cmbListeKategorileri.SelectedIndex = cmbCategory.SelectedIndex;
                else
                    UrunListesiniDoldur(cmbListeKategorileri.SelectedItem as Categories);
            }
        }

        private void lstProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstProduct.SelectedIndex == -1)
                return;
            seciliProduct = lstProduct.SelectedItem as Products;
            txtProductName.Text = seciliProduct.ProductName;
            nudUnitPrice.Value = seciliProduct.UnitPrice;
            nudUnitsInStock.Value = seciliProduct.UnitsInStock;
            cbDiscontinued.Checked = seciliProduct.Discontinued;
            for (int i = 0; i < cmbCategory.Items.Count; i++)
            {
                cmbCategory.SelectedIndex = i;
                if ((cmbCategory.SelectedItem as Categories).ID == seciliProduct.CategoryID)
                    break;
            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                cmd.CommandText = "UPDATE Products SET ProductName = @prname, CategoryID = @ctgid, UnitPrice = @unitprice, UnitsInStock = @stock, Discontinued = @discontinued WHERE (ProductID = @id)";
                cmd.Parameters.AddWithValue("@id", seciliProduct.ID);
                cmd.Parameters.AddWithValue("@prname", txtProductName.Text);
                cmd.Parameters.AddWithValue("@ctgid", (cmbCategory.SelectedItem as Categories).ID);
                cmd.Parameters.AddWithValue("@unitprice", nudUnitPrice.Value);
                cmd.Parameters.AddWithValue("@stock", Convert.ToInt16(nudUnitsInStock.Value));
                cmd.Parameters.AddWithValue("@discontinued", cbDiscontinued.Checked);
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
                if (cmbListeKategorileri.SelectedIndex != cmbCategory.SelectedIndex)
                    cmbListeKategorileri.SelectedIndex = cmbCategory.SelectedIndex;
                else
                    UrunListesiniDoldur(cmbListeKategorileri.SelectedItem as Categories);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                cmd.CommandText = "DELETE FROM Products WHERE(ProductID = @id)";
                cmd.Parameters.AddWithValue("@id", seciliProduct.ID);
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
                if (cmbListeKategorileri.SelectedIndex != cmbCategory.SelectedIndex)
                    cmbListeKategorileri.SelectedIndex = cmbCategory.SelectedIndex;
                else
                    UrunListesiniDoldur(cmbListeKategorileri.SelectedItem as Categories);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (lstProduct.Items.Count > 0)
                    lstProduct.Items.Clear();
                con.Open();
                cmd.CommandText = "select * from Products where ProductName like @name and CategoryID=@id";
                cmd.Parameters.AddWithValue("@id", (cmbListeKategorileri.SelectedItem as Categories).ID);
                cmd.Parameters.AddWithValue("@name", "%" + txtSearch.Text + "%");
                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    Products prd = new Products()
                    {
                        ID = Convert.ToInt32(rd["ProductID"]),
                        ProductName = rd["ProductName"].ToString(),
                        CategoryID = Convert.ToInt32(rd["CategoryID"]),
                        Discontinued = Convert.ToBoolean(rd["Discontinued"]),
                        UnitPrice = Convert.ToDecimal(rd["UnitPrice"]),
                        UnitsInStock = Convert.ToInt16(rd["UnitsInStock"])
                    };
                    lstProduct.Items.Add(prd);
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
    }
}
