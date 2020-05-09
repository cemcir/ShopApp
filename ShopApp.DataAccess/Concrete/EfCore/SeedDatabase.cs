using Microsoft.EntityFrameworkCore;
using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopApp.DataAccess.Concrete.EfCore
{
    public static class SeedDatabase
    {
        public static void Seed()
        {
            var context = new ShopContext();

            if (context.Database.GetPendingMigrations().Count() == 0)
            {
                if (context.Categories.Count() == 0)
                {
                    context.Categories.AddRange(Categories);
                }

                if (context.Products.Count() == 0)
                {
                    context.Products.AddRange(Products);
                    context.ProductCategories.AddRange(ProductCategory);
                }
                context.SaveChanges();
            }
        }

        private static Category[] Categories = {
            new Category(){ Name="Telefon"},
            new Category(){ Name="Bilgisayar"},
            new Category(){ Name="Elektronik"}
        };

        private static Product[] Products = {
            new Product{Name="Samsung S5",Price=2000 ,ImgeUrl="1.jpeg",Description=
                "<p>Güzel Telefon</p>"},
            new Product{Name="Samsung S6",Price=3000 ,ImgeUrl="2.jpeg",Description=
                "<p>Güzel Telefon</p>"},
            new Product{Name="Samsung S7",Price=4000 ,ImgeUrl="3.jpeg",Description=
                "<p>Güzel Telefon</p>"},
            new Product{Name="Samsung S8",Price=5000 ,ImgeUrl="4.jpeg",Description=
                "<p>Güzel Telefon</p>"},
            new Product{Name="Samsung S9",Price=5500 ,ImgeUrl="5.jpeg",Description=
                "<p>Güzel Telefon</p>"},
            new Product{Name="Iphone 6",Price=3500 ,ImgeUrl="6.jpeg",Description=
                "<p>Güzel Telefon</p>"},
            new Product{Name="Iphone 7",Price=4200 ,ImgeUrl="7.jpeg",Description=
                "<p>Güzel Telefon</p>"},
        };

        private static ProductCategory[] ProductCategory =
        {
            new ProductCategory() {Product=Products[0],Category=Categories[0]},
            new ProductCategory() {Product=Products[0],Category=Categories[2]},
            new ProductCategory() {Product=Products[1],Category=Categories[0]},
            new ProductCategory() {Product=Products[1],Category=Categories[1]},
            new ProductCategory() {Product=Products[2],Category=Categories[0]},
            new ProductCategory() {Product=Products[2],Category=Categories[2]},
            new ProductCategory() {Product=Products[3],Category=Categories[1]},
        };
    }
}
