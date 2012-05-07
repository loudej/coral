using Coral.Engine.Logger;

namespace Coral.Engine.Tests.Fakes
{
    public class FakeLoggerFactory : ILoggerFactory
    {
        public ILogger Create(string category)
        {
            return new FakeLogger();
        }

        public class FakeLogger : ILogger
        {
            public void Log(string level, string message)
            {                
            }
        }
    }
}