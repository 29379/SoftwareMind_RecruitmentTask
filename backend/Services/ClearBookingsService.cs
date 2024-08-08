using HotDeskBookingSystem.Data.Models;
using HotDeskBookingSystem.Interfaces.Repositories;
using HotDeskBookingSystem.Interfaces.ServiceInterfaces;

namespace HotDeskBookingSystem.Services
{
    public class ClearBookingsService : IHostedService, IDisposable, IClearBookingsService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ClearBookingsService> _logger;
        private Timer _timer;
        private CancellationTokenSource _cancellationTokenSource;

        public ClearBookingsService(IServiceScopeFactory scopeFactory, ILogger<ClearBookingsService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ClearBookingsService running.");
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
            _timer = new Timer(
                async _ => await ClearOldBookings(_cancellationTokenSource.Token),
                null,
                TimeSpan.Zero,
                TimeSpan.FromMinutes(1)
            );
            return Task.CompletedTask;
        }

        private async Task ClearOldBookings(CancellationToken token)
        {
            _logger.LogInformation("ClearBookingsService is working.");
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var bookingRepository = scope.ServiceProvider.GetRequiredService<IBookingRepository>();

                    var bookedBookings = await bookingRepository.GetAllBookingsByBookingStatusAsync("BOOKED");

                    foreach (var booking in bookedBookings)
                    {
                        if (booking.endTime < DateTime.Now.AddDays(-7))
                        {
                            await bookingRepository.ChangeBookingStatus(booking.BookingId, "COMPLETED");
                            _logger.LogInformation($"Old booking with id: {booking.BookingId} was updated.");
                        }
                    }
                    
                    var completedBookingsTask = await bookingRepository.GetAllBookingsByBookingStatusAsync("COMPLETED");
                    var canceledBookingsTask = await bookingRepository.GetAllBookingsByBookingStatusAsync("CANCELED");

                    var bookings = completedBookingsTask.Concat(canceledBookingsTask);
                    var cutoffDate = DateTime.Now.AddDays(-7);

                    var oldBookings = bookings
                        .Where(b => b.endTime < cutoffDate)
                        .ToList();

                    foreach (var booking in oldBookings)
                    {
                        await bookingRepository.DeleteBookingAutomaticallyAsync(booking.BookingId, token);
                        _logger.LogInformation($"Old booking with id: {booking.BookingId} was deleted.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while clearing old bookings\n" + ex.Message + "\n" + ex.Source + "\n");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("ClearBookingsService is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            _cancellationTokenSource?.Cancel();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
            _cancellationTokenSource?.Dispose();
        }

        Task IClearBookingsService.ClearOldBookings(CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
