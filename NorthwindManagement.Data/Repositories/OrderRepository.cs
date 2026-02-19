using System;
using Microsoft.Data.SqlClient;
using NorthwindManagement.Models;

namespace NorthwindManagement.Data.Repositories
{
    public class OrderRepository
    {
        // Transaction ishlatish uchun overload (OrderService ichida transaction qilamiz)
        public int CreateOrder(Order o, SqlConnection conn, SqlTransaction tx)
        {
            using var cmd = new SqlCommand(@"
INSERT INTO Orders(CustomerID, EmployeeID, OrderDate, RequiredDate, ShippedDate,
                  ShipName, ShipAddress, ShipCity, ShipCountry, Freight)
VALUES(@cust, @emp, @orderDate, @reqDate, @shipDate,
       @shipName, @shipAddr, @shipCity, @shipCountry, @freight);
SELECT CAST(SCOPE_IDENTITY() AS int);", conn, tx);

            cmd.Parameters.AddWithValue("@cust", (object?)o.CustomerId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@emp", (object?)o.EmployeeId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@orderDate", (object?)o.OrderDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@reqDate", (object?)o.RequiredDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@shipDate", (object?)o.ShippedDate ?? DBNull.Value);

            cmd.Parameters.AddWithValue("@shipName", (object?)o.ShipName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@shipAddr", (object?)o.ShipAddress ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@shipCity", (object?)o.ShipCity ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@shipCountry", (object?)o.ShipCauntry ?? DBNull.Value);

            cmd.Parameters.AddWithValue("@freight", (object?)o.Freight ?? DBNull.Value);

            return (int)cmd.ExecuteScalar();
        }

        // Oddiy holatda (transaction yo‘q) ham ishlashi uchun
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
            conn.Open();

            using var cmd = new SqlCommand(@"
SELECT OrderID, CustomerID, EmployeeID, OrderDate, RequiredDate, ShippedDate,
       ShipName, ShipAddress, ShipCity, ShipCountry, Freight
FROM Orders
WHERE OrderID=@id", conn);

            cmd.Parameters.AddWithValue("@id", orderId);

            using var r = cmd.ExecuteReader();
            if (!r.Read()) return null;

            return new Order
            {
                OrderId = r.GetInt32(0),
                CustomerId = r.IsDBNull(1) ? null : r.GetString(1).Trim(), // nchar(5) bo‘lishi mumkin
                EmployeeId = r.IsDBNull(2) ? null : r.GetInt32(2),
                OrderDate = r.IsDBNull(3) ? null : r.GetDateTime(3),
                RequiredDate = r.IsDBNull(4) ? null : r.GetDateTime(4),
                ShippedDate = r.IsDBNull(5) ? null : r.GetDateTime(5),
                ShipName = r.IsDBNull(6) ? null : r.GetString(6),
                ShipAddress = r.IsDBNull(7) ? null : r.GetString(7),
                ShipCity = r.IsDBNull(8) ? null : r.GetString(8),
                ShipCauntry = r.IsDBNull(9) ? null : r.GetString(9),
                Freight = r.IsDBNull(10) ? null : r.GetDecimal(10)
            };
        }
    }
}
