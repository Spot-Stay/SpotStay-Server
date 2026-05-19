using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MyTourApi_Server.Models;
using MyTourApi_Server.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MyTourApi_Server.Repositories.Impls
{
    public class TouristSpotRepository : ITouristSpotRepository
    {
        private readonly string _connectionString;

        public TouristSpotRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                                ?? throw new InvalidOperationException("Connection string not found.");
        }

        public async Task<List<TouristSpot>> SearchSpotsAsync(string keyword, string regionSido)
        {
            // ⭐ jjh.TouristSpot 스키마 적용 및 조건부 LIKE 검색 쿼리
            string query = @"
                SELECT SpotId, ContentId, Name, Address, Category, 
                       Description, Phone, Homepage, ImageUrl, Latitude, Longitude, 
                       RegionSido, RegionSigungu
                FROM jjh.TouristSpot
                WHERE (Name LIKE @Keyword OR @Keyword = '')
                  AND (RegionSido LIKE @Region OR @Region = '')";

            // Dapper 파라미터 바인딩 (% 기호를 붙여 부분 검색 가능하게 만듦)
            var parameters = new
            {
                Keyword = string.IsNullOrEmpty(keyword) ? "" : $"%{keyword}%",
                Region = string.IsNullOrEmpty(regionSido) ? "" : $"%{regionSido}%"
            };

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var result = await db.QueryAsync<TouristSpot>(query, parameters);
                return result.AsList();
            }
        }

        public async Task<TouristSpot?> GetSpotByIdAsync(int spotId)
        {
            // ⭐ 특정 SpotId와 일치하는 데이터 1개만 들고오는 쿼리
            string query = @"
        SELECT SpotId, ContentId, Name, Address, Category, 
               Description, Phone, Homepage, ImageUrl, Latitude, Longitude, 
               RegionSido, RegionSigungu
        FROM jjh.TouristSpot
        WHERE SpotId = @SpotId";

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                // 1개만 찾을 때는 QueryFirstOrDefaultAsync를 씁니다.
                return await db.QueryFirstOrDefaultAsync<TouristSpot>(query, new { SpotId = spotId });
            }
        }
    }
}