using System.Data.SqlClient;
using MobileShopee.Db;
using MobileShopee.Models;

namespace MobileShopee.Repository
{
    public class SaleRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public SaleRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void AddSale(Sale sale)
        {
            using (SqlConnection conn = _connectionFactory.CreateConnection())
            {
                conn.Open();
                string query = "INSERT INTO tbl_Sales (SId, IMEINO, PurchaseDate, Price, CustId) " +
                              "VALUES (@SId, @IMEINO, @PurchaseDate, @Price, @CustId)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SId", sale.SId);
                    cmd.Parameters.AddWithValue("@IMEINO", sale.IMEINO);
                    cmd.Parameters.AddWithValue("@PurchaseDate", sale.PurchaseDate);
                    cmd.Parameters.AddWithValue("@Price", sale.Price);
                    cmd.Parameters.AddWithValue("@CustId", sale.CustId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}