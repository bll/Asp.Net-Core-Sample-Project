using System.Collections.Generic;
using DotNetCoreSamples.Data.Entities;

namespace DotNetCoreSamples.Data
{
    public interface IRepository
    {
        IEnumerable<Product> GetAllProduct();
        IEnumerable<Product> GetProductsByCategory(string category);
        bool SaveChanges();
    }
}