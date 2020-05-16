using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.Webb.Identity;
using ShopApp.Webb.Models;

namespace ShopApp.Webb.Controllers
{
    public class CartItemController : Controller
    {
        private ICartItemService _cartItemService;
        private UserManager<ApplicationUser> _userManager;

        public CartItemController(ICartItemService cartItemService, UserManager<ApplicationUser> userManager) {
            _cartItemService = cartItemService;
            _userManager = userManager;
        }

        [HttpPost]
        public IActionResult AddToCart(CartItemModel model)
        {
            _cartItemService.AddToCartItem(_userManager.GetUserId(User), model.ProductId, model.Quantity);
            return RedirectToAction("Index","Cart");
        }
    }
}