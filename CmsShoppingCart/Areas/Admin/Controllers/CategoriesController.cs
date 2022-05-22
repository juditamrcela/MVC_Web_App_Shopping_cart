using CmsShoppingCart.Infrastructure;
using CmsShoppingCart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    [Authorize(Roles ="admin")]
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        //dependency injection
        private readonly CmsSchoppingCartContext context;
        public CategoriesController(CmsSchoppingCartContext context)
        {
            this.context = context;
        }
        //GET /admin/categories
        public async Task<IActionResult> Index()
        {
            return View(await context.Categories.OrderBy(x => x.Sorting).ToListAsync()); ;
        }
        //GET /admin/category/create
        public IActionResult Create() => View();

        //POST /admin/categories/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category) //model binding
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.ToLower().Replace(" ", "-");
                category.Sorting = 100;

                var slug = await context.Pages.FirstOrDefaultAsync(x => x.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Category already exists");
                    return View(category);
                }
                context.Add(category);
                await context.SaveChangesAsync();

                TempData["Success"] = "The category has been added";

                return RedirectToAction("Index");
            }
            return View(category);
        }
        //GET /admin/categories/edit/id
        public async Task<IActionResult> Edit(int id)
        {
            Category category = await context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        //POST /admin/categories/edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Category category) //model binding
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.ToLower().Replace(" ", "-");

                var slug = await context.Categories.Where(x => x.Id != id).FirstOrDefaultAsync(x => x.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Category already exists");
                    return View(category);
                }
                context.Update(category);
                await context.SaveChangesAsync();

                TempData["Success"] = "The categroy has been edited";

                return RedirectToAction("Edit", new { id = category.Id });
            }
            return View(category);
        }
        //GET /admin/categories/delete/id

        public async Task<IActionResult> Delete(int id)
        {
            Category category = await context.Categories.FindAsync(id);
            if (category == null)
            {
                TempData["Error"] = "The category does not exists";
            }
            else
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();

                TempData["Success"] = "The category has been removed";
            }
            return RedirectToAction("Index");
        }
        //POST /admin/categories/reorder
        [HttpPost]
        public async Task<IActionResult> Reorder(int[] id)
        {
            int count = 1;//pocinje od 1 jer je home na 0 a mi zelimo poceti od sljedeceg

            foreach (var categoryId in id)
            {
                Category category = await context.Categories.FindAsync(categoryId);
                category.Sorting = count;
                context.Update(category);
                await context.SaveChangesAsync();
                count++;
            }
            return Ok();
        }


    }
}
