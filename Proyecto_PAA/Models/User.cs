using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proyecto_PAA.Models
{
    public class User : BaseEntity
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)] // FirstName Varchar(50)
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; } // asijdhasijhdiuas - iojuinvuyishbasui

        public DateTime? BirthDate { get; set; }



        public int? Age
        {
            get
            {
                if (BirthDate != null)
                    return DateTime.Now.Year - BirthDate.Value.Year;

                return null;
            }
        }
        public string FullName => $"{FirstName} {LastName}";
    }
}