using System;
using System.Data;
using System.Data.SqlClient;
using MobileShopee.Db;

namespace MobileShopee.Repository
{
    public class UserRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public UserRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public bool ValidateUser(string username, string password)
        {
            using (SqlConnection conn = _connectionFactory.CreateConnection())
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM tbl_User WHERE UserName = @Username AND PWO = @Password";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password); // Lưu ý: Mã hóa mật khẩu trong thực tế
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }

        public DataTable GetCompanies()
        {
            using (SqlConnection conn = _connectionFactory.CreateConnection())
            {
                conn.Open();
                string query = "SELECT CompId, CName FROM tbl_Company";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                {
                    DataTable companyTable = new DataTable();
                    adapter.Fill(companyTable);
                    return companyTable;
                }
            }
        }

        public DataTable GetModelsByCompany(string compId)
        {
            using (SqlConnection conn = _connectionFactory.CreateConnection())
            {
                conn.Open();
                string query = "SELECT ModelId, ModelNum FROM tbl_Model WHERE CompId = @CompId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CompId", compId);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable modelTable = new DataTable();
                        adapter.Fill(modelTable);
                        return modelTable;
                    }
                }
            }
        }

        public DataTable GetAvailableImeiByModel(string modelId)
        {
            using (SqlConnection conn = _connectionFactory.CreateConnection())
            {
                conn.Open();
                string query = "SELECT IMEINO, Price FROM tbl_Mobile WHERE ModelId = @ModelId AND Status = 'Available'";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ModelId", modelId);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable imeiTable = new DataTable();
                        adapter.Fill(imeiTable);
                        return imeiTable;
                    }
                }
            }
        }

        public decimal GetPriceByImei(string imeiNo)
        {
            using (SqlConnection conn = _connectionFactory.CreateConnection())
            {
                conn.Open();
                string query = "SELECT Price FROM tbl_Mobile WHERE IMEINO = @IMEINO";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IMEINO", imeiNo);
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToDecimal(result) : 0;
                }
            }
        }

        public void UpdateMobileStatus(string imeiNo, string status)
        {
            using (SqlConnection conn = _connectionFactory.CreateConnection())
            {
                conn.Open();
                string query = "UPDATE tbl_Mobile SET Status = @Status WHERE IMEINO = @IMEINO";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@IMEINO", imeiNo);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AddCustomer(string custId, string custName, string mobileNumber, string address, string email)
        {
            using (SqlConnection conn = _connectionFactory.CreateConnection())
            {
                conn.Open();
                string query = "INSERT INTO tbl_Customer (CustId, Cust_Name, MobileNumber, Address, Email) " +
                              "VALUES (@CustId, @CustName, @MobileNumber, @Address, @Email)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CustId", custId);
                    cmd.Parameters.AddWithValue("@CustName", custName);
                    cmd.Parameters.AddWithValue("@MobileNumber", mobileNumber);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AddSale(string salesId, string imeiNo, DateTime purchaseDate, decimal price, string custId)
        {
            using (SqlConnection conn = _connectionFactory.CreateConnection())
            {
                conn.Open();
                string query = "INSERT INTO tbl_Sales (SId, IMEINO, PurchaseDate, Price, CustId) " +
                              "VALUES (@SId, @IMEINO, @PurchaseDate, @Price, @CustId)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SId", salesId);
                    cmd.Parameters.AddWithValue("@IMEINO", imeiNo);
                    cmd.Parameters.AddWithValue("@PurchaseDate", purchaseDate);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@CustId", custId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateModelQuantity(string modelId)
        {
            using (SqlConnection conn = _connectionFactory.CreateConnection())
            {
                conn.Open();
                string query = "UPDATE tbl_Model SET AvailableQty = AvailableQty - 1 WHERE ModelId = @ModelId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ModelId", modelId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}