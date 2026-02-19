using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using NorthwindManagement.Models;

namespace NorthwindManagement.Data.Repositories
{
    public class ProductRepository
    {
        public List<Product> GetAll()
        {
            var result = new List<Product>();

            using var conn = Db.CreateConnection();
            conn.Open();

            using var cmd = new SqlCommand(@"
SELECT ProductID, ProductName, CategoryID, UnitPrice, UnitsInStock, Discontinued
FROM Products
ORDER BY ProductID", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Product
                {
                    ProductId = reader.GetInt32(0),
                    ProductName = reader.GetString(1),
                    CategoryId = reader.IsDBNull(2) ? null : reader.GetInt32(2),
                    UnitPrice = reader.IsDBNull(3) ? null : reader.GetDecimal(3),
                    UnitsInStock = reader.IsDBNull(4) ? null : reader.GetInt16(4),
                    Discontinued = reader.GetBoolean(5)
                });
            }

            return result;
        }

        public Product? GetById(int id)
        {
            using var conn = Db.CreateConnection();
            conn.Open();

            using var cmd = new SqlCommand(@"
SELECT ProductID, ProductName, CategoryID, UnitPrice, UnitsInStock, Discontinued
FROM Products
WHERE ProductID=@id", conn);

            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return new Product
            {
                ProductId = reader.GetInt32(0),
                ProductName = reader.GetString(1),
                CategoryId = reader.IsDBNull(2) ? null : reader.GetInt32(2),
                UnitPrice = reader.IsDBNull(3) ? null : reader.GetDecimal(3),
                UnitsInStock = reader.IsDBNull(4) ? null : reader.GetInt16(4),
                Discontinued = reader.GetBoolean(5)
            };
        }

        public int Insert(Product p)
        {
            using var conn = Db.CreateConnection();
            conn.Open();

            using var cmd = new SqlCommand(@"
INSERT INTO Products(ProductName, CategoryID, UnitPrice, UnitsInStock, Discontinued)
VALUES(@name, @catId, @price, @stock, @disc);
SELECT CAST(SCOPE_IDENTITY() AS int);", conn);

            cmd.Parameters.AddWithValue("@name", p.ProductName);
            cmd.Parameters.AddWithValue("@catId", (object?)p.CategoryId ?? System.DBNull.Value);
            cmd.Parameters.AddWithValue("@price", (object?)p.UnitPrice ?? System.DBNull.Value);
            cmd.Parameters.AddWithValue("@stock", (object?)p.UnitsInStock ?? System.DBNull.Value);
            cmd.Parameters.AddWithValue("@disc", p.Discontinued);

            return (int)cmd.ExecuteScalar();
        }

        public bool Update(Product p)
        {
            using var conn = Db.CreateConnection();
            conn.Open();

            using var cmd = new SqlCommand(@"
UPDATE Products
SET ProductName=@name,
    CategoryID=@catId,
    UnitPrice=@price,
    UnitsInStock=@stock,
    Discontinued=@disc
WHERE ProductID=@id", conn);

            cmd.Parameters.AddWithValue("@id", p.ProductId);
            cmd.Parameters.AddWithValue("@name", p.ProductName);
            cmd.Parameters.AddWithValue("@catId", (object?)p.CategoryId ?? System.DBNull.Value);
            cmd.Parameters.AddWithValue("@price", (object?)p.UnitPrice ?? System.DBNull.Value);
            cmd.Parameters.AddWithValue("@stock", (object?)p.UnitsInStock ?? System.DBNull.Value);
            cmd.Parameters.AddWithValue("@disc", p.Discontinued);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int id)
        {
            using var conn = Db.CreateConnection();
            conn.Open();

            using var cmd = new SqlCommand("DELETE FROM Products WHERE ProductID=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
