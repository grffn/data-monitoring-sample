using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMonitoring.Core.Models
{
    public interface AssetPosition
    {
        public long AssetId { get; set; }

        public DateTimeOffset EventTime { get; set; }

        public Coordinates Coordinates { get; set; }
    }
}
