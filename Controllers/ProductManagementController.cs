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
        public IActionResult add_product()
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
            context.saveProduct(newProduct);
            return RedirectToAction("add_product");

        }

        public IActionResult view_product()
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllProduct = context.getAllProduct();
            return View();
        }
        public void unactive_product(int ProductId)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            context.updateProductStatus(ProductId, 0); 
        }
        public void active_product(int ProductId)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            context.updateProductStatus(ProductId, 1);
        }

        public void delete_product(int ProductId)
        {
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            context.deleteProduct(ProductId);
        }

        public IActionResult update_product(int productId)
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
        public IActionResult save_update_product(Product product, List<IFormFile> productImage)
        {
            //System.Diagnostics.Debug.WriteLine("PImg: " + product.ProductImage +"-"+ product.ProductName);
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
            return RedirectToAction("view_product");
        }
        public IActionResult view_productgallary(int productId)
        {
            var context = new ITGoShopLINQContext();
            ViewBag.ProductGallary = context.getProductGallary(productId);
            ViewBag.ProductId = productId;
            return View();
        }

        public void update_productgallery_status(int GallaryId, int Status)
        {
            var context = new ITGoShopLINQContext();
            context.updateProductGallaryStatus(GallaryId, Status);
        }

        public void delete_productgallery(int GallaryId)
        {
            var context = new ITGoShopLINQContext();
            context.deleteProductGallary(GallaryId);
        }

        [Obsolete]
        public IActionResult add_productgallery(int productId, List<IFormFile> ProductGallary)
        {
            var context = new ITGoShopLINQContext();

            string path = Path.Combine(this.Environment.WebRootPath, "public/images_upload/product-gallary");
            foreach (IFormFile postedFile in ProductGallary)
            {
                ProductGallary productGallary = new ProductGallary()
                {
                    GallaryImage = DateTime.Now.ToString("yyyy_MM_dd_HHmmss_") + postedFile.FileName,
                    GallaryStatus = 1,
                    UserId = (int)HttpContext.Session.GetInt32("adminId"),
                    ProductId = productId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                context.saveProductGallary(productGallary);
                // Lưu file vào project
                string fileName = Path.GetFileName(DateTime.Now.ToString("yyyy_MM_dd_HHmmss_") + postedFile.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
            }
            return RedirectToAction("view_productgallary", new { productId = productId });
        }
    }
}
