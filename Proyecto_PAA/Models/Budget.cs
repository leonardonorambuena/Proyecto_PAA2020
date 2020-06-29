using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Proyecto_PAA.Models
{
    public class Budget
    {
        public int BudgetId { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdateAt { get; set; }

        public int Price { get; set; }

        public int Quantity { get; set; }

        public int Total => Price * Quantity;

    }
}