using System.ComponentModel.DataAnnotations;

namespace HotDeskBookingSystem.Data.Models
{
    public class BookingStatus
    {
        [Key]
        [StringLength(50)]
        public string StatusName { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}
