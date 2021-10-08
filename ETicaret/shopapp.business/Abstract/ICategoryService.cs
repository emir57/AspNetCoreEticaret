using System.Collections.Generic;
using System.Threading.Tasks;
using shopapp.entity;

namespace shopapp.business.Abstract
{
    public interface ICategoryService : IValidator<Category>
    {
        Task<Category> GetById(int id);
        Task<List<Category>> GetAll();
        void Add(Category entity);
        void Update(Category entity);
        void Delete(Category entity);
        Category GetByIdWithProducts(int categoryId);
        void DeleteFromCategory(int productId,int categoryId);
    }
}