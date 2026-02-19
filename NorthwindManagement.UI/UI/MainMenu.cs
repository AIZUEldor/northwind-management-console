using System;
using NorthwindManagement.Services;

namespace NorthwindManagement.UI
{
    public class MainMenu
    {
        private readonly CategoryMenu _categoryMenu;
        private readonly ProductMenu _productMenu;
        private readonly OrderMenu _orderMenu;

        public MainMenu(CategoryService categoryService, ProductService productService, OrderService orderService)
        {
            _categoryMenu = new CategoryMenu(categoryService);
            _productMenu = new ProductMenu(productService);
            _orderMenu = new OrderMenu(orderService);
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Northwind Management Console ===");
                Console.WriteLine("1) Categories (CRUD)");
                Console.WriteLine("2) Products (CRUD)");
                Console.WriteLine("3) Orders (Create + Check)");
                Console.WriteLine("0) Exit");
                Console.Write("Tanlang: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        _categoryMenu.Run();
                        break;
                    case "2":
                        _productMenu.Run();
                        break;
                    case "3":
                        _orderMenu.Run();
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
    }
}
