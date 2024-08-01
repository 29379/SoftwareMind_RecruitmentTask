using System.ComponentModel.DataAnnotations;
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
        [MinLength(8)]
        [StringLength(50)]
        public string HashPassword { get; set; }

        public ICollection<Booking> Bookings { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
