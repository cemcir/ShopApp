using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopApp.DataAccess.Concrete.EfCore
{
    public class EfCoreCategoryDal : EfCoreGenericRepository<Category, ShopContext>, ICategoryDal
    {
        public void DeleteFromCategory(int categoryId,int productId)
        {
            using (var context=new ShopContext())
            {
                var entity = context.ProductCategories.
                    Where(p => p.CategoryId == categoryId && p.ProductId == productId).FirstOrDefault();
                context.ProductCategories.Remove(entity);
                context.SaveChanges();
                /*
                var cmd = @"delete from ProductCategories where ProductId='"+productId+"' And CategoryId='"+categoryId+"' ";

                context.ProductCategories.FromSql($"delete from ProductCategories where ProductId={productId} And CategoryId={categoryId}");
                context.SaveChanges();
                */
            }
        }

        public Category GetByCategoryName(string name) {
            using (var context=new ShopContext()) {
                return context.Categories.
                    Where(c => c.Name == name).
                    FirstOrDefault();
            }
        }

        public Category GetByIdWidthProducts(int id)
        {
            using (var context=new ShopContext())
            {
                return context.Categories.
                    Where(c => c.Id == id).
                    Include(cat => cat.ProductCategories).
                    ThenInclude(cat => cat.Product).
                    FirstOrDefault();
            }
        }

        public List<Category> GetCategoryList()
        {
            using (var context = new ShopContext())
            {
                return context.Categories.OrderBy(c => c.Name).ToList();
            }
        }
    }
}
