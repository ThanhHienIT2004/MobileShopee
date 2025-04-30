using MobileShopee.Db;
using MobileShopee.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace MobileShopee.Repository
{
    public class TransactionRepository
    {
        private readonly DbConnectionFactory _connectionFactory;
        public TransactionRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public string GetNextTransId()
        {
            try
            {
                using (SqlConnection conn = _connectionFactory.CreateConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_GetNextTransantionId", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    var result = cmd.ExecuteScalar();
                    return result?.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy mã giao dịch: " + ex.Message);
            }
        }

        public bool PostTrans(string transId, string modelId, int quantity, DateTime date, SqlMoney amount)
        {
            try
            {
                using (SqlConnection conn = _connectionFactory.CreateConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(
                        "INSERT INTO tbl_Transaction (TransId, ModelId, Quantity, Date, Amount) " +
                        "VALUES (@TransId, @ModelId, @Quantity, @Date, @Amount)", conn);
                    cmd.Parameters.AddWithValue("@TransId", transId);
                    cmd.Parameters.AddWithValue("@ModelId", modelId);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.Parameters.AddWithValue("@Date", date);
                    cmd.Parameters.AddWithValue("@Amount", amount);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm giao dịch: " + ex.Message);
            }
        }

        public List<Transaction> GetTrans()
        {
            var transactions = new List<Transaction>();
            try
            {
                using (SqlConnection conn = _connectionFactory.CreateConnection())
                {
                    conn.Open();
                    var cmd = new SqlCommand("SELECT TransId, ModelId, Quantity, Date, Amount FROM tbl_Transaction", conn);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        transactions.Add(new Transaction
                        {
                            TransId = reader.GetString(0),
                            ModelId = reader.GetString(1),
                            Quantity = reader.GetInt32(2),
                            Date = reader.GetDateTime(3),
                            Amount = reader.GetSqlMoney(4)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách giao dịch: " + ex.Message);
            }
            return transactions;
        }
    }
}
