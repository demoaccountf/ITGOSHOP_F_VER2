using ITGoShop_F_Ver2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ITGoShop_F_Ver2.Controllers
{
    public class HomeController : Controller
    {
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
            ViewBag.GiamGiaSoc = linqContext.getGiamGiaSoc();
            return View();
        }

        public IActionResult login()
        {
            return View();
        }

        public IActionResult search_result()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
