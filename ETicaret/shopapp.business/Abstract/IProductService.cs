using shopapp.entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace shopapp.business.Abstract
{
    public interface IProductService : IValidator<Product>
    {
        Task<Product> GetById(int id);
        Task<List<Product>> GetAll(Expression<Func<Product,bool>> filter=null);
        Product GetProductDetails(string url);
        List<Product> GetProductsByCategory(string name);
        
        void Add(Product entity);
        Task<Product> AddAsync(Product entity);

        List<Product> GetHomePageProducts();

        void Update(Product entity);
        Task UpdateAsync(Product entityToUpdate,Product entity);

        void Update(Product entity,int[] categoryIds);
        void Delete(Product entity);
        Task DeleteAsync(Product entity);
        int GetCountByCategory(string category);
        List<Product> GetSearchResult(string searchString);
        Product GetByIdWithCategories(int id);
    }
}