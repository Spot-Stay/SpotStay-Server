using Microsoft.Data.SqlClient;
using MyTourApi_Server.Models;
using MyTourApi_Server.Repositories.Interfaces;

namespace MyTourApi_Server.Repositories.Impls
{
    public class CampSiteRepository : ICampSiteRepository
    {
        private readonly string connectionString;

        public CampSiteRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public int InsertCampSites(List<CampSite> campsites)
        {
            int insertCount = 0;

            string sql =
                "INSERT INTO CampSite(Name, ParkName, Address, Phone, SiteCount, Latitude, Longitude) " +
                "VALUES(@Name, @ParkName, @Address, @Phone, @SiteCount, @Latitude, @Longitude)";

            using SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            foreach (CampSite item in campsites)
            {
                using SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Name", (object?)item.Name ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ParkName", (object?)item.ParkName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Address", (object?)item.Address ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Phone", (object?)item.Phone ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@SiteCount", (object?)item.SiteCount ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Latitude", (object?)item.Latitude ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Longitude", (object?)item.Longitude ?? DBNull.Value);

                insertCount += cmd.ExecuteNonQuery();
            }

            return insertCount;
        }

        public List<CampSite> SelectCampSites(string? parkName)
        {
            List<CampSite> list = new List<CampSite>();

            string sql =
                "SELECT CampId, Name, ParkName, Address, Phone, SiteCount, Latitude, Longitude " +
                "FROM CampSite ";

            if (!string.IsNullOrWhiteSpace(parkName))
            {
                sql += "WHERE ParkName LIKE @ParkName ";
            }

            sql += "ORDER BY ParkName, Name";

            using SqlConnection conn = new SqlConnection(connectionString);
            using SqlCommand cmd = new SqlCommand(sql, conn);

            if (!string.IsNullOrWhiteSpace(parkName))
            {
                cmd.Parameters.AddWithValue("@ParkName", "%" + parkName + "%");
            }

            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(ReadCampSite(reader));
            }

            return list;
        }

        public CampSite? SelectCampSiteById(int campId)
        {
            string sql =
                "SELECT CampId, Name, ParkName, Address, Phone, SiteCount, Latitude, Longitude " +
                "FROM CampSite " +
                "WHERE CampId = @CampId";

            using SqlConnection conn = new SqlConnection(connectionString);
            using SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@CampId", campId);

            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return ReadCampSite(reader);
            }

            return null;
        }

        private CampSite ReadCampSite(SqlDataReader reader)
        {
            return new CampSite
            {
                CampId = Convert.ToInt32(reader["CampId"]),
                Name = reader["Name"] == DBNull.Value ? null : reader["Name"].ToString(),
                ParkName = reader["ParkName"] == DBNull.Value ? null : reader["ParkName"].ToString(),
                Address = reader["Address"] == DBNull.Value ? null : reader["Address"].ToString(),
                Phone = reader["Phone"] == DBNull.Value ? null : reader["Phone"].ToString(),
                SiteCount = reader["SiteCount"] == DBNull.Value ? null : Convert.ToInt32(reader["SiteCount"]),
                Latitude = reader["Latitude"] == DBNull.Value ? null : Convert.ToDouble(reader["Latitude"]),
                Longitude = reader["Longitude"] == DBNull.Value ? null : Convert.ToDouble(reader["Longitude"])
            };
        }
    }
}