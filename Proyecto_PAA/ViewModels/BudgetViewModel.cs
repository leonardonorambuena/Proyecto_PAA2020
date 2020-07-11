using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Proyecto_PAA.Models;

namespace Proyecto_PAA.ViewModels
{
    public class BudgetViewModel
    {
        [Required]
        public int BudgetId { get; set; }
        [Required]
        public int ProductId { get; set; }

        [Required] public int Quantity { get; set; }

        public Budget Budget { get; set; }

        public List<Product> Products { get; set; }
    }
}