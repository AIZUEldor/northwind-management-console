using Microsoft.Data.SqlClient;

namespace NorthwindManagement.Data
{
    public static class Db
    {
        private static readonly string _connectionString =
            "Server=(localdb)\\MSSQLLocalDB;Database=Lesson1Course;Trusted_Connection=True;TrustServerCertificate=True;";

        public static SqlConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}