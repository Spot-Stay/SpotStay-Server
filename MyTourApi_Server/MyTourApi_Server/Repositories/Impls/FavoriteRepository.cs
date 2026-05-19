using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MyTourApi_Server.DTOs.Request;
using MyTourApi_Server.Models;
using MyTourApi_Server.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MyTourApi_Server.Repositories.Impls
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly string _connectionString = string.Empty;

        // "TouristSpot" -> "SPOT" 변환용 (팀원분 DB 설계 매핑 법칙 반영)
        private string ConvertTargetTypeToDb(string targetType) =>
            targetType == "TouristSpot" ? "SPOT" : (targetType == "Accommodation" ? "ACCOM" : targetType);

        // 1. 즐겨찾기 추가 (중복 검사 포함 및 jjh. 스키마 적용)
        public async Task<bool> AddAsync(FavoriteRequestDto request)
        {
            string dbTargetType = ConvertTargetTypeToDb(request.TargetType);

            string sql = @"
                IF NOT EXISTS (
                    SELECT 1 FROM jjh.Favorite
                    WHERE MemberId = @MemberId AND TargetType = @TargetType AND TargetId = @TargetId
                )
                BEGIN
                    INSERT INTO jjh.Favorite (MemberId, TargetType, TargetId, CreatedAt)
                    VALUES (@MemberId, @TargetType, @TargetId, GETDATE())
                END";

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                int rows = await db.ExecuteAsync(sql, new { request.MemberId, TargetType = dbTargetType, request.TargetId });
                return rows > 0;
            }
        }

        // 2. 마이페이지용 목록 조회 (관광지/숙소 테이블 조인 통합본)
        public async Task<List<Favorite>> GetByMemberIdAsync(int memberId)
        {
            string sql = @"
                SELECT F.FavoriteId, F.MemberId, M.UserId, F.TargetType, F.TargetId, F.CreatedAt,
                       CASE WHEN F.TargetType = 'SPOT' THEN T.Name WHEN F.TargetType = 'ACCOM' THEN A.Name END AS TargetName,
                       CASE WHEN F.TargetType = 'SPOT' THEN T.Address WHEN F.TargetType = 'ACCOM' THEN A.Address END AS Address,
                       CASE WHEN F.TargetType = 'SPOT' THEN T.ImageUrl WHEN F.TargetType = 'ACCOM' THEN A.ImageUrl END AS ImageUrl
                FROM jjh.Favorite F
                INNER JOIN jjh.Member M ON F.MemberId = M.MemberId
                LEFT JOIN jjh.TouristSpot T ON F.TargetType = 'SPOT' AND F.TargetId = T.SpotId
                LEFT JOIN jjh.Accommodation A ON F.TargetType = 'ACCOM' AND F.TargetId = A.AccomId
                WHERE F.MemberId = @MemberId
                ORDER BY F.CreatedAt DESC";

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var result = await db.QueryAsync<Favorite>(sql, new { MemberId = memberId });
                var list = result.AsList();

                // DB에서 읽어온 'SPOT' 코드를 다시 클라이언트용 문자열로 원복
                foreach (var item in list)
                {
                    item.TargetType = item.TargetType == "SPOT" ? "TouristSpot" : (item.TargetType == "ACCOM" ? "Accommodation" : item.TargetType);
                }
                return list;
            }
        }

        // 3. 즐겨찾기 해제(삭제)
        public async Task<bool> DeleteAsync(int favoriteId)
        {
            string sql = "DELETE FROM jjh.Favorite WHERE FavoriteId = @FavoriteId";

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                int rows = await db.ExecuteAsync(sql, new { FavoriteId = favoriteId });
                return rows > 0;
            }
        }
    }
}