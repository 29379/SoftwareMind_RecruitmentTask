namespace HotDeskBookingSystem.Data.Dto.Booking
{
    public class CreateBookingDto
    {
        public string Email { get; set; }
        public int DeskId { get; set; }
        public ReservationTimesDto ReservationTimes { get; set; }
    }
}
