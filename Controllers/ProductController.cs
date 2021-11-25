using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGoShop_F_Ver2.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult product_detail ()
        {
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
