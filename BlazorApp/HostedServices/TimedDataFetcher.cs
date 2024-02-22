using Application.Interfaces;
using System.Diagnostics;

namespace BlazorApp.HostedServices
{
    public class TimedDataFetcher : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceProvider _serviceProvider;

        public TimedDataFetcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(2));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var bondService = scope.ServiceProvider.GetRequiredService<IBondService>();
                Debug.WriteLine($"fetching bots at time {DateTime.Now}");
                await bondService.AcquireBondSnapshotsAsync("BOT");
                Debug.WriteLine($"fetching btps");
                await bondService.AcquireBondSnapshotsAsync("BTP");
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
