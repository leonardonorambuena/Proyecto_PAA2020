using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Proyecto_PAA.Models;

namespace Proyecto_PAA.ViewModels
{
    public class ProductsViewModel
    {
        public int ProductId { get; set; }
        [Required]
        [Display(Name = "Nombre del producto")]
        public string ProductName { get; set; }
        [Required]
        [Display(Name = "Categoría")]
        public int CategoryId { get; set; }
        [Required]
        [Display(Name = "Stock")]
        public int? ProductStock { get; set; }
        [Required]
        [Display(Name = "Precio")]
        public int? ProductPrice { get; set; }

        public List<Product> Products { get; set; }

        public List<Category> Categories { get; set; }
    }
}