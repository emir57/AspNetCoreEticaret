using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shopapp.business.Abstract;

namespace shopapp.webui.ViewComponents
{
    public class CategoriesViewComponent:ViewComponent
    {
        private ICategoryService _categoryService;
        public CategoriesViewComponent(ICategoryService categoryService)
        {
            this._categoryService = categoryService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            
            if(RouteData.Values["action"]!=null)
            {
                ViewBag.selectedCategory=RouteData?.Values["category"];
            }
            var categories = await _categoryService.GetAll();
            return View(categories);
        }
    }
}