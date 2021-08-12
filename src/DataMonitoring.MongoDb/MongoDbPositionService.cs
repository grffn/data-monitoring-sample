using DataMonitoring.Core;
using DataMonitoring.Core.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataMonitoring.Storage.MongoDb
{
    public class MongoDbPositionService : IPositionService
    {
        private readonly IMongoCollection<AssetPosition> _positionsCollection;
        private readonly ILogger<MongoDbPositionService> _logger;

        public MongoDbPositionService(IMongoCollection<AssetPosition> positionsCollection, ILogger<MongoDbPositionService> logger)
        {
            _positionsCollection = positionsCollection;
            _logger = logger;
        }

        public async Task SavePositions(IEnumerable<AssetPosition> positions)
        {
            if (!positions.Any())
            {
                throw new ArgumentException("Should be at least one position", nameof(positions));
            }
            await _positionsCollection.InsertManyAsync(positions);
        }
    }
}