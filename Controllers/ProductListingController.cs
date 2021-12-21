using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITGoShop_F_Ver2.Models;
using X.PagedList;
using System.Linq.Dynamic; // nhúng vào tập tin 
using System.Linq.Expressions; // nhúng vào tập tin 

namespace ITGoShop_F_Ver2.Controllers
{
    public class ProductListingController : Controller
    {
        private ITGoShopContext db = new ITGoShopContext();
        public object product_listing(int? page,int brandId, string sortOrder)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllCategory = context.getAllCategory();
            ViewBag.AllBrand = context.getAllBrand();
            ViewBag.AllSubBrand = context.getAllSubBrand();
            ViewBag.SubBrand = context.getSubBrand(brandId);
            ViewBag.Brand = context.getBrand(brandId);

            var linqContext = new ITGoShopLINQContext();
            var BProduct = linqContext.getBrandProduct(brandId);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var onePageOfProducts = BProduct.ToPagedList(pageNumber, 6); // will only contain 25 products max because of the pageSize
            ViewBag.OnePageOfProducts = onePageOfProducts;

            ViewData["tangdan"] = String.IsNullOrEmpty(sortOrder) ? "tangdan" : "";
            ViewData["giamdan"] = String.IsNullOrEmpty(sortOrder) ? "giamdan" : "";
            ViewData["az"] = String.IsNullOrEmpty(sortOrder) ? "az" : "";
            ViewData["za"] = String.IsNullOrEmpty(sortOrder) ? "za" : "";
            var products = from s in linqContext.getBrandProduct(brandId)
                           select s;
            if (!String.IsNullOrEmpty(sortOrder))
            {
                switch (sortOrder)
                {
                    case "tangdan":
                        products = products.OrderBy(s => s.Price);
                        break;
                    case "giamdan":
                        products = products.OrderByDescending(s => s.Price);
                        break;
                    case "az":
                        products = products.OrderBy(s => s.ProductName);
                        break;
                    case "za":
                        products = products.OrderByDescending(s => s.ProductName);
                        break;
                    default:
                        products = products.OrderByDescending(s => s.ProductName);
                        break;
                }
                ViewBag.OnePageOfProducts = products;
            }


            return View();
        }
        public object product_listing2(int? page,string categoryId)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllCategory = context.getAllCategory();
            ViewBag.AllBrand = context.getAllBrand();
            ViewBag.AllSubBrand = context.getAllSubBrand();
            ViewBag.Cate = context.getCate(categoryId);

            var linqContext = new ITGoShopLINQContext();
            var CateProduct = linqContext.getCateProduct(categoryId);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var onePageOfProducts = CateProduct.ToPagedList(pageNumber, 6); // will only contain 25 products max because of the pageSize

            ViewBag.OnePageOfProducts = onePageOfProducts;

            
            
            return View();
        }
        public object product_listing3(int? page,string subbrandId)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllCategory = context.getAllCategory();
            ViewBag.AllBrand = context.getAllBrand();
            ViewBag.AllSubBrand = context.getAllSubBrand();

            var linqContext = new ITGoShopLINQContext();
            var SubProduct = linqContext.getSubProduct(subbrandId);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var onePageOfProducts = SubProduct.ToPagedList(pageNumber, 6); // will only contain 25 products max because of the pageSize

            ViewBag.OnePageOfProducts = onePageOfProducts;

            ViewBag.Brand = context.getSub(subbrandId);

            return View();
        }
        public IActionResult product_listing4(int? page,string brandName)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllCategory = context.getAllCategory();
            ViewBag.AllBrand = context.getAllBrand();
            ViewBag.AllSubBrand = context.getAllSubBrand();

            var linqContext = new ITGoShopLINQContext();
            var BNProduct = linqContext.getBNProduct(brandName);
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var onePageOfProducts = BNProduct.ToPagedList(pageNumber, 6); // will only contain 25 products max because of the pageSize

            ViewBag.OnePageOfProducts = onePageOfProducts;
            ViewBag.Brand = context.getBrand(brandName);
            return View();
        }
    }
}
