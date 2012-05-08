using System.Threading.Tasks;
using Coral.Engine.Tender;

namespace Coral.Engine
{
    public interface ICoralEngine
    {
        void Start();
        Task Stop();
        IProcessTender ProcessTender { get; }
        IScheduler Scheduler { get; }
    }
}