using ITGoShop_F_Ver2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

namespace ITGoShop_F_Ver2.Controllers
{
    public class AdminController : Controller
    {
        string adminId = "";
        string adminLastName = "";
        string adminFirstName = "";
        string adminImage = "";

        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string message)
        {
            if(!string.IsNullOrEmpty(message))
            {
                ViewBag.message = message;
            }    
            return View();
        }
        public IActionResult Dashboard(User userInput)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            User userInfo = context.getUserInfo(userInput.Email, userInput.Password);
            if(userInfo != null)
            {
                //System.Diagnostics.Debug.WriteLine("Admin: " + userInfo.UserId);
                HttpContext.Session.SetInt32("adminId", userInfo.UserId);
                HttpContext.Session.SetString("adminLastName", userInfo.LastName);
                HttpContext.Session.SetString("adminFirstName", userInfo.FirstName);
                HttpContext.Session.SetString("adminImage", userInfo.UserImage);

                // Update last login
                context.updateLastLogin(userInfo.UserId);

                // Chuyển dữ liệu admin qua
                ViewBag.numberCustomer = context.countCustomer();
                ViewBag.numberProduct= context.countProduct();
                ViewBag.numberOrder = context.countOrder();
                ViewBag.numberLoginThisYear = context.countLoginThisYear();
                ViewBag.totalRevenueThisMonth = context.getRevenue(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), DateTime.Now);
                ViewBag.numberOrderToday = context.countOrder(DateTime.Now, DateTime.Now);
                ViewBag.numberLoginToday = context.countLogin(DateTime.Now, DateTime.Now);
                //ViewBag.numberLoginToday = context.getRevenue(DateTime.Now, DateTime.Now);
                return View();
            }
            return RedirectToAction("Index", new { message = "Mật khẩu hoặc tài khoản sai. Xin nhập lại!" });
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove(adminId);
            HttpContext.Session.Remove(adminLastName);
            HttpContext.Session.Remove(adminFirstName);
            HttpContext.Session.Remove(adminImage);
            return View("Index");
        }
    }
}
