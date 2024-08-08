using HotDeskBookingSystem.Data.Dto;
using HotDeskBookingSystem.Data.Models;

namespace HotDeskBookingSystem.Interfaces.Repositories
{
    public interface IOfficeRepository
    {
        Task<Office?> GetOfficeByIdAsync(int officeId);
        Task<IEnumerable<Office>> GetAllOfficesAsync();
        Task<Office?> AddOfficeAsync(OfficeDto office);
        Task<Office?> UpdateOfficeAsync(Office office);
        Task<Office?> DeleteOfficeAsync(int officeId);
    }
}
