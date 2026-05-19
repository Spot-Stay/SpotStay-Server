using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MyTourApi.Repositories.Interfaces;
using MyTourApi_Server.Models;
using System.Data;
using System.Threading.Tasks;

namespace MyTourApi.Repositories.Impls
{
    public class MemberRepository : IMemberRepository
    {
        private readonly string _connectionString;

        public MemberRepository(IConfiguration configuration)
        {
            // appsettings.json에서 DefaultConnection 문자열을 읽어옴
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                                ?? throw new InvalidOperationException("Connection string not found.");
        }

        public async Task<Member?> GetByUserIdAsync(string userId)
        {
            // 파라미터 바인딩으로 SQL 인젝션 방어
            string query = "SELECT * FROM jjh.Member WHERE UserId = @UserId";

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                // Dapper의 QuerySingleOrDefaultAsync를 사용해 1개의 행을 매핑
                return await db.QuerySingleOrDefaultAsync<Member>(query, new { UserId = userId });
            }
        }
    }
}