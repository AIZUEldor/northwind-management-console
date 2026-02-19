using System;
using NorthwindManagement.Services;

namespace NorthwindManagement.UI
{
    public class ProductMenu
    {
        private readonly ProductService _service;

        public ProductMenu(ProductService service)
        {
            _service = service;
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Products Menu ===");
                Console.WriteLine("1) List products");
                Console.WriteLine("2) View product by ID");
                Console.WriteLine("3) Create product");
                Console.WriteLine("4) Update product");
                Console.WriteLine("5) Delete product");
                Console.WriteLine("0) Back");
                Console.Write("Tanlang: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ListAll();
                        break;
                    case "2":
                        ViewById();
                        break;
                    case "3":
                        Create();
                        break;
                    case "4":
                        Update();
                        break;
                    case "5":
                        Delete();
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

        private void ListAll()
        {
            Console.Clear();
            var list = _service.GetAll();

            Console.WriteLine($"Topildi: {list.Count} ta product");
            Console.WriteLine("--------------------------------------------");
            foreach (var p in list)
            {
                Console.WriteLine($"{p.ProductId}) {p.ProductName} | Price: {p.UnitPrice} | Stock: {p.UnitsInStock} | Disc: {p.Discontinued}");
            }

            Console.WriteLine("\nEnter bosing...");
            Console.ReadLine();
        }

        private void ViewById()
        {
            Console.Clear();
            Console.Write("Product ID kiriting: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID noto'g'ri. Enter bosing...");
                Console.ReadLine();
                return;
            }

            var p = _service.GetById(id);
            if (p == null)
            {
                Console.WriteLine("Topilmadi. Enter bosing...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"ID: {p.ProductId}");
            Console.WriteLine($"Name: {p.ProductName}");
            Console.WriteLine($"CategoryId: {p.CategoryId}");
            Console.WriteLine($"UnitPrice: {p.UnitPrice}");
            Console.WriteLine($"UnitsInStock: {p.UnitsInStock}");
            Console.WriteLine($"Discontinued: {p.Discontinued}");

            Console.WriteLine("\nEnter bosing...");
            Console.ReadLine();
        }

        private void Create()
        {
            Console.Clear();

            Console.Write("Product name: ");
            string? name = Console.ReadLine();

            Console.Write("CategoryId (optional): ");
            int? categoryId = int.TryParse(Console.ReadLine(), out int cid) ? cid : (int?)null;

            Console.Write("UnitPrice (optional): ");
            decimal? unitPrice = decimal.TryParse(Console.ReadLine(), out decimal price) ? price : (decimal?)null;

            Console.Write("UnitsInStock (optional): ");
            short? stock = short.TryParse(Console.ReadLine(), out short s) ? s : (short?)null;

            Console.Write("Discontinued (0/1): ");
            bool discontinued = Console.ReadLine() == "1";

            try
            {
                int newId = _service.Create(name ?? "", categoryId, unitPrice, stock, discontinued);
                Console.WriteLine($"Yangi product yaratildi. ID={newId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Xatolik: {ex.Message}");
            }

            Console.WriteLine("Enter bosing...");
            Console.ReadLine();
        }

        private void Update()
        {
            Console.Clear();
            Console.Write("Product ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID noto'g'ri. Enter bosing...");
                Console.ReadLine();
                return;
            }

            Console.Write("New product name: ");
            string? name = Console.ReadLine();

            Console.Write("New CategoryId (optional): ");
            int? categoryId = int.TryParse(Console.ReadLine(), out int cid) ? cid : (int?)null;

            Console.Write("New UnitPrice (optional): ");
            decimal? unitPrice = decimal.TryParse(Console.ReadLine(), out decimal price) ? price : (decimal?)null;

            Console.Write("New UnitsInStock (optional): ");
            short? stock = short.TryParse(Console.ReadLine(), out short s) ? s : (short?)null;

            Console.Write("Discontinued (0/1): ");
            bool discontinued = Console.ReadLine() == "1";

            try
            {
                bool ok = _service.Update(id, name ?? "", categoryId, unitPrice, stock, discontinued);
                Console.WriteLine(ok ? "Yangilandi ✅" : "Yangilanmadi (ID topilmadi) ❗");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Xatolik: {ex.Message}");
            }

            Console.WriteLine("Enter bosing...");
            Console.ReadLine();
        }

        private void Delete()
        {
            Console.Clear();
            Console.Write("Product ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID noto'g'ri. Enter bosing...");
                Console.ReadLine();
                return;
            }

            bool ok = _service.Delete(id);
            Console.WriteLine(ok ? "O'chirildi ✅" : "O'chirilmadi (ID topilmadi) ❗");

            Console.WriteLine("Enter bosing...");
            Console.ReadLine();
        }
    }
}
