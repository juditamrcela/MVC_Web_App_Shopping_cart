using CmsShoppingCart.Infrastructure;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShoppingCart.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required, MinLength(2, ErrorMessage = "Minimalna dužina je 2")]
        [Display(Name = "Naziv")]
        public string Name { get; set; }

        public string Slug { get; set; }
        [Required, MinLength(4, ErrorMessage = "Minimalna dužina je 4")]
        [Display(Name="Opis")]
        public string Description { get; set; }

        //[Column(TypeName="decimal(18,2")]
        [Display(Name = "Cijena")]
        public decimal Price { get; set; }

        [Display(Name="Kategorija")]
        [Range(1,int.MaxValue,ErrorMessage="Morate odabrati kategoriju!")]
        public int CategoryId { get; set; } //Foregin key za kategorije,jedna ketgorija-više proizvoda

        [Required]
        [Display(Name = "Dostupna količina")]
        public int Quantity { get; set; }

        [Display(Name = "Slika proizvoda")]
        public string Image { get; set; }

        //konekcija izmedu tablica za bazu podataka
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [NotMapped] //tako da nema nikakve veze s tablicom u bazi podataka
        //radim rucnu svoju validaciju, ova ugradena nije dobra
        //Moja: [FileExtension]
        [FileExtension]
        public IFormFile ImageUpload { get; set; }
    }
}
