using ITGoShop_F_Ver2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MyCardSession.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ITGoShop_F_Ver2.Controllers
{
    public class HomeController : Controller
    {
        string customerId = "";
        string customerLastName = "";
        string customerFirstName = "";
        string customerImage = "";

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllCategory = context.getAllCategory();
            ViewBag.AllBrand = context.getAllBrand();
            ViewBag.AllSubBrand = context.getAllSubBrand();
            ViewBag.AllBlog = context.getAllBlog();
            ViewBag.AllBannerSlider = context.getAllBannerSlider();
            ViewBag.Top3ProductView = context.getTop3ProductView();
            ViewBag.Top3Product = context.get3Product();
            ViewBag.Blog = context.getBlog();
            ViewBag.New = context.get2Blog();
            ViewBag.LTProduct = context.getLTProduct();
            ViewBag.PCProduct = context.getPCProduct();
            ViewBag.PKProduct = context.getPKProduct();

            ViewBag.SliderForHomePage = context.getSliderForHomePage();

            var linqContext = new ITGoShopLINQContext();
            ViewBag.GiamGiaSoc = context.getGiamGiaSoc();
            return View();
        }

        public IActionResult login(string message)
        {
            if (!string.IsNullOrEmpty(ViewBag.message))
            {
                ViewBag.message = message;
            }
            return View();
        }
        public IActionResult check_password(User userInput)
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
            return RedirectToAction("login", new { message = "Mật khẩu hoặc tài khoản sai. Xin nhập lại!" });
        }
        public ActionResult search_result(string kw_submit)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllCategory = context.getAllCategory();
            ViewBag.AllBrand = context.getAllBrand();
            ViewBag.AllSubBrand = context.getAllSubBrand();
            ViewBag.AllBlog = context.getAllBlog();
            var linqContext = new ITGoShopLINQContext();
            ViewData["CurrentFilter"] = kw_submit;
            var blog = from s in linqContext.Blog select s;
            if (!String.IsNullOrEmpty(kw_submit))
            {
                blog = blog.Where(s => s.Author.Contains(kw_submit)
                               || s.Title.Contains(kw_submit)
                               || s.Summary.Contains(kw_submit)); 
            }
            ViewBag.Result2 = blog;
            var product = from s in linqContext.Product select s;
            if (!String.IsNullOrEmpty(kw_submit))
            {
                product = product.Where(s => s.ProductName.Contains(kw_submit)
                               || s.Content.Contains(kw_submit));
            }

            ViewBag.Result1 = product;


            return View();
        }

        private IActionResult View(List<Blog> blogs, List<Product> products)
        {
            throw new NotImplementedException();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public int load_cart_quantity()
        {
            if (SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart") == null)
            {
                return 0;
            }
            List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            return cart.Sum(item => item.Quantity);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove(customerId);
            HttpContext.Session.Remove(customerLastName);
            HttpContext.Session.Remove(customerFirstName);
            HttpContext.Session.Remove(customerImage);
            return RedirectToAction("Index");
        }

    }
}
