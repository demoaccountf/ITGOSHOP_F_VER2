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
    public class CategoryManagementController : Controller
    {
        [Obsolete]
        private IHostingEnvironment Environment;

        [Obsolete]
        public CategoryManagementController(IHostingEnvironment _environment)
        {
            Environment = _environment;
        }
        public IActionResult add_product_category()
        {
            
            return View();
        }

        [Obsolete]
        public IActionResult SaveCategory(Category newCate)
        {

            var context = new ITGoShopLINQContext();
            context.saveCategory(newCate);
            return RedirectToAction("add_product_category");

        }
        public IActionResult all_product_category()
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllCategory = context.getAllCategory();
            return View();
        }
        public IActionResult update_category()
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

        public IActionResult update_category(int productId)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.ProductInfo = context.getProductInfo(productId);
            ViewBag.AllCategory = context.getAllCategory();
            ViewBag.AllBrand = context.getAllBrand();
            ViewBag.AllSubBrand = context.getAllSubBrand();
            ViewBag.AllBannerSlider = context.getAllBannerSlider();
            return View();
        }

        [Obsolete]
        public IActionResult save_update_category(Product product, List<IFormFile> productImage)
        {
            System.Diagnostics.Debug.WriteLine("PImg: " + product.ProductImage + "-" + product.ProductName);
            if (!string.IsNullOrEmpty(product.ProductImage))
            {
                //Lưu ảnh sản phẩm vào trước
                string path = Path.Combine(this.Environment.WebRootPath, "public/images_upload/product");
                foreach (IFormFile postedFile in productImage)
                {
                    // Lấy tên file
                    product.ProductImage = DateTime.Now.ToString("yyyy_MM_dd_HHmmss_") + postedFile.FileName;
                    // Lưu file vào project
                    string fileName = Path.GetFileName(DateTime.Now.ToString("yyyy_MM_dd_HHmmss_") + postedFile.FileName);
                    using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                    }
                }
            }
            var context = new ITGoShopLINQContext();
            context.updateProduct(product);
            return RedirectToAction("all_product_category");

        }
    }
}
