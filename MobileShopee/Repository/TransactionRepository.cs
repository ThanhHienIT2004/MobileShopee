using System.Data.SqlClient;
using MobileShopee.Db;
using MobileShopee.Models;

namespace MobileShopee.Repository
{
    public class TransactionRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public TransactionRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void AddTransaction(Transaction transaction)
        {
            using (SqlConnection conn = _connectionFactory.CreateConnection())
            {
                conn.Open();
                string query = "INSERT INTO tbl_Transaction (TransId, ModelId, Quantity, Date, Amount) " +
                              "VALUES (@TransId, @ModelId, @Quantity, @Date, @Amount)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TransId", transaction.TransId);
                    cmd.Parameters.AddWithValue("@ModelId", transaction.ModelId);
                    cmd.Parameters.AddWithValue("@Quantity", transaction.Quantity);
                    cmd.Parameters.AddWithValue("@Date", transaction.Date);
                    cmd.Parameters.AddWithValue("@Amount", transaction.Amount);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}