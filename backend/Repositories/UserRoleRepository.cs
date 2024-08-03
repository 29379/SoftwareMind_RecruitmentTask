using HotDeskBookingSystem.Data.Models;
using HotDeskBookingSystem.Interfaces;

namespace HotDeskBookingSystem.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        public Task<UserRole?> AddUserRoleAsync(UserRole userRole)
        {
            throw new NotImplementedException();
        }

        public Task<UserRole?> DeleteUserRoleAsync(int userRoleId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserRole>> GetAllUserRolesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserRole?> GetUserRoleByIdAsync(int userRoleId)
        {
            throw new NotImplementedException();
        }

        public Task<UserRole?> UpdateUserRoleAsync(UserRole userRole)
        {
            throw new NotImplementedException();
        }
    }
}
