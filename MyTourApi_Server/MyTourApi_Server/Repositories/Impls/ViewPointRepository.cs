using Microsoft.Data.SqlClient;
using MyTourApi_Server.Models;
using MyTourApi_Server.Repositories.Interfaces;

namespace MyTourApi_Server.Repositories.Impls
{
    public class ViewPointRepository : IViewPointRepository
    {
        private readonly string connectionString;

        public ViewPointRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public List<ViewPoint> GetByParkName(string parkName)
        {
            List<ViewPoint> list = new List<ViewPoint>();

            string sql = @"
                SELECT 
                    ViewPointId, 
                    Name, 
                    ParkName, 
                    Description, 
                    Latitude, 
                    Longitude
                FROM ViewPoint
                WHERE ParkName LIKE @ParkName
                  AND Latitude IS NOT NULL
                  AND Longitude IS NOT NULL
                ORDER BY Name";

            using SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ParkName", "%" + parkName + "%");

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ViewPoint viewPoint = new ViewPoint
                {
                    ViewPointId = Convert.ToInt32(reader["ViewPointId"]),
                    Name = reader["Name"] == DBNull.Value ? "" : reader["Name"].ToString(),
                    ParkName = reader["ParkName"] == DBNull.Value ? "" : reader["ParkName"].ToString(),
                    Description = reader["Description"] == DBNull.Value ? "" : reader["Description"].ToString(),
                    Latitude = reader["Latitude"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["Latitude"]),
                    Longitude = reader["Longitude"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["Longitude"])
                };

                list.Add(viewPoint);
            }

            return list;
        }
    }
}