using Dapper;
using Microsoft.Data.SqlClient;
using MyTourApi_Server.DTOs.Request;
using MyTourApi_Server.Models;
using MyTourApi_Server.Repositories.Interfaces;
using System.Data;

namespace MyTourApi_Server.Repositories.Impls
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly string _connectionString;

        public FavoriteRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        private string ConvertTargetTypeToDb(string targetType)
        {
            if (targetType == "TouristSpot")
                return "SPOT";

            if (targetType == "Accommodation")
                return "ACCOM";

            return targetType;
        }

        public async Task<bool> AddAsync(FavoriteRequestDto request)
        {
            string dbTargetType = ConvertTargetTypeToDb(request.TargetType);

            string sql = @"
                IF NOT EXISTS (
                    SELECT 1 
                    FROM Favorite
                    WHERE MemberId = @MemberId 
                      AND TargetType = @TargetType 
                      AND TargetId = @TargetId
                )
                BEGIN
                    INSERT INTO Favorite (MemberId, TargetType, TargetId, CreatedAt)
                    VALUES (@MemberId, @TargetType, @TargetId, GETDATE())
                END";

            using IDbConnection db = new SqlConnection(_connectionString);

            int rows = await db.ExecuteAsync(sql, new
            {
                MemberId = request.MemberId,
                TargetType = dbTargetType,
                TargetId = request.TargetId
            });

            return rows > 0;
        }

        public async Task<List<Favorite>> GetByMemberIdAsync(int memberId)
        {
            string sql = @"
                SELECT 
                    F.FavoriteId, 
                    F.MemberId, 
                    M.UserId, 
                    F.TargetType, 
                    F.TargetId, 
                    F.CreatedAt,
                    CASE 
                        WHEN F.TargetType = 'SPOT' THEN T.Name 
                        WHEN F.TargetType = 'ACCOM' THEN A.Name 
                    END AS TargetName,
                    CASE 
                        WHEN F.TargetType = 'SPOT' THEN T.Address 
                        WHEN F.TargetType = 'ACCOM' THEN A.Address 
                    END AS Address,
                    CASE 
                        WHEN F.TargetType = 'SPOT' THEN T.ImageUrl 
                        WHEN F.TargetType = 'ACCOM' THEN A.ImageUrl 
                    END AS ImageUrl
                FROM Favorite F
                INNER JOIN Member M 
                    ON F.MemberId = M.MemberId
                LEFT JOIN TouristSpot T 
                    ON F.TargetType = 'SPOT' 
                   AND F.TargetId = T.SpotId
                LEFT JOIN Accommodation A 
                    ON F.TargetType = 'ACCOM' 
                   AND F.TargetId = A.AccomId
                WHERE F.MemberId = @MemberId
                ORDER BY F.CreatedAt DESC";

            using IDbConnection db = new SqlConnection(_connectionString);

            var result = await db.QueryAsync<Favorite>(sql, new
            {
                MemberId = memberId
            });

            List<Favorite> list = result.AsList();

            foreach (Favorite item in list)
            {
                if (item.TargetType == "SPOT")
                    item.TargetType = "TouristSpot";
                else if (item.TargetType == "ACCOM")
                    item.TargetType = "Accommodation";
            }

            return list;
        }

        public async Task<bool> DeleteAsync(int favoriteId)
        {
            string sql = @"
                DELETE FROM Favorite 
                WHERE FavoriteId = @FavoriteId";

            using IDbConnection db = new SqlConnection(_connectionString);

            int rows = await db.ExecuteAsync(sql, new
            {
                FavoriteId = favoriteId
            });

            return rows > 0;
        }
    }
}