using HotDeskBookingSystem.Data.Models;

namespace HotDeskBookingSystem.Interfaces.Repositories
{
    public interface IOfficeRepository
    {
        Task<Office?> GetOfficeByIdAsync(int officeId);
        Task<IEnumerable<Office>> GetAllOfficesAsync();
        Task<Office?> AddOfficeAsync(Office office);
        Task<Office?> UpdateOfficeAsync(Office office);
        Task<Office?> DeleteOfficeAsync(int officeId);
    }
}
