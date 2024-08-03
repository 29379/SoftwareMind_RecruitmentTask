using HotDeskBookingSystem.Data.Models;
using HotDeskBookingSystem.Interfaces;

namespace HotDeskBookingSystem.Repositories
{
    public class OfficeFloorRepository : IOfficeFloorRepository
    {
        public Task<OfficeFloor?> AddOfficeFloorAsync(OfficeFloor officeFloor)
        {
            throw new NotImplementedException();
        }

        public Task<OfficeFloor?> DeleteOfficeFloorAsync(int officeFloorId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OfficeFloor>> GetAllOfficeFloorsByOfficeIdAsync(int officeId)
        {
            throw new NotImplementedException();
        }

        public Task<OfficeFloor?> GetOfficeFloorByIdAsync(int officeFloorId)
        {
            throw new NotImplementedException();
        }

        public Task<OfficeFloor?> UpdateOfficeFloorAsync(OfficeFloor officeFloor)
        {
            throw new NotImplementedException();
        }
    }
}
