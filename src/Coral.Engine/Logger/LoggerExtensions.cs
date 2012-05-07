using System;

namespace Coral.Engine.Logger
{
    public static class LoggerExtensions
    {
        public static ILogger Create<T>(this ILoggerFactory factory)
        {
            return factory.Create(typeof(T).FullName);
        }

        public static ILogger Create(this ILoggerFactory factory, Type type)
        {
            return factory.Create(type.FullName);
        }

        public static void Info(this ILogger logger, string message)
        {
            logger.Log("info", message);
        }

        public static void Info(this ILogger logger, string format, params object[] args)
        {
            logger.Log("info", string.Format(format, args));
        }
    }
}
