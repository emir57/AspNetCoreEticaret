using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using shopapp.business.Abstract;
using shopapp.data.Abstract;
using shopapp.entity;
using shopapp.webui.ViewModels;

namespace shopapp.webui.Controllers
{
    public class HomeController : Controller
    {
        private IProductService _productService;
        public HomeController(IProductService productRepository)
        {
            this._productService = productRepository;
        }
        public IActionResult Index()
        {
            var model = new ProductListViewModel()
            {
                Products = _productService.GetHomePageProducts()
            };
            return View(model);
        }
        public async Task<IActionResult> GetProductFromRestApi()
        {
            var products = new List<Product>();
            using(var http = new HttpClient())
            {
                using(var response = await http.GetAsync("https://localhost:4200/api/product"))
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    products = JsonConvert.DeserializeObject<List<Product>>(apiResponse);
                }
            }
            return View(products);
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        
    }
}