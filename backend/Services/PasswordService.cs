using HotDeskBookingSystem.Exceptions;
using HotDeskBookingSystem.Interfaces.Services;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace HotDeskBookingSystem.Services
{
    public class PasswordService : IPasswordService
    {
        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter
                .ToString(hashedBytes)
                .Replace("-", "")
                .ToLower();
        }

        public bool VerifyPassword(string password, string hash)
        {
            var hashedInput = HashPassword(password);
            return hash == hashedInput;
        }
    }
}
