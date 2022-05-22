using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Models
{
    public class UserEdit
    {
       
        [Required, EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password), MinLength(4, ErrorMessage = "Minimalna duljina je 4")]
        [Display(Name ="Šifra")]
        public string Password { get; set; }

        public UserEdit()
        {

        }
        public UserEdit(AppUser appuser)
        {
            
            Email = appuser.Email;
            Password = appuser.PasswordHash;
        }
    }
}
