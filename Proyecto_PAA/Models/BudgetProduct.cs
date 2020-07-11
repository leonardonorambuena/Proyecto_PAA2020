﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Proyecto_PAA.Models
{
    public class BudgetProduct
    {
        [Key]
        public int BudgetProductId { get; set; }

        public int ProductId { get; set; }


        public int BudgetId { get; set; }


        public int Price { get; set; }

        public int Quantity { get; set; }

        public int Total => Price * Quantity;


        [ForeignKey("BudgetId")]
        public Budget Budget { get; set; }
    
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}