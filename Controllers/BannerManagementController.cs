using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGoShop_F_Ver2.Controllers
{
    public class BannerManagementController : Controller
    {
        public IActionResult view_banner_slider()
        {
            return View();
        }
        public IActionResult add_banner_slider()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
