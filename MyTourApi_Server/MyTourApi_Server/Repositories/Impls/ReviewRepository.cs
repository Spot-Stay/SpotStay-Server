//ReviewRepository.cs

using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MyTourApi_Server.Models;
using MyTourApi_Server.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using MyTourApi_Server.DTOs.Request;

namespace MyTourApi_Server.Repositories.Impls
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly string _connectionString;

        public ReviewRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                                ?? throw new InvalidOperationException("Connection string not found.");
        }

        // "TouristSpot" -> "SPOT", "Accommodation" -> "ACCOM" 변환 (DB 공통 규격 매핑)
        private string ConvertTargetTypeToDb(string targetType) =>
            targetType == "TouristSpot" ? "SPOT" : (targetType == "Accommodation" ? "ACCOM" : targetType);

        private string ConvertTargetTypeToApi(string targetType) =>
            targetType == "SPOT" ? "TouristSpot" : (targetType == "ACCOM" ? "Accommodation" : targetType);

        // 1. 리뷰 등록
        public async Task<bool> AddAsync(ReviewRequestDto request)
        {
            string dbTargetType = ConvertTargetTypeToDb(request.TargetType);

            string sql = @"
                INSERT INTO jjh.Review (MemberId, TargetType, TargetId, Rating, Content, CreatedAt)
                VALUES (@MemberId, @TargetType, @TargetId, @Rating, @Content, GETDATE())";

            var parameters = new
            {
                MemberId = request.MemberId,
                TargetType = dbTargetType,
                TargetId = request.TargetId,
                Rating = request.Rating,
                Content = request.Content
            };

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                int rows = await db.ExecuteAsync(sql, parameters);
                return rows > 0;
            }
        }

        // 2. 상세페이지용 리뷰 목록 조회 (작성자 ID/이름 조인 포함)
        public async Task<List<Review>> GetByTargetAsync(string targetType, int targetId)
        {
            string dbTargetType = ConvertTargetTypeToDb(targetType);

            string sql = @"
                SELECT R.ReviewId, R.MemberId, R.TargetType, R.TargetId, R.Rating, R.Content, R.CreatedAt, R.UpdatedAt,
                       M.UserId, M.Name AS MemberName
                FROM jjh.Review R
                INNER JOIN jjh.Member M ON R.MemberId = M.MemberId
                WHERE R.TargetType = @TargetType AND R.TargetId = @TargetId
                ORDER BY R.CreatedAt DESC";

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var result = await db.QueryAsync<Review>(sql, new { TargetType = dbTargetType, TargetId = targetId });
                var list = result.AsList();

                foreach (var item in list)
                {
                    item.TargetType = ConvertTargetTypeToApi(item.TargetType);
                }
                return list;
            }
        }

        // 3. 마이페이지용 특정 회원의 리뷰 목록 조회 (관광지/숙소 이름 분기 조인 포함)
        public async Task<List<Review>> GetByMemberIdAsync(int memberId)
        {
            string sql = @"
                SELECT R.ReviewId, R.MemberId, R.TargetType, R.TargetId, R.Rating, R.Content, R.CreatedAt, R.UpdatedAt,
                       M.UserId, M.Name AS MemberName,
                       CASE WHEN R.TargetType = 'SPOT' THEN T.Name WHEN R.TargetType = 'ACCOM' THEN A.Name END AS TargetName
                FROM jjh.Review R
                INNER JOIN jjh.Member M ON R.MemberId = M.MemberId
                LEFT JOIN jjh.TouristSpot T ON R.TargetType = 'SPOT' AND R.TargetId = T.SpotId
                LEFT JOIN jjh.Accommodation A ON R.TargetType = 'ACCOM' AND R.TargetId = A.AccomId
                WHERE R.MemberId = @MemberId
                ORDER BY R.CreatedAt DESC";

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var result = await db.QueryAsync<Review>(sql, new { MemberId = memberId });
                var list = result.AsList();

                foreach (var item in list)
                {
                    item.TargetType = ConvertTargetTypeToApi(item.TargetType);
                }
                return list;
            }
        }

        // 4. 리뷰 수정
        public async Task<bool> UpdateAsync(int reviewId, ReviewUpdateRequestDto request)
        {
            string sql = @"
                UPDATE jjh.Review
                SET Rating = @Rating, Content = @Content, UpdatedAt = GETDATE()
                WHERE ReviewId = @ReviewId";

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                int rows = await db.ExecuteAsync(sql, new { Rating = request.Rating, Content = request.Content, ReviewId = reviewId });
                return rows > 0;
            }
        }

        // 5. 리뷰 삭제
        public async Task<bool> DeleteAsync(int reviewId)
        {
            string sql = "DELETE FROM jjh.Review WHERE ReviewId = @ReviewId";

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                int rows = await db.ExecuteAsync(sql, new { ReviewId = reviewId });
                return rows > 0;
            }
        }
    }
}