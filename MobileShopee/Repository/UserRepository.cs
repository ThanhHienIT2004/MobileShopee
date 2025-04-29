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
    }

}