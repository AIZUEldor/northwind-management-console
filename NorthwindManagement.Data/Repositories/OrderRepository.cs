using Dapper;
using Microsoft.Data.SqlClient;
using NorthwindManagement.Models;

namespace NorthwindManagement.Data.Repositories
{
    public class OrderRepository
    {

        public int CreateOrder(Order o, SqlConnection conn, SqlTransaction tx)
        {
            string sql = @"
INSERT INTO Orders
(
    CustomerID, EmployeeID, OrderDate, RequiredDate, ShippedDate,
    ShipName, ShipAddress, ShipCity, ShipCountry, Freight
)
VALUES
(
    @CustomerId, @EmployeeId, @OrderDate, @RequiredDate, @ShippedDate,
    @ShipName, @ShipAddress, @ShipCity, @ShipCountry, @Freight
);

SELECT CAST(SCOPE_IDENTITY() AS int);";

            return conn.ExecuteScalar<int>(sql, o, transaction: tx);
        }
        public int CreateOrder(Order o)
        {
            using var conn = Db.CreateConnection();
            conn.Open();

            using var tx = conn.BeginTransaction();
            try
            {
                int id = CreateOrder(o, conn, tx);
                tx.Commit();
                return id;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        public Order? GetById(int orderId)
        {
            using var conn = Db.CreateConnection();


            string sql = @"
SELECT
    OrderID AS OrderId,
    RTRIM(CustomerID) AS CustomerId,
    EmployeeID AS EmployeeId,
    OrderDate,
    RequiredDate,
    ShippedDate,
    ShipName,
    ShipAddress,
    ShipCity,
    ShipCountry,
    Freight
FROM Orders
WHERE OrderID = @Id;";

            return conn.QueryFirstOrDefault<Order>(sql, new { Id = orderId });
        }
    }
}