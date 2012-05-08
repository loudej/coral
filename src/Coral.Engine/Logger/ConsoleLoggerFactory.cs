using System;

namespace Coral.Engine.Logger
{
    public class ConsoleLoggerFactory : ILoggerFactory
    {
        public ILogger Create(string category)
        {
            return new ConsoleLogger(category);
        }

        public class ConsoleLogger : ILogger
        {
            private readonly string _category;

            public ConsoleLogger(string category)
            {
                _category = category;
            }

            public void Log(string level, string message)
            {
                Console.WriteLine("[{0}] {1}: {2}", _category, level, message);
            }
        }
    }
}