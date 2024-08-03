using HotDeskBookingSystem.Data.Models;

namespace HotDeskBookingSystem.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking?> GetBookingByIdAsync(int bookingId);
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<IEnumerable<Booking>> GetBookingsByUserEmailAsync(string email);
        Task<IEnumerable<Booking>> GetBookingsByDeskIdAsync(int deskId);
        Task<IEnumerable<Booking>> GetAllBookingsByBookingStatusAsync(string bookingStatusName);
        Task<Booking?> BookDeskAsync(string email, int deskId, DateTime timeOfReservation);
        Task<Booking?> ChangeReservationAsync(string email, int bookingId, DateTime newTimeOfReservation);
        Task<Booking?> CancelReservationAsync(string email, int bookingId);
    }
}
