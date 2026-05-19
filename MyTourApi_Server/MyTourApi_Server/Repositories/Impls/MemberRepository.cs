using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MyTourApi_Server.Repositories.Interfaces;
using MyTourApi_Server.Models;
using System.Data;
using System.Threading.Tasks;
using System;

namespace MyTourApi_Server.Repositories.Impls
{
    public class MemberRepository : IMemberRepository
    {
        private readonly string _connectionString;

        public MemberRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                                ?? throw new InvalidOperationException("Connection string not found.");
        }

        public async Task<Member?> GetByUserIdAsync(string userId)
        {
            string query = "SELECT * FROM Member WHERE UserId = @UserId";

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QuerySingleOrDefaultAsync<Member>(query, new { UserId = userId });
            }
        }
        public async Task<Member?> GetProfileByIdAsync(int memberId)
        {
            // 마이페이지에 보여줄 회원 정보 조회 쿼리 (비밀번호 제외)
            string query = @"
            SELECT MemberId, UserId, Name
            FROM Member
            WHERE MemberId = @MemberId";

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                return await db.QueryFirstOrDefaultAsync<Member>(query, new { MemberId = memberId });
            }
        }

    }
}