using System.ComponentModel.DataAnnotations;

namespace HotDeskBookingSystem.Data.Models
{
    public class Role
    {
        [Key]
        public string RoleName { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
