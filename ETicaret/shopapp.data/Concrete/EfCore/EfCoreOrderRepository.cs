using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using shopapp.data.Abstract;
using shopapp.entity;

namespace shopapp.data.Concrete.EfCore
{
    public class EfCoreOrderRepository : EfCoreGenericRepository<ShopContext, Order>, IOrderRepository
    {
        public List<Order> GetOrders(string userId)
        {
            using(var context = new ShopContext())
            {
                var orders = context.Orders
                        .Include(a=>a.OrderItems)
                        .ThenInclude(a=>a.Product)
                        .AsQueryable();
                if(!string.IsNullOrEmpty(userId))
                {
                    orders = orders.Where(a=>a.UserId == userId);
                }
                return orders.ToList();
            }
        }
    }
}