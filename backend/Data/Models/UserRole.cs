using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotDeskBookingSystem.Data.Models
{
    public class UserRole
    {
        [Key, Column(Order = 0)]
        public string UserEmail { get; set; }
        [Key, Column(Order = 1)]
        public string RoleName { get; set; }
        [ForeignKey("UserEmail")]
        public AppUser User { get; set; }
        [ForeignKey("RoleName")]
        public Role Role { get; set; }
    }
}
