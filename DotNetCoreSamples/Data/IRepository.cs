using System.Collections.Generic;
using DotNetCoreSamples.Data.Entities;

namespace DotNetCoreSamples.Data
{
    public interface IRepository
    {
        IEnumerable<Product> GetAllProduct();
        IEnumerable<Product> GetProductsByCategory(string category);

        IEnumerable<Order> GetAllOrders(bool includeItems);
        IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems);

        Order GetOrderById(string username,int id);

        bool SaveChanges();

        void AddEntity(object model);
    }
}