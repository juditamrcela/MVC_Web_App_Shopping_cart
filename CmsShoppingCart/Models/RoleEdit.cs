﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Models
{
    public class RoleEdit
    {
        public IdentityRole Role{get;set;}
        public IEnumerable<AppUser> Member { get; set; }
        public IEnumerable<AppUser> NonMember { get; set; }
        
        public string RoleName { get; set; }
        public string[] AddIds { get; set; }
        public string[] DeleteId { get; set; }

    }
}
