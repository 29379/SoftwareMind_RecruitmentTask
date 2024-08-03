using HotDeskBookingSystem.Data.Models;

namespace HotDeskBookingSystem.Interfaces
{
    public interface IOfficeFloorRepository
    {
        Task<OfficeFloor?> GetOfficeFloorByIdAsync(int officeFloorId);
        Task<IEnumerable<OfficeFloor>> GetAllOfficeFloorsByOfficeIdAsync(int officeId);
        Task<OfficeFloor?> AddOfficeFloorAsync(OfficeFloor officeFloor);
        Task<OfficeFloor?> UpdateOfficeFloorAsync(OfficeFloor officeFloor);
        Task<OfficeFloor?> DeleteOfficeFloorAsync(int officeFloorId);
    }
}
