using MobileShopee.Db;
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
