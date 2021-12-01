using ITGoShop_F_Ver2.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGoShop_F_Ver2.Controllers
{
    public class OrderManagementController : Controller
    {
        public IActionResult view_order()
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllOrder = context.getAllOrder();
            return View();
        }

        public void update_order_status(int OrderId, string OrderStatus, string PaymentStatus)
        {
            var context = new ITGoShopLINQContext();
            context.updateOrderStatus(OrderId, OrderStatus, PaymentStatus);
        }
    }
}
