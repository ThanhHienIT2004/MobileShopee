using System;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using MobileShopee.Db;
using MobileShopee.Models;

namespace MobileShopee.Repository
{

    public class MobileRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public MobileRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public bool PostMobile(string modelId, string imeiNo, string status, DateTime warranty, SqlMoney price)
        {
            try
            {
                using (SqlConnection conn = _connectionFactory.CreateConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(
                        "INSERT INTO tbl_Mobile (ModelId, IMEINo, Status, Warranty, Price) " +
                        "VALUES (@ModelId, @IMEINo, @Status, @Warranty, @Price)", conn);
                    cmd.Parameters.AddWithValue("@ModelId", modelId);
                    cmd.Parameters.AddWithValue("@IMEINo", imeiNo);
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@Warranty", warranty);
                    cmd.Parameters.AddWithValue("@Price", price);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm mobile: " + ex.Message);
            }
        }
    }
}

