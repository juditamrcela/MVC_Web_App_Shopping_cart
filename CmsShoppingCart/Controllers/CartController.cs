﻿using CmsShoppingCart.Infrastructure;
using CmsShoppingCart.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Controllers
{
    public class CartController : Controller
    {
        private readonly CmsSchoppingCartContext context;
        public CartController(CmsSchoppingCartContext context)
        {
            this.context = context;
        }
        //GET /cart
        public IActionResult Index()
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartViewModel cartVM = new CartViewModel
            {
                CartItems = cart,
                GrandTotal = cart.Sum(x => x.Price * x.Quantity)
            };
            return View(cartVM);
        }
        //GET /cart/add/id
        public async Task<IActionResult> Add(int id)
        {
            Product product = await context.Products.FindAsync(id);
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            CartItem cartItem = cart.Where(x => x.ProductId == id).FirstOrDefault();
            if (cartItem == null)
            {
                cart.Add(new CartItem(product));
            }
            else
            {
                cartItem.Quantity += 1;
                product.Quantity--;
                context.SaveChanges();
            }
            HttpContext.Session.SetJson("Cart", cart);
            if(HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                return RedirectToAction("Index");

            return ViewComponent("SmallCart");
            
        }
        //GET /cart/decrease/id
        public async Task<IActionResult> Decrease(int id)
        {
            Product product = await context.Products.FindAsync(id);
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            CartItem cartItem = cart.Where(x => x.ProductId == id).FirstOrDefault();
            if (cartItem.Quantity > 1)
            {
                --cartItem.Quantity;
                product.Quantity++;
                context.SaveChanges();
            }
            else
            {
                cart.RemoveAll(x => x.ProductId == id);
            }
         
            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }
        //GET /cart/remove/id
        public IActionResult Remove(int id)
        {

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            cart.RemoveAll(x => x.ProductId == id);

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }
        //GET /cart/clear
        public IActionResult Clear()
        {
            HttpContext.Session.Remove("Cart");



            // return RedirectToAction("Page", "Pages");
            //return Redirect("/");
            if (HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                return Redirect(Request.Headers["Referer"].ToString());//vrati me na stranicu na kojoj sam predhodno bila

            return Ok();
        }
        
        //public IActionResult Checkout()
        //{
        //    List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

        //    //Quentity +/- Database

        //    for (int i = 0; i < cart.Count; i++)
        //    {

        //        var t = context.Products.Single(x => x.Id == cart[i].ProductId);


        //        if (t?.Quantity > 0)
        //        {
        //            t.Quantity = (t.Quantity - cart[i].Quantity);

        //            context.SaveChanges();
        //        }
        //    }
        //    return Ok();
        //}
    }
}
