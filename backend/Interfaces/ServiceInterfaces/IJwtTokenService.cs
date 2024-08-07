using HotDeskBookingSystem.Data.Models;

namespace HotDeskBookingSystem.Interfaces.Services
{
    public interface IJwtTokenService
    {
        string GenerateJwtToken(AppUser user);
        bool IsTokenValid(string jti);
        void RemoveToken(string jti);
        void ClearExpiredTokens();
    }
}
