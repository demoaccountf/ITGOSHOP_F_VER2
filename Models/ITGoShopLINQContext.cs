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

        public void SaveProduct(Product newProduct)
        {
            newProduct.StartsAt = DateTime.Now;
            newProduct.CreatedAt = DateTime.Now;
            newProduct.UpdatedAt = DateTime.Now;
            this.Product.Add(newProduct);
            this.SaveChanges();
        }
    }
}
