using System.Threading.Tasks;
using Coral.Engine.Logger;
using Coral.Engine.Tender;

namespace Coral.Engine
{
    public class CoralEngine : AbstractService, ICoralEngine
    {
        private readonly IScheduler _scheduler;
        private readonly IProcessTender _processTender;

        public CoralEngine(ILoggerFactory loggerFactory, IScheduler scheduler, IProcessTender processTender) : base(loggerFactory)
        {
            _scheduler = scheduler;
            _processTender = processTender;
        }

        public void Start()
        {
            _scheduler.Start();
        }

        public Task Stop()
        {
            return _scheduler.Stop();
        }

        public IProcessTender ProcessTender
        {
            get { return _processTender; }
        }

        public IScheduler Scheduler
        {
            get { return _scheduler; }
        }
    }
}

