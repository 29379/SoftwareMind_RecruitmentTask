using HotDeskBookingSystem.Data;
using HotDeskBookingSystem.Data.Dto;
using HotDeskBookingSystem.Data.Models;
using HotDeskBookingSystem.Exceptions;
using HotDeskBookingSystem.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotDeskBookingSystem.Repositories
{
    public class DeskRepository : IDeskRepository
    {
        private readonly DataContext _context;

        public DeskRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Desk?> AddDeskAsync(Desk desk)
        {
            var floor = await _context.Floors
                .FindAsync(desk.OfficeFloorId);
            if (floor == null)
            {
                throw new NotFoundException($"Office floor with id {desk.OfficeFloorId} not found");
            }
            _context.Add(desk);
            await _context.SaveChangesAsync();
            return desk;
        }

        public async Task<Desk?> DeleteDeskAsync(int deskId)
        {
            var toBeDeleted = await _context.Desks
                .FindAsync(deskId);
            if (toBeDeleted == null)
            {
                throw new NotFoundException($"Desk with id {deskId} not found");
            }

            var activeBookingsForDesk = await _context.Bookings
                .Where(b => b.DeskId == deskId && b.BookingStatusName == "BOOKED")
                .ToListAsync();
            if (activeBookingsForDesk.Count > 0)
            {
                throw new UnsupportedOperationException($"Desk with id {deskId} has " +
                    $"{activeBookingsForDesk.Count} active bookings, so it cannot be deleted");
            }

            _context.Desks.Remove(toBeDeleted);
            await _context.SaveChangesAsync();
            return toBeDeleted;
        }

        public async Task<IEnumerable<Desk>> GetAllDesksAsync()
        {
            return await _context.Desks
                .Include(d => d.OfficeFloor)
                .ThenInclude(f => f.Office)
                .ToListAsync();
        }

        public async Task<IEnumerable<Desk>> GetAvailableDesksByOfficeIdAsync(int officeId, ReservationTimesDto reservationTimes)
        {
            if (reservationTimes.end <= reservationTimes.start && reservationTimes.start >= DateTime.Now.AddDays(1)) {
                throw new InvalidInputException("End time must be after start time for a booking to be valid");
            }

            var floors = await _context.Floors
                .Where(f => f.OfficeId == officeId)
                .ToListAsync();
            var availableDesks = new List<Desk>();
            foreach (var floor in floors)
            {
                var desksPerFloor = await _context.Desks
                    .Where(d => d.OfficeFloorId == floor.OfficeFloorId)
                    .ToListAsync();

                foreach (var desk in desksPerFloor)
                {
                    var bookingsForDesk = await _context.Bookings
                        .Where(
                            b => b.DeskId == desk.DeskId
                                && 
                            b.BookingStatusName == "BOOKED"
                                &&
                            (
                                b.endTime > reservationTimes.start && b.endTime < reservationTimes.end
                                    ||
                                b.startTime > reservationTimes.start && b.startTime < reservationTimes.end
                            )
                        )
                        .ToListAsync();
                    if (bookingsForDesk.Count == 0)
                    {
                        availableDesks.Add(desk);
                    }
                }
            }
            return availableDesks;
        }

        public async Task<Desk?> GetDeskByIdAsync(int deskId)
        {
            return await _context.Desks
                .Include(d => d.OfficeFloor)
                .ThenInclude(f => f.Office)
                .Include(d => d.Bookings)
                .ThenInclude(b => b.User.Email)
                .FirstOrDefaultAsync(d => d.DeskId == deskId);
        }

        public async Task<IEnumerable<Desk>> GetDesksByOfficeFloorIdAsync(int officeFloorId)
        {
            return await _context.Desks
                .Where(d => d.OfficeFloorId == officeFloorId)
                .Include(d => d.OfficeFloor)
                .ThenInclude(f => f.Office)
                .ToListAsync();
        }

        public async Task<IEnumerable<Desk>> GetDesksByOfficeIdAsync(int officeId)
        {
            var floors = await _context.Floors
                .Where(f => f.OfficeId == officeId)
                .ToListAsync();

            var desks = new List<Desk>();
            foreach (var floor in floors)
            {
                var desksPerFloor = await _context.Desks
                    .Where(d => d.OfficeFloorId == floor.OfficeFloorId)
                    .ToListAsync();
                desks.AddRange(desksPerFloor);
            }
            return desks;
        }

        public async Task<Desk?> UpdateDeskAsync(Desk desk)
        {
            var existingDesk = await _context.Desks
                .FindAsync(desk.DeskId);
            if (existingDesk == null)
            {
                throw new NotFoundException($"Desk with id {desk.DeskId} not found");
            }
            var existingOfficeFloor = await _context.Floors
                .FindAsync(desk.OfficeFloorId);
            if (existingOfficeFloor == null)
            {
                throw new NotFoundException($"Office floor with id {desk.OfficeFloorId} not found");
            }

            var activeBookingsForDesk = await _context.Bookings
                .Where(b => b.DeskId == desk.DeskId && b.BookingStatusName == "BOOKED")
                .ToListAsync();
                        if (activeBookingsForDesk.Count > 0)
                        {
                            throw new UnsupportedOperationException($"Desk with id {desk.DeskId} has " +
                                $"{activeBookingsForDesk.Count} active bookings, so it cannot be updated");
                        }

            existingDesk.OfficeFloorId = desk.OfficeFloorId;
            existingDesk.Bookings = desk.Bookings;
            await _context.SaveChangesAsync();
            return existingDesk;
        }

        public async Task<bool> IsDeskAvailableAsync(int deskId, ReservationTimesDto reservationTimes)
        {
            if (reservationTimes.end <= reservationTimes.start )
            {
                throw new InvalidInputException("End time must be after start time for a booking to be valid");
            }
            var bookingsForDesk = await _context.Bookings
                .Where(
                    b => b.DeskId == deskId
                        &&
                    b.BookingStatusName == "BOOKED"
                        &&
                    (
                        b.endTime > reservationTimes.start && b.endTime < reservationTimes.end
                            ||
                        b.startTime > reservationTimes.start && b.startTime < reservationTimes.end
                    )
                )
                .ToListAsync();
            return bookingsForDesk.Count == 0;
        }
    }
}
