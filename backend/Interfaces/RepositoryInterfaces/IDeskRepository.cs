using HotDeskBookingSystem.Data.Dto;
using HotDeskBookingSystem.Data.Models;

namespace HotDeskBookingSystem.Interfaces.Repositories
{
    public interface IDeskRepository
    {
        Task<Desk?> GetDeskByIdAsync(int deskId);
        Task<IEnumerable<Desk>> GetAllDesksAsync();
        Task<IEnumerable<Desk>> GetDesksByOfficeFloorIdAsync(int officeFloorId);
        Task<IEnumerable<Desk>> GetDesksByOfficeIdAsync(int officeId);
        Task<IEnumerable<Desk>> GetAvailableDesksByOfficeIdAsync(int officeId, ReservationTimesDto reservationTimes);
        Task<Desk?> AddDeskAsync(Desk desk);
        Task<Desk?> UpdateDeskAsync(Desk desk);
        Task<Desk?> DeleteDeskAsync(int deskId);
        Task<bool> IsDeskAvailableAsync(int deskId, ReservationTimesDto reservationTimes);
    }
}
