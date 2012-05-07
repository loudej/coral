using Coral.Engine.Logger;

namespace Coral.Engine
{
    public abstract class AbstractService
    {
        protected ILogger Log { get; private set; }

        protected AbstractService(ILoggerFactory loggerFactory)
        {
            Log = loggerFactory.Create(GetType());
        }
    }
}