using DotNetCoreSamples.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreSamples.Data
{
    public class MyDbSeeder
    {
        private readonly MyDbContext _context;
        private readonly IHostingEnvironment _hosting;

        public MyDbSeeder(MyDbContext context, IHostingEnvironment hosting)
        {
            _context = context;
            _hosting = hosting; // runtime da proje kök klasörüne erişmek için
        }

        public void Seed()
        {
            _context.Database.EnsureCreated(); // veritabanının yaratıldığından emin olmak istiyorum

            if (!_context.Products.Any())
            {
                //product datalarını art.json dosyasından çekeceğim
                var filePath = Path.Combine(_hosting.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(filePath);

                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
                _context.Products.AddRange(products);

                var order = new Order()
                {
                    OrderDate = DateTime.Now,
                    OrderNumber = "12345",
                    Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product=products.First(),
                            Quantity= 5,
                            UnitPrice = products.First().Price
                        }
                    }
                };

                _context.Orders.Add(order);
                _context.SaveChanges();

            }
        }
    }
}
