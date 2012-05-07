using System;
using System.Threading.Tasks;

namespace Coral.Engine.Scheduler
{
    public interface IScheduler
    {
        void Start();
        Task Stop();
        void Post(Action work);
    }
}