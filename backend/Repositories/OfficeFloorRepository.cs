using HotDeskBookingSystem.Data;
using HotDeskBookingSystem.Data.Dto;
using HotDeskBookingSystem.Data.Models;
using HotDeskBookingSystem.Exceptions;
using HotDeskBookingSystem.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotDeskBookingSystem.Repositories
{
    public class OfficeFloorRepository : IOfficeFloorRepository
    {
        private readonly DataContext _context;
    
        public OfficeFloorRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<OfficeFloor?> AddOfficeFloorAsync(OfficeFloorDto officeFloor)
        {
            var office = await _context.Floors
                .FirstOrDefaultAsync(f => f.OfficeId == officeFloor.OfficeId);
            if (office == null)
            {
                throw new NotFoundException($"Office with id {officeFloor.OfficeId} was not found.");
            }

            var floor = new OfficeFloor
            {
                OfficeId = officeFloor.OfficeId,
                FloorNumber = officeFloor.FloorNumber,
                NumberOfDesks = officeFloor.NumberOfDesks
            };
            _context.Floors.Add(floor);
            await _context.SaveChangesAsync();
            return floor;
        }

        public async Task<OfficeFloor?> DeleteOfficeFloorAsync(int officeFloorId)
        {
            var toBeDeleted = await _context.Floors
                .FindAsync(officeFloorId);
            if (toBeDeleted == null)
            {
                throw new NotFoundException($"Office floor with id {officeFloorId} was not found " +
                    $"and can't be deleted");
            }

            var desksForFloor = await _context.Desks
                .Where(d => d.OfficeFloorId == officeFloorId)
                .ToListAsync();
            if (desksForFloor.Count > 0)
            {
                throw new UnsupportedOperationException($"Can't delete a floor that has " +
                    $"{desksForFloor.Count} existing desks in it.");
            }
            _context.Floors.Remove(toBeDeleted);
            await _context.SaveChangesAsync();
            return toBeDeleted;
        }

        public async Task<IEnumerable<OfficeFloor>> GetAllOfficeFloorsByOfficeIdAsync(int officeId)
        {
            return await _context.Floors
                .Where(f => f.OfficeId == officeId)
                .ToListAsync();
        }

        public async Task<OfficeFloor?> GetOfficeFloorByIdAsync(int officeFloorId)
        {
            var floor = await _context.Floors
                .FindAsync(officeFloorId);
            if (floor == null)
            {
                throw new NotFoundException($"Office floor with id {officeFloorId} was not found.");
            }
            return floor;
        }

        public async Task<OfficeFloor?> UpdateOfficeFloorAsync(OfficeFloor officeFloor)
        {
            var toBeUpdated = await _context.Floors
                .FindAsync(officeFloor.OfficeFloorId);
            if (toBeUpdated == null)
            {
                throw new NotFoundException($"Office floor {officeFloor.ToString()} was not found, " +
                    $"so it cannot be updated");
            }

            toBeUpdated.NumberOfDesks = officeFloor.NumberOfDesks;
            toBeUpdated.FloorNumber = officeFloor.FloorNumber;
            foreach(var desk in officeFloor.Desks)
            {
                if (!toBeUpdated.Desks.Contains(desk))
                {
                    toBeUpdated.Desks.Add(desk);
                }
            }

            await _context.SaveChangesAsync();
            return toBeUpdated;
        }
    }
}
