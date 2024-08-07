using HotDeskBookingSystem.Data.Models;

namespace HotDeskBookingSystem.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task<Role?> GetRoleByNameAsync(string roleName);
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role?> AddRoleAsync(Role role);
        Task<Role?> UpdateRoleAsync(Role role);
        Task<Role?> DeleteRoleAsync(string roleName);
    }
}
