using HotDeskBookingSystem.Data;
using HotDeskBookingSystem.Data.Dto.Booking;
using HotDeskBookingSystem.Data.Models;
using HotDeskBookingSystem.Exceptions;
using HotDeskBookingSystem.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
            DeskNotAvailable,
            InvalidBookingDates
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

        public async Task<Booking?> BookDeskAsync(CreateBookingDto bookingDto)
        {
            var canBookDesk = await checkIfBookingIsPossible(bookingDto.Email, bookingDto.DeskId, bookingDto.ReservationTimes);
            if (!canBookDesk.Success)
            {
                switch (canBookDesk.FailureReason)
                {
                    case BookingCheckFailureReason.UserNotFound:
                        throw new NotFoundException("User with email: " + bookingDto.Email + " not found");
                    case BookingCheckFailureReason.DeskNotFound:
                        throw new NotFoundException("Desk with ID: " + bookingDto.DeskId + " not found");
                    case BookingCheckFailureReason.DeskNotAvailable:
                        throw new DeskNotAvailableException("Desk with ID: " + bookingDto.DeskId + " not available from " + bookingDto.ReservationTimes.Start + " to " + bookingDto.ReservationTimes.End);
                    case BookingCheckFailureReason.InvalidBookingDates:
                        throw new InvalidDataException($"Invalid booking dates. {bookingDto.ReservationTimes.Start} and {bookingDto.ReservationTimes.End} do not follow the rules of this service.");
                    default:
                        throw new InvalidDataException("An unexpected error occured. Booking the desk with id: "
                            + bookingDto.DeskId + "from " + bookingDto.ReservationTimes.Start + " to " + bookingDto.ReservationTimes.End + " is not possible");
                }
            }
            
            var newBooking = new Booking
            {
                UserEmail = bookingDto.Email,
                DeskId = bookingDto.DeskId,
                BookingStatusName = "BOOKED",
                startTime = new DateTime(bookingDto.ReservationTimes.Start.Year, bookingDto.ReservationTimes.Start.Month, bookingDto.ReservationTimes.Start.Day, 7, 0, 0),
                endTime = new DateTime(bookingDto.ReservationTimes.End.Year, bookingDto.ReservationTimes.End.Month, bookingDto.ReservationTimes.End.Day, 22, 0, 0)
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
            if (toBeChanged.startTime < DateTime.Now.AddHours(24))
            {
                throw new UnsupportedOperationException("Cannot change a booking that is less than 24 hours from now");
            }

            toBeChanged.UserEmail = newBooking.UserEmail;
            toBeChanged.DeskId = newBooking.DeskId;
            toBeChanged.startTime = newBooking.startTime;
            toBeChanged.endTime = newBooking.endTime;
            toBeChanged.BookingStatusName = newBooking.BookingStatusName;
            
            await _context.SaveChangesAsync();
            return toBeChanged;
        }

        public async Task<Booking?> DeleteBookingAutomaticallyAsync(int bookingId, CancellationToken cancellationToken)
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

        public async Task<bool> DeleteBookingManuallyAsync(Booking booking)
        {
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Booking?> ChangeBookingStatus(int bookingId, string newStatus)
        {
            var status = await _context.BookingStatuses
                .FindAsync(newStatus);
            if (status == null)
            {
                throw new NotFoundException("Booking status with name: " + newStatus + "not found");
            }
            var toBeChanged = await _context.Bookings
                .FindAsync(bookingId);
            if (toBeChanged == null)
            {
                throw new NotFoundException("Booking with ID: " + bookingId + " not found");
            }

            toBeChanged.BookingStatusName = newStatus;
            toBeChanged.BookingStatus = status;
            await _context.SaveChangesAsync();
            return toBeChanged;
        }
        //  - - - helper content - - -

        public async Task<BookingCheckResult> checkIfBookingIsPossible(string email, int deskId, ReservationTimesDto reservationTimes)
        {
            if (reservationTimes.End <= reservationTimes.Start)
            {
                return new BookingCheckResult
                {
                    Success = false,
                    Message = $"End time must be later than start time.",
                    FailureReason = BookingCheckFailureReason.InvalidBookingDates
                };
            }

            var reservationStartDate = reservationTimes.Start;
            var reservationEndDate = reservationTimes.End;
            var startDateTime = new DateTime(reservationStartDate.Year, reservationStartDate.Month, reservationStartDate.Day, 7, 0, 0);
            if (startDateTime < DateTime.Now.AddHours(24))
            {
                return new BookingCheckResult
                {
                    Success = false,
                    Message = $"Start time ({startDateTime}) must be at least 24 hours from now ({DateTime.Now})",
                    FailureReason = BookingCheckFailureReason.InvalidBookingDates
                };
            }

            int totalDays = 0;
            for (var date = reservationStartDate; date <= reservationEndDate; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                {
                    totalDays++;
                }
            }
            if (totalDays > 5)
            {
                return new BookingCheckResult
                {
                    Success = false,
                    Message = $"A maximum booking duration length is a week (without weekends, 5 office days). " +
                    $"You tried to make a booking for {totalDays} days.",
                    FailureReason = BookingCheckFailureReason.InvalidBookingDates
                };
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
                    Message = $"Desk with ID '{deskId}' is not available from '{reservationTimes.Start}' to '{reservationTimes.End}'.",
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
