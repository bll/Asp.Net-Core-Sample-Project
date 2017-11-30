using System.Collections.Generic;
using DotNetCoreSamples.Data.Entities;

namespace DotNetCoreSamples.Data
{
    public interface IRepository
    {
        IEnumerable<Product> GetAllProduct();
        IEnumerable<Product> GetProductsByCategory(string category);

        IEnumerable<Order> GetAllOrders();
        Order GetOrderById(int id);

        bool SaveChanges();

        void AddEntity(object model);
    }
}