using CmsShoppingCart.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmsShoppingCart.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,editor")]
    [Area("Admin")]
    public class PagesController : Controller
    {
  
            private readonly CmsSchoppingCartContext context;

            public PagesController(CmsSchoppingCartContext context)
            {
                this.context = context;
            }
            //GET /admin/pages
            public async Task<IActionResult> Index()
            {
                IQueryable<Page> pages = from p in context.Pages orderby p.Sorting select p;
                List<Page> pagelist = await pages.ToListAsync();

                return View(pagelist);
            }
            //GET /admin/pages/ditails/id
            public async Task<IActionResult> Details(int id)
            {
                Page page = await context.Pages.FirstOrDefaultAsync(x => x.Id == id);
                if(page==null)
                {
                    return NotFound();
                }
                return View(page);
            }
            //GET /admin/pages/create
            public IActionResult Create() => View();
            //POST /admin/pages/create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(Page page) //model binding
            {
                if (ModelState.IsValid)
                {
                    page.Slug = page.Title.ToLower().Replace(" ", "-");
                    page.Sorting = 100;

                    var slug = await context.Pages.FirstOrDefaultAsync(x => x.Slug == page.Slug);
                    if(slug!=null)
                    {
                        ModelState.AddModelError("", "Stranica već postoji");
                        return View(page);
                    }
                    context.Add(page);
                    await context.SaveChangesAsync();

                    TempData["Success"] = "Stranica je dodana";

                    return RedirectToAction("Index");
                }
                return View(page);
            }
            //GET /admin/pages/edit/id
            public async Task<IActionResult> Edit(int id)
            {
                Page page = await context.Pages.FindAsync(id);
                if (page == null)
                {
                    return NotFound();
                }
                return View(page);
            }
            //PUT /admin/pages/edit
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(Page page) //model binding
            {
                if (ModelState.IsValid)
                {
                    page.Slug = page.Id == 1 ? "home" : page.Title.ToLower().Replace(" ", "-");
                    
                    var slug = await context.Pages.Where(x=>x.Id!=page.Id).FirstOrDefaultAsync(x => x.Slug == page.Slug);
                    if (slug != null)
                    {
                        ModelState.AddModelError("", "Page already exists");
                        return View(page);
                    }
                    context.Update(page);
                    await context.SaveChangesAsync();

                    TempData["Success"] = "The page has been edited";

                    return RedirectToAction("Edit",new { id=page.Id});
                }
                return View(page);
            }
            //GET /admin/pages/delete/id
           
            public async Task<IActionResult> Delete(int id) 
            {
                Page page = await context.Pages.FindAsync(id);
                if (page == null)
                {
                    TempData["Error"] = "The page does not exists";
                }
                else
                {
                    context.Pages.Remove(page);
                    await context.SaveChangesAsync();

                    TempData["Success"] = "The page has been removed";
                }
                return RedirectToAction("Index");
            }
            //POST /admin/pages/reorder
            [HttpPost]
           
            public async Task<IActionResult> Reorder (int[] id) 
            {
                int count = 1;//pocinje od 1 jer je home na 0 a mi zelimo poceti od sljedeceg

                foreach(var pageId in id)
                {
                    Page page = await context.Pages.FindAsync(pageId);
                    page.Sorting = count;
                    context.Update(page);
                    await context.SaveChangesAsync();
                    count++;
                }
                return Ok();
            }

        
    }
}
