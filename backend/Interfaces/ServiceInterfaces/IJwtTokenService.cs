using HotDeskBookingSystem.Data.Models;

namespace HotDeskBookingSystem.Interfaces.Services
{
    public interface IJwtTokenService
    {
        string GenerateJwtToken(AppUser user);
    }
}
