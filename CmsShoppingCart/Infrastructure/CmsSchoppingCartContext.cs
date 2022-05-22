using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmsShoppingCart.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CmsShoppingCart.Infrastructure
{
    public class CmsSchoppingCartContext : IdentityDbContext<AppUser>
    {
        public CmsSchoppingCartContext(DbContextOptions<CmsSchoppingCartContext> options)
            :base(options)
        {

        }
        public DbSet<Page> Pages { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
       
    }
}
