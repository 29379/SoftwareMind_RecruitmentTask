using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
namespace HotDeskBookingSystem.Data.Models
{
    public class AppUser
    {
        [Key]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(100)]
        public string LastName { get; set; }
        [Required]
        [JsonIgnore]
        public string HashPassword { get; set; }

        [JsonIgnore]
        public ICollection<Booking> Bookings { get; set; }
        [JsonIgnore]
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
