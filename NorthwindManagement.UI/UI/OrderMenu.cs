using System;
using System.Collections.Generic;
using NorthwindManagement.Models;
using NorthwindManagement.Services;

namespace NorthwindManagement.UI
{
    public class OrderMenu
    {
        private readonly OrderService _service;

        public OrderMenu(OrderService service)
        {
            _service = service;
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Orders Menu ===");
                Console.WriteLine("1) Create new order");
                Console.WriteLine("2) View order receipt (check)");
                Console.WriteLine("0) Back");
                Console.Write("Tanlang: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateOrder();
                        break;
                    case "2":
                        ViewReceipt();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Noto'g'ri tanlov. Enter bosing...");
                        Console.ReadLine();
                        break;
                }
            }
        }

        private void CreateOrder()
        {
            Console.Clear();
            Console.WriteLine("=== Create New Order ===");

            var order = new Order();

            Console.Write("CustomerID (5 harf, optional): ");
            order.CustomerId = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(order.CustomerId)) order.CustomerId = null;

            Console.Write("EmployeeID (optional): ");
            if (int.TryParse(Console.ReadLine(), out int empId))
                order.EmployeeId = empId;
            else
                order.EmployeeId = null;

            Console.Write("ShipName (optional): ");
            order.ShipName = EmptyToNull(Console.ReadLine());

            Console.Write("ShipAddress (optional): ");
            order.ShipAddress = EmptyToNull(Console.ReadLine());

            Console.Write("ShipCity (optional): ");
            order.ShipCity = EmptyToNull(Console.ReadLine());

            Console.Write("ShipCountry (optional): ");
            order.ShipCauntry = EmptyToNull(Console.ReadLine());

            Console.Write("Freight (optional): ");
            order.Freight = decimal.TryParse(Console.ReadLine(), out decimal freight) ? freight : (decimal?)null;

            // Items kiritish
            var items = new List<(int productId, short quantity)>();

            Console.WriteLine("\nMahsulot qo'shish (ProductId va Quantity). Tugatish uchun ProductId=0 kiriting.");

            while (true)
            {
                Console.Write("ProductId (0=finish): ");
                if (!int.TryParse(Console.ReadLine(), out int pid))
                {
                    Console.WriteLine("ProductId noto'g'ri.");
                    continue;
                }

                if (pid == 0) break;

                Console.Write("Quantity: ");
                if (!short.TryParse(Console.ReadLine(), out short qty) || qty <= 0)
                {
                    Console.WriteLine("Quantity noto'g'ri (0 dan katta bo'lsin).");
                    continue;
                }

                items.Add((pid, qty));
                Console.WriteLine("✅ Qo'shildi.");
            }

            try
            {
                int newOrderId = _service.CreateOrderWithDetails(order, items);
                Console.WriteLine($"\nOrder yaratildi ✅ OrderID={newOrderId}");

                // Darrov check ko'rsatamiz
                var receipt = _service.GetReceipt(newOrderId);
                if (receipt != null)
                {
                    PrintReceipt(receipt);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Xatolik: {ex.Message}");
            }

            Console.WriteLine("\nEnter bosing...");
            Console.ReadLine();
        }

        private void ViewReceipt()
        {
            Console.Clear();
            Console.Write("OrderID kiriting: ");
            if (!int.TryParse(Console.ReadLine(), out int orderId))
            {
                Console.WriteLine("OrderID noto'g'ri. Enter bosing...");
                Console.ReadLine();
                return;
            }

            var receipt = _service.GetReceipt(orderId);
            if (receipt == null)
            {
                Console.WriteLine("Order topilmadi. Enter bosing...");
                Console.ReadLine();
                return;
            }

            PrintReceipt(receipt);

            Console.WriteLine("\nEnter bosing...");
            Console.ReadLine();
        }

        private void PrintReceipt(OrderReceipt r)
        {
            Console.WriteLine("\n========== RECEIPT ==========");
            Console.WriteLine($"OrderID: {r.OrderId}");
            Console.WriteLine($"CustomerID: {r.CustomerId}");
            Console.WriteLine($"OrderDate: {r.OrderDate}");
            Console.WriteLine($"Ship: {r.ShipName}");
            Console.WriteLine($"Address: {r.ShipAddress}, {r.ShipCity}, {r.ShipCountry}");
            Console.WriteLine("------------------------------");

            foreach (var line in r.Lines)
            {
                Console.WriteLine($"{line.ProductId} | {line.ProductName} | {line.UnitPrice} x {line.Quantity} | Disc:{line.Discount} | Sum:{line.LineTotal}");
            }

            Console.WriteLine("------------------------------");
            Console.WriteLine($"SubTotal: {r.SubTotal}");
            Console.WriteLine($"Freight: {r.Freight}");
            Console.WriteLine($"TOTAL: {r.Total}");
            Console.WriteLine("==============================\n");
        }

        private string? EmptyToNull(string? s)
        {
            s = s?.Trim();
            return string.IsNullOrWhiteSpace(s) ? null : s;
        }
    }
}
