using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITGoShop_F_Ver2.Models;


namespace ITGoShop_F_Ver2.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult checkout()
        {
            return View();
        }
        public IActionResult login_to_checkout()
        {
            return View();
        }
        public IActionResult shipping_address()
        {
            return View();
        }
    }
}
