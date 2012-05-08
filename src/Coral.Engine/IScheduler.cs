using System;
using System.Threading.Tasks;

namespace Coral.Engine
{
    public interface IScheduler
    {
        void Start();
        Task Stop();
        void Post(string label, Action work);
    }
}