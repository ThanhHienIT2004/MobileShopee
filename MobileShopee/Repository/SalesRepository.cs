using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MobileShopee.Db;

namespace MobileShopee.Repository
{
    class SalesRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;

        public SalesRepository(DbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public void AddSale(string custName, string mobileNo, string address, string email, string imei, decimal price)
        {
            using (var conn = _dbConnectionFactory.CreateConnection())
            {
                conn.Open();

                // Thêm vào tbl_Customer
                string custId = "CUST" + DateTime.Now.Ticks.ToString().Substring(0, 8);
                string customerQuery = "INSERT INTO tbl_Customer (CustId, CustName, MobileNo, Address, Email) " +
                                     "VALUES (@CustId, @CustName, @MobileNo, @Address, @Email)";

                using (SqlCommand cmd = new SqlCommand(customerQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@CustId", custId);
                    cmd.Parameters.AddWithValue("@CustName", custName);
                    cmd.Parameters.AddWithValue("@MobileNo", mobileNo);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.ExecuteNonQuery();
                }

                // Thêm vào tbl_Sales
                string salesId = "SALE" + DateTime.Now.Ticks.ToString().Substring(0, 8);
                string salesQuery = "INSERT INTO tbl_Sales (SId, IMEINO, PurchaseDate, Price, CustId) " +
                                   "VALUES (@SId, @IMEINO, @PurchaseDate, @Price, @CustId)";

                using (SqlCommand cmd = new SqlCommand(salesQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@SId", salesId);
                    cmd.Parameters.AddWithValue("@IMEINO", imei);
                    cmd.Parameters.AddWithValue("@PurchaseDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@CustId", custId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
} 