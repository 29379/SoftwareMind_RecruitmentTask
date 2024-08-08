using HotDeskBookingSystem.Data.Dto.Booking;
using HotDeskBookingSystem.Data.Dto.Desk;
using HotDeskBookingSystem.Data.Models;

namespace HotDeskBookingSystem.Interfaces.Repositories
{
    public interface IDeskRepository
    {
        Task<DeskDto?> GetDeskByIdAsync(int deskId);
        Task<IEnumerable<DeskDto>> GetAllDesksAsync();
        Task<IEnumerable<DeskDto>> GetDesksByOfficeFloorIdAsync(int officeFloorId);
        Task<IEnumerable<DeskDto>> GetDesksByOfficeIdAsync(int officeId);

        Task<IEnumerable<Desk>> GetAvailableDesksByOfficeIdAsync(int officeId, ReservationTimesDto reservationTimes);
        Task<IEnumerable<Desk>> GetAvailableDesksByOfficeFloorIdAsync(int officeFloorId, ReservationTimesDto reservationTimes);
        
        Task<Desk?> AddDeskAsync(Desk desk);
        Task<Desk?> UpdateDeskAsync(Desk desk);
        Task<Desk?> DeleteDeskAsync(int deskId);
        Task<bool> IsDeskAvailableAsync(int deskId, ReservationTimesDto reservationTimes);
    }
}
