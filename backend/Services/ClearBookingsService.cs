using HotDeskBookingSystem.Interfaces;

namespace HotDeskBookingSystem.Services
{
    public class ClearBookingsService : IHostedService, IDisposable
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ILogger<ClearBookingsService> _logger;
        private Timer _timer;
        private CancellationTokenSource _cancellationTokenSource;

        public ClearBookingsService(IBookingRepository bookingRepository, ILogger<ClearBookingsService> logger)
        {
            _bookingRepository = bookingRepository;
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
                TimeSpan.FromHours(6)
            ); 
            return Task.CompletedTask;
        }

        private async Task ClearOldBookings(object state)
        {
            _logger.LogInformation("ClearBookingsService is working.");
            try
            {
                var completedBookingsTask = _bookingRepository
                    .GetAllBookingsByBookingStatusAsync("COMPLETED");
                var canceledBookingsTask = _bookingRepository
                    .GetAllBookingsByBookingStatusAsync("CANCELED");
                await Task.WhenAll(completedBookingsTask, canceledBookingsTask);

                var bookings = completedBookingsTask.Result.Concat(canceledBookingsTask.Result);
                var cutoffDate = DateTime.Now.AddDays(-7);

                var oldBookings = bookings
                    .Where(b => b.endTime < cutoffDate)
                    .ToList();

                foreach (var booking in oldBookings)
                {
                    await _bookingRepository.DeleteBookingAsync(booking.BookingId);
                    _logger.LogInformation($"Old booking with id: {booking.BookingId} was deleted.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while clearing old bookings.");
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

    }
}
