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
        public void UpdateModelQuantity(string modelId)
        {
            using (SqlConnection conn = _connectionFactory.CreateConnection())
            {
                conn.Open();
                string query = "UPDATE tbl_Model SET AvailableQty = AvailableQty - 1 WHERE ModelId = @ModelId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ModelId", modelId);
                    cmd.ExecuteNonQuery();
                }
            }
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
                            AvailableQty = reader.GetInt32(3) // Chuyển int thành string để khớp với Model
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
            if (string.IsNullOrWhiteSpace(compId))
            {
                throw new ArgumentException("CompId không được để trống hoặc null.", nameof(compId));
            }

            var models = new List<Model>();
            try
            {
                using (SqlConnection conn = _connectionFactory.CreateConnection())
                {
                    conn.Open();
                    string query = "SELECT ModelId, CompId, ModelNum, AvailableQty FROM tbl_Model WHERE CompId = @CompId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@CompId", SqlDbType.VarChar, 20).Value = compId;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                models.Add(new Model
                                {
                                    ModelId = reader["ModelId"].ToString(),
                                    CompId = reader["CompId"].ToString() ,
                                    ModelNum =  reader["ModelNum"].ToString() ,
                                    AvailableQty = Convert.ToInt32(reader["AvailableQty"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi không xác định khi lấy danh sách model cho CompId {compId}: {ex.Message}", ex);
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

        public bool UpdateModelQuantity(string modelId, int quantity)
        {
            try
            {
                using (SqlConnection conn = _connectionFactory.CreateConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(
                        "UPDATE tbl_Model SET AvailableQty = @Quantity " +
                        "WHERE ModelId = @ModelId", conn);
                    cmd.Parameters.AddWithValue("@ModelId", modelId);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật số lượng model: " + ex.Message);
            }
        }
    }

}
