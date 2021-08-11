using System;

namespace DataMonitoring.Core.Models
{
    public record AssetPositionEvent(long AssetId, DateTimeOffset EventTime, Coordinates Coordinates);

    public record Coordinates(double X, double Y);
}
