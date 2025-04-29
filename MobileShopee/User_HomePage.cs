using MobileShopee.Db;
using MobileShopee.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MobileShopee
{
    public partial class User_HomePage : Form
    {
        private readonly UserRepository _userRepository;

        public User_HomePage()
        {
            InitializeComponent();
            _userRepository = new UserRepository(new DbConnectionFactory());

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void User_HomePage_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string connStr = ConfigurationManager.ConnectionStrings["MobileShopee"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    // Thêm vào tbl_Customer
                    string custId = "CUST" + DateTime.Now.Ticks.ToString().Substring(0, 8);
                    string customerQuery = "INSERT INTO tbl_Customer (CustId, CustName, MobileNo, Address, Email) " +
                                         "VALUES (@CustId, @CustName, @MobileNo, @Address, @Email)";

                    using (SqlCommand cmd = new SqlCommand(customerQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustId", custId);
                        cmd.Parameters.AddWithValue("@CustName", txtName.Text);
                        cmd.Parameters.AddWithValue("@MobileNo", txtPhone.Text);
                        cmd.Parameters.AddWithValue("@Address", txtAddr.Text);
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                        cmd.ExecuteNonQuery();
                    }

                    // Thêm vào tbl_Sales
                    string salesId = "SALE" + DateTime.Now.Ticks.ToString().Substring(0, 8);
                    string salesQuery = "INSERT INTO tbl_Sales (SId, IMEINO, PurchaseDate, Price, CustId) " +
                                       "VALUES (@SId, @IMEINO, @PurchaseDate, @Price, @CustId)";

                    using (SqlCommand cmd = new SqlCommand(salesQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@SId", salesId);
                        cmd.Parameters.AddWithValue("@IMEINO", txtImei.Text);
                        cmd.Parameters.AddWithValue("@PurchaseDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Price", decimal.Parse(txtPrice.Text));
                        cmd.Parameters.AddWithValue("@CustId", custId);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Ghi nhận bán hàng thành công!");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}
