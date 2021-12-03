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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Hàm này để set primary key cho Entity Framework
            modelBuilder.Entity<OrderDetail>().HasKey(od => new {od.OrderId, od.ProductId });
            modelBuilder.Entity<WishList>().HasKey(wl => new { wl.ProductId, wl.UserId });
        }
        public DbSet<User> User { set; get; }   // Bảng User trong DataBase, <User> tên lớp
        public DbSet<Product> Product { set; get; }
        public DbSet<Brand> Brand { set; get; }
        public DbSet<SubBrand> SubBrand { set; get; }
        public DbSet<Category> Category { set; get; }
        public DbSet<BannerSlider> BannerSlider { set; get; }
        public DbSet<Campaign> Campaign { set; get; }
        public DbSet<Order> Order { set; get; }
        public DbSet<OrderDetail> OrderDetail { set; get; }
        
        public DbSet<LoginHistory> LoginHistory { set; get; }
        public DbSet<ShipMethod> ShipMethod { set; get; }
        public DbSet<ProductGallary> ProductGallary { set; get; }
        public DbSet<Comment> Comment { set; get; }
        public DbSet<devvn_quanhuyen> devvn_quanhuyen { set; get; }
        public DbSet<devvn_tinhthanhpho> devvn_tinhthanhpho { set; get; }
        public DbSet<devvn_xaphuongthitran> devvn_xaphuongthitran { set; get; }
        public DbSet<ShippingAddress> ShippingAddress { set; get; }
        public DbSet<Statistic> Statistic { set; get; }
        public DbSet<WishList> WishList { set; get; }

        public void saveProduct(Product newProduct)
        {
            newProduct.StartsAt = DateTime.Now;
            newProduct.CreatedAt = DateTime.Now;
            newProduct.UpdatedAt = DateTime.Now;
            Product.Add(newProduct);
            SaveChanges();
        }

        public void saveCategory(Category newCate)
        {
            Category.Add(newCate);
            SaveChanges();
        }
        public void saveBrand(Brand newBrand)
        {
            Brand.Add(newBrand);
            SaveChanges();
        }

        public void saveSubBrand(SubBrand newSubBrand)
        {
            SubBrand.Add(newSubBrand);
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

        public void deleteCategory(string categoryId)
        {
            var cate = (from p in Category
                           where (p.CategoryId == categoryId)
                           select p).FirstOrDefault();

            if (cate != null)
            {
                Remove(cate);
                SaveChanges();
            }
        }

        public void deleteBrand(int brandId)
        {
            var brand = (from p in Brand
                        where (p.BrandId == brandId)
                        select p).FirstOrDefault();

            if (brand != null)
            {
                Remove(brand);
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
        public List<ShipMethod> getShipMethodToCheckout()
        {
            return ShipMethod.Where(s => s.Status == 1).ToList();
        }

        public List<devvn_tinhthanhpho> getAllThanhPho()
        {
            return devvn_tinhthanhpho.ToList();
        }

        public List<devvn_quanhuyen> getAllQuanHuyen()
        {
            return devvn_quanhuyen.ToList();
        }
        public List<devvn_xaphuongthitran> getAllXaPhuong()
        {
            return devvn_xaphuongthitran.ToList();
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
        public void updateOrderStatus(int OrderId, string OrderStatus)
        {
            var order = Order.Where(p => p.OrderId == OrderId).FirstOrDefault();
            order.OrderStatus = OrderStatus;
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

        public List<Product> getRelatedProduct(int productId, string categoryId, int brandId)
        {
            var relatedProduct = Product.Where(p => p.ProductId != productId && p.CategoryId == categoryId && p.BrandId == brandId).Take(10);
            return relatedProduct.ToList();
        }

        public void deleteShippingAddress(int ShippingAddressId)
        {
            var shippingAddress = ShippingAddress.Where(p => p.ShippingAddressId == ShippingAddressId).FirstOrDefault();

            if (shippingAddress != null)
            {
                Remove(shippingAddress);
                SaveChanges();
            }
        }

        public List<devvn_xaphuongthitran> load_xaphuongthitran_dropdownbox(string maqh)
        {
            var xaphuong = devvn_xaphuongthitran.Where(x => x.Maqh == maqh).ToList();
            return xaphuong;
        }
        public List<devvn_quanhuyen> load_quanhuyen_dropdownbox(string matp)
        {
            var quanhuyen = devvn_quanhuyen.Where(q => q.Matp == matp).ToList();
            return quanhuyen;
        }

        public void change_default_shipping_address(int ShippingAddressId, int customerId)
        {
            
            // Set các địa chỉ isDefault = 0
            var shippingAddressList = ShippingAddress.Where(s => s.UserId == customerId).ToList();
            foreach (var item in shippingAddressList)
            {
                System.Diagnostics.Debug.WriteLine("id=" + item.ShippingAddressId);
                item.IsDefault = 0;
            }
            SaveChanges();

            // Set isDefault = 1 đối với địa chỉ đã chọn
            var shippingAddress = ShippingAddress.Where(s => s.ShippingAddressId == ShippingAddressId).FirstOrDefault();
            shippingAddress.IsDefault = 1;
            System.Diagnostics.Debug.WriteLine("cus=" + shippingAddress.ReceiverName + shippingAddress.IsDefault);
            SaveChanges();
        }

        public void saveShippingAddress(ShippingAddress shippingAddress)
        {
            // Set các địa chỉ isDefault = 0
            var shippingAddressList = ShippingAddress.Where(s => s.UserId == shippingAddress.UserId).ToList();
            foreach (var item in shippingAddressList)
            {
                item.IsDefault = 0;
            }
            SaveChanges();

            shippingAddress.IsDefault = 1;
            shippingAddress.UpdatedAt = DateTime.Now;
            shippingAddress.CreatedAt = DateTime.Now;
            ShippingAddress.Add(shippingAddress);
            SaveChanges();
        }
        public void update_shipping_address(ShippingAddress shippingAddress)
        {
            var shippingAddressUpdate = ShippingAddress.Where(s => s.ShippingAddressId == shippingAddress.ShippingAddressId).FirstOrDefault();
            if (shippingAddressUpdate != null)
            {
                shippingAddressUpdate.ReceiverName = shippingAddress.ReceiverName;
                shippingAddressUpdate.ShippingAddressType = shippingAddress.ShippingAddressType;
                shippingAddressUpdate.Xaid = shippingAddress.Xaid;
                shippingAddressUpdate.Maqh = shippingAddress.Maqh;
                shippingAddressUpdate.Matp = shippingAddress.Matp;
                shippingAddressUpdate.Address = shippingAddress.Address;
                shippingAddressUpdate.Phone = shippingAddress.Phone;
                SaveChanges();
            }
        }

        public int createOrder(Order order)
        {
            Order.Add(order);
            SaveChanges();
            return order.OrderId;
        }

        //public ShipMethod getShipMethod(int ShipMethodId)
        //{
        //    var shipMethod = ShipMethod.Where(s => s.ShipMethodId == ShipMethodId).FirstOrDefault();
        //    return shipMethod;
        //}
        public void saveOrderDetail(OrderDetail orderDetail)
        {
            OrderDetail.Add(orderDetail);
            SaveChanges();
        }

        public Order getOrderInfo(int orderId)
        {
            var orderInfo = Order.Where(o => o.OrderId == orderId).FirstOrDefault();
            return orderInfo;
        }

        public Statistic getStatistic(DateTime statisticDate)
        {
            // Chuyển đổi như vầy để set Time = 00:00:00 để so sánh với statisticDate (kiểu Date, ko có Time) trong CSDL
            DateTime myDate = DateTime.ParseExact(statisticDate.ToString("yyyy-MM-dd"), "yyyy-MM-dd",
                                       System.Globalization.CultureInfo.InvariantCulture);

            var statistic = Statistic.Where(s => s.StatisticDate == myDate).FirstOrDefault();
            if(statistic != null)
                System.Diagnostics.Debug.WriteLine("SD: " + statistic.StatisticDate);
            return statistic;
        }

        public void addStatistic(Statistic newStatisticLine)
        {
            Statistic.Add(newStatisticLine);
            SaveChanges();
        }

        public void updateStatistic(Statistic newData)
        {
            // Chuyển đổi như vầy để set Time = 00:00:00 để so sánh với statisticDate (kiểu Date, ko có Time) trong CSDL
            DateTime myDate = DateTime.ParseExact(newData.StatisticDate.ToString("yyyy-MM-dd"), "yyyy-MM-dd",
                                       System.Globalization.CultureInfo.InvariantCulture);

            var statistic = Statistic.Where(s => s.StatisticDate == myDate).FirstOrDefault();
            statistic.Sales += newData.Sales;
            statistic.Profit += newData.Profit;
            SaveChanges();
        }

        public void updateSoldProduct(int productId, int newSold)
        {
            var product = Product.Where(p => p.ProductId == productId).FirstOrDefault();
            product.Sold += newSold;
            product.Quantity -= newSold;
            SaveChanges();
        }

        public void updateSoldProduct(List<object> orderDetail)
        {
            foreach(var item in orderDetail)
            {
                int productId = (int)item.GetType().GetProperty("ProductId").GetValue(item, null);
                var product = Product.Where(p => p.ProductId == productId).FirstOrDefault();
                product.Sold -= (int)item.GetType().GetProperty("OrderQuantity").GetValue(item, null);
                product.Quantity += (int)item.GetType().GetProperty("OrderQuantity").GetValue(item, null);
            }
            SaveChanges();
        }

        public List<Order> getOrderListOfCustomer(int userId)
        {
            var orderList = Order.Where(o => o.UserId == userId).ToList();
            return orderList;
        }

        public void remove_product_from_wishlist(int userId, int productId)
        {
            var wishlist = WishList.Where(p => p.UserId == userId && p.ProductId == productId).FirstOrDefault();

            if (wishlist != null)
            {
                Remove(wishlist);
                SaveChanges();
            }
        }
        public void add_product_to_wishlist(int userId, int productId)
        {
            WishList newItem = new WishList()
            {
                UserId = userId,
                ProductId = productId,
                CreatedAt = DateTime.Now
            };
            WishList.Add(newItem);
            SaveChanges();
        }
        public int isProductExistInWishlist(int userId, int productId)
        {
            var wishlist = WishList.Where(p => p.UserId == userId && p.ProductId == productId).FirstOrDefault();

            if (wishlist != null)
            {
                System.Diagnostics.Debug.WriteLine("Wish list: " + wishlist.UserId +" " +wishlist.ProductId);
                return 0; //Sản phẩm đã tồn tại trong wishlist
            }
            return 1;
        }

        public void updateCommentStatus(int ParentComment)
        {
            var comment = Comment.Where(c => c.CommentId == ParentComment).FirstOrDefault();
            comment.Reply = 1;
            SaveChanges();
        }

        public void addComment(Comment comment)
        {
            Comment.Add(comment);
            SaveChanges();
        }
    }
}
