using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotDeskBookingSystem.Data.Models
{
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookingId { get; set; }
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public int DeskId { get; set; }
        [Required]
        public string BookingStatusName { get; set; }

        [Required]
        public DateTime startTime { get; set; }
        [Required]
        public DateTime endTime { get; set; }
        
        [ForeignKey("DeskId")]
        public Desk Desk { get; set; }
        [ForeignKey("UserEmail")]
        public AppUser User { get; set; }
        [ForeignKey("BookingStatusName")]
        public BookingStatus BookingStatus { get; set; }
    }
}
