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
    public class ProductManagementController : Controller
    {
        [Obsolete]
        private IHostingEnvironment Environment;

        [Obsolete]
        public ProductManagementController(IHostingEnvironment _environment)
        {
            Environment = _environment;
        }
        public IActionResult AddProduct()
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllCategory = context.getAllCategory();
            ViewBag.AllBrand = context.getAllBrand();
            ViewBag.AllSubBrand = context.getAllSubBrand();
            ViewBag.AllBannerSlider = context.getAllBannerSlider();
            return View();
        }

        [Obsolete]
        public IActionResult SaveProduct(Product newProduct, List<IFormFile> ProductImage)
        {
            // Lưu ảnh sản phẩm vào trước
            string path = Path.Combine(this.Environment.WebRootPath, "public/images_upload/product");
            foreach (IFormFile postedFile in ProductImage)
            {
                // Lấy tên file
                newProduct.ProductImage = DateTime.Now.ToString("yyyy_MM_dd_HHmmss_") + postedFile.FileName;
                // Lưu file vào project
                string fileName = Path.GetFileName(DateTime.Now.ToString("yyyy_MM_dd_HHmmss_") + postedFile.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
            }

            var context = new ITGoShopLINQContext();
            context.SaveProduct(newProduct);
            return RedirectToAction("AddProduct");

        }
    }
}
