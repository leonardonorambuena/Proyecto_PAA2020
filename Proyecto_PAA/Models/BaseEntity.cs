using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_PAA.Models
{
    public class BaseEntity
    {
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}