using ITGoShop_F_Ver2.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PagedList;
using PagedList.Mvc;

namespace ITGoShop_F_Ver2.Controllers
{
    public class ProductDetailController : Controller
    {
        public IActionResult Index(int productId)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllCategory = context.getAllCategory();
            ViewBag.AllBrand = context.getAllBrand();
            ViewBag.AllSubBrand = context.getAllSubBrand();

            ViewBag.ProductDetail = context.getProductInfo(productId);
            return View("product_detail");
        }
    }
}
