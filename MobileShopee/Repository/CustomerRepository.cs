using System.Data.SqlClient;
using MobileShopee.Db;
using MobileShopee.Models;

namespace MobileShopee.Repository
{
    public class CustomerRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public CustomerRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void AddCustomer(Customer customer)
        {
            using (SqlConnection conn = _connectionFactory.CreateConnection())
            {
                conn.Open();
                string query = "INSERT INTO tbl_Customer (CustId, Cust_Name, MobileNumber, Address, Email) " +
                              "VALUES (@CustId, @CustName, @MobileNumber, @Address, @Email)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CustId", customer.CustId);
                    cmd.Parameters.AddWithValue("@CustName", customer.CustName);
                    cmd.Parameters.AddWithValue("@MobileNumber", customer.MobileNumber);
                    cmd.Parameters.AddWithValue("@Address", customer.Address);
                    cmd.Parameters.AddWithValue("@Email", customer.Email);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}