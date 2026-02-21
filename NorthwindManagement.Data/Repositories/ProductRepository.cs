using Dapper;
using NorthwindManagement.Models;

namespace NorthwindManagement.Data.Repositories
{
    public class ProductRepository
    {
        public List<Product> GetAll()
        {
            using var conn = Db.CreateConnection();

            string sql = @"
SELECT
    ProductID   AS ProductId,
    ProductName,
    CategoryID  AS CategoryId,
    UnitPrice,
    UnitsInStock,
    Discontinued
FROM Products
ORDER BY ProductID;";

            return conn.Query<Product>(sql).ToList();
        }

        public Product? GetById(int id)
        {
            using var conn = Db.CreateConnection();

            string sql = @"
SELECT
    ProductID   AS ProductId,
    ProductName,
    CategoryID  AS CategoryId,
    UnitPrice,
    UnitsInStock,
    Discontinued
FROM Products
WHERE ProductID = @Id;";

            return conn.QueryFirstOrDefault<Product>(sql, new { Id = id });
        }

        public int Insert(Product p)
        {
            using var conn = Db.CreateConnection();

            string sql = @"
INSERT INTO Products (ProductName, CategoryID, UnitPrice, UnitsInStock, Discontinued)
VALUES (@ProductName, @CategoryId, @UnitPrice, @UnitsInStock, @Discontinued);

SELECT CAST(SCOPE_IDENTITY() AS int);";

            return conn.ExecuteScalar<int>(sql, p);
        }

        public bool Update(Product p)
        {
            using var conn = Db.CreateConnection();

            string sql = @"
UPDATE Products
SET
    ProductName   = @ProductName,
    CategoryID    = @CategoryId,
    UnitPrice     = @UnitPrice,
    UnitsInStock  = @UnitsInStock,
    Discontinued  = @Discontinued
WHERE ProductID = @ProductId;";

            return conn.Execute(sql, p) > 0;
        }

        public bool Delete(int id)
        {
            using var conn = Db.CreateConnection();

            string sql = @"DELETE FROM Products WHERE ProductID = @Id;";

            return conn.Execute(sql, new { Id = id }) > 0;
        }
    }
}
