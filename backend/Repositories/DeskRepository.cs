using HotDeskBookingSystem.Data;
using HotDeskBookingSystem.Data.Dto.Booking;
using HotDeskBookingSystem.Data.Dto.Desk;
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

        public async Task<IEnumerable<DeskDto>> GetAllDesksAsync()
        {
            return await _context.Desks
                .Select(d => new DeskDto
                {
                    DeskId = d.DeskId,
                    OfficeId = d.OfficeFloor.OfficeId,
                    OfficeFloorId = d.OfficeFloorId,
                    FloorNumber = d.OfficeFloor.FloorNumber,
                    Bookings = d.Bookings.Select(b => new BookingInfoDto
                    {
                        UserEmail = b.UserEmail,
                        StartTime = b.startTime.ToString("dd/MM/yyyy, HH:mm"),
                        EndTime = b.endTime.ToString("dd/MM/yyyy, HH:mm")
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<Desk>> GetAvailableDesksByOfficeIdAsync(int officeId, ReservationTimesDto reservationTimes)
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
                            && b.BookingStatusName == "BOOKED"
                            && (
                                b.startTime < reservationTimes.End
                                && b.endTime > reservationTimes.Start
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

        public async Task<IEnumerable<Desk>> GetAvailableDesksByOfficeFloorIdAsync(int officeFloorId, ReservationTimesDto reservationTimes)
        {
            if (reservationTimes.End <= reservationTimes.Start && reservationTimes.Start >= DateTime.Now.AddDays(1))
            {
                throw new InvalidInputException("End time must be after start time for a booking to be valid");
            }

            var floor = await _context.Floors
                .FindAsync(officeFloorId);
            if (floor == null)
            {
                throw new NotFoundException($"Office floor with id {officeFloorId} not found");
            }

            var availableDesks = new List<Desk>();
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
                            b.endTime > reservationTimes.Start && b.endTime < reservationTimes.End
                                ||
                            b.startTime > reservationTimes.Start && b.startTime < reservationTimes.End
                        )
                    )
                    .ToListAsync();
                if (bookingsForDesk.Count == 0)
                {
                    availableDesks.Add(desk);
                }
            }
       
            return availableDesks;
        }

        public async Task<DeskDto?> GetDeskByIdAsync(int deskId)
        {
            return await _context.Desks
                .Select(d => new DeskDto
                {
                    DeskId = d.DeskId,
                    OfficeId = d.OfficeFloor.OfficeId,
                    OfficeFloorId = d.OfficeFloorId,
                    FloorNumber = d.OfficeFloor.FloorNumber,
                    Bookings = d.Bookings.Select(b => new BookingInfoDto
                    {
                        UserEmail = b.UserEmail,
                        StartTime = b.startTime.ToString("dd/MM/yyyy, HH:mm"),
                        EndTime = b.endTime.ToString("dd/MM/yyyy, HH:mm")
                    }).ToList()
                })
                .FirstOrDefaultAsync(d => d.DeskId == deskId);
        }

        public async Task<IEnumerable<DeskDto>> GetDesksByOfficeFloorIdAsync(int officeFloorId)
        {
            return await _context.Desks
                .Where(d => d.OfficeFloorId == officeFloorId)
                .Select(d => new DeskDto
                {
                    DeskId = d.DeskId,
                    OfficeId = d.OfficeFloor.OfficeId,
                    OfficeFloorId = d.OfficeFloorId,
                    FloorNumber = d.OfficeFloor.FloorNumber,
                    Bookings = d.Bookings.Select(b => new BookingInfoDto
                    {
                        UserEmail = b.UserEmail,
                        StartTime = b.startTime.ToString("dd/MM/yyyy, HH:mm"),
                        EndTime = b.endTime.ToString("dd/MM/yyyy, HH:mm")
                    }).ToList()
                })
                .ToListAsync();

        }

        public async Task<IEnumerable<DeskDto>> GetDesksByOfficeIdAsync(int officeId)
        {
            var floors = await _context.Floors
                .Where(f => f.OfficeId == officeId)
                .ToListAsync();

            var deskDtos = new List<DeskDto>();

            foreach (var floor in floors)
            {
                var desksPerFloor = await _context.Desks
                    .Where(d => d.OfficeFloorId == floor.OfficeFloorId)
                    .Select(d => new DeskDto
                    {
                        DeskId = d.DeskId,
                        OfficeId = d.OfficeFloor.OfficeId,
                        OfficeFloorId = d.OfficeFloorId,
                        FloorNumber = d.OfficeFloor.FloorNumber,
                        Bookings = d.Bookings.Select(b => new BookingInfoDto
                        {
                            UserEmail = b.UserEmail,
                            StartTime = b.startTime.ToString("dd/MM/yyyy, HH:mm"),
                            EndTime = b.endTime.ToString("dd/MM/yyyy, HH:mm")
                        }).ToList()
                    })
                    .ToListAsync();

                deskDtos.AddRange(desksPerFloor);
            }

            return deskDtos;
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
            if (reservationTimes.End <= reservationTimes.Start )
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
                        b.endTime > reservationTimes.Start && b.endTime < reservationTimes.End
                            ||
                        b.startTime > reservationTimes.Start && b.startTime < reservationTimes.End
                    )
                )
                .ToListAsync();
            return bookingsForDesk.Count == 0;
        }
    }
}
