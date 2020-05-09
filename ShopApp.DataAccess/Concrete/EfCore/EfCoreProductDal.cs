using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ShopApp.DataAccess.Concrete.EfCore
{
    public class EfCoreProductDal : EfCoreGenericRepository<Product, ShopContext>,
       IProductDal
    {
        public Product GetByIdWithCategories(int id)
        {
            using (var context=new ShopContext())
            {
                return context.Products.
                    Where(i => i.Id == id).
                    Include(i => i.ProductCategories).
                    ThenInclude(i => i.Category).
                    FirstOrDefault();
            }
        }

        public int GetCountByCategory(string category)
        {
            using (var context = new ShopContext())
            {

                var products = context.Products.AsQueryable();

                if (!string.IsNullOrEmpty(category))
                {
                    products = products.
                         Include(p => p.ProductCategories).
                         ThenInclude(p => p.Category).
                         Where(p => p.ProductCategories.Any(c => c.Category.Name.ToLower() == category.ToLower()));
                }

                return products.Count();
                
                /*
                var products = context.ProductCategories.AsQueryable();

                if (!string.IsNullOrEmpty(category)) {
                    products = products.
                        Where(p => p.Category.Name == category);
                }
                return products.Count();
                */
            }
        }

        public Product GetProductDetails(int id)
        {
            using (var context=new ShopContext())
            {
                return context.Products.
                    Where(p => p.Id == id).
                    Include(p =>p.ProductCategories).
                    ThenInclude(p=>p.Category).
                    FirstOrDefault();
            }
        }

        public List<Product> GetProductsByCategory(string category,int page,int pageSize)
        {
            using (var context=new ShopContext()) {

                var products = context.Products.AsQueryable();

                if (!string.IsNullOrEmpty(category)) {
                    products = products.
                         Include(p => p.ProductCategories).
                         ThenInclude(p => p.Category).
                         Where(p => p.ProductCategories.Any(c => c.Category.Name.ToLower() == category.ToLower()));
                }

                return products.Skip((page - 1)*pageSize).Take(pageSize).ToList();
            }
        }

        public void UpdateProduct(Product entity, int[] categoryIds)
        {
            using (var context=new ShopContext()) {
                var product = context.Products.
                    //Include(p => p.ProductCategories).
                    Where(p => p.Id == entity.Id).
                    FirstOrDefault();

                if (product != null) {
                    product.Name = entity.Name;
                    product.ImgeUrl = entity.ImgeUrl;
                    product.Price = entity.Price;
                    product.Description = entity.Description;

                    context.SaveChanges();

                    List<ProductCategory> catproduct = context.ProductCategories.Where(p => p.ProductId == entity.Id).ToList();
                    if (catproduct.Count>0) {
                        foreach(var item in catproduct)
                        context.ProductCategories.Remove(item);
                        context.SaveChanges();
                    }
                    
                    foreach (var catid in categoryIds) {
                        var productcat = new ProductCategory()
                        {
                            CategoryId=catid,
                            ProductId=entity.Id
                        };
                        context.ProductCategories.Add(productcat);
                        context.SaveChanges();
                    }
                    
                }
                
            }
            
        }
    }
}
