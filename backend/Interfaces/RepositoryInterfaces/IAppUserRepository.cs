using HotDeskBookingSystem.Data.Dto;
using HotDeskBookingSystem.Data.Models;

namespace HotDeskBookingSystem.Interfaces.Repositories
{
    public interface IAppUserRepository
    {
        Task<bool> UserExistsAsync(string email);
        Task<AppUser?> GetUserByEmailAsync(string email);
        Task<IEnumerable<AppUser>> GetAllUsersAsync();
        Task<AppUser?> RegisterUserAsync(RegisterDto registerDto);
        Task<AppUser?> LoginUserAsync(LoginDto user);
        Task<AppUser?> UpdateUserAsync(UpdateUserDto userDto);
        Task<AppUser?> DeleteUserAsync(string email);
    }
}
