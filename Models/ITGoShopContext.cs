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



    }
}
