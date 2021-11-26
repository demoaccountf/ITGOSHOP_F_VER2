﻿using ITGoShop_F_Ver2.Controllers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGoShop_F_Ver2.Models
{
    public class ITGoShopLINQContext : DbContext
    {
        private const string connectionString = "server=localhost;port=3307;database=itgoshop;uid=root;password=";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySQL(connectionString);
        }

        public DbSet<User> User { set; get; }   // Bảng User trong DataBase, <User> tên lớp
        public DbSet<Product> Product { set; get; }
        public DbSet<Brand> Brand { set; get; }
        public DbSet<Category> Category { set; get; }
        public void saveProduct(Product newProduct)
        {
            newProduct.StartsAt = DateTime.Now;
            newProduct.CreatedAt = DateTime.Now;
            newProduct.UpdatedAt = DateTime.Now;
            this.Product.Add(newProduct);
            this.SaveChanges();
        }

        //public void unactiveProduct(int productId)
        //{
        //    System.Diagnostics.Debug.WriteLine("PID: " + productId);
        //    var product = Product.Where(p => p.ProductId == productId).FirstOrDefault();

        //    product.Status = 0;
        //    this.SaveChanges();
        //}

        public void updateProduct(Product productInfo)
        {
            var product = Product.Where(p => p.ProductId == productInfo.ProductId).FirstOrDefault();
            product.ProductName = productInfo.ProductName;
            product.Quantity = productInfo.Quantity;
            product.Cost = productInfo.Cost;
            product.Price = productInfo.Price;
            product.Content = productInfo.Content;
            product.Discount= productInfo.Discount;
            product.UpdatedAt = DateTime.Now;
            product.Status = productInfo.Status;
            product.BrandId = productInfo.BrandId;
            product.SliderId = productInfo.SliderId;
            product.CategoryId = productInfo.CategoryId;
            product.SubBrandId = productInfo.SubBrandId;
            if(!string.IsNullOrEmpty(productInfo.ProductImage))
            {
                product.ProductImage = productInfo.ProductImage;
                // Chưa làm xóa ảnh cũ ở đây
            }    
            this.SaveChanges();
        }
        public void deleteProduct(int productId)
        {
            var product = (from p in Product
                           where (p.ProductId == productId)
                           select p).FirstOrDefault();

            if (product != null)
            {
                Remove(product);
                SaveChanges();
            }
        }

        //public IQueryable getAllProduct()
        //{
        //    var products = from p in Product
        //                   join b in Brand on p.BrandId equals b.BrandId
        //                   join c in Category on p.CategoryId equals c.CategoryId
        //                   select new
        //                   {
        //                       ProductId = p.ProductId,
        //                       ProductName = p.ProductName
        //                   };
        //    foreach (var item in products)
        //    {
        //        System.Diagnostics.Debug.WriteLine(item.ProductName);
        //    }
        //    return products;
        //}
    }
}
