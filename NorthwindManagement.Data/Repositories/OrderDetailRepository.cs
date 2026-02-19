using System.Collections.Generic;
using Microsoft.Data.SqlClient;

using NorthwindManagement.Models;

namespace NorthwindManagement.Data.Repositories
{
    public class OrderDetailRepository
    {
        public void AddItems(int orderId, List<OrderDetail> items, SqlConnection conn, SqlTransaction tx)
        {
            foreach (var item in items)
            {
                using var cmd = new SqlCommand(@"
INSERT INTO [Order Details](OrderID, ProductID, UnitPrice, Quantity, Discount)
VALUES(@orderId, @productId, @price, @qty, @disc)", conn, tx);

                cmd.Parameters.AddWithValue("@orderId", orderId);
                cmd.Parameters.AddWithValue("@productId", item.ProductId);
                cmd.Parameters.AddWithValue("@price", item.UnitPrice);
                cmd.Parameters.AddWithValue("@qty", item.Quantity);
                cmd.Parameters.AddWithValue("@disc", item.Discount);

                cmd.ExecuteNonQuery();
            }
        }

        public List<OrderDetail> GetByOrderId(int orderId)
        {
            var result = new List<OrderDetail>();

            using var conn = Db.CreateConnection();
            conn.Open();

            using var cmd = new SqlCommand(@"
SELECT OrderID, ProductID, UnitPrice, Quantity, Discount
FROM [Order Details]
WHERE OrderID=@id", conn);

            cmd.Parameters.AddWithValue("@id", orderId);

            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                result.Add(new OrderDetail
                {
                    OrderId = r.GetInt32(0),
                    ProductId = r.GetInt32(1),
                    UnitPrice = r.GetDecimal(2),
                    Quantity = r.GetInt16(3),
                    Discount = (float)r.GetDouble(4) 
                });
            }

            return result;
        }
    }
}
