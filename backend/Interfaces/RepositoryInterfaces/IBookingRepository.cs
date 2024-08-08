using HotDeskBookingSystem.Data.Dto.Booking;
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
        Task<Booking?> BookDeskAsync(CreateBookingDto bookingDto);
        Task<Booking?> ChangeReservationAsync(int bookingId, Booking newBooking);
        Task<Booking?> CancelReservationAsync(string email, int bookingId);
        Task<Booking?> DeleteBookingAutomaticallyAsync(int bookingId, CancellationToken cancellationToken);
        Task<bool> DeleteBookingManuallyAsync(Booking booking);
        Task<Booking> ChangeBookingStatus(int bookingId, string newStatus);
    }
}
