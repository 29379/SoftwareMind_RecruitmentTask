using HotDeskBookingSystem.Data.Models;
using HotDeskBookingSystem.Interfaces;

namespace HotDeskBookingSystem.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        public Task<Role?> AddRoleAsync(Role role)
        {
            throw new NotImplementedException();
        }

        public Task<Role?> DeleteRoleAsync(int roleId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Role?> GetRoleByIdAsync(int roleId)
        {
            throw new NotImplementedException();
        }

        public Task<Role?> UpdateRoleAsync(Role role)
        {
            throw new NotImplementedException();
        }
    }
}
