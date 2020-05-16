using ShopApp.Business.Abstract;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Business.Concrete
{
    public class CartItemManager : ICartItemService
    {
        private ICartItemDal _cartItemDal;
        private ICartDal _cartDal;

        public CartItemManager(ICartItemDal cartItemDal,ICartDal cartDal)
        {
            _cartItemDal = cartItemDal;
            _cartDal = cartDal;
        }

        public void AddToCartItem(string userId, int productId, int quantity)
        {
            var cart = _cartDal.GetByUserId(userId);
            if (cart != null) {
                var cartItem = _cartItemDal.GetCartItemById(productId);
                if (cartItem == null) {
                    _cartItemDal.Create(new CartItem() {
                        ProductId=productId,
                        CartId=cart.Id,
                        Quantity=quantity
                    });
                }
                else {
                    var cItem = cartItem;
                    cItem.ProductId = productId;
                    cItem.Quantity = cItem.Quantity + quantity;
                    cItem.CartId = cart.Id;
                    _cartItemDal.Update(cItem);
                }
            }
        }
    }
}
