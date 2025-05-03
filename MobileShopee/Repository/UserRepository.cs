using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
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
        public bool UserExists(string userName)
        {
            try
            {
                using (SqlConnection conn = _connectionFactory.CreateConnection())


                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM tbl_User WHERE UserName = @UserName";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserName", userName);
                        return (int)cmd.ExecuteScalar() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
                return false;
            }

        }

        public (bool Success, string Role, string Message) Login(string username, string password)
        {
            try
            {
                if (!UserExists(username))
                {
                    return (false, "", "Không có tài khoản " + username);
                }

                using (SqlConnection conn = _connectionFactory.CreateConnection())
                {
                    conn.Open();
                    string query = "SELECT Password, Role FROM tbl_User WHERE UserName = @UserName";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserName", username);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedPWD = reader["Password"].ToString();
                                string role = reader["Role"].ToString();

                                if (password == storedPWD)
                                {
                                    return (true, role, "Đăng nhập thành công");
                                }
                                else
                                {
                                    return (false, "", "Mật khẩu sai");
                                }
                            }
                        }

                        return (false, "", "Lỗi trong quá trình đọc dữ liệu");
                    }
                }
            }
            catch (Exception ex)
            {
                return (false, "", "Lỗi: " + ex.Message);
            }
        }
    }
}