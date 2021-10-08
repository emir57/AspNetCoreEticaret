using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using shopapp.data.Abstract;
using shopapp.entity;

namespace shopapp.data.Concrete.EfCore
{
    public class EfCoreCategoryRepository : EfCoreGenericRepository<ShopContext, Category>, ICategoryRepository
    {
        public void DeleteFromCategory(int productId, int categoryId)
        {
            using(var context = new ShopContext())
            {
                var cmd = $"delete from ProductCategory where ProductId=@p0 and CategoryId=@p1";
                context.Database.ExecuteSqlRaw(cmd,productId,categoryId);
                
            }
        }

        public Category GetByIdWithProducts(int categoryId)
        {
            using(var context = new ShopContext())
            {
                return context.Categories
                    .Where(e=>e.CategoryId==categoryId)
                    .Include(e=>e.ProductCategories)
                    .ThenInclude(e=>e.Product)
                    .FirstOrDefault();
            }
        }
    }
}