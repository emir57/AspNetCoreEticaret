using System.Collections.Generic;
using System.Threading.Tasks;
using shopapp.business.Abstract;
using shopapp.data.Abstract;
using shopapp.entity;

namespace shopapp.business.Concrete
{
    public class CategoryManager:ICategoryService
    {
        private ICategoryRepository _category;
        public CategoryManager(ICategoryRepository category)
        {
            _category = category;
        }

        

        public void Add(Category entity)
        {
            _category.Add(entity);
        }

        public void Delete(Category entity)
        {
            _category.Delete(entity);
        }

        public void DeleteFromCategory(int productId, int categoryId)
        {
            _category.DeleteFromCategory(productId,categoryId);
        }

        public async Task<List<Category>> GetAll()
        {
            return await _category.GetAll();
        }

        public async Task<Category> GetById(int id)
        {
            return await _category.GetById(id);
        }

        public Category GetByIdWithProducts(int categoryId)
        {
            return _category.GetByIdWithProducts(categoryId);
        }

        public void Update(Category entity)
        {
            _category.Update(entity);
        }

        public string ErrorMessage { get; set; }

        public bool Validation(Category entity)
        {
            throw new System.NotImplementedException();
        }
    }
}