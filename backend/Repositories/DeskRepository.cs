using HotDeskBookingSystem.Data;
using HotDeskBookingSystem.Data.Models;
using HotDeskBookingSystem.Interfaces;
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
            _context.Add(desk);
            await _context.SaveChangesAsync();
            return desk;
        }

        public async Task<Desk?> DeleteDeskAsync(int deskId)
        {
            var toBeDeleted = _context.Desks
                .Find(deskId);
            if (toBeDeleted != null)
            {
                _context.Desks
                    .Remove(toBeDeleted);
                await _context.SaveChangesAsync();
                return toBeDeleted;
            }
            return null;
        }

        public async Task<IEnumerable<Desk>> GetAllDesksAsync()
        {
            return await _context.Desks
                .ToListAsync();
        }

        public async Task<IEnumerable<Desk>> GetAvailableDesksByOfficeIdAsync(int officeId, DateTime startReservation, DateTime endReservation)
        {
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
                                b.endTime > startReservation && b.endTime < endReservation
                                    ||
                                b.startTime > startReservation && b.startTime < endReservation
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
                .FirstOrDefaultAsync(d => d.DeskId == deskId);
        }

        public async Task<IEnumerable<Desk>> GetDesksByOfficeFloorIdAsync(int officeFloorId)
        {
            return await _context.Desks
                .Where(d => d.OfficeFloorId == officeFloorId)
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

        public Task<Desk?> UpdateDeskAsync(Desk desk)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsDeskAvailableAsync(int deskId, DateTime startReservation, DateTime endReservation)
        {
            var bookingsForDesk = await _context.Bookings
                .Where(
                    b => b.DeskId == deskId
                        &&
                    b.BookingStatusName == "BOOKED"
                        &&
                    (
                        b.endTime > startReservation && b.endTime < endReservation
                            ||
                        b.startTime > startReservation && b.startTime < endReservation
                    )
                )
                .ToListAsync();
            return bookingsForDesk.Count == 0;
        }
    }
}
