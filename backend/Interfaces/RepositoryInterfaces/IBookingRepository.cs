using HotDeskBookingSystem.Data.Models;

namespace HotDeskBookingSystem.Interfaces.Repositories
{
    public interface IBookingRepository
    {
        Task<Booking?> GetBookingByIdAsync(int bookingId);
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<IEnumerable<Booking>> GetBookingsByUserEmailAsync(string email);
        Task<IEnumerable<Booking>> GetBookingsByDeskIdAsync(int deskId);
        Task<IEnumerable<Booking>> GetAllBookingsByBookingStatusAsync(string bookingStatusName);
        Task<Booking?> BookDeskAsync(string email, int deskId, DateTime startOfReservation, DateTime endOfReservation);
        Task<Booking?> ChangeReservationAsync(int bookingId, Booking newBooking);
        Task<Booking?> CancelReservationAsync(string email, int bookingId);
        Task<Booking?> DeleteBookingAsync(int bookingId, CancellationToken cancellationToken);
    }
}
