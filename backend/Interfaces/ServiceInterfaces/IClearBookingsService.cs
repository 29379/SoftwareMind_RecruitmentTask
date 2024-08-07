namespace HotDeskBookingSystem.Interfaces.ServiceInterfaces
{
    public interface IClearBookingsService
    {
        Task ClearOldBookings(CancellationToken token);
    }
}
