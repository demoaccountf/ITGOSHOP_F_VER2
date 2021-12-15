using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITGoShop_F_Ver2.Models;
using X.PagedList;

namespace ITGoShop_F_Ver2.Controllers
{
    public class ProductListingController : Controller
    {
        private ITGoShopContext db = new ITGoShopContext();
        public ActionResult product_listing(int? page,int brandId)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllCategory = context.getAllCategory();
            ViewBag.AllBrand = context.getAllBrand();
            ViewBag.AllSubBrand = context.getAllSubBrand();
            ViewBag.SubBrand = context.getSubBrand(brandId);
            ViewBag.Brand = context.getBrand(brandId);

            var pageNumber = page ?? 1;
            var pageSize = 3; //Show 10 rows every time
            var linqContext = new ITGoShopLINQContext();
            var BProduct = linqContext.getBrandProduct(brandId).ToPagedList(pageNumber, pageSize);

            
            return View(BProduct);
        }
        public IActionResult product_listing2(int? page,string categoryId)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllCategory = context.getAllCategory();
            ViewBag.AllBrand = context.getAllBrand();
            ViewBag.AllSubBrand = context.getAllSubBrand();
            ViewBag.Cate = context.getCate(categoryId);

            var pageNumber = page ?? 1;
            var pageSize = 9; //Show 10 rows every time
            var linqContext = new ITGoShopLINQContext();
            var CateProduct = linqContext.getCateProduct(categoryId).ToPagedList(pageNumber, pageSize);
            
            return View(CateProduct);
        }
        public IActionResult product_listing3(int? page,string subbrandId)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllCategory = context.getAllCategory();
            ViewBag.AllBrand = context.getAllBrand();
            ViewBag.AllSubBrand = context.getAllSubBrand();
            var pageNumber = page ?? 1;
            var pageSize = 9; //Show 10 rows every time
            var linqContext = new ITGoShopLINQContext();
            var SubProduct = linqContext.getSubProduct(subbrandId).ToPagedList(pageNumber, pageSize);

            
            ViewBag.Brand = context.getSub(subbrandId);

            return View(SubProduct);
        }
        public IActionResult product_listing4(int? page,string brandName)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllCategory = context.getAllCategory();
            ViewBag.AllBrand = context.getAllBrand();
            ViewBag.AllSubBrand = context.getAllSubBrand();
            return View();
        }
    }
}
