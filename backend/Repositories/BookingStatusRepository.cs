using HotDeskBookingSystem.Data;
using HotDeskBookingSystem.Data.Models;
using HotDeskBookingSystem.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotDeskBookingSystem.Repositories
{
    public class BookingStatusRepository : IBookingStatusRepository
    {
        private readonly DataContext _context;

        public BookingStatusRepository(DataContext context) {
            _context = context;
        }

        public async Task<IEnumerable<BookingStatus>> GetAllBookingStatusesAsync()
        {
            return await _context.BookingStatuses
                .ToListAsync();
        }

        public async Task<BookingStatus?> GetBookingStatusByNameAsync(string bookingStatusName)
        {
            return await _context.BookingStatuses
                .FirstOrDefaultAsync(b => b.StatusName == bookingStatusName);
        }
        public async Task<BookingStatus?> AddBookingStatusAsync(BookingStatus bookingStatus)
        {
            _context.BookingStatuses.Add(bookingStatus);
            await _context.SaveChangesAsync();
            return bookingStatus;
        }

        public Task<BookingStatus?> UpdateBookingStatusAsync(BookingStatus bookingStatus)
        {
            throw new NotImplementedException();
        }

        public async Task<BookingStatus?> DeleteBookingStatusAsync(int bookingStatusId)
        {
            var toBeDeleted = await _context.BookingStatuses
                .FindAsync(bookingStatusId);
            if (toBeDeleted != null)
            {
                _context.BookingStatuses
                    .Remove(toBeDeleted);
                await _context.SaveChangesAsync();
                return toBeDeleted;
            }
            return null;
        }
    }
}
