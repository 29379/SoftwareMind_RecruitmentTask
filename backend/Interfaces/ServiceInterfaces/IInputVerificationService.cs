namespace HotDeskBookingSystem.Interfaces.Services
{
    public interface IInputVerificationService
    {
        bool IsValidEmail(string email);
        bool IsValidPassword(string password);
    }
}
