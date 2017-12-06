using DotNetCoreSamples.Data.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DotNetCoreSamples.Data
{
    public class MyRepository : IRepository
    {
        private readonly MyDbContext _ctx;
        private readonly ILogger<MyRepository> _logger;

        public MyRepository(MyDbContext ctx, ILogger<MyRepository> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        public void AddOrder(Order newOrder)
        {
            //angular'dan gelen siparişleri gez ve ürünId nesnelerini ürünler listesinden çekip sipariş ürünü olarak ekle ve siparişi kaydet
            foreach (var item in newOrder.Items)
            {
                item.Product = _ctx.Products.Find(item.Product.Id);
            }

            AddEntity(newOrder);
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems)
        {
            if (includeItems)
            {
                return _ctx.Orders
              .Include(o => o.Items)
              .ThenInclude(i => i.Product)
              .ToList();
            }
            else
            {
                return _ctx.Orders.ToList();
            }

        }
        public IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems)
        {
            if (includeItems)
            {
                return _ctx.Orders
                    .Where(o => o.User.UserName == username)
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .ToList();
            }
            else
            {
                return _ctx.Orders.Where(o => o.User.UserName == username).ToList();
            }
        }

        public Order GetOrderById(string username, int id)
        {
            return _ctx.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.Id == id && o.User.UserName == username)
                .FirstOrDefault();
        }

        public IEnumerable<Product> GetAllProduct()
        {
            try
            {
                _logger.LogInformation("GetAllProducts metodu çalıştı");

                return _ctx.Products
                    .OrderBy(p => p.Title)
                    .ToList();
            }
            catch (Exception ex)
            {

                _logger.LogError($"GetAllProduct metodunda bir hata oluştu: {ex}");
                return null;
            }

        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return _ctx.Products
                .Where(p => p.Category == category)
                .ToList();
        }

        public bool SaveChanges()
        {
            return _ctx.SaveChanges() > 0;
        }

        public void AddEntity(object model)
        {
            _ctx.Add(model);
        }

      
    }
}
