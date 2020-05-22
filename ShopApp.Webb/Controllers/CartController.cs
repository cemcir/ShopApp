using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IyzipayCore;
using IyzipayCore.Model;
using IyzipayCore.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.Webb.Identity;
using ShopApp.Webb.Models;

namespace ShopApp.Webb.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private ICartService _cartService;
        private UserManager<ApplicationUser> _userManager;

        public CartController(ICartService cartService,UserManager<ApplicationUser> userManager) {
            _cartService = cartService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));
            return View(new CartModel() {
                CartId = cart.Id,
                CartItems =cart.CartItems.Select(i => new CartItemModel() {
                    CartItemId=i.Id,
                    ProductId=i.Product.Id,
                    Name=i.Product.Name,
                    Price=(decimal)i.Product.Price,
                    ImageUrl=i.Product.ImgeUrl,
                    Quantity=i.Quantity
                }).ToList()
            });
        }

        [HttpPost]
        public IActionResult AddToCart(CartItemModel model)
        {
            return RedirectToAction("Index","Cart");
        }

        [HttpGet]
        public IActionResult CheckOut()
        {
            var cart = _cartService.GetCartByUserId(_userManager.GetUserId(User));

            var orderModel = new OrderModel();

            orderModel.CartModel = new CartModel()
            {
                CartId = cart.Id,
                CartItems=cart.CartItems.Select(i=>new CartItemModel() {
                    CartItemId=i.Id,
                    ProductId=i.ProductId,
                    Name=i.Product.Name,
                    Price=(decimal)i.Product.Price,
                    ImageUrl=i.Product.ImgeUrl,
                    Quantity=i.Quantity
                }).ToList()
            };
            return View(orderModel);
        }

        [HttpPost]
        public IActionResult CheckOut(OrderModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var payment = PaymentProcess(model);
                var cart = _cartService.GetCartByUserId(userId);

                model.CartModel = new CartModel()
                {
                    CartId = cart.Id,
                    CartItems = cart.CartItems.Select(i=>new CartItemModel() {
                         

                    }).ToList()
                };

                if (payment.Status == "success")
                {
                    SaveOrder(model, payment, userId);
                    ClearCart(userId);
                }

            }
            return View(model);
        }

        private void ClearCart(string userId)
        {
            throw new NotImplementedException();
        }

        private void SaveOrder(OrderModel model, Payment payment, string userId)
        {
            throw new NotImplementedException();
        }

        private Payment PaymentProcess(OrderModel model)
        {
            Options options = new Options();
            options.ApiKey = "sandbox-Ps7hMcpjCUmCvvsD9GYJLfeCGO3n2Cqt";
            options.SecretKey = "sandbox-NWE5CNYhST6pDq54Wpiu0GEH3jZu9nka";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";

            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = "123456789";
            request.Price = "1";
            request.PaidPrice = "1.2";
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = "B67832";
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = "John Doe";
            paymentCard.CardNumber = "5528790000000008";
            paymentCard.ExpireMonth = "12";
            paymentCard.ExpireYear = "2030";
            paymentCard.Cvc = "123";
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            Buyer buyer = new Buyer();
            buyer.Id = "BY789";
            buyer.Name = "John";
            buyer.Surname = "Doe";
            buyer.GsmNumber = "+905350000000";
            buyer.Email = "email@email.com";
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2013-04-21 15:12:09";
            buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            buyer.Ip = "85.34.78.112";
            buyer.City = "Istanbul";
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = "Jane Doe";
            shippingAddress.City = "Istanbul";
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = "Jane Doe";
            billingAddress.City = "Istanbul";
            billingAddress.Country = "Turkey";
            billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();
            BasketItem firstBasketItem = new BasketItem();
            firstBasketItem.Id = "BI101";
            firstBasketItem.Name = "Binocular";
            firstBasketItem.Category1 = "Collectibles";
            firstBasketItem.Category2 = "Accessories";
            firstBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
            firstBasketItem.Price = "0.3";
            basketItems.Add(firstBasketItem);

            BasketItem secondBasketItem = new BasketItem();
            secondBasketItem.Id = "BI102";
            secondBasketItem.Name = "Game code";
            secondBasketItem.Category1 = "Game";
            secondBasketItem.Category2 = "Online Game Items";
            secondBasketItem.ItemType = BasketItemType.VIRTUAL.ToString();
            secondBasketItem.Price = "0.5";
            basketItems.Add(secondBasketItem);

            BasketItem thirdBasketItem = new BasketItem();
            thirdBasketItem.Id = "BI103";
            thirdBasketItem.Name = "Usb";
            thirdBasketItem.Category1 = "Electronics";
            thirdBasketItem.Category2 = "Usb / Cable";
            thirdBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
            thirdBasketItem.Price = "0.2";
            basketItems.Add(thirdBasketItem);
            request.BasketItems = basketItems;

            return Payment.Create(request, options);
            /*
            if (payment.Status == "success")
            {
                return RedirectToAction("Index", "Cart");
            }
            */
        }
    }
}