using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
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

        public List<Mobile> GetAvailableImeiByModel(string modelId)
        {
            List<Mobile> mobiles = new List<Mobile>();
            using (SqlConnection conn = _connectionFactory.CreateConnection())
            {
                conn.Open();
                string query = "SELECT ModelId, IMEINO, Status, Warranty, Price FROM tbl_Mobile WHERE ModelId = @ModelId AND Status = 'Available'";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ModelId", modelId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            mobiles.Add(new Mobile
                            {
                                ModelId = reader["ModelId"].ToString(),
                                IMEINO = reader["IMEINO"].ToString(),
                                Status = reader["Status"].ToString(),
                                Warranty = reader["Warranty"] != DBNull.Value ? Convert.ToDateTime(reader["Warranty"]) : (DateTime?)null,
                                Price = Convert.ToDecimal(reader["Price"])
                            });
                        }
                    }
                }
            }
            return mobiles;
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

                    return Convert.ToDecimal(result);
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
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new Exception("Không thể cập nhật trạng thái. IMEI không tồn tại.");
                    }
                }
            }
        }

        public bool PostMobile(string modelId, string imeiNo, string status, DateTime warranty, SqlMoney price)
        {
            try
            {
                using (SqlConnection conn = _connectionFactory.CreateConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(
                        "INSERT INTO tbl_Mobile (ModelId, IMEINO, Status, Warranty, Price) " +
                        "VALUES (@ModelId, @IMEINO, @Status, @Warranty, @Price)", conn);
                    cmd.Parameters.AddWithValue("@ModelId", modelId);
                    cmd.Parameters.AddWithValue("@IMEINO", imeiNo);
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@Warranty", warranty);
                    cmd.Parameters.AddWithValue("@Price", price); // Đã dùng SqlMoney

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