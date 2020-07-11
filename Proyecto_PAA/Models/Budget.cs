using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace Proyecto_PAA.Models
{
    public class Budget : BaseEntity
    {
        public int BudgetId { get; set; }
        [Required]
        public int AuthorId { get; set; }

        public int Total => BudgetProducts?.Sum(x => x.Total) ?? 0;
        [Required]
        public BudgetState BudgetState { get; set; }

        public string BudgetStateString => BudgetState.ToString();

        public List<BudgetProduct> BudgetProducts { get; set; }
        [ForeignKey("AuthorId")]
        public User Author { get; set; }



    }

    public enum BudgetState
    {
        Creado,
        Enviado,
        Aprobado
    }
}