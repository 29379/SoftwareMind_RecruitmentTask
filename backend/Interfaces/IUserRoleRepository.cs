using HotDeskBookingSystem.Data.Models;

namespace HotDeskBookingSystem.Interfaces
{
    public interface IUserRoleRepository
    {
        Task<UserRole?> GetUserRoleByIdAsync(int userRoleId);
        Task<IEnumerable<UserRole>> GetAllUserRolesAsync();
        Task<UserRole?> AddUserRoleAsync(UserRole userRole);
        Task<UserRole?> UpdateUserRoleAsync(UserRole userRole);
        Task<UserRole?> DeleteUserRoleAsync(int userRoleId);
    }
}
