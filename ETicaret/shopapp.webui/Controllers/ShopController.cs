using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using shopapp.business.Abstract;
using shopapp.entity;
using shopapp.webui.Models;
using shopapp.webui.ViewModels;
using X.PagedList;

namespace shopapp.webui.Controllers
{
    public class ShopController:Controller
    {
        private IProductService _productService;
        public ShopController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult List(string category, int page=1)
        {
            const int pagePerItem = 4;
            double count = _productService.GetCountByCategory(category);
            var productListViewModel = new ProductListViewModel
            {
                Products = _productService.GetProductsByCategory(category).ToPagedList(page,pagePerItem).ToList(),
                CurrentPage = page,
                TotalPage = (int)Math.Ceiling(count/pagePerItem),
                CurrentCategory = category
            };
            
            return View(productListViewModel);
        }
        public IActionResult Details(string url)
        {
            if(url==null)
            {
                return NotFound();
            }
            Product product = _productService.GetProductDetails(url);
            if(product==null)
            {
                return NotFound();
            }
            return View(new ProductDetailModel(){
                Product=product,
                Categories = product.ProductCategories.Select(e=>e.Category).ToList()
            });
        }
        public IActionResult Search(string q )
        {
            var productListViewModel = new ProductListViewModel
            {
                Products = _productService.GetSearchResult(q)
            };
            return View(productListViewModel);
        }
    }
}