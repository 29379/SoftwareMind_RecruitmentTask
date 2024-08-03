using HotDeskBookingSystem.Data.Models;
using HotDeskBookingSystem.Interfaces;

namespace HotDeskBookingSystem.Repositories
{
    public class BookingStatusRepository : IBookingStatusRepository
    {
        public Task<BookingStatus?> AddBookingStatusAsync(BookingStatus bookingStatus)
        {
            throw new NotImplementedException();
        }

        public Task<BookingStatus?> DeleteBookingStatusAsync(int bookingStatusId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BookingStatus>> GetAllBookingStatusesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BookingStatus?> GetBookingStatusByIdAsync(int bookingStatusId)
        {
            throw new NotImplementedException();
        }

        public Task<BookingStatus?> UpdateBookingStatusAsync(BookingStatus bookingStatus)
        {
            throw new NotImplementedException();
        }
    }
}
