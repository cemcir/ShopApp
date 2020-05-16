using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Business.Abstract
{
    public interface ICartItemService
    {
        void AddToCartItem(string userId, int productId, int quantity);
    }
}
