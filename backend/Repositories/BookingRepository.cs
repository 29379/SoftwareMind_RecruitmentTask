using HotDeskBookingSystem.Data;
using HotDeskBookingSystem.Data.Models;
using HotDeskBookingSystem.Exceptions;
using HotDeskBookingSystem.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotDeskBookingSystem.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly DataContext _context; 

        public BookingRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _context.Bookings
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsByBookingStatusAsync(string bookingStatusName)
        {
            var status = await _context.BookingStatuses.FindAsync(bookingStatusName);
            if (status == null)
            {
                throw new NotFoundException("Booking status with name: " + bookingStatusName + "not found");
            }
            return await _context.Bookings
                .Where(b => b.BookingStatusName == bookingStatusName)
                .ToListAsync();
        }

        public async Task<Booking?> GetBookingByIdAsync(int bookingId)
        {
            return await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByDeskIdAsync(int deskId)
        {
            return await _context.Bookings
                .Where(b => b.DeskId == deskId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByUserEmailAsync(string email)
        {
            return await _context.Bookings
                .Where(b => b.UserEmail == email)
                .ToListAsync();
        }

        public Task<Booking?> BookDeskAsync(string email, int deskId, DateTime timeOfReservation)
        {
            throw new NotImplementedException();
        }

        public Task<Booking?> CancelReservationAsync(string email, int bookingId)
        {
            throw new NotImplementedException();
        }

        public Task<Booking?> ChangeReservationAsync(string email, int bookingId, DateTime newTimeOfReservation)
        {
            throw new NotImplementedException();
        }
    }
}
