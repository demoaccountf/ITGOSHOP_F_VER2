using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITGoShop_F_Ver2.Models;
using System.Web;
using PagedList;

namespace ITGoShop_F_Ver2.Controllers
{
    public class ProductListingController : Controller
    {
        private ITGoShopContext db = new ITGoShopContext();
        public IActionResult product_listing(int brandId)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllCategory = context.getAllCategory();
            ViewBag.AllBrand = context.getAllBrand();
            ViewBag.AllSubBrand = context.getAllSubBrand();
            ViewBag.SubBrand = context.getSubBrand(brandId);
            ViewBag.Brand = context.getBrand(brandId);
            ViewBag.BrandProduct = context.getBrandProduct(brandId);
            
            return View();
        }
        public IActionResult product_listing2(string categoryId)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllCategory = context.getAllCategory();
            ViewBag.AllBrand = context.getAllBrand();
            ViewBag.AllSubBrand = context.getAllSubBrand();
            ViewBag.CateProduct = context.getCateProduct(categoryId);
            ViewBag.Cate = context.getCate(categoryId);
            return View();
        }
        public IActionResult product_listing3(string subbrandId)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllCategory = context.getAllCategory();
            ViewBag.AllBrand = context.getAllBrand();
            ViewBag.AllSubBrand = context.getAllSubBrand();
            ViewBag.SubProduct = context.getSubProduct(subbrandId);

            
            ViewBag.Brand = context.getSub(subbrandId);

            return View();
        }
        public IActionResult product_listing4(string brandName)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllCategory = context.getAllCategory();
            ViewBag.AllBrand = context.getAllBrand();
            ViewBag.AllSubBrand = context.getAllSubBrand();
            return View();
        }
    }
}
