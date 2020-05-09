using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proyecto_PAA.Models
{
    public class Role : BaseEntity
    {
        [Key]
        public int RoleId { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}