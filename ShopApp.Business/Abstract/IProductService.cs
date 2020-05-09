using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Business.Abstract
{
    public interface IProductService:IValidator<Product>
    {
        Product GetById(int id);

        List<Product> GetAll();

        Product GetProductDetails(int id);

        List<Product> GetProductByCategory(string category,int page,int pageSize);

        bool Create(Product entity);

        void Update(Product entity);

        void Delete(Product entity);

        int GetCountByCategory(string category);

        Product GetByIdWithCategories(int id);

        void UpdateProduct(Product entity,int[] categoryIds);
    }
}
