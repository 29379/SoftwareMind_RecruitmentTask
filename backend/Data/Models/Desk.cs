using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotDeskBookingSystem.Data.Models
{
    public class Desk
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeskId { get; set; }
        [Required]
        public int OfficeFloorId { get; set; }

        [ForeignKey("OfficeFloorId")]
        public OfficeFloor OfficeFloor { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }
}
