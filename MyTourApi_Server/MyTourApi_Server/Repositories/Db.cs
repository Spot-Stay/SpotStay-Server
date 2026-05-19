using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MyTourApi_Server.Repositories
{
    public class Db
    {
        private readonly string connectionString;

        public Db(IConfiguration configuration)
        {
            // 종혁님의 appsettings.json 연결 문자열 매핑 완벽 반영
            connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}