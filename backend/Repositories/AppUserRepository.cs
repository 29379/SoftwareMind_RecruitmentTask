using HotDeskBookingSystem.Data;
using HotDeskBookingSystem.Data.Dto.User;
using HotDeskBookingSystem.Data.Models;
using HotDeskBookingSystem.Exceptions;
using HotDeskBookingSystem.Interfaces.Repositories;
using HotDeskBookingSystem.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace HotDeskBookingSystem.Repositories
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly DataContext _context;
        private readonly IPasswordService _passwordService;
        private readonly IInputVerificationService _inputVerificationService;


        public AppUserRepository(
            DataContext context, 
            IPasswordService passwordService, 
            IInputVerificationService inputVerificationService
        )
        {
            _context = context;
            _passwordService = passwordService;
            _inputVerificationService = inputVerificationService;
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Email == email);
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

        public async Task<AppUser?> RegisterUserAsync(RegisterDto registerDto)
        {
            bool isValidEmail = _inputVerificationService.IsValidEmail(registerDto.Email);
            if (!isValidEmail)
            {
                throw new InvalidEmailException("Invalid email address");
            }
            bool isValidPassword = _inputVerificationService.IsValidPassword(registerDto.HashPassword);
            if (!isValidPassword)
            {
                throw new InvalidPasswordException("Password must be between 8 and 30 characters long and contain " +
                    "at least one uppercase letter, one lowercase letter, one digit and one special character");
            }

            registerDto.HashPassword = _passwordService
                .HashPassword(registerDto.HashPassword);
            AppUser newUser = new AppUser
            {
                Email = registerDto.Email,
                HashPassword = registerDto.HashPassword,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Bookings = new List<Booking>(),
                UserRoles = new List<UserRole>()
            };

            if (registerDto.Roles == null || registerDto.Roles.Count == 0)
            {
                registerDto.Roles = new List<string> { "EMPLOYEE" };
            }
            
            foreach (string role in registerDto.Roles)
            {
                var existingUserRole = await _context.UserRoles
                    .FirstOrDefaultAsync(ur => ur.RoleName == role && ur.UserEmail == registerDto.Email);
                if (existingUserRole == null) // if not, then this user already has this role
                {
                    UserRole newUserRole = new UserRole
                    {
                        RoleName = role,
                        UserEmail = registerDto.Email
                    };
                    newUser.UserRoles.Add(newUserRole);
                    _context.UserRoles.Add(newUserRole);
                }
            }
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }

        public async Task<AppUser?> LoginUserAsync(LoginDto user)
        {
            var foundUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == user.Email);
            if (foundUser == null || !_passwordService.VerifyPassword(user.Password, foundUser.HashPassword))
            {
                throw new BadCredentialsException("Invalid email or password");
            }

            var userRoles = await _context.UserRoles
                .Where(ur => ur.UserEmail == user.Email)
                .ToListAsync();
            foundUser.UserRoles = userRoles;
            await _context.SaveChangesAsync();
            return foundUser;
        }

        public async Task<AppUser?> UpdateUserAsync(UpdateUserDto updatedUserDto)
        {
            var existingUser = await _context.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Email == updatedUserDto.Email);

            if (existingUser == null)
            {
                throw new NotFoundException($"User with email: {updatedUserDto.Email} not found");
            }

            existingUser.FirstName = updatedUserDto.FirstName;
            existingUser.LastName = updatedUserDto.LastName;

            if (!_passwordService.VerifyPassword(updatedUserDto.HashPassword, existingUser.HashPassword))
            {
                existingUser.HashPassword = _passwordService.HashPassword(updatedUserDto.HashPassword);
            }

            // Removing roles
            var rolesToRemove = existingUser.UserRoles
                .Where(ur => !updatedUserDto.Roles.Contains(ur.RoleName))
                .ToList();

            foreach (var role in rolesToRemove)
            {
                existingUser.UserRoles.Remove(role);
                _context.UserRoles.Remove(role);
            }

            // Adding roles
            foreach (var roleName in updatedUserDto.Roles)
            {
                if (!existingUser.UserRoles.Any(ur => ur.RoleName == roleName))
                {
                    var role = new UserRole
                    {
                        RoleName = roleName,
                        UserEmail = existingUser.Email
                    };
                    existingUser.UserRoles.Add(role);
                    _context.UserRoles.Add(role);
                }
            }

            await _context.SaveChangesAsync();
            return existingUser;
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
