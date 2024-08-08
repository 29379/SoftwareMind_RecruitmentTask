using HotDeskBookingSystem.Data.Dto.Booking;

namespace HotDeskBookingSystem.Data.Dto.Desk
{
    public class DeskDto
    {
        public int DeskId { get; set; }
        public int OfficeId { get; set; }
        public int OfficeFloorId { get; set; }
        public int FloorNumber { get; set; }
        public List<BookingInfoDto> Bookings { get; set; }
    }

}
