using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Business.Abstract
{
    public interface ICategoryService
    {
        Category GetById(int id);

        Category GetByCategoryName(string name);

        Category GetByIdWithProducts(int id);

        List<Category> GetAll();

        List<Category> GetCategoryList();

        void Create(Category entity);

        void Update(Category entity);

        void Delete(Category entity);

        void DeleteFromCategory(int categoryId,int productId);
    }
}
