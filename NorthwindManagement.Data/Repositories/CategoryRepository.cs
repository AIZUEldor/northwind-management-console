using System.Collections.Generic;
using Microsoft.Data.SqlClient;

using NorthwindManagement.Models;

namespace NorthwindManagement.Data.Repositories
{
    public class CategoryRepository
    {
        public List<Category> GetAll()
        {
            var result = new List<Category>();

            using var conn = Db.CreateConnection();
            conn.Open();

            using var cmd = new SqlCommand(
                "SELECT CategoryID, CategoryName, Description FROM Categories ORDER BY CategoryID",
                conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Category
                {
                    CategoryId = reader.GetInt32(0),
                    CategoryName = reader.GetString(1),
                    Description = reader.IsDBNull(2) ? null : reader.GetString(2)
                });
            }

            return result;
        }

        public Category? GetById(int id)
        {
            using var conn = Db.CreateConnection();
            conn.Open();

            using var cmd = new SqlCommand(
                "SELECT CategoryID, CategoryName, Description FROM Categories WHERE CategoryID=@id",
                conn);

            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return new Category
            {
                CategoryId = reader.GetInt32(0),
                CategoryName = reader.GetString(1),
                Description = reader.IsDBNull(2) ? null : reader.GetString(2)
            };
        }

        public int Insert(Category c)
        {
            using var conn = Db.CreateConnection();
            conn.Open();

            using var cmd = new SqlCommand(@"
INSERT INTO Categories(CategoryName, Description)
VALUES(@name, @desc);
SELECT CAST(SCOPE_IDENTITY() AS int);", conn);

            cmd.Parameters.AddWithValue("@name", c.CategoryName);
            cmd.Parameters.AddWithValue("@desc", (object?)c.Description ?? System.DBNull.Value);

            return (int)cmd.ExecuteScalar();
        }

        public bool Update(Category c)
        {
            using var conn = Db.CreateConnection();
            conn.Open();

            using var cmd = new SqlCommand(@"
UPDATE Categories
SET CategoryName=@name, Description=@desc
WHERE CategoryID=@id", conn);

            cmd.Parameters.AddWithValue("@id", c.CategoryId);
            cmd.Parameters.AddWithValue("@name", c.CategoryName);
            cmd.Parameters.AddWithValue("@desc", (object?)c.Description ?? System.DBNull.Value);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int id)
        {
            using var conn = Db.CreateConnection();
            conn.Open();

            using var cmd = new SqlCommand("DELETE FROM Categories WHERE CategoryID=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
