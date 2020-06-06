using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Proyecto_PAA.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int CategoryId { get; set; }

        public int ProductStock { get; set; }

        public int ProductPrice { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        
    }
}