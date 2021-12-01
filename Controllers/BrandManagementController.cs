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
    }
}
