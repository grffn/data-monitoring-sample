using AutoMapper;
using DataMonitoring.Core;
using DataMonitoring.Core.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataMonitoring.Worker
{
    class DataProcessor : IDataProcessor
    {
        private readonly IExternalDataSource _dataSource;
        private readonly IPositionService _positionService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public DataProcessor(IExternalDataSource dataSource, IPositionService positionService, IMapper mapper, ILogger<DataProcessor> logger)
        {
            _dataSource = dataSource;
            _positionService = positionService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task ProcessData()
        {
            _logger.LogInformation("Fetching last events");
            var events = await _dataSource.GetLastEvents();
            _logger.LogInformation($"Fetched {events.Count} events");

            if (events.Count == 0)
            {
                _logger.LogInformation("Nothing to save");
                return;
            }
            var positions = _mapper.Map<ICollection<AssetPosition>>(events);

            _logger.LogInformation("Saving events");
            await _positionService.SavePositions(positions);
            _logger.LogInformation("Saved successfully");
        }
    }
}
