using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Models
{
    public class User
    {
        [Required, MinLength(2, ErrorMessage = "Minimalmnu duljina je 2")]
        [Display(Name ="Korisničko ime")]
        public string UserName { get; set; }
        [Required,EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password),Required,MinLength(4,ErrorMessage ="Minimalmnu duljina je 4")]
        [Display(Name = "Šifra")]
        public string Password { get; set; }

       
     
    }
}
