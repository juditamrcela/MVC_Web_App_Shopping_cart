using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CmsShoppingCart.Models
{
    public class Page
    {
        public int Id { get; set; }
        [Required, MinLength(2,ErrorMessage ="Minimalna dužina je 2")]
        [Display(Name = "Naslov")]
        public string Title { get; set; }
       
        public string Slug { get; set; }
        [Required, MinLength(4, ErrorMessage = "Minimalna dužina je 4")]
        [Display(Name ="Sadržaj")]
        public string Content { get; set; }
        [Display(Name = "Sortiranje")]
        public int Sorting { get; set; }
    }
}
