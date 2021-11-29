﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITGoShop_F_Ver2.Models;
using MyCardSession.Helpers;

namespace ITGoShop_F_Ver2.Controllers
{
    public class CartController : Controller
    {
        public IActionResult show_cart()
        {
            /*===Cái này để load layout ===*/
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;
            ViewBag.AllCategory = context.getAllCategory();
            ViewBag.AllBrand = context.getAllBrand();
            ViewBag.AllSubBrand = context.getAllSubBrand();
            /*======*/

            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            if (cart == null)
            {
                ViewBag.total = 0;
                ViewBag.numberItem = 0;
            }   
            else
            {
                ViewBag.numberItem = cart.Sum(item => item.Quantity);
                ViewBag.total = cart.Sum(item => item.Product.Price * item.Quantity);
            }
            return View("cart");
        }

        public void add_to_cart(int ProductId, int Quantity)
        {
            System.Diagnostics.Debug.WriteLine("h: " + ProductId+" " + Quantity);
            ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;

            if (SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart") == null)
            {
                List<CartItem> cart = new List<CartItem>(); //mảng các item
                CartItem newCartItem = new CartItem { Product = context.getProductInfo(ProductId), Quantity = Quantity };
                System.Diagnostics.Debug.WriteLine("nh: " + newCartItem.Product.ProductId + " " + newCartItem.Product.Quantity);
                cart.Add(newCartItem);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
                int index = isExist(ProductId);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new CartItem { Product = context.getProductInfo(ProductId), Quantity = Quantity });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
        }
        public void remove_item(int id)
        {
            List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            int index = isExist(id);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
        }

        public void update_quantity(int ProductId, int newQuantity)
        {
            List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            int index = isExist(ProductId);
            cart[index].Quantity = newQuantity;
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
        }
        //================Code thầy Hùng=============//
        //public IActionResult Buy(int id)
        //{
        //    ITGoShopContext context = HttpContext.RequestServices.GetService(typeof(ITGoShop_F_Ver2.Models.ITGoShopContext)) as ITGoShopContext;

        //    if (SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart") == null)
        //    {
        //        List<CartItem> cart = new List<CartItem>(); //mảng các item
        //        cart.Add(new CartItem { Product = context.find(id), Quantity = 1 });
        //        SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
        //    }
        //    else
        //    {
        //        List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
        //        int index = isExist(id);
        //        if (index != -1)
        //        {
        //            cart[index].Quantity++;
        //        }
        //        else
        //        {
        //            cart.Add(new CartItem { Product = context.find(id), Quantity = 1 });
        //        }
        //        SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
        //    }
        //    return RedirectToAction("Index");
        //}



        private int isExist(int id)
        {
            List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.ProductId.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }
        //============================================
    }
}
