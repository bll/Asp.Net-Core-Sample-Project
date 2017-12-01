using DotNetCoreSamples.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DotNetCoreSamples.Data
{
    public class MyDbSeeder
    {
        private readonly MyDbContext _context;
        private readonly IHostingEnvironment _hosting;
        private readonly UserManager<StoreUser> _userManager;

        public MyDbSeeder(MyDbContext context, IHostingEnvironment hosting,
            UserManager<StoreUser> userManager)
        {
            _context = context;
            _hosting = hosting; // runtime da proje kök klasörüne erişmek için
            _userManager = userManager;
        }

        public async Task Seed()
        {
            _context.Database.EnsureCreated(); // veritabanının yaratıldığından emin olmak istiyorum

            var user = await _userManager.FindByEmailAsync("bilal@bilal.com");

            if (user == null)
            {
                user = new StoreUser()
                {
                    FirstName = "Bilal",
                    LastName = "Yanık",
                    UserName = "bll",
                    Email = "bilal@bilal.com"
                };

                var result = await _userManager.CreateAsync(user, "P@ssw0rd!");

                if (result != IdentityResult.Success)
                {
                   throw new InvalidOperationException("Default user oluşturulamadı");
                }
            }
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
                    User = user,
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
