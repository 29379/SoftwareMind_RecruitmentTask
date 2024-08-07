using HotDeskBookingSystem.Data;
using HotDeskBookingSystem.Data.Models;
using HotDeskBookingSystem.Exceptions;
using HotDeskBookingSystem.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotDeskBookingSystem.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly DataContext _context;
        private readonly IRoleRepository _roleRepository;
        private readonly IAppUserRepository _appUserRepository;

        public UserRoleRepository(DataContext context, IRoleRepository roleRepository, IAppUserRepository appUserRepository)
        {
            _context = context;
            _roleRepository = roleRepository;
            _appUserRepository = appUserRepository;
        }

        public async Task<UserRole?> AddUserRoleAsync(string email, string roleName)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                throw new NotFoundException("User with email: " + email + " not found");
            }

            var role = await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleName == roleName);
            if (role == null)
            {
               throw new NotFoundException("Role with name: " + roleName + " not found");
            }

            var existingUserRoles = _context.UserRoles
                .Where(ur => ur.UserEmail == email && ur.RoleName == roleName);
            if (existingUserRoles.Any())
            {
                throw new AlreadyExistsException($"User {email} already has the role of {roleName}");
            }

            UserRole newUserRole = new UserRole
            {
                UserEmail = email,
                RoleName = roleName,
                User = user,
                Role = role
            };

            _context.UserRoles.Add(newUserRole);
            await _context.SaveChangesAsync();
            return newUserRole;
        }

        public Task<UserRole?> DeleteUserRoleAsync(string email, string roleName)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserRole>> GetAllUserRolesAsync()
        {
            return await _context.UserRoles
                .ToListAsync();
        }

        public async Task<UserRole?> GetUserRoleByEmailAndRoleNameAsync(string email, string roleName)
        {
            return await _context.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserEmail == email && ur.RoleName == roleName);
        }

        public Task<UserRole?> UpdateUserRoleAsync(UserRole userRole)
        {
            throw new NotImplementedException();
        }
    }
}
