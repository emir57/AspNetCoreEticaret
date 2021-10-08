using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using shopapp.business.Abstract;
using shopapp.entity;
using shopapp.webui.Extensions;
using shopapp.webui.Identity;
using shopapp.webui.Models;
using shopapp.webui.ViewModels;

namespace shopapp.webui.Controllers
{
    // emir,efe,yiğit => admin
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private IProductService _productService;
        private ICategoryService _categoryService;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;
        public AdminController(IProductService productService, ICategoryService categoryService,RoleManager<IdentityRole> roleManager,UserManager<User> userManager)
        {
            this._productService = productService;
            this._categoryService = categoryService;
            this._roleManager = roleManager;
            this._userManager = userManager;
        }
        public async Task<IActionResult> UserEdit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user!=null)
            {
                var selectedRoles = await _userManager.GetRolesAsync(user);
                var roles = _roleManager.Roles.Select(i=>i.Name);

                ViewBag.Roles = roles;
                return View(new UserDetailsModel(){
                    UserId = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    SelectedRoles = selectedRoles
                });
            }
            return Redirect("admin/user/list");
        }
        
        
        public async Task<IActionResult> UserEdit(UserDetailsModel model, string[] selectedRoles)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if(user!=null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;
                    user.EmailConfirmed = model.EmailConfirmed;

                    var result = await _userManager.UpdateAsync(user);
                    if(result.Succeeded)
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);
                        selectedRoles = selectedRoles?? new string[]{};
                        await _userManager.AddToRolesAsync(user,selectedRoles.Except(userRoles).ToArray<string>());
                        await _userManager.RemoveFromRolesAsync(user,userRoles.Except(selectedRoles).ToArray<string>());
                        return Redirect("/admin/user/list");
                    
                    }
                }
            }
            return View(model);
        }

        public IActionResult UserList()
        {
            return View(_userManager.Users);
        }

        public async Task<IActionResult> RoleEdit(string RoleId)
        {
            var role = await _roleManager.FindByIdAsync(RoleId);
            var members = new List<User>();
            var nonMembers = new List<User>();

            foreach (var user in _userManager.Users.ToList())
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name)
                    ?members:nonMembers;
                list.Add(user);
            }
            var model = new RoleDetails()
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> RoleEdit(RoleEditModel model)
        {
            if(ModelState.IsValid)
            {
                foreach (var userId in model.IdsToadd ?? new string[]{})
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if(user!=null)
                    {
                        var result = await _userManager.AddToRoleAsync(user,model.RoleName);
                        if(!result.Succeeded)
                        {   
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("",error.Description);
                            }
                        }
                    }
                }
            
                foreach (var userId in model.IdsToDelete ?? new string[]{})
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if(user!=null)
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user,model.RoleName);
                        if(!result.Succeeded)
                        {   
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("",error.Description);
                            }
                        }
                    }
                }
            
                return Redirect("/admin/role/"+model.RoleId);
            }
            return View();
        }
        public IActionResult RoleList()
        {
            return View(_roleManager.Roles);
        }
        public IActionResult RoleCreate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleModel model)
        {
            if(ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(model.Name));
                if(result.Succeeded)
                {
                    return RedirectToAction("RoleList");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("",error.Description);
                    }
                }
            }
            return View(model);
        }

        public async Task<IActionResult> ProductList()
        {
            var products = await _productService.GetAll();
            return View(new ProductListViewModel()
            {
                Products = products
            });
        }
        public async Task<IActionResult> CategoryList()
        {
            var categories = await _categoryService.GetAll();
            return View(new CategoryListViewModel()
            {
                Categories = categories
            });
        }
        [HttpGet]
        public IActionResult ProductCreate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ProductCreate(ProductModel p)
        {
            if (ModelState.IsValid)
            {
                var entity = new Product()
                {
                    Name = p.Name,
                    Url = p.Url,
                    Price = p.Price,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl
                };
                _productService.Add(entity);
                TempData.Put("message", new AlertMessage()
                {
                    Title = "Ürün Ekleme işlemi.",
                    Message = $"{entity.Name} isimli ürün eklendi.",
                    AlertType = "success"
                });
                return RedirectToAction("ProductList");
            }

            return View(p);

        }
        [HttpGet]
        public IActionResult CategoryCreate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CategoryCreate(CategoryModel p)
        {
            if (ModelState.IsValid)
            {
                var entity = new Category()
                {
                    Name = p.Name,
                    Url = p.Url

                };
                _categoryService.Add(entity);
                TempData.Put("message", new AlertMessage()
                {
                    Title = "Kategori Ekleme işlemi.",
                    Message = $"{entity.Name} isimli kategori eklendi.",
                    AlertType = "success"
                });

                return RedirectToAction("CategoryList");
            }
            return View();

        }
        [HttpGet]
        public IActionResult CategoryEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var entity = _categoryService.GetByIdWithProducts((int)id);
            if (entity == null)
            {
                return NotFound();
            }
            var model = new CategoryModel()
            {
                CategoryId = entity.CategoryId,
                Name = entity.Name,
                Url = entity.Url,
                Products = entity.ProductCategories.Select(a => a.Product).ToList()
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CategoryEdit(CategoryModel p)
        {
            if (ModelState.IsValid)
            {
                var entity = await _categoryService.GetById(p.CategoryId);
                if (entity == null)
                {
                    return NotFound();
                }
                entity.Name = p.Name;
                entity.Url = p.Url;
                _categoryService.Update(entity);

                TempData.Put("message", new AlertMessage()
                {
                    Title = "Kategori Düzenleme işlemi.",
                    Message = $"{entity.Name} isimli kategori güncellendi.",
                    AlertType = "success"
                });

                return RedirectToAction("CategoryList");
            }
            return View(p);

        }
        [HttpPost]
        public async Task<IActionResult> CategoryDelete(int id)
        {
            var entity = await _categoryService.GetById(id);
            if (entity == null)
            {
                return NotFound();
            }
            _categoryService.Delete(entity);
            TempData.Put("message", new AlertMessage()
            {
                Title = "Kategori Silme işlemi.",
                Message = $"{entity.Name} isimli kategori silindi.",
                AlertType = "danger"
            });


            return RedirectToAction("CategoryList");
        }
        [HttpGet]
        public IActionResult ProductEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var entity = _productService.GetByIdWithCategories((int)id);
            if (entity == null)
            {
                return NotFound();
            }
            var model = new ProductModel()
            {
                ProductId = entity.ProductId,
                Name = entity.Name,
                Url = entity.Url,
                Price = entity.Price,
                ImageUrl = entity.ImageUrl,
                Description = entity.Description,
                IsApproved = entity.IsApproved,
                IsHome = entity.IsHome,
                SelectedCategories = entity.ProductCategories.Select(a => a.Category).ToList()
            };
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductModel p, int[] categoryIds, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var entity = await _productService.GetById(p.ProductId);
                if (entity == null)
                {
                    return NotFound();
                }
                entity.Name = p.Name;
                entity.Url = p.Url;
                entity.Price = p.Price;
                entity.Description = p.Description;
                entity.IsApproved = p.IsApproved;
                entity.IsHome = p.IsHome;
                if (file != null)
                {
                    var extension = Path.GetExtension(file.FileName);
                    var randomName = string.Format($"{Guid.NewGuid()}{extension}");
                    entity.ImageUrl = randomName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/", randomName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                _productService.Update(entity, categoryIds);

                TempData.Put("message", new AlertMessage()
                {
                    Title = "Ürün Düzenleme işlemi.",
                    Message = $"{entity.Name} isimli ürün güncellendi.",
                    AlertType = "success"
                });


                return RedirectToAction("ProductList");
            }
            ViewBag.Categories = _categoryService.GetAll();
            return View(p);

        }
        [HttpPost]
        public async Task<IActionResult> ProductDelete(int id)
        {
            var entity = await _productService.GetById(id);
            if (entity == null)
            {
                return NotFound();
            }
            _productService.Delete(entity);
            TempData.Put("message", new AlertMessage()
            {
                Title = "Ürün Silme işlemi.",
                Message = $"{entity.Name} isimli ürün silindi.",
                AlertType = "danger"
            });


            return RedirectToAction("ProductList");
        }
        [HttpPost]
        public IActionResult DeleteFromCategory(int productId, int categoryId)
        {
            _categoryService.DeleteFromCategory(productId, categoryId);
            return Redirect("/admin/categories/" + categoryId);
        }
    }
}