using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmsShoppingCart.Infrastructure;
using Microsoft.EntityFrameworkCore;
using CmsShoppingCart.Models;
using Microsoft.AspNetCore.Identity;

namespace CmsShoppingCart
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //dodajem da mogu koristiti sesije
            services.AddMemoryCache();
            services.AddSession(options=> 
            {
                //options.IdleTimeout = TimeSpan.FromSeconds(2) ako zelim da se zatvori sesija nakon 2 sec
                
            
            });
            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddControllersWithViews();

            services.AddDbContext<CmsSchoppingCartContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CmsSchoppingCartContext")));

            services.AddIdentity<AppUser, IdentityRole>(options=> {
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<CmsSchoppingCartContext>().AddDefaultTokenProviders();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            //ovo isto dodajem za sesije
            app.UseSession();
            //za identity
            app.UseAuthentication();
            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "pages",
                    "{slug?}",
                    defaults:new {controller="Pages",action="Page"}
                    );
                endpoints.MapControllerRoute(
                    "products",
                    "products/{categorySlug}",
                    defaults: new { controller = "Products", action = "ProductsByCategory" }
                    );
                endpoints.MapControllerRoute(
                    name: "areas",
                   pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                    );
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
