using ITGoShop_F_Ver2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace ITGoShop_F_Ver2.Controllers
{
    public class BrandManagementController : Controller
    {
        [Obsolete]
        private IHostingEnvironment Environment;

        [Obsolete]
        public BrandManagementController(IHostingEnvironment _environment)
        {
            Environment = _environment;
        }
        public IActionResult add_brand()
        {
            return View();
        }
        [Obsolete]
        public IActionResult SaveBrand(Brand newBrand)
        {

            var context = new ITGoShopLINQContext();
            
            return RedirectToAction("add_product_category");

        }
        public IActionResult view_brand()
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllBrand = context.getAllBrand();
            return View();
        }
        public IActionResult update_brand()
        {
            return View();
        }
        public void unactive_brand(int BrandId)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            context.updateBrandStatus(BrandId, 0);
        }
        public void active_brand(int BrandId)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            context.updateBrandStatus(BrandId, 1);
        }

        public void delete_brand(int BrandId)
        {
            var context = new ITGoShopLINQContext();
            context.deleteBrand(BrandId);
        }
    }
}
