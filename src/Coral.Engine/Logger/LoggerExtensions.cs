using System;

namespace Coral.Engine.Logger
{
    public static class LoggerExtensions
    {
        public static ILogger Create<T>(this ILoggerFactory factory)
        {
            return factory.Create(typeof(T).Name);
        }

        public static ILogger Create(this ILoggerFactory factory, Type type)
        {
            return factory.Create(type.Name);
        }

        public static void Data(this ILogger logger, string message)
        {
            logger.Log("data", message);
        }

        public static void Info(this ILogger logger, string message)
        {
            logger.Log("info", message);
        }

        public static void Info(this ILogger logger, string format, params object[] args)
        {
            logger.Log("info", string.Format(format, args));
        }

        public static void Error(this ILogger logger, string message)
        {
            logger.Log("error", message);
        }

        public static void Error(this ILogger logger, string format, params object[] args)
        {
            logger.Log("error", string.Format(format, args));
        }

        public static void Error(this ILogger logger, Exception ex)
        {
            logger.Log("error", ex.Message);
        }

        public static void Debug(this ILogger logger, string message)
        {
            logger.Log("debug", message);
        }

        public static void Debug(this ILogger logger, string format, params object[] args)
        {
            logger.Log("debug", string.Format(format, args));
        }
    }
}
