using HotDeskBookingSystem.Data;
using HotDeskBookingSystem.Data.Dto;
using HotDeskBookingSystem.Data.Models;
using HotDeskBookingSystem.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotDeskBookingSystem.Repositories
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly DataContext _context;

        public AppUserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
        {
            return await _context.Users
                .ToListAsync();
        }

        public async Task<AppUser?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<AppUser?> RegisterUserAsync(AppUser user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public Task<AppUser?> LoginUserAsync(LoginDto user)
        {
            throw new NotImplementedException();
        }

        public Task<AppUser?> UpdateUserAsync(AppUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<AppUser?> DeleteUserAsync(string email)
        {
            var toBeDeleted = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
            if (toBeDeleted != null)
            {
                _context.Users.Remove(toBeDeleted);
                await _context.SaveChangesAsync();
                return toBeDeleted;
            }
            return null;
        }

    }
}
