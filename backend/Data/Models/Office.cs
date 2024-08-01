using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotDeskBookingSystem.Data.Models
{
    public class Office
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OfficeId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        [Range(0, 350)]
        public int TotalFloors { get; set; }
        [Required]
        [Range(0, Int16.MaxValue)]
        public int TotalDesks { get; set; }
        public ICollection<OfficeFloor> OfficeFloors { get; set; }
    }
}
