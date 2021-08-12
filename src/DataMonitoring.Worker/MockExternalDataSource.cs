using DataMonitoring.Core;
using DataMonitoring.Core.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataMonitoring.Worker
{
    class MockExternalDataSource : IExternalDataSource, IDisposable
    {
        private readonly Queue<AssetPositionEvent> _eventStorage = new Queue<AssetPositionEvent>();
        private readonly Timer _timer;
        private readonly Random _random;
        private readonly ILogger<MockExternalDataSource> _logger;
        private bool disposedValue;

        public MockExternalDataSource(ILogger<MockExternalDataSource> logger)
        {
            _timer = new Timer(AddNewEvent, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            _random = new Random();
            _logger = logger;
        }

        public Task<ICollection<AssetPositionEvent>> GetLastEvents()
        {
            lock (_eventStorage)
            {
                _logger.LogDebug($"{_eventStorage.Count} events in the storage");
                var eventCount = _eventStorage.Count;
                var eventArray = new AssetPositionEvent[eventCount];
                for (var i = 0; i < eventCount; i++)
                {
                    eventArray[i] = _eventStorage.Dequeue();
                }
                return Task.FromResult((ICollection<AssetPositionEvent>)eventArray);
            }
        }

        private void AddNewEvent(object? state)
        {
            if (!(_random.NextDouble() < 0.2))
            {
                return;
            }

            var positionEvent = new AssetPositionEvent(
                _random.Next(1, 100),
                DateTimeOffset.UtcNow,
                new Coordinates(_random.NextDouble() * 100, _random.NextDouble() * 100)
            );

            lock (_eventStorage)
            {
                _logger.LogDebug("Adding new event to the storage");
                _eventStorage.Enqueue(positionEvent);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _timer.Dispose();
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
