using ITGoShop_F_Ver2.Controllers;
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
        public DbSet<BannerSlider> BannerSlider { set; get; }
        public DbSet<Campaign> Campaign { set; get; }
        public DbSet<Order> Order { set; get; }
        public DbSet<LoginHistory> LoginHistory { set; get; }
        public DbSet<ShipMethod> ShipMethod { set; get; }
        public DbSet<ProductGallary> ProductGallary { set; get; }
        public DbSet<Comment> Comment { set; get; }
        public void saveProduct(Product newProduct)
        {
            newProduct.StartsAt = DateTime.Now;
            newProduct.CreatedAt = DateTime.Now;
            newProduct.UpdatedAt = DateTime.Now;
            Product.Add(newProduct);
            SaveChanges();
        }

        public void saveShipMethod(ShipMethod newShipMethod)
        {
            //newShipMethod.CreatedAt = DateTime.Now; // Câu lệnh này bị lỗi
            ShipMethod.Add(newShipMethod);
            SaveChanges();
        }

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

        public List<BannerSlider> getAllBannerSliders()
        {
            return BannerSlider.OrderByDescending(b => b.CreatedAt).ToList();
        }

        public List<Campaign> getAllCampaign()
        {
            return Campaign.ToList();
        }

        public List<ShipMethod> getAllShipMethod()
        {
            return ShipMethod.ToList();
        }
        //1 bảng
        //public List<Comment> getAllComments()
        //{
        //    return Comment.ToList();
        //}

        //join nhiều bảng
        //public IQueryable getAllComments()
        //{
        //    //var commentList = Comment.Join(User,c => c.UserId,u => u.UserId, (c, u) => new
        //    //{
        //    //    CommentId = c.CommentId,
        //    //    UserId = c.UserId,
        //    //    UserName = u.LastName
        //    //});
        //    var commentList = from c in Comment
        //                 join u in User on c.UserId equals u.UserId
        //                 select new  MyCategory
        //                 {
        //                     CommentId = c.CommentId,
        //                     UserId = c.UserId,
        //                     UserName = u.LastName
        //                 };
        //    return commentList;
        //}
       

        public void updateSliderStatus(int sliderId, int status)
        {
            var slider = BannerSlider.Where(p => p.SliderId == sliderId).FirstOrDefault();
            slider.SliderStatus = status;
            SaveChanges();
        }

        public void deleteSlider(int sliderId)
        {
            System.Diagnostics.Debug.WriteLine("PID: " + sliderId);
            // Code thiếu chỗ xóa các bảng liên quan ở đây
            var slider = BannerSlider.Where(p => p.SliderId == sliderId).FirstOrDefault();

            if (slider != null)
            {
                Remove(slider);
                SaveChanges();
            }
        }

        public void saveSlider(BannerSlider newSlider)
        {
            newSlider.CreatedAt = DateTime.Now;
            newSlider.UpdatedAt = DateTime.Now;
            BannerSlider.Add(newSlider);
            SaveChanges();
        }


        public void updateLoginHistory(LoginHistory login)
        {
            LoginHistory.Add(login);
            SaveChanges();
        }

        public void updateOrderStatus(int OrderId, string OrderStatus, string PaymentStatus)
        {
            var order = Order.Where(p => p.OrderId == OrderId).FirstOrDefault();
            order.OrderStatus = OrderStatus;
            order.PaymentStatus = PaymentStatus;
            SaveChanges();
        }

        public void updateShipMethodStatus(int shipMethodId, int status)
        {
            var shipmethod = ShipMethod.Where(p => p.ShipMethodId == shipMethodId).FirstOrDefault();
            shipmethod.Status = status;
            this.SaveChanges();
        }
        public void deleteShipMethod(int shipmethodId)
        {
            var shipmethod = ShipMethod.Where(p => p.ShipMethodId == shipmethodId).FirstOrDefault();

            if (shipmethod != null)
            {
                Remove(shipmethod);
                SaveChanges();
            }
        }
        public List<Product> getGiamGiaSoc()
        {
            return Product.Where(p => p.Status == 1 && p.Discount != 0).OrderByDescending(b => b.Discount).Take(6).ToList();
        }
        //public List<Product> getSliderForHomePage()
        //{
        //    return Product.Where(p => p.Status == 1 && p.Discount != 0).OrderByDescending(b => b.Discount).Take(6).ToList();
        //}
        public List<ProductGallary> getProductGallary(int productId)
        {
            return ProductGallary.Where(p => p.ProductId == productId).ToList();
        }

        public void updateProductGallaryStatus(int GallaryId, int status)
        {
            var productGallary = ProductGallary.Where(p => p.GallaryId == GallaryId).FirstOrDefault();
            productGallary.GallaryStatus = status;
            SaveChanges();
        }

        public void deleteProductGallary(int GallaryId)
        {
            var productGallary = ProductGallary.Where(p => p.GallaryId == GallaryId).FirstOrDefault();

            if (productGallary != null)
            {
                Remove(productGallary);
                SaveChanges();
            }
        }

        public void saveProductGallary(ProductGallary productGallary)
        {
            ProductGallary.Add(productGallary);
            SaveChanges();
        }
    }
}
