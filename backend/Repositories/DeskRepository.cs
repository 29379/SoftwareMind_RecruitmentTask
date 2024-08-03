using HotDeskBookingSystem.Data.Models;
using HotDeskBookingSystem.Interfaces;

namespace HotDeskBookingSystem.Repositories
{
    public class DeskRepository : IDeskRepository
    {
        public Task<Desk?> AddDeskAsync(Desk desk)
        {
            throw new NotImplementedException();
        }

        public Task<Desk?> DeleteDeskAsync(int deskId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Desk>> GetAllDesksAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Desk>> GetAvailableDesksByOfficeIdAsync(int officeId, DateTime timeOfReservation)
        {
            throw new NotImplementedException();
        }

        public Task<Desk?> GetDeskByIdAsync(int deskId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Desk>> GetDesksByOfficeFloorIdAsync(int officeFloorId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Desk>> GetDesksByOfficeIdAsync(int officeId)
        {
            throw new NotImplementedException();
        }

        public Task<Desk?> UpdateDeskAsync(Desk desk)
        {
            throw new NotImplementedException();
        }
    }
}
