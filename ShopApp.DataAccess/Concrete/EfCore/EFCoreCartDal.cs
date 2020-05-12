using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopApp.DataAccess.Concrete.EfCore
{
    public class EFCoreCartDal : EfCoreGenericRepository<Cart, ShopContext>, ICartDal
    {
        public Cart GetByUserId(string userId)
        {
            using (var context = new ShopContext())
            {
                return context.Carts.
                    Include(c => c.CartItems).
                    ThenInclude(c=>c.Product).
                    FirstOrDefault(c=>c.UserId==userId);
            }
        }
    }
}
