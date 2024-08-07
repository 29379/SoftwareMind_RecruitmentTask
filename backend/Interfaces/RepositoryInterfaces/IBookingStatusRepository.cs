using HotDeskBookingSystem.Data.Models;

namespace HotDeskBookingSystem.Interfaces.Repositories
{
    public interface IBookingStatusRepository
    {
        Task<BookingStatus?> GetBookingStatusByNameAsync(string bookingStatusName);
        Task<IEnumerable<BookingStatus>> GetAllBookingStatusesAsync();
        Task<BookingStatus?> AddBookingStatusAsync(BookingStatus bookingStatus);
        Task<BookingStatus?> UpdateBookingStatusAsync(BookingStatus bookingStatus);
        Task<BookingStatus?> DeleteBookingStatusAsync(int bookingStatusId);
    }
}
