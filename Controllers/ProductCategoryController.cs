using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGoShop_F_Ver2.Controllers
{
    public class ProductCategoryController : Controller
    {
        public IActionResult add_product_category()
        {
            return View();
        }
        public IActionResult all_product_category()
        {
            return View();
        }
        public IActionResult update_product_category()
        {
            return View();
        }
    }
}
