using Dapper;
using Microsoft.Data.SqlClient;
using NorthwindManagement.Models;

namespace NorthwindManagement.Data.Repositories
{
    public class OrderDetailRepository
    {

        public void AddItems(int orderId, List<OrderDetail> items, SqlConnection conn, SqlTransaction tx)
        {
            string sql = @"
INSERT INTO [Order Details] (OrderID, ProductID, UnitPrice, Quantity, Discount)
VALUES (@OrderId, @ProductId, @UnitPrice, @Quantity, @Discount);";


            foreach (var item in items)
                item.OrderId = orderId;

            conn.Execute(sql, items, transaction: tx);
        }

        public List<OrderDetail> GetByOrderId(int orderId)
        {
            using var conn = Db.CreateConnection();

            string sql = @"
SELECT
    OrderID AS OrderId,
    ProductID AS ProductId,
    UnitPrice,
    Quantity,
    Discount
FROM [Order Details]
WHERE OrderID = @Id;";

            return conn.Query<OrderDetail>(sql, new { Id = orderId }).ToList();
        }
    }
}