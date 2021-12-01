using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITGoShop_F_Ver2.Models;
using Microsoft.AspNetCore.Http;
using MyCardSession.Helpers;

namespace ITGoShop_F_Ver2.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            int customerId = Convert.ToInt32(HttpContext.Session.GetInt32("customerId"));
            if (customerId != 0) // Nếu customer đã đăng nhập
            {
                /*===Cái này để load layout ===*/
                ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
                ViewBag.AllCategory = context.getAllCategory();
                ViewBag.AllBrand = context.getAllBrand();
                ViewBag.AllSubBrand = context.getAllSubBrand();
                /*======*/

                var linqContext = new ITGoShopLINQContext();
                ViewBag.DefaultShippingAddress = context.getDefaultShippingAddress(customerId);
                ViewBag.AllShipMethod = linqContext.getShipMethodToCheckout();
                var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
                ViewBag.cart = cart;
                if (cart == null)
                {
                    ViewBag.total = 0;
                    ViewBag.numberItem = 0;
                }
                else
                {
                    ViewBag.numberItem = cart.Sum(item => item.Quantity);
                    ViewBag.total = cart.Sum(item => item.Product.Price * item.Quantity);
                }
                ViewBag.AllThanhPho = linqContext.getAllThanhPho();
                ViewBag.AllQuanHuyen = linqContext.getAllQuanHuyen();
                ViewBag.AllXaPhuong = linqContext.getAllXaPhuong();
                return View();
            }
            return RedirectToAction("login_to_checkout");
        }
        public IActionResult login_to_checkout(string message)
        {
            if (!string.IsNullOrEmpty(ViewBag.message))
            {
                ViewBag.message = message;
            }
            return View();
        }

        public IActionResult checkout_after_login(User userInput)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            User userInfo = context.getUserInfo(userInput.Email, userInput.Password, 0);
            if (userInfo != null)
            {
                HttpContext.Session.SetInt32("customerId", userInfo.UserId);
                HttpContext.Session.SetString("customerLastName", userInfo.LastName);
                HttpContext.Session.SetString("customerFirstName", userInfo.FirstName);
                HttpContext.Session.SetString("customerImage", userInfo.UserImage);
                var LINQContext = new ITGoShopLINQContext();
                LoginHistory login = new LoginHistory(userInfo.UserId, DateTime.Now, DateTime.Now);
                LINQContext.updateLoginHistory(login);
                // Update last login
                context.updateLastLogin(userInfo.UserId);
                return RedirectToAction("Index");
            }
            return RedirectToAction("login_to_checkout", new { message = "Mật khẩu hoặc tài khoản sai. Xin nhập lại!" });
        }

        public IActionResult show_shipping_address()
        {
            int customerId = Convert.ToInt32(HttpContext.Session.GetInt32("customerId"));
            if (customerId != 0) // Nếu customer đã đăng nhập
            {
                /*===Cái này để load layout ===*/
                ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
                ViewBag.AllCategory = context.getAllCategory();
                ViewBag.AllBrand = context.getAllBrand();
                ViewBag.AllSubBrand = context.getAllSubBrand();
                /*======*/


                var linqContext = new ITGoShopLINQContext();
                ViewBag.DefaultShippingAddress = context.getDefaultShippingAddress(customerId);
                ViewBag.ShippingAddressList = context.getShippingAddressOfCustomer(customerId);
                ViewBag.AllThanhPho = linqContext.getAllThanhPho();
                ViewBag.AllQuanHuyen = linqContext.getAllQuanHuyen();
                ViewBag.AllXaPhuong = linqContext.getAllXaPhuong();
            }
            return View("shipping_address");
        }
        public IActionResult add_shipping_address()
        {
            return RedirectToAction("shipping_address");
        }
    }
}
