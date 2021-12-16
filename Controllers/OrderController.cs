using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITGoShop_F_Ver2.Models;
using MyCardSession.Helpers;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace ITGoShop_F_Ver2.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult my_orders()
        {
            int customerId = Convert.ToInt32(HttpContext.Session.GetInt32("customerId"));
            if (customerId != 0) // Nếu customer đã đăng nhập
            {
                /*===Cái này để load layout ===*/
                ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
                var linqContext = new ITGoShopLINQContext();
                ViewBag.AllCategory = linqContext.getAllCategory();
                ViewBag.AllBrand = linqContext.getAllBrand();
                ViewBag.AllSubBrand = linqContext.getAllSubBrand();
                /*======*/

                ViewBag.OrderList = linqContext.getOrderListOfCustomer(customerId);
                return View();
            }
            return RedirectToAction("login", "Home");
        }
        public IActionResult order_detail(int orderId)
        {
            /*===Cái này để load layout ===*/
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllCategory = context.getAllCategory();
            ViewBag.AllBrand = context.getAllBrand();
            ViewBag.AllSubBrand = context.getAllSubBrand();
            /*======*/
            var linqContext = new ITGoShopLINQContext();
            int customerId = Convert.ToInt32(HttpContext.Session.GetInt32("customerId"));
            ViewBag.DefaultShippingAddress = context.getDefaultShippingAddress(customerId);
            ViewBag.OrderInfo = linqContext.getOrderInfo(orderId);
            ViewBag.OrderDetail = context.getOrderDetail(orderId);
            return View();
        }

        public async Task<IActionResult> create_orderAsync(Order order)
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            ITGoShopLINQContext linqContext = new ITGoShopLINQContext();
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            //ShipMethod shipMethod = linqContext.getShipMethod(ShipMethodId);
            order.Total = order.ShipFee + cart.Sum(item => item.Product.Price * item.Quantity);
            order.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("customerId"));
            order.DateUpdate = DateTime.Now;
            order.OrderDate = DateTime.Now;
            order.OrderStatus = "Đặt hàng thành công";
            order.PaymentStatus = "Chờ thanh toán";
            int numberOfProduct = cart.Sum(item => item.Quantity);
            order.Description = cart.First().Product.ProductName;
            if(numberOfProduct > 1)
                order.Description +=" và " + (numberOfProduct - 1) + " sản phẩm khác";
            int orderId = linqContext.createOrder(order);
            // Thêm theo dõi đơn hàng
            linqContext.addOrderTracking(orderId, "Đặt hàng thành công");

            foreach (var item in cart)
            {
                OrderDetail orderDetail = new OrderDetail() 
                { 
                    OrderQuantity = item.Quantity, 
                    ProductId = item.Product.ProductId, 
                    UnitPrice = item.Product.Price,
                    OrderId = order.OrderId
                };
                linqContext.saveOrderDetail(orderDetail);

                Product productInfo = context.getProductInfo(item.Product.ProductId);
                //Update thông tin doanh thu lên bảng statistic
                Statistic statistic = linqContext.getStatistic(DateTime.Now);
                Statistic newStatisticInfo = new Statistic()
                {
                    StatisticDate = DateTime.Now,
                    Sales = (int)(item.Quantity * item.Product.Price),
                    Profit = (int)((item.Product.Price - item.Product.Cost) * item.Quantity)
                };
                if (statistic != null)
                {
                    // Nếu đã có dòng thống kê doanh thu cho hôm nay thì cập nhật dữ liệu
                    linqContext.updateStatistic(newStatisticInfo);
                }
                else
                {
                    // Nếu chưa có dòng thống kê cho hôm nay thì tạo mới
                    linqContext.addStatistic(newStatisticInfo);
                }

                // Trừ số lượng tồn kho
                linqContext.updateSoldProduct(productInfo.ProductId, item.Quantity);

                // Gửi mail
                string customerFirstName = HttpContext.Session.GetString("customerFirstName");
                string customerLastName = HttpContext.Session.GetString("customerLastName");
                string mailContent = getMailContent(order, customerFirstName, customerLastName);
                await MailUtils.SendMailGoogleSmtp("itgoshop863@gmail.com", "qhuy.bh1901@gmail.com", $"Chào {customerFirstName}, ITGoShop đã nhận được đơn hàng của bạn", mailContent,
                                              "itgoshop863@gmail.com", "Itgoshop");

            }
            return RedirectToAction("order_detail", new { orderId = orderId });
        }

        public string getMailContent(Order orderInfo, string customerFirstName, string customerLastName)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            string ShipFee = orderInfo.ShipFee.ToString("#,###", cul.NumberFormat);
            string ToTal = orderInfo.Total.ToString("#,###", cul.NumberFormat);
            string EstimatedDeliveryTime = orderInfo.EstimatedDeliveryTime.ToString("dd-MM-yyyy");
            string OrderDate = orderInfo.OrderDate.ToString("HH:mm dd-MM-yyyy");
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            object shippingAddress = context.getShippingAddress(orderInfo.ShippingAddressId);

            string output = @$"<body>
                <div class='card' style='margin: 40px 100px;'>
                   <img src = 'https://lh3.googleusercontent.com/7GUtF9Gd14QM4jHIhXwNwW5AZCQDNbauFmKObH3Oa1bcdDI_8DaFYorS6GEYEp4Bnb0Ah1W72kwRYrTASNYinLNQLgIxLBTQtQzuPBGfHzyoHlJU3XjkqTP9BxKMeJelN8esvUHZMES6no3qpOq3F8AR9Arx05KZLqIzI8DYyTPsUus8nxC3zogAg_OOEyvrKDZDuHg5oirg8HuFKCHUTb-c5PzRvn6x3Fjn_hdBTh7roRmwl1xlI_tmujqs2-sKxQ8r-K8lCeoi2Ejxc5dc91b4bpis6X9JH3cSNio3JKr87HWO-1qdeBVcedmpimwU55JmS5ebv2YzjdciiUQXGomRMUQFHDpce6Zwj9g3hfs0ns67L91nh5_Ydcav6j9J5gM0PJse3gAw6cfiKPkB8mkgTeJE460Ki-w88wAF9VHCnibWOWsm76Z2bQqs3n_Kw1a6epqmanz5NVLyuqkdYo7YIvb1X-wQanekzE6vaIQu7ziO5Uh9HT3vZrm7cJu3L5rNQrhQRkT992MlvHRQjBhZWUGSezFctkOnDg3-bzOSCPCQAsw_0UR03yyUDXk9wXTCKaPfDWVSrCr3aBQiMiJySCdeo956H1skmix8qcaR4BFbrZTmqv7m9zeMXzeNQdb0jH-ePoS4k4e7tcJXJXoxa87FpgWqZoswvut-lm9g8vrjclHOIzO2aHeKF-HEGexNToMQzSgJU4TUb9ycyh9l=w220-h64-no?authuser=1' alt = '' style = 'height: 50px;'>
                   <hr style = 'background-color: #77ACF1;padding: 2px;'>
                        <div class='card-body' style='font-size:16px'>
                            <h2>Cảm ơn quý khách {customerLastName} {customerFirstName} đã đặt hàng tại ITGoShop,</h2>
                            <p class='card-text'>ITGoShop rất vui thông báo đơn hàng #{orderInfo.OrderId} của quý khách đã được tiếp nhận và đang trong quá trình xử lý. ITGoShop sẽ thông báo đến quý khách ngay khi hàng chuẩn bị được giao.</p>
                            <p class='card-text' style='color:#77ACF1;'><b>THÔNG TIN ĐƠN HÀNG #{orderInfo.OrderId}</b>  (Thời gian đặt hàng: {OrderDate})</p>
                            <hr>
                            <p class='card-text'><b>Mô tả đơn hàng:</b> {orderInfo.Description}</p>
                            <p class= 'card-text'><b> Địa chỉ giao hàng:</b></p>
                            <p> Tên người nhận: {shippingAddress.GetType().GetProperty("ReceiverName").GetValue(shippingAddress, null)}</p>
                            <p> Địa chỉ: {shippingAddress.GetType().GetProperty("Address").GetValue(shippingAddress, null)}, {shippingAddress.GetType().GetProperty("XaPhuong").GetValue(shippingAddress, null)}, {shippingAddress.GetType().GetProperty("QuanHuyen").GetValue(shippingAddress, null)}, {shippingAddress.GetType().GetProperty("ThanhPho").GetValue(shippingAddress, null)}</p>
                            </p> Điện thoại: {shippingAddress.GetType().GetProperty("Phone").GetValue(shippingAddress, null)}</p>
                            <p class= 'card-text'><b> Phương thức thanh toán:</b> {orderInfo.ShipMethod}</p>
                            <p class= 'card-text'><b> Thời gian giao hàng dự kiến: </b> dự kiến giao hàng vào ngày {EstimatedDeliveryTime}</p>
                            <p class= 'card-text'><b> Phí vận chuyển: </b>{ShipFee} ₫</p>
                            <p class= 'card-text'><b> TỔNG TRỊ GIÁ ĐƠN HÀNG: </b><b style = 'color:red; font-size: 20px' >{ToTal} ₫</b></p>
                            <p class= 'card-text'> Trân trọng,</p>
                            <p class='card-text'> Đội ngũ ITGoShop.</p>
                            <p class= 'card-text'><i> Lưu ý: Với những đơn hàng thanh toán trả trước, xin vui lòng đảm bảo người nhận hàng đúng thông tin đã đăng kí trong đơn hàng, và chuẩn bị giấy tờ tùy thân để đơn vị giao nhận có thể xác thực thông tin khi giao hàng</i></p>
                        </div>
                  </div>
            </body>";
            return output;
        }

        public void cancel_order(int orderId)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ITGoShopLINQContext linqContext = new ITGoShopLINQContext();
            linqContext.updateOrderStatus(orderId, "Đã hủy");
            List<object> orderDetail = context.getOrderDetail(orderId);

            // Cập nhật số lượng tồn kho và đã bán của các sản phẩm trong đơn hàng
            linqContext.updateSoldProduct(orderDetail);
        }
        
        public int is_rating_exit(int ProductId)
        {
            int customerId = Convert.ToInt32(HttpContext.Session.GetInt32("customerId"));
            ITGoShopLINQContext linqContext = new ITGoShopLINQContext();
            return linqContext.isRatingeExit(ProductId, customerId);
        }

        public void add_rating(int ProductId, string Title, string Content, int Rating)
        {
            int customerId = Convert.ToInt32(HttpContext.Session.GetInt32("customerId"));
            ITGoShopLINQContext linqContext = new ITGoShopLINQContext();
            linqContext.addRating(ProductId, Title, Content, Rating, customerId);
        }

        public string get_product(int ProductId)
        {
            ITGoShopLINQContext linqContext = new ITGoShopLINQContext();
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            Product product = context.getProductInfo(ProductId);
            string output = $"<p><b style='font-size:20px'>ĐÁNH GIÁ SẢN PHẨM #{product.ProductId}</b></p><img src='/public/images_upload/product/{product.ProductImage}'  style='margin: auto; max-width: 100px; max-height: 80px; width: auto; height: auto;'/>";
            output += $"<p style='display:inline-block; margin-left:10px'>{product.ProductName}</p>";
            output += $"<input type='hidden' value='{product.ProductId}</p>";
            return output;
        }
    }
}
