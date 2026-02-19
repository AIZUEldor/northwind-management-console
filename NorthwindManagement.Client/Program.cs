using System;
using NorthwindManagement.Data.Repositories;
using NorthwindManagement.Services;
using NorthwindManagement.UI;

namespace NorthwindManagement.Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // 1) Repositories (Data layer)
                var categoryRepo = new CategoryRepository();
                var productRepo = new ProductRepository();
                var orderRepo = new OrderRepository();
                var orderDetailRepo = new OrderDetailRepository();

                // 2) Services (Business logic layer)
                var categoryService = new CategoryService(categoryRepo);
                var productService = new ProductService(productRepo);
                var orderService = new OrderService(orderRepo, orderDetailRepo, productRepo);

                // 3) UI (Console menus)
                var mainMenu = new MainMenu(categoryService, productService, orderService);

                // Run application
                mainMenu.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Kutilmagan xatolik yuz berdi:");
                Console.WriteLine(ex.Message);

                Console.WriteLine("\nEnter bosing...");
                Console.ReadLine();
            }
        }
    }
}
