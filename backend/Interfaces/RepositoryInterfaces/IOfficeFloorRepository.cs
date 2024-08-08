using HotDeskBookingSystem.Data.Dto;
using HotDeskBookingSystem.Data.Models;

namespace HotDeskBookingSystem.Interfaces.Repositories
{
    public interface IOfficeFloorRepository
    {
        Task<OfficeFloor?> GetOfficeFloorByIdAsync(int officeFloorId);
        Task<IEnumerable<OfficeFloor>> GetAllOfficeFloorsByOfficeIdAsync(int officeId);
        Task<OfficeFloor?> AddOfficeFloorAsync(OfficeFloorDto officeFloor);
        Task<OfficeFloor?> UpdateOfficeFloorAsync(OfficeFloor officeFloor);
        Task<OfficeFloor?> DeleteOfficeFloorAsync(int officeFloorId);
    }
}
