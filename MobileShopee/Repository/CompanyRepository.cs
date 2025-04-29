using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MobileShopee.Db;
using MobileShopee.Models;

namespace MobileShopee.Repository
{
    public class CompanyRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public CompanyRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        // Phương thức gọi Stored Procedure để lấy ID tiếp theo
        public string GetNextCompanyId()
        {
            using (SqlConnection conn = _connectionFactory.CreateConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetNextCompId", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                var result = cmd.ExecuteScalar();
                return result?.ToString(); // Trả về ID tiếp theo
            }
        }

        public List<Company> GetCompany()
        {
            var companies = new List<Company>();

            using (SqlConnection conn = _connectionFactory.CreateConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT CompId, CName FROM tbl_Company", conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    companies.Add(new Company
                    {
                        CompId = reader.GetString(0),
                        CName = reader.GetString(1)
                    });
                }
            }

            return companies;
        }

        // Phương thức chèn công ty mới
        public bool PostCompany(string compId, string cName)
        {
            using (SqlConnection conn = _connectionFactory.CreateConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO tbl_Company (CompId, CName) VALUES (@id, @name)", conn);
                cmd.Parameters.AddWithValue("@id", compId);
                cmd.Parameters.AddWithValue("@name", cName);

                // Kiểm tra xem có ít nhất 1 bản ghi được chèn hay không
                return cmd.ExecuteNonQuery() > 0;
            }
        }


    }
}
