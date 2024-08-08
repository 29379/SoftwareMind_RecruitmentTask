using HotDeskBookingSystem.Data;
using HotDeskBookingSystem.Data.Dto;
using HotDeskBookingSystem.Data.Models;
using HotDeskBookingSystem.Exceptions;
using HotDeskBookingSystem.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotDeskBookingSystem.Repositories
{
    public class OfficeRepository : IOfficeRepository
    {
        private readonly DataContext _context;

        public OfficeRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Office?> AddOfficeAsync(OfficeDto office)
        {
            var newOffice = new Office
            {
                Name = office.Name,
                City = office.City,
                TotalFloors = office.TotalFloors,
                TotalDesks = office.TotalDesks
            };

            _context.Add(newOffice);
            await _context.SaveChangesAsync();
            return newOffice;
        }

        public async Task<Office?> DeleteOfficeAsync(int officeId)
        {
            var toBeDeleted = await _context.Offices
                .FindAsync(officeId);
            if (toBeDeleted == null)
            {
                throw new NotFoundException($"Office with id {officeId} was not found," +
                    $" so it cannot be deleted.");
            }

            var desksForOfficeCount = 0;
            var floorsForOffice = await _context.Floors
                .Where(f => f.OfficeId == officeId)
                .ToListAsync();
            foreach (var floor in floorsForOffice)
            {
                var desksForFloor = await _context.Desks
                    .Where(d => d.OfficeFloorId == floor.OfficeFloorId)
                    .ToListAsync();
                desksForOfficeCount += desksForFloor.Count;
            }
            if (desksForOfficeCount > 0)
            {
                throw new UnsupportedOperationException($"Cannot delete an office, that has existing floors" +
                    $" with existing desks inside of it.");
            }

            _context.Offices.Remove(toBeDeleted);
            await _context.SaveChangesAsync();
            return toBeDeleted;

        }

        public async Task<IEnumerable<Office>> GetAllOfficesAsync()
        {
            return await _context.Offices
                .ToListAsync();
        }

        public async Task<Office?> GetOfficeByIdAsync(int officeId)
        {
            var office = await _context.Offices
                .FindAsync(officeId);
            if (office == null)
            {
                throw new NotFoundException($"Can't find an office with id {officeId}.");
            }
            return office;
        }

        public async Task<Office?> UpdateOfficeAsync(Office office)
        {
            var toBeUpdated = await _context.Offices
                .FindAsync(office.OfficeId);
            if (toBeUpdated == null)
            {
                throw new NotFoundException($"Office {office.ToString()} was not found, so it cannot" +
                    $" be updated");
            }

            toBeUpdated.Name = office.Name;
            toBeUpdated.City = office.City;
            toBeUpdated.TotalFloors = office.TotalFloors;
            toBeUpdated.TotalDesks = office.TotalDesks;
            foreach (var floor in office.OfficeFloors)
            {
                if (!toBeUpdated.OfficeFloors.Contains(floor))
                {
                    toBeUpdated.OfficeFloors.Add(floor);
                }
            }

            await _context.SaveChangesAsync();
            return toBeUpdated;
        }
    }
}
