using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotDeskBookingSystem.Data.Models
{
    public class OfficeFloor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OfficeFloorId { get; set; }
        [Required]
        public int OfficeId { get; set; }
        [Required]
        [Range(-100, 250)]
        public int FloorNumber { get; set; }
        [Required]
        [Range(0, Int16.MaxValue)]
        public int NumberOfDesks { get; set; }

        [ForeignKey("OfficeId")]
        [JsonIgnore]
        public Office Office { get; set; }
        [JsonIgnore]
        public ICollection<Desk> Desks { get; set; }
    }
}
