using ITGoShop_F_Ver2.Controllers;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGoShop_F_Ver2.Models
{
    public class ITGoShopContext
    {
        public string ConnectionString { get; set; }

        public ITGoShopContext(string connectionString) 
        {
            this.ConnectionString = connectionString;
        }
        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
        public User getUserInfo(string email, string password)
        {
            User userInfo = new User();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "select * from User where Email = @email AND Password = @password";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("email", email);
                cmd.Parameters.AddWithValue("password", password);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        userInfo.UserId = Convert.ToInt32(reader["UserId"]);
                        userInfo.FirstName = reader["FirstName"].ToString();
                        userInfo.LastName = reader["LastName"].ToString();
                        userInfo.Mobile = reader["Mobile"].ToString();
                        userInfo.UserImage = reader["UserImage"].ToString();
                        userInfo.Admin = Convert.ToInt32(reader["Admin"]);
                    }
                    else
                        return null;
                }
            }
            return userInfo;
        }

        public void updateLastLogin(int userId)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "UPDATE USER SET LastLogin = SYSDATE() where UserId = @userId";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("userId", userId);
                cmd.ExecuteNonQuery();
            }
        }

        public int countCustomer()
        {
            int numberCustomer = 0;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT COUNT(*) AS NUMBER_CUSTOMER FROM USER WHERE ADMIN = 0";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        numberCustomer = Convert.ToInt32(reader["NUMBER_CUSTOMER"]);
                    }
                }
            }
            return numberCustomer;
        }

        public int countProduct()
        {
            int numberProduct = 0;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT COUNT(*) AS NUMBER_PRODUCT FROM PRODUCT";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        numberProduct = Convert.ToInt32(reader["NUMBER_PRODUCT"]);
                    }
                }
            }
            return numberProduct;
        }

        public int countOrder()
        {
            int numberOrder = 0;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT COUNT(*) AS NUMBER_ORDER FROM `ORDER` WHERE ORDERSTATUS <> 'Đã hủy'";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        numberOrder = Convert.ToInt32(reader["NUMBER_ORDER"]);
                    }
                }
            }
            return numberOrder;
        }

        public int countOrder(DateTime startDate, DateTime endDate)
        {
            int numberOrder = 0;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT COUNT(*) AS NUMBER_ORDER FROM `ORDER` WHERE ORDERSTATUS <> 'Đã hủy' AND (DATE(ORDERDATE) BETWEEN @startdate AND @enddate)";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("startdate", startDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("enddate", endDate.ToString("yyyy-MM-dd"));
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        numberOrder = Convert.ToInt32(reader["NUMBER_ORDER"]);
                    }
                }
            }
            return numberOrder;
        }

        public int countLoginThisYear()
        {
            int numberLoginThisYear = 0;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT COUNT(*) AS Number_LoginThisYear FROM `loginhistory` WHERE YEAR(LoginDate) = YEAR(SYSDATE())";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        numberLoginThisYear = Convert.ToInt32(reader["Number_LoginThisYear"]);
                    }
                }
            }
            return numberLoginThisYear;
        }

        public int countLogin(DateTime startDate, DateTime endDate)
        {
            int numberLogin = 0;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT COUNT(*) AS Number_Login FROM `loginhistory` WHERE LOGINDATE BETWEEN @startdate AND @enddate";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("startdate", startDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("enddate", endDate.ToString("yyyy-MM-dd"));
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        numberLogin = Convert.ToInt32(reader["Number_Login"]);
                    }
                }
            }
            return numberLogin;
        }

        public List<object> countLoginByDate(DateTime startDate, DateTime endDate)
        {
            List<object> loginHistory = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT COUNT(*) AS NUMBER_LOGIN, LoginDate FROM loginhistory WHERE LOGINDATE BETWEEN @startdate AND @enddate GROUP BY LoginDate";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("startdate", startDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("enddate", endDate.ToString("yyyy-MM-dd"));
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            period = ((DateTime)reader["LoginDate"]).ToString("dd-MM-yyyy"),
                            number_access = Convert.ToInt32(reader["NUMBER_LOGIN"]),
                        };
                        loginHistory.Add(obj);
                    }
                }
            }
            return loginHistory;
        }

        public long getRevenue(DateTime startDate, DateTime endDate)
        {
            int revenue = 0;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT SUM(PROFIT) AS TOTAL_REVENUE FROM STATISTIC WHERE STATISTICDATE BETWEEN @startdate AND @enddate";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("startdate", startDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("enddate", endDate.ToString("yyyy-MM-dd"));
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        revenue = Convert.ToInt32(reader["TOTAL_REVENUE"]);
                    }
                }
            }
            return revenue;
        }
        public List<object> getTopProduct(DateTime startDate, DateTime endDate)
        {
            List<object> products = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT SUM(OrderQuantity) AS NumberSolded , ProductName, ProductImage, P.ProductId, StartsAt, Quantity, Cost, Price " +
                    "FROM (`product` P JOIN `orderdetail` OD ON P.ProductId = OD.ProductId) " +
                    "JOIN `order` O ON O.OrderId = OD.OrderId " +
                    "WHERE OrderStatus <> 'Đã hủy' " +
                    "AND ORDERDATE BETWEEN @startdate AND @enddate " +
                    "GROUP BY ProductName, ProductImage, P.ProductId, StartsAt, Quantity, Cost, Price " +
                    "ORDER BY SUM(OrderQuantity) DESC " +
                    "LIMIT 5;";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("startdate", startDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("enddate", endDate.ToString("yyyy-MM-dd"));
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            ProductImage = reader["ProductImage"].ToString(),
                            StartsAt = (DateTime)reader["StartsAt"],
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Cost = Convert.ToInt32(reader["Cost"]),
                            Price = Convert.ToInt32(reader["Price"]),
                            NumberSolded = Convert.ToInt32(reader["NumberSolded"])
                        };
                        products.Add(obj);
                    }
                }
            }
            return products;
        }

        public List<Blog> getTopBlogView()
        {
            List<Blog> blogs = new List<Blog>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT * FROM BLOG ORDER BY VIEW DESC LIMIT 5";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        blogs.Add(new Blog()
                        {
                            BlogId = Convert.ToInt32(reader["BlogId"]),
                            Author = reader["Author"].ToString(),
                            Title = reader["Title"].ToString(),
                            Summary = reader["Summary"].ToString(),
                            DateCreate = (DateTime)reader["DateCreate"],
                            Image = reader["Image"].ToString(),
                            View = Convert.ToInt32(reader["View"]),
                        });
                    }
                }
            }
            return blogs;
        }

        public List<Product> getTopProductView()
        {
            List<Product> products = new List<Product>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT * FROM PRODUCT ORDER BY VIEW DESC LIMIT 5";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //System.Diagnostics.Debug.WriteLine("1");
                        products.Add(new Product()
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            ProductImage = reader["ProductImage"].ToString(),
                            StartsAt = (DateTime)reader["StartsAt"],
                            View = Convert.ToInt32(reader["View"]),
                        });
                    }
                }
            }
            return products;
        }

        public List<Product> getInventoryList()
        {
            List<Product> products = new List<Product>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT * FROM PRODUCT ORDER BY SOLD DESC, StartsAt ASC LIMIT 5;";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new Product()
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            ProductName = reader["ProductName"].ToString(),
                            ProductImage = reader["ProductImage"].ToString(),
                            StartsAt = (DateTime)reader["StartsAt"],
                            View = Convert.ToInt32(reader["View"]),
                        });
                    }
                }
            }
            return products;
        }
        public List<object> countOrderByDate(DateTime startDate, DateTime endDate)
        {
            List<object> ordersInfo = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT COUNT(*) AS Number_Order, OrderStatus " +
                    "FROM `ORDER` " +
                    "WHERE OrderDate BETWEEN @startdate AND @enddate " +
                    "GROUP BY OrderStatus";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("startdate", startDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("enddate", endDate.ToString("yyyy-MM-dd"));
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //System.Diagnostics.Debug.WriteLine("1");
                        var obj = new
                        {
                            label = reader["OrderStatus"].ToString(),
                            value = Convert.ToInt32(reader["Number_Order"]),
                        };
                        ordersInfo.Add(obj);
                    }
                }
            }
            return ordersInfo;
        }

        public List<object> getRevenueByDate(DateTime startDate, DateTime endDate)
        {
            List<object> revenueInfo = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT * " +
                    "FROM STATISTIC " +
                    "WHERE STATISTICDATE BETWEEN @startdate AND @enddate " +
                    "ORDER BY STATISTICDATE ASC";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("startdate", startDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("enddate", endDate.ToString("yyyy-MM-dd"));
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            period = ((DateTime)reader["StatisticDate"]).ToString("dd-MM-yyyy"),
                            sales = Convert.ToInt32(reader["Sales"]),
                            profit = Convert.ToInt32(reader["Profit"]),
                        };
                        revenueInfo.Add(obj);
                    }
                }
            }
            return revenueInfo;
        }

        public List<Category> getAllCategory()
        {
            List<Category> list = new List<Category>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "select * from Category";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Category()
                        {
                            CategoryId = reader["CategoryId"].ToString(),
                            CategoryName = reader["CategoryName"].ToString(),
                            Status = Convert.ToInt32(reader["Status"]),
                            
                        });
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }

        public List<SubBrand> getAllSubBrand()
        {
            List<SubBrand> list = new List<SubBrand>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "select * from SubBrand";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new SubBrand()
                        {
                            SubBrandId = reader["SubBrandId"].ToString(),
                            SubBrandName = reader["SubBrandName"].ToString(),
                            BrandId = Convert.ToInt32(reader["BrandId"])
                        });
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }

        public List<Brand> getAllBrand()
        {
            List<Brand> list = new List<Brand>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "select * from Brand";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Brand()
                        {
                            BrandId = Convert.ToInt32(reader["BrandId"]),
                            BrandName = reader["BrandName"].ToString(),
                            Description = reader["Description"].ToString(),
                            CategoryId = reader["CategoryId"].ToString(),
                            Status = Convert.ToInt32(reader["Status"]),
                            BrandLogo = reader["BrandLogo"].ToString(),
                        });
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }
        public List<Blog> getAllBlog()
        {
            List<Blog> list = new List<Blog>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "select * from blog where Status = 1";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Blog()
                        {
                            BlogId = Convert.ToInt32(reader["BlogId"]),
                            Author = reader["Author"].ToString(),
                            Title = reader["Title"].ToString(),
                            Summary = reader["Summary"].ToString(),
                            Content = reader["Content"].ToString(),
                            Image = reader["Image"].ToString(),
                        });
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }
        public List<Blog> getBlog()
        {
            List<Blog> list = new List<Blog>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "select * from blog ORDER BY DatePost DESC LIMIT 3";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Blog()
                        {
                            BlogId = Convert.ToInt32(reader["BlogId"]),
                            Author = reader["Author"].ToString(),
                            Title = reader["Title"].ToString(),
                            Summary = reader["Summary"].ToString(),
                            Content = reader["Content"].ToString(),
                            DatePost = (DateTime)reader["DatePost"],
                            Image = reader["Image"].ToString(),
                        });
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }

        public List<BannerSlider> getAllBannerSlider()
        {
            List<BannerSlider> list = new List<BannerSlider>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "select * from BannerSlider";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new BannerSlider()
                        {
                            SliderId = Convert.ToInt32(reader["SliderId"]),
                            SliderImage = reader["SliderImage"].ToString(),
                            SliderName = reader["SliderName"].ToString(),
                            SliderStatus = Convert.ToInt32(reader["SliderStatus"]),
                            //BlogId = Convert.ToInt32(reader["BlogId"])
                        });
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }

        public List<object> getAllProduct()
        {
            List<object> list = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "SELECT * FROM (Product P JOIN category C ON P.CategoryId = C.CategoryId) JOIN brand B ON B.BrandId = P.BrandId;";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            CategoryName = reader["CategoryName"].ToString(),
                            BrandName = reader["BrandName"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Price = Convert.ToInt32(reader["Price"]),
                            Status = Convert.ToInt32(reader["Status"]),
                            ProductImage = reader["ProductImage"].ToString()
                        };
                        list.Add(obj);
                }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }
        public List<object> get3Product()
        {
            List<object> products = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT SUM(OrderQuantity) AS NumberSolded , ProductName, ProductImage, P.ProductId, StartsAt, Quantity, Cost, Price " +
                    "FROM (`product` P JOIN `orderdetail` OD ON P.ProductId = OD.ProductId) " +
                    "JOIN `order` O ON O.OrderId = OD.OrderId " +
                    "WHERE OrderStatus <> 'Đã hủy' " +
                    
                    "GROUP BY ProductName, ProductImage, P.ProductId, StartsAt, Quantity, Cost, Price " +
                    "ORDER BY SUM(OrderQuantity) DESC " +
                    "LIMIT 3;";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            ProductImage = reader["ProductImage"].ToString(),
                            StartsAt = (DateTime)reader["StartsAt"],
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Cost = Convert.ToInt32(reader["Cost"]),
                            Price = Convert.ToInt32(reader["Price"]),
                            NumberSolded = Convert.ToInt32(reader["NumberSolded"])
                        };
                        products.Add(obj);
                    }
                    reader.Close();
                }
                conn.Close();
            }
            return products;
        }
        public List<Product> getTop3ProductView()
        {
            List<Product> products = new List<Product>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT * FROM PRODUCT ORDER BY VIEW DESC LIMIT 3";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //System.Diagnostics.Debug.WriteLine("1");
                        products.Add(new Product()
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            ProductImage = reader["ProductImage"].ToString(),
                            StartsAt = (DateTime)reader["StartsAt"],
                            Price = Convert.ToInt32(reader["Price"]),
                            View = Convert.ToInt32(reader["View"]),
                        });
                    }
                }
            }
            return products;
        }

        public List<object> getLTProduct()
        {
            List<object> list = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "SELECT * FROM Product P JOIN category C ON P.CategoryId = C.CategoryId WHERE P.CategoryId = 'LT000' ORDER BY VIEW DESC LIMIT 8 ;";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            CategoryName = reader["CategoryName"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Price = Convert.ToInt32(reader["Price"]),
                            Status = Convert.ToInt32(reader["Status"]),
                            ProductImage = reader["ProductImage"].ToString(),
                            View = Convert.ToInt32(reader["View"]),
                            Discount = Convert.ToDouble(reader["Discount"]),
                            StartsAt = (DateTime)reader["StartsAt"],
                        };
                        list.Add(obj);
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }

        public List<object> getPCProduct()
        {
            List<object> list = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "SELECT * FROM Product P JOIN category C ON P.CategoryId = C.CategoryId WHERE P.CategoryId = 'PC000' ORDER BY VIEW DESC LIMIT 8 ;";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            CategoryName = reader["CategoryName"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Price = Convert.ToInt32(reader["Price"]),
                            Status = Convert.ToInt32(reader["Status"]),
                            ProductImage = reader["ProductImage"].ToString(),
                            View = Convert.ToInt32(reader["View"]),
                            Discount = Convert.ToDouble(reader["Discount"]),
                            StartsAt = (DateTime)reader["StartsAt"],
                        };
                        list.Add(obj);
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }

        public List<object> getPKProduct()
        {
            List<object> list = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "SELECT * FROM Product P JOIN category C ON P.CategoryId = C.CategoryId WHERE P.CategoryId = 'PK000' ORDER BY VIEW DESC LIMIT 8 ;";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            CategoryName = reader["CategoryName"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Price = Convert.ToInt32(reader["Price"]),
                            Status = Convert.ToInt32(reader["Status"]),
                            ProductImage = reader["ProductImage"].ToString(),
                            View = Convert.ToInt32(reader["View"]),
                            Discount = Convert.ToDouble(reader["Discount"]),
                            StartsAt = (DateTime)reader["StartsAt"],
                        };
                        list.Add(obj);
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }

        public void updateProductStatus(int productId, int status)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "UPDATE PRODUCT SET Status = @status WHERE ProductId = @productId";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("status", status);
                cmd.Parameters.AddWithValue("productId", productId);
                cmd.ExecuteNonQuery();
            }
        }

        public void deleteProduct(int productId)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "DELETE FROM OrderDetail WHERE ProductId = @productId";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("productId", productId);
                cmd.ExecuteNonQuery();

                str = "DELETE FROM Comment WHERE ProductId = @productId";
                cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("productId", productId);
                cmd.ExecuteNonQuery();

                str = "DELETE FROM WishList WHERE ProductId = @productId";
                cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("productId", productId);
                cmd.ExecuteNonQuery();

                str = "DELETE FROM ProductGallary WHERE ProductId = @productId";
                cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("productId", productId);
                cmd.ExecuteNonQuery();

                str = "DELETE FROM CartItem WHERE ProductId = @productId";
                cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("productId", productId);
                cmd.ExecuteNonQuery();

                str = "DELETE FROM Product WHERE ProductId = @productId";
                cmd = new MySqlCommand(str, conn);

                cmd.Parameters.AddWithValue("productId", productId);
                cmd.ExecuteNonQuery();
            }
        }

        public Product getProductInfo(int productId)
        {
            Product productInfo = new Product();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT * FROM (Product P JOIN category C ON P.CategoryId = C.CategoryId) " +
                    "JOIN brand B ON B.BrandId = P.BrandId " +
                    "where ProductId = @productId";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("ProductId", productId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        productInfo.ProductId = Convert.ToInt32(reader["ProductId"]);
                        productInfo.Quantity = Convert.ToInt32(reader["Quantity"]);
                        productInfo.ProductName = reader["ProductName"].ToString();
                        productInfo.ProductImage = reader["ProductImage"].ToString();
                        productInfo.Sold = Convert.ToInt32(reader["Sold"]);
                        productInfo.Cost = Convert.ToInt32(reader["Cost"]);
                        productInfo.Price = Convert.ToInt32(reader["Price"]);
                        productInfo.Status = Convert.ToInt32(reader["Status"]);
                        productInfo.Discount = Convert.ToInt32(reader["Discount"]);
                        productInfo.CategoryId = reader["CategoryId"].ToString();
                        productInfo.SubBrandId = reader["SubBrandId"].ToString();
                        productInfo.BrandId = Convert.ToInt32(reader["BrandId"]);
                        productInfo.Content = reader["Content"].ToString();
                    }
                    else
                        return null;
                }
            }
            return productInfo;
        }

        public List<object> getAllOrder()
        {
            List<object> list = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "SELECT* FROM `ORDER` O JOIN USER U ON O.UserId = U.UserId ORDER BY O.ORDERID DESC";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            OrderId = Convert.ToInt32(reader["OrderId"]),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            OrderStatus = reader["OrderStatus"].ToString(),
                            Total = Convert.ToInt32(reader["Total"]),
                            PaymentStatus = reader["PaymentStatus"].ToString()
                        };
                        list.Add(obj);
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }

        public List<object> getAllComments()
        {
            List<object> list= new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT * FROM (COMMENT C " +
                    "JOIN USER U ON C.UserId = U.UserId) " +
                    "JOIN PRODUCT P ON P.ProductId = C.ProductId " +
                    "WHERE ParentComment IS NULL " +
                    "ORDER BY Reply ASC, CommentId DESC;";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            CommentId = Convert.ToInt32(reader["CommentId"]),
                            CommentContent = reader["CommentContent"].ToString(),
                            UserName = reader["LastName"].ToString() + " " + reader["FirstName"].ToString(),
                            ProductName = reader["ProductName"].ToString(),
                            CreatedAt = ((DateTime)reader["CreatedAt"]).ToString("dd-MM-yyyy"),
                            Reply = Convert.ToInt32(reader["Reply"]),
                            CommentStatus = Convert.ToInt32(reader["CommentStatus"]),
                        };
                        list.Add(obj);
                    }
                }
            }
            return list;
        }

        public List<object> getSliderForHomePage()
        {
            List<object> list = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "SELECT * FROM bannerslider BS JOIN BLOG B ON BS.BlogId = B.BlogId WHERE SliderStatus = 1 ORDER BY CreatedAt ASC LIMIT 8";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        System.Diagnostics.Debug.WriteLine("bid: " + Convert.ToInt32(reader["BlogId"]));
                        var obj = new
                        {
                            SliderId = Convert.ToInt32(reader["SliderId"]),
                            SliderName = reader["SliderName"].ToString(),
                            SliderImage = reader["SliderImage"].ToString(),
                            BlogId = Convert.ToInt32(reader["BlogId"]),
                        };
                        list.Add(obj);
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }

        public Blog getBlogDetail(int blogId)
        {
            Blog Info = new Blog();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT * FROM Blog " +
                    "where BlogId = @blogId";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("BlogId", blogId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                            Info.BlogId = Convert.ToInt32(reader["BlogId"]);
                            Info.Author = reader["Author"].ToString();
                            Info.Title = reader["Title"].ToString();
                            Info.Summary = reader["Summary"].ToString();
                            Info.Content = reader["Content"].ToString();
                            Info.DatePost = (DateTime)reader["DatePost"];
                            Info.Image = reader["Image"].ToString();
                    }
                    else
                        return null;
                }
            }
            return Info;
        }
        public List<Blog> getBlogRelate()
        {
            List<Blog> list = new List<Blog>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "select * from blog ORDER BY DatePost DESC LIMIT 3";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Blog()
                        {
                            BlogId = Convert.ToInt32(reader["BlogId"]),
                            Author = reader["Author"].ToString(),
                            Title = reader["Title"].ToString(),
                            Summary = reader["Summary"].ToString(),
                            Content = reader["Content"].ToString(),
                            DatePost = (DateTime)reader["DatePost"],
                            Image = reader["Image"].ToString(),
                        });
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }

    }
}
