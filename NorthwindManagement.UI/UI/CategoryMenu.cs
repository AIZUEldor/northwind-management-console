using System;
using NorthwindManagement.Services;

namespace NorthwindManagement.UI
{
    public class CategoryMenu
    {
        private readonly CategoryService _service;

        public CategoryMenu(CategoryService service)
        {
            _service = service;
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Categories Menu ===");
                Console.WriteLine("1) List categories");
                Console.WriteLine("2) View category by ID");
                Console.WriteLine("3) Create category");
                Console.WriteLine("4) Update category");
                Console.WriteLine("5) Delete category");
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

            Console.WriteLine($"Topildi: {list.Count} ta category");
            Console.WriteLine("------------------------------");
            foreach (var c in list)
            {
                Console.WriteLine($"{c.CategoryId}) {c.CategoryName}");
            }

            Console.WriteLine("\nEnter bosing...");
            Console.ReadLine();
        }

        private void ViewById()
        {
            Console.Clear();
            Console.Write("Category ID kiriting: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID noto'g'ri. Enter bosing...");
                Console.ReadLine();
                return;
            }

            var c = _service.GetById(id);
            if (c == null)
            {
                Console.WriteLine("Topilmadi. Enter bosing...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"ID: {c.CategoryId}");
            Console.WriteLine($"Name: {c.CategoryName}");
            Console.WriteLine($"Description: {c.Description}");

            Console.WriteLine("\nEnter bosing...");
            Console.ReadLine();
        }

        private void Create()
        {
            Console.Clear();
            Console.Write("Category name: ");
            string? name = Console.ReadLine();

            Console.Write("Description (optional): ");
            string? desc = Console.ReadLine();

            try
            {
                int newId = _service.Create(name ?? "", desc);
                Console.WriteLine($"Yangi category yaratildi. ID={newId}");
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
            Console.Write("Category ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID noto'g'ri. Enter bosing...");
                Console.ReadLine();
                return;
            }

            Console.Write("New category name: ");
            string? name = Console.ReadLine();

            Console.Write("New description (optional): ");
            string? desc = Console.ReadLine();

            try
            {
                bool ok = _service.Update(id, name ?? "", desc);
                Console.WriteLine(ok ? "Yangilandi " : "Yangilanmadi (ID topilmadi) ❗");
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
            Console.Write("Category ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID noto'g'ri. Enter bosing...");
                Console.ReadLine();
                return;
            }

            bool ok = _service.Delete(id);
            Console.WriteLine(ok ? "O'chirildi " : "O'chirilmadi (ID topilmadi) ❗");

            Console.WriteLine("Enter bosing...");
            Console.ReadLine();
        }
    }
}
