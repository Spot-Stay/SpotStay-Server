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
                FROM jjh.ViewPoint
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

        public bool ExistsByNameAndParkName(string name, string parkName)
        {
            string sql = @"
                SELECT COUNT(*) 
                FROM jjh.ViewPoint 
                WHERE Name = @Name 
                  AND ParkName = @ParkName";

            using SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@ParkName", parkName);

            int count = Convert.ToInt32(cmd.ExecuteScalar());

            return count > 0;
        }

        public int InsertViewPoint(ViewPoint viewPoint)
        {
            string sql = @"
                INSERT INTO jjh.ViewPoint
                (
                    Name, 
                    ParkName, 
                    Description, 
                    Latitude, 
                    Longitude
                )
                VALUES
                (
                    @Name, 
                    @ParkName, 
                    @Description, 
                    @Latitude, 
                    @Longitude
                )";

            using SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Name", viewPoint.Name ?? "");
            cmd.Parameters.AddWithValue("@ParkName", viewPoint.ParkName ?? "");
            cmd.Parameters.AddWithValue("@Description", viewPoint.Description ?? "");
            cmd.Parameters.AddWithValue("@Latitude", viewPoint.Latitude);
            cmd.Parameters.AddWithValue("@Longitude", viewPoint.Longitude);

            return cmd.ExecuteNonQuery();
        }
    }
}