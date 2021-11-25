using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGoShop_F_Ver2.Controllers
{
    public class CartController : Controller
    {
        public IActionResult cart()
        {
            return View();
        }
    }
}
