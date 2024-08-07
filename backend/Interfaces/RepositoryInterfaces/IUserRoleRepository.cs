using HotDeskBookingSystem.Data.Models;

namespace HotDeskBookingSystem.Interfaces.Repositories
{
    public interface IUserRoleRepository
    {
        Task<UserRole?> GetUserRoleByEmailAndRoleNameAsync(string email, string roleName);
        Task<IEnumerable<UserRole>> GetAllUserRolesAsync();
        Task<UserRole?> AddUserRoleAsync(string email, string roleName);
        Task<UserRole?> UpdateUserRoleAsync(UserRole userRole);
        Task<UserRole?> DeleteUserRoleAsync(string email, string roleName);
    }
}
