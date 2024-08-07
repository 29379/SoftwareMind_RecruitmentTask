using HotDeskBookingSystem.Data;
using HotDeskBookingSystem.Data.Dto;
using HotDeskBookingSystem.Data.Models;
using HotDeskBookingSystem.Exceptions;
using HotDeskBookingSystem.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotDeskBookingSystem.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly DataContext _context; 
        private readonly IDeskRepository _deskRepository;

        public BookingRepository(DataContext context, IDeskRepository deskRepository)
        {
            _context = context;
            _deskRepository = deskRepository;
        }

        public class BookingCheckResult
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public BookingCheckFailureReason FailureReason { get; set; }
        }

        public enum BookingCheckFailureReason
        {
            None,
            UserNotFound,
            DeskNotFound,
            DeskNotAvailable
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

        public async Task<Booking?> BookDeskAsync(string email, int deskId, ReservationTimesDto reservationTimes)
        {
            var canBookDesk = await checkIfBookingIsPossible(email, deskId, reservationTimes);
            if (!canBookDesk.Success)
            {
                switch (canBookDesk.FailureReason)
                {
                    case BookingCheckFailureReason.UserNotFound:
                        throw new NotFoundException("User with email: " + email + " not found");
                    case BookingCheckFailureReason.DeskNotFound:
                        throw new NotFoundException("Desk with ID: " + deskId + " not found");
                    case BookingCheckFailureReason.DeskNotAvailable:
                        throw new DeskNotAvailableException("Desk with ID: " + deskId + " not available from " + reservationTimes.start + " to " + reservationTimes.end);
                    default:
                        throw new InvalidDataException("An unexpected error occured. Booking the desk with id: "
                            + deskId + "from " + reservationTimes.start + " to " + reservationTimes.end + " is not possible");
                }
            }
            var newBooking = new Booking
            {
                UserEmail = email,
                DeskId = deskId,
                BookingStatusName = "BOOKED",
                startTime = reservationTimes.start,
                endTime = reservationTimes.end
            };
            _context.Bookings.Add(newBooking);
            await _context.SaveChangesAsync();
            return newBooking;
        }

        public async Task<Booking?> CancelReservationAsync(string email, int bookingId)
        {
            var toBeCanceled = await _context.Bookings
                .FindAsync(bookingId);
            if (toBeCanceled != null)
            {
                toBeCanceled.BookingStatusName = "CANCELED";
                await _context.SaveChangesAsync();
                return toBeCanceled;
            }
            return null;
        }

        public async Task<Booking?> ChangeReservationAsync(int bookingId, Booking newBooking)
        {
            var toBeChanged = await _context.Bookings
                .FindAsync(bookingId);
            if (toBeChanged == null)
            {
                throw new NotFoundException("Booking with ID: " + bookingId + " not found");
            }
            toBeChanged.UserEmail = newBooking.UserEmail;
            toBeChanged.DeskId = newBooking.DeskId;
            toBeChanged.startTime = newBooking.startTime;
            toBeChanged.endTime = newBooking.endTime;
            toBeChanged.BookingStatusName = newBooking.BookingStatusName;
            await _context.SaveChangesAsync();
            return toBeChanged;
        }

        public async Task<Booking?> DeleteBookingAsync(int bookingId, CancellationToken cancellationToken)
        {
            var toBeDeleted = await _context.Bookings
                .FindAsync(bookingId);
            if (toBeDeleted != null)
            {
                _context.Bookings.Remove(toBeDeleted);
                await _context.SaveChangesAsync();
                return toBeDeleted;
            }
            return null;
        } 

        //  - - - helper content - - -

        public async Task<BookingCheckResult> checkIfBookingIsPossible(string email, int deskId, ReservationTimesDto reservationTimes)
        {
            if (reservationTimes.end <= reservationTimes.start && reservationTimes.start >= DateTime.Now.AddDays(1))
            {
                throw new InvalidInputException("End time must be after start time, and start time must be" +
                    " at least 24 hours from now for a booking to be valid");
            }

            var user = await _context.Users
                .FindAsync(email);
            if (user == null)
            {
                return new BookingCheckResult
                {
                    Success = false,
                    Message = $"User with email '{email}' not found.",
                    FailureReason = BookingCheckFailureReason.UserNotFound
                };
            }
            var desk = await _context.Desks.FindAsync(deskId);
            if (desk == null)
            {
                return new BookingCheckResult
                {
                    Success = false,
                    Message = $"Desk with ID '{deskId}' not found.",
                    FailureReason = BookingCheckFailureReason.DeskNotFound
                };
            }

            bool isAvailable = await _deskRepository
                .IsDeskAvailableAsync(deskId, reservationTimes);
            if (!isAvailable)
            {
                return new BookingCheckResult
                {
                    Success = false,
                    Message = $"Desk with ID '{deskId}' is not available from '{reservationTimes.start}' to '{reservationTimes.end}'.",
                    FailureReason = BookingCheckFailureReason.DeskNotAvailable
                };
            }
            return new BookingCheckResult
            {
                Success = true,
                Message = "Booking is possible.",
                FailureReason = BookingCheckFailureReason.None
            };
        }
    }
}
