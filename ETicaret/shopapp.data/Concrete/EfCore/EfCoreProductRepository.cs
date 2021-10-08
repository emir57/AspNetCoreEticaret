using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using shopapp.data.Abstract;
using shopapp.entity;

namespace shopapp.data.Concrete.EfCore
{
    public class EfCoreProductRepository : EfCoreGenericRepository<ShopContext, Product>, IProductRepository
    {
        public Product GetByIdWithCategories(int id)
        {
            using (var context = new ShopContext())
            {
                return context.Products
                    .Where(a => a.ProductId == id)
                    .Include(a => a.ProductCategories)
                    .ThenInclude(a => a.Category)
                    .FirstOrDefault();
            }
        }

        public int GetCountByCategory(string category)
        {
            using (var context = new ShopContext())
            {
                var products = context.Products.AsQueryable();
                if (!string.IsNullOrEmpty(category))
                {
                    products = products
                        .Include(e => e.ProductCategories)
                        .ThenInclude(e => e.Category)
                        .Where(e => e.ProductCategories.Any(e => e.Category.Url.ToLower() == category));
                }
                return products.Count();
            }
        }

        public List<Product> GetHomePageProducts()
        {
            using (var context = new ShopContext())
            {
                return context.Products.Where(a => a.IsHome && a.IsApproved).ToList();
            }
        }

        public List<Product> GetPopularProducts()
        {
            throw new System.NotImplementedException();
        }

        public Product GetProductDetails(string url)
        {
            using (var context = new ShopContext())
            {
                return context.Products
                    .Where(e => e.Url == url)
                    .Include(e => e.ProductCategories)
                    .ThenInclude(e => e.Category)
                    .FirstOrDefault();
            }
        }

        public List<Product> GetProductsByCategory(string name)
        {
            using (var context = new ShopContext())
            {
                var products = context.Products.AsQueryable();
                if (!string.IsNullOrEmpty(name))
                {
                    products = products
                        .Where(e=>e.IsApproved==true)
                        .Include(e => e.ProductCategories)
                        .ThenInclude(e => e.Category)
                        .Where(e => e.ProductCategories.Any(e => e.Category.Url.ToLower() == name));
                }
                // return products.Skip((page-1) * pageSize).Take(pageSize).ToList();
                return products.ToList();
            }
        }

        public List<Product> GetSearchResult(string searchString)
        {
            using (var context = new ShopContext())
            {
                return context.Products
                    .Where(e => e.Name.ToLower().Contains(searchString.ToLower())).ToList();
            }
        }

        public List<Product> GetTop5Products()
        {
            using (var context = new ShopContext())
            {
                return context.Products.ToList();
            }
        }

        public void Update(Product entity, int[] categoryIds)
        {
            using (var context = new ShopContext())
            {
                var product = context.Products
                    .Include(a => a.ProductCategories)
                    .FirstOrDefault(a => a.ProductId == entity.ProductId);

                if(product!=null)
                {
                    product.Name = entity.Name;
                    product.Price = entity.Price;
                    product.Description = entity.Description;
                    product.Url = entity.Url;
                    product.ImageUrl = entity.ImageUrl;
                    product.IsApproved = entity.IsApproved;
                    product.IsHome = entity.IsHome;

                    product.ProductCategories = categoryIds.Select(catid=>new ProductCategory(){
                        ProductId = entity.ProductId,
                        CategoryId = catid
                    }).ToList();
                    context.SaveChanges();
                }


            }
        }
    }
}