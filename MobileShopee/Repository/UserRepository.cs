using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using MobileShopee.Db;
using MobileShopee.Models;

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
                    string query = "SELECT PWD, Role FROM tbl_User WHERE UserName = @UserName";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserName", username);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedPWD = reader["PWD"].ToString();
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

        public (bool success, string message) AddEmployee(User employee)
        {
            try
            {
                using (SqlConnection conn = _connectionFactory.CreateConnection())
                {
                    conn.Open();
                    string query = "INSERT INTO tbl_User (UserName, PWD, EmployeeName, Address, MobileNumber, Hint, Role) " +
                                  "VALUES (@UserName, @PWD, @EmployeeName, @Address, @MobileNumber, @Hint, @Role)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserName", employee.UserName);
                        cmd.Parameters.AddWithValue("@PWD", employee.PWD);
                        cmd.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                        cmd.Parameters.AddWithValue("@Address", employee.Address);
                        cmd.Parameters.AddWithValue("@MobileNumber", employee.MobileNumber);
                        cmd.Parameters.AddWithValue("@Hint", employee.Hint);
                        cmd.Parameters.AddWithValue("@Role", employee.Role);
                        cmd.ExecuteNonQuery();
                    }
                    return (true, "Tạo tài khoản thành công");
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}