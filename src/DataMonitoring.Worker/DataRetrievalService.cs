using System;
using System.Threading;
using System.Threading.Tasks;
using DataMonitoring.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DataMonitoring.Worker
{
    public class DataRetrievalService : IHostedService, IDisposable
    {
        private readonly IDataProcessor _dataProcessor;
        private readonly ILogger<DataRetrievalService> _logger;
        private Timer? _timer;
        private bool disposedValue;

        public DataRetrievalService(IDataProcessor dataProcessor, ILogger<DataRetrievalService> logger)
        {
            _dataProcessor = dataProcessor;
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
            await _dataProcessor.ProcessData();
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
