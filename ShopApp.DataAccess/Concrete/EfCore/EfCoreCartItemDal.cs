using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopApp.DataAccess.Concrete.EfCore
{
    public class EfCoreCartItemDal : EfCoreGenericRepository<CartItem, ShopContext>, ICartItemDal
    {
        public CartItem GetCartItemById(int productId)
        {
            using (var context = new ShopContext())
            {
                return context.CartItems.Where(x => x.ProductId == productId).FirstOrDefault();
            }
        }
    }
}
