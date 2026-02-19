using System;
using System.Collections.Generic;
using NorthwindManagement.Data.Repositories;
using NorthwindManagement.Models;

namespace NorthwindManagement.Services
{
    public class CategoryService
    {
        private readonly CategoryRepository _repo;

        public CategoryService(CategoryRepository repo)
        {
            _repo = repo;
        }

        public List<Category> GetAll()
        {
            return _repo.GetAll();
        }

        public Category? GetById(int id)
        {
            if (id <= 0) return null;
            return _repo.GetById(id);
        }

        public int Create(string name, string? description)
        {
            name = (name ?? "").Trim();
            if (name.Length == 0)
                throw new ArgumentException("Category name bo'sh bo'lishi mumkin emas.");

            var category = new Category
            {
                CategoryName = name,
                Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim()
            };

            return _repo.Insert(category);
        }

        public bool Update(int id, string name, string? description)
        {
            if (id <= 0) return false;

            name = (name ?? "").Trim();
            if (name.Length == 0)
                throw new ArgumentException("Category name bo'sh bo'lishi mumkin emas.");

            var category = new Category
            {
                CategoryId = id,
                CategoryName = name,
                Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim()
            };

            return _repo.Update(category);
        }

        public bool Delete(int id)
        {
            if (id <= 0) return false;
            return _repo.Delete(id);
        }
    }
}
