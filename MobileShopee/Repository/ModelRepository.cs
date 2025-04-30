using MobileShopee.Db;
using MobileShopee.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileShopee.Repository
{
    public class ModelRepository
    {
        private readonly DbConnectionFactory _connectionFactory;
        public ModelRepository(DbConnectionFactory connectionFactory) {
            _connectionFactory = connectionFactory;
        }

        public string GetNextModelId()
        {
            using (SqlConnection conn = _connectionFactory.CreateConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetNextModelId", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                var result = cmd.ExecuteScalar();
                return result?.ToString(); // Trả về ID tiếp theo
            }
        }

        public List<Model> GetModel()
        {
            var models = new List<Model>();

            try
            {
                using (SqlConnection conn = _connectionFactory.CreateConnection())
                {
                    conn.Open();
                    var cmd = new SqlCommand("SELECT ModelId, CompId, ModelNum, AvailableQty FROM tbl_Model", conn);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        models.Add(new Model
                        {
                            ModelId = reader.GetString(0),
                            CompId = reader.GetString(1),
                            ModelNum = reader.GetString(2),
                            AvailableQty = reader.GetInt32(3).ToString() // Chuyển int thành string để khớp với Model
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                // Log hoặc ném lỗi để xử lý ở tầng trên
                throw new Exception("Lỗi khi lấy danh sách model: " + ex.Message);
            }

            return models;
        }

        public List<Model> GetModelsByCompany(string compId)
        {
            var models = new List<Model>();
            try
            {
                using (SqlConnection conn = _connectionFactory.CreateConnection())
                {
                    conn.Open();
                    var cmd = new SqlCommand("sp_GetModelsByCompany", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CompId", compId);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        models.Add(new Model
                        {
                            ModelId = reader.GetString(0),
                            CompId = reader.GetString(1),
                            ModelNum = reader.GetString(2),
                            AvailableQty = reader.GetInt32(3).ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách model theo công ty: " + ex.Message);
            }
            return models;
        }

        public bool PostModel(string modelId,string compId,string modelNum,int availableQty)
        {
            using (SqlConnection conn = _connectionFactory.CreateConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO tbl_Model (ModelId, CompId, ModelNum, AvailableQty) VALUES (@ModelId, @CompId,@ModelNum,@AvailableQty)", conn);
                cmd.Parameters.AddWithValue("@ModelId", modelId);
                cmd.Parameters.AddWithValue("@CompId", compId);
                cmd.Parameters.AddWithValue("@ModelNum", modelNum);
                cmd.Parameters.AddWithValue("@AvailableQty", availableQty);

                // Kiểm tra xem có ít nhất 1 bản ghi được chèn hay không
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }

}
