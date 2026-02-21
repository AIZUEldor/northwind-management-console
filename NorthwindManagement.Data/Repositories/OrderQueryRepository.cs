using Dapper;

namespace NorthwindManagement.Data.Repositories
{
    public class OrderQueryRepository
    {
        public OrderReceiptData? GetReceiptData(int orderId)
        {
            using var conn = Db.CreateConnection();

            string headerSql = @"
SELECT
    OrderID AS OrderId,
    RTRIM(CustomerID) AS CustomerId,
    OrderDate,
    ShipName,
    ShipAddress,
    ShipCity,
    ShipCountry,
    ISNULL(Freight, 0) AS Freight
FROM Orders
WHERE OrderID = @Id;";

            var header = conn.QueryFirstOrDefault<OrderReceiptHeader>(headerSql, new { Id = orderId });
            if (header == null) return null;

            string linesSql = @"
SELECT
    od.ProductID AS ProductId,
    p.ProductName,
    od.UnitPrice,
    od.Quantity,
    od.Discount
FROM [Order Details] od
JOIN Products p ON p.ProductID = od.ProductID
WHERE od.OrderID = @Id
ORDER BY od.ProductID;";

            var lines = conn.Query<OrderReceiptLineData>(linesSql, new { Id = orderId }).ToList();

            return new OrderReceiptData
            {
                Header = header,
                Lines = lines
            };
        }
    }

    public class OrderReceiptData
    {
        public OrderReceiptHeader Header { get; set; } = new();
        public List<OrderReceiptLineData> Lines { get; set; } = new();
    }

    public class OrderReceiptHeader
    {
        public int OrderId { get; set; }
        public string? CustomerId { get; set; }
        public System.DateTime? OrderDate { get; set; }
        public string? ShipName { get; set; }
        public string? ShipAddress { get; set; }
        public string? ShipCity { get; set; }
        public string? ShipCountry { get; set; }
        public decimal Freight { get; set; }
    }

    public class OrderReceiptLineData
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; }
    }
}