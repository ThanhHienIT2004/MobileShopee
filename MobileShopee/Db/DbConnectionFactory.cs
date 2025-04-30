using System.Data.SqlClient;
using System.Configuration;

namespace MobileShopee.Db
{
    public class DbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["MobileShopee"].ConnectionString;
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }   
    }
}