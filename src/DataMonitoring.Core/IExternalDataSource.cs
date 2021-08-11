using DataMonitoring.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataMonitoring.Core
{
    public interface IExternalDataSource
    {
        public Task<ICollection<AssetPositionEvent>> GetLastEvents();
    }
}
