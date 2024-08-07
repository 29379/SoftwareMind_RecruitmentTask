using HotDeskBookingSystem.Data;
using HotDeskBookingSystem.Data.Models;
using HotDeskBookingSystem.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotDeskBookingSystem.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DataContext _context;

        public RoleRepository(DataContext dataContext) {
            _context = dataContext;
        }

        public Task<Role?> AddRoleAsync(Role role)
        {
            throw new NotImplementedException();
        }

        public Task<Role?> DeleteRoleAsync(string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Role?> GetRoleByNameAsync(string roleName)
        {
            return _context.Roles
                .FirstOrDefaultAsync(r => r.RoleName == roleName);
        }

        public Task<Role?> UpdateRoleAsync(Role role)
        {
            throw new NotImplementedException();
        }
    }
}
