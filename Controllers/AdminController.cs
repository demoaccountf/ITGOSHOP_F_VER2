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
                DateTime homNay = DateTime.Now;
                DateTime dauThangNay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                int soNgayThangTruoc = DateTime.DaysInMonth(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month) - 1;
                DateTime cuoiThangTruoc = ((new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).AddMonths(-1)).AddDays(soNgayThangTruoc);
                DateTime dauThangTruoc = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).AddMonths(-1);
                DateTime dauNamNay = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).AddYears(-1);

                ViewBag.numberCustomer = context.countCustomer();
                ViewBag.numberProduct= context.countProduct();
                ViewBag.numberOrder = context.countOrder();
                ViewBag.numberLoginThisYear = context.countLoginThisYear();
                ViewBag.totalRevenueThisMonth = context.getRevenue(dauThangNay, homNay);
                ViewBag.numberOrderToday = context.countOrder(homNay, homNay);
                ViewBag.numberLoginToday = context.countLogin(homNay, homNay);
                ViewBag.topProducts = context.getTopProduct(dauThangTruoc, homNay);
                ViewBag.topBlogView = context.getTopBlogView();
                ViewBag.topProductView = context.getTopProductView();
                ViewBag.inventoryList = context.getInventoryList();
                ViewBag.numberLoginThangNay = context.countLogin(dauThangNay, homNay);
                ViewBag.numberLoginThangTruoc = context.countLogin(dauThangTruoc, cuoiThangTruoc);
                ViewBag.numberLoginNamNay = context.countLogin(dauNamNay, homNay);
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
