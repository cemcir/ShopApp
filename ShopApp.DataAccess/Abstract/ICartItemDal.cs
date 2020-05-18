using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ShopApp.DataAccess.Abstract
{
    public interface ICartItemDal : IRepository<CartItem>
    {
        CartItem GetCartItemById(int productId);

        CartItem GetCartItemByItemId(int cartItemId);
    }
}
