using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using NorthwindManagement.Data;
using NorthwindManagement.Data.Repositories;
using NorthwindManagement.Models;

namespace NorthwindManagement.Services
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepo;
        private readonly OrderDetailRepository _detailRepo;
        private readonly ProductRepository _productRepo;

        public OrderService(
            OrderRepository orderRepo,
            OrderDetailRepository detailRepo,
            ProductRepository productRepo)
        {
            _orderRepo = orderRepo;
            _detailRepo = detailRepo;
            _productRepo = productRepo;
        }

        
        public int CreateOrderWithDetails(Order orderHeader, List<(int productId, short quantity)> items)
        {
            if (items == null || items.Count == 0)
                throw new ArgumentException("Order uchun kamida 1 ta mahsulot tanlanishi kerak.");

            
            orderHeader.OrderDate ??= DateTime.Now;

           
            var details = new List<OrderDetail>();

            foreach (var (productId, quantity) in items)
            {
                if (productId <= 0) throw new ArgumentException("ProductId xato.");
                if (quantity <= 0) throw new ArgumentException("Quantity 0 dan katta bo'lishi kerak.");

                var product = _productRepo.GetById(productId);
                if (product == null)
                    throw new ArgumentException($"Bunday product topilmadi: ID={productId}");

                var unitPrice = product.UnitPrice ?? 0m;

                details.Add(new OrderDetail
                {
                    ProductId = productId,
                    Quantity = quantity,
                    UnitPrice = unitPrice,
                    Discount = 0f
                });
            }

            
            using var conn = Db.CreateConnection();
            conn.Open();

            using var tx = conn.BeginTransaction();

            try
            {
                int newOrderId = _orderRepo.CreateOrder(orderHeader, conn, tx);
                _detailRepo.AddItems(newOrderId, details, conn, tx);

                tx.Commit();
                return newOrderId;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

     
        public OrderReceipt? GetReceipt(int orderId)
        {
            var order = _orderRepo.GetById(orderId);
            if (order == null) return null;

            var details = _detailRepo.GetByOrderId(orderId);

            
            var lines = new List<OrderReceiptLine>();
            decimal total = 0m;

            foreach (var d in details)
            {
                var p = _productRepo.GetById(d.ProductId);
                string productName = p?.ProductName ?? $"ProductID={d.ProductId}";

                decimal lineTotal = d.UnitPrice * d.Quantity * (1 - (decimal)d.Discount);
                total += lineTotal;

                lines.Add(new OrderReceiptLine
                {
                    ProductId = d.ProductId,
                    ProductName = productName,
                    UnitPrice = d.UnitPrice,
                    Quantity = d.Quantity,
                    Discount = d.Discount,
                    LineTotal = lineTotal
                });
            }

            return new OrderReceipt
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                ShipName = order.ShipName,
                ShipAddress = order.ShipAddress,
                ShipCity = order.ShipCity,
                ShipCountry = order.ShipCauntry,
                Freight = order.Freight ?? 0m,
                Lines = lines,
                SubTotal = total,
                Total = total + (order.Freight ?? 0m)
            };
        }
    }

  
    public class OrderReceipt
    {
        public int OrderId { get; set; }
        public string? CustomerId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? ShipName { get; set; }
        public string? ShipAddress { get; set; }
        public string? ShipCity { get; set; }
        public string? ShipCountry { get; set; }

        public decimal Freight { get; set; }

        public List<OrderReceiptLine> Lines { get; set; } = new();

        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
    }

    public class OrderReceiptLine
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; }
        public decimal LineTotal { get; set; }
    }
}
