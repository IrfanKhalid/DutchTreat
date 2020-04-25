using DutchTreat.Data.Entities;
using System.Collections.Generic;

namespace DutchTreat.Data
{
    public interface IDutchRepository
    {
        IEnumerable<Product> GetProduct();
        IEnumerable<Product> GetProductByCategory(string categoryName);

        IEnumerable<Order> GetAllOrder();
        IEnumerable<Order> GetAllOrderByUserName(string Username);
        
        Order GetOrderById(int id);

        bool saveAll();
        void AddModel(object data);
    }
}