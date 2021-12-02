using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGoShop_F_Ver2.Controllers
{
    public class BrandManagementController : Controller
    {
        public IActionResult add_brand()
        {
            return View();
        }
        public IActionResult view_brand()
        {
            return View();
        }
        public IActionResult update_brand()
        {
            return View();
        }
        public void unactive_category(string CategoryId)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            context.updateCateStatus(CategoryId, 0);
        }
        public void active_category(string CategoryId)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            context.updateCateStatus(CategoryId, 1);
        }

        public void delete_category(string CategoryId)
        {
            var context = new ITGoShopLINQContext();
            context.deleteCategory(CategoryId);
        }
    }
}
