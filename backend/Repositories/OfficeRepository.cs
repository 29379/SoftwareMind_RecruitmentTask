using HotDeskBookingSystem.Data.Models;
using HotDeskBookingSystem.Interfaces.Repositories;

namespace HotDeskBookingSystem.Repositories
{
    public class OfficeRepository : IOfficeRepository
    {
        public Task<Office?> AddOfficeAsync(Office office)
        {
            throw new NotImplementedException();
        }

        public Task<Office?> DeleteOfficeAsync(int officeId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Office>> GetAllOfficesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Office?> GetOfficeByIdAsync(int officeId)
        {
            throw new NotImplementedException();
        }

        public Task<Office?> UpdateOfficeAsync(Office office)
        {
            throw new NotImplementedException();
        }
    }
}
