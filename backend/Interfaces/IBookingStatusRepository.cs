using HotDeskBookingSystem.Data.Models;

namespace HotDeskBookingSystem.Interfaces
{
    public interface IBookingStatusRepository
    {
        Task<BookingStatus?> GetBookingStatusByIdAsync(int bookingStatusId);
        Task<IEnumerable<BookingStatus>> GetAllBookingStatusesAsync();
        Task<BookingStatus?> AddBookingStatusAsync(BookingStatus bookingStatus);
        Task<BookingStatus?> UpdateBookingStatusAsync(BookingStatus bookingStatus);
        Task<BookingStatus?> DeleteBookingStatusAsync(int bookingStatusId);
    }
}
