using System.Threading.Tasks;

namespace DataMonitoring.Core
{
    public interface IDataProcessor
    {
        Task ProcessData();
    }
}
