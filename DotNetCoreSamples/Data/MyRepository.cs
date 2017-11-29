using DotNetCoreSamples.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreSamples.Data
{
    public class MyRepository : IRepository
    {
        private readonly MyDbContext _Ctx;

        public MyRepository(MyDbContext ctx)
        {
            _Ctx = ctx;
        }

        public IEnumerable<Product> GetAllProduct()
        {
            return _Ctx.Products
                .OrderBy(p => p.Title)
                .ToList();
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return _Ctx.Products
                .Where(p => p.Category == category)
                .ToList();
        }

        public bool SaveChanges()
        {
            return _Ctx.SaveChanges() > 0;
        }
    }
}
