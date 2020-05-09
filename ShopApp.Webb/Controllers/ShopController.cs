using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.Entities;
using ShopApp.Webb.Models;

namespace ShopApp.WebUI.Controllers
{
    public class ShopController : Controller
    {
        private IProductService _productService;
        private ICategoryService _categoryService;

        public ShopController(IProductService productService,ICategoryService categoryService) {
            _productService = productService;
            _categoryService = categoryService;
        }

        public IActionResult Details(int? id) {
            if (id == null) {
                return NotFound();
            }
            Product product = _productService.GetProductDetails((int)id);
            if (product == null) {
                return NotFound();
            }
            return View(new ProductDetailsModel() {
                Product = product,
                Categories = product.ProductCategories.Select(p => p.Category).ToList()
            });
        }

        //products/telefon?page=2
        public IActionResult List(string category,int page = 1) {

            const int pageSize = 3;
            
            var cat = _categoryService.GetByCategoryName(category);
            
            var product = _productService.GetProductByCategory(category,page,pageSize);

            if (product.Count > 0)
            {
                return View(new ProductListModel()
                {
                    PageInfo = new PageInfo()
                    {
                       TotalItems = _productService.GetCountByCategory(category),
                       CurrentPage = page,
                       ItemsPerPage = pageSize,
                       CurrentCategory = category
                    },
                    Products = product
                });
            }
            else if (cat == null) {
                return NotFound();
            }
            else if (product.Count==0) {
                return View(new ProductListModel() {
                    Products=null
                });
            }

            return NotFound();
        }

    }
}