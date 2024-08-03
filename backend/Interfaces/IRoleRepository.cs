using HotDeskBookingSystem.Data.Models;

namespace HotDeskBookingSystem.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role?> GetRoleByIdAsync(int roleId);
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role?> AddRoleAsync(Role role);
        Task<Role?> UpdateRoleAsync(Role role);
        Task<Role?> DeleteRoleAsync(int roleId);
    }
}
