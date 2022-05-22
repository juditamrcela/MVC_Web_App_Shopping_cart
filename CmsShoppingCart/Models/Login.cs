using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Models
{
    public class Login
    {
        [Display(Name ="Korisničko ime")]
        public string UserName { get; set; }
        [Required, EmailAddress]
      
        public string Email { get; set; }
        [DataType(DataType.Password), Required, MinLength(4, ErrorMessage = "Minimalna duljina 4")]
        [Display(Name = "Šifra")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
