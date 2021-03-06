using CmsShoppingCart.Infrastructure;
using CmsShoppingCart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly CmsSchoppingCartContext context;
        
        public ProductsController(CmsSchoppingCartContext context)
        {
            this.context = context;
            
        }
        //GET /products
        public async Task<IActionResult> Index(int p = 1)
        {
            int pageSize = 6; //dodajem koliko produkata zelim vidjeti na stranici
            var product = context.Products.OrderByDescending(x => x.Id).Skip((p - 1) * pageSize).Take(pageSize);

            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.Products.Count() / pageSize);


            return View(await product.ToListAsync()); ;
        }
        //GET /products/category
        public async Task<IActionResult> ProductsByCategory(string categorySlug,int p = 1)
        {
            Category category = await context.Categories.Where(x => x.Slug == categorySlug).FirstOrDefaultAsync();
            if (category == null)
            {
                return RedirectToAction("Index");
            }
            int pageSize = 6; //dodajem koliko produkata zelim vidjeti na stranici
            var product = context.Products.OrderByDescending(x => x.Id).Where(x=>x.CategoryId==category.Id).Skip((p - 1) * pageSize).Take(pageSize);

            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.Products.Where(x=>x.CategoryId==category.Id).Count() / pageSize);
            ViewBag.CategoryName = category.Name;
            ViewBag.CategorySlug = category.Slug;


            return View(await product.ToListAsync()); ;
        }
    }
}
