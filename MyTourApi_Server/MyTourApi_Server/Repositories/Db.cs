using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MyTourApi_Server.Repositories
{
    public class Db
    {
        private readonly string connectionString;

        public Db(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}