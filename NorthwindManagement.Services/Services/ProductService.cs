using System;
using System.Collections.Generic;
using NorthwindManagement.Data.Repositories;
using NorthwindManagement.Models;

namespace NorthwindManagement.Services
{
    public class ProductService
    {
        private readonly ProductRepository _repo;

        public ProductService(ProductRepository repo)
        {
            _repo = repo;
        }

        public List<Product> GetAll()
        {
            return _repo.GetAll();
        }

        public Product? GetById(int id)
        {
            if (id <= 0) return null;
            return _repo.GetById(id);
        }

        public int Create(string name, int? categoryId, decimal? unitPrice, short? unitsInStock, bool discontinued)
        {
            name = (name ?? "").Trim();
            if (name.Length == 0)
                throw new ArgumentException("Product name bo'sh bo'lishi mumkin emas.");

            if (categoryId.HasValue && categoryId.Value <= 0) categoryId = null;
            if (unitPrice.HasValue && unitPrice.Value < 0) throw new ArgumentException("UnitPrice manfiy bo'lmaydi.");
            if (unitsInStock.HasValue && unitsInStock.Value < 0) throw new ArgumentException("UnitsInStock manfiy bo'lmaydi.");

            var product = new Product
            {
                ProductName = name,
                CategoryId = categoryId,
                UnitPrice = unitPrice,
                UnitsInStock = unitsInStock,
                Discontinued = discontinued
            };

            return _repo.Insert(product);
        }

        public bool Update(int id, string name, int? categoryId, decimal? unitPrice, short? unitsInStock, bool discontinued)
        {
            if (id <= 0) return false;

            name = (name ?? "").Trim();
            if (name.Length == 0)
                throw new ArgumentException("Product name bo'sh bo'lishi mumkin emas.");

            if (categoryId.HasValue && categoryId.Value <= 0) categoryId = null;
            if (unitPrice.HasValue && unitPrice.Value < 0) throw new ArgumentException("UnitPrice manfiy bo'lmaydi.");
            if (unitsInStock.HasValue && unitsInStock.Value < 0) throw new ArgumentException("UnitsInStock manfiy bo'lmaydi.");

            var product = new Product
            {
                ProductId = id,
                ProductName = name,
                CategoryId = categoryId,
                UnitPrice = unitPrice,
                UnitsInStock = unitsInStock,
                Discontinued = discontinued
            };

            return _repo.Update(product);
        }

        public bool Delete(int id)
        {
            if (id <= 0) return false;
            return _repo.Delete(id);
        }
    }
}
