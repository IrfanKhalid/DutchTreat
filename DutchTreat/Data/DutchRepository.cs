using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext context;
        private readonly ILogger<DutchRepository> logger;

        public DutchRepository(DutchContext context, ILogger<DutchRepository> logger )
        {
            this.context = context;
            this.logger = logger;
        }

        public void AddModel(object data)
        {
            context.Add(data);
        }

        public IEnumerable<Order> GetAllOrder()
        {
            try
            {
                return context.Orders.
                    Include(y=>y.Items)
                    .ThenInclude(x=>x.Product)
                    .OrderBy(x => x.Id)
                    .ToList();
            }
            catch(Exception ex)
            {
                logger.LogError($"It's failed due to {ex}");
                return null;
            }
        }

        public Order GetOrderById(int OrderId)
        {
            try {
                return context.Orders
                    .Where(x => x.Id == OrderId)
                    .Include(x => x.Items)
                    .ThenInclude(y => y.Product)
                    .FirstOrDefault();
                    ;
                }
            catch (Exception ex)
            {
                logger.LogError($"Its failed due to {ex}");
                return null;
            }
        }

        public IEnumerable<Product> GetProduct()
        {
            try
            {
               // throw new InvalidOperationException("Invalid exception, Go Get Business");
                logger.LogInformation("Get Product is calle");
                return context.Products
                    .OrderBy(x => x.Title)
                    .ToList();

            }
            catch (Exception ex)
            {
                logger.LogError($"Exception is raised at {ex}");
                return null;
            }
        }

        public IEnumerable<Product> GetProductByCategory(string categoryName)
        {
            return context.Products
                .Where(x => x.Category.Contains(categoryName))
                .ToList();
        }

        public bool saveAll()
        {
            if (context.SaveChanges()>0)
                return true;
            else
                return false;
        }
    }
}
