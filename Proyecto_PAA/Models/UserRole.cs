using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_PAA.Models
{
    public class UserRole
    {
        [Key]
        public int UserRoleId { get; set; }
        [Required]
        public int UserId { get; set; } // not null
        [Required]
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}