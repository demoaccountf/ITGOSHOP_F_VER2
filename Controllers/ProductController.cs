using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITGoShop_F_Ver2.Models;


namespace ITGoShop_F_Ver2.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult product_detail (int productId)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllCategory = context.getAllCategory();
            ViewBag.AllBrand = context.getAllBrand();
            ViewBag.AllSubBrand = context.getAllSubBrand();

            ViewBag.ProductDetail = context.getProductDetail(productId);
            return View();
        }
        public IActionResult product_listing()
        {
            return View();
        }
        public IActionResult product_listing2()
        {
            return View();
        }
        public IActionResult product_listing3()
        {
            return View();
        }
    }
}
