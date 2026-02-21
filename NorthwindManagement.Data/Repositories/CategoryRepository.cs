using System.Collections.Generic;
using Dapper;
using Microsoft.Data.SqlClient;


using NorthwindManagement.Models;

namespace NorthwindManagement.Data.Repositories
{
    public class CategoryRepository
    {
        public List<Category> GetAll()
        {
            using var conn = Db.CreateConnection();

            string sql = @"
SELECT
    CategoryID AS CategoryId,
    CategoryName,
    Description
FROM Categories
ORDER BY CategoryID;";

            return conn.Query<Category>(sql).ToList();
        }

        public Category? GetById(int id)
        {
            using var conn = Db.CreateConnection();

            string sql = @"
SELECT
    CategoryID AS CategoryId,
    CategoryName,
    Description
FROM Categories
WHERE CategoryID = @Id;";

            return conn.QueryFirstOrDefault<Category>(sql, new { Id = id });
        }

        public int Insert(Category c)
        {
            using var conn = Db.CreateConnection();

            string sql = @"
INSERT INTO Categories (CategoryName, Description)
VALUES (@CategoryName, @Description);

SELECT CAST(SCOPE_IDENTITY() AS int);";

            return conn.ExecuteScalar<int>(sql, c);
        }

        public bool Update(Category c)
        {
            using var conn = Db.CreateConnection();

            string sql = @"
UPDATE Categories
SET CategoryName = @CategoryName,
    Description = @Description
WHERE CategoryID = @CategoryId;";

            return conn.Execute(sql, c) > 0;
        }
        public bool Delete(int id)
        {
            using var conn = Db.CreateConnection();

            string sql = @"
DELETE FROM Categories
WHERE CategoryID = @Id;";

            return conn.Execute(sql, new { Id = id }) > 0;
        }

        public bool HasProducts(int categoryId)
        {
            using var conn = Db.CreateConnection();
            string sql = "SELECT COUNT(1) FROM Products WHERE CategoryID = @Id;";
            return conn.ExecuteScalar<int>(sql, new { Id = categoryId }) > 0;
        }
    }
}
