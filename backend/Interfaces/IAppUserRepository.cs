using HotDeskBookingSystem.Data.Dto;
using HotDeskBookingSystem.Data.Models;

namespace HotDeskBookingSystem.Interfaces
{
    public interface IAppUserRepository
    {
        // BASIC CRUD
        Task<AppUser?> GetUserByEmailAsync(string email);
        Task<IEnumerable<AppUser>> GetAllUsersAsync();
        Task<AppUser?> RegisterUserAsync(AppUser user);
        Task<AppUser?> LoginUserAsync(LoginDto user);
        Task<AppUser?> UpdateUserAsync(AppUser user);
        Task<AppUser?> DeleteUserAsync(string email);
    }
}
