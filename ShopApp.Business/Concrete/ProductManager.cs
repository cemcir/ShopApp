using ShopApp.Business.Abstract;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Business.Concrete
{
    public class ProductManager : IProductService
    {
        private IProductDal _productDal;

        public ProductManager(IProductDal productDal)
        {
            this._productDal = productDal;
        }

        public string ErrorMessage { get; set; }

        public bool Create(Product entity)
        {
            if (Validate(entity)) {
                this._productDal.Create(entity);
                return true;
            }
            return false;
        }

        public void Delete(Product entity)
        {
            this._productDal.Delete(entity);
        }

        public List<Product> GetAll()
        {
            return _productDal.GetAll();
        }

        public Product GetById(int id)
        {
            return _productDal.GetById(id);
        }

        public Product GetByIdWithCategories(int id)
        {
            return _productDal.GetByIdWithCategories(id);
        }

        public int GetCountByCategory(string category)
        {
            return _productDal.GetCountByCategory(category);
        }

        public List<Product> GetProductByCategory(string category,int page,int pageSize)
        {
            return _productDal.GetProductsByCategory(category,page,pageSize);
        }

        public Product GetProductDetails(int id)
        {
            return _productDal.GetProductDetails(id);
        }

        public void Update(Product entity)
        {
            _productDal.Update(entity);
        }

        public void UpdateProduct(Product entity, int[] categoryIds)
        {
            _productDal.UpdateProduct(entity, categoryIds);
        }

        public bool Validate(Product entity)
        {
            var isValid = true;

            if (string.IsNullOrEmpty(entity.Name)) {
                ErrorMessage += "ürün ismi girmelisiniz";
                isValid = false;
            }
            return isValid;
        }
    }
}
