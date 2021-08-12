using System;
using System.Threading;
using System.Threading.Tasks;
using DataMonitoring.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DataMonitoring.Worker
{
    public class DataRetrievalService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DataRetrievalService> _logger;
        private Timer? _timer;
        private bool disposedValue;

        public DataRetrievalService(IServiceProvider serviceProvider, ILogger<DataRetrievalService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service is starting.");
            _timer = new Timer(ProcessData, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        // async but void to be complacent with Times's constructor signature
        private async void ProcessData(object? state)
        {
            _logger.LogDebug($"{nameof(ProcessData)} was triggered");

            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    var dataProcessor = scope.ServiceProvider.GetRequiredService<IDataProcessor>();
                    await dataProcessor.ProcessData();
                }
                // logging error and discarding it: we can do nothing at this point
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Error while processing data");
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _timer?.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
