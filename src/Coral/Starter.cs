using Autofac;
using Coral.Engine;
using Coral.Engine.Logger;
using Coral.Engine.Scheduler;

namespace Coral
{
    public class Starter
    {
        public static ICoralEngine CreateEngine()
        {
            var container = CreateContainer();
            var engine = container.Resolve<ICoralEngine>();
            return engine;
        }

        public static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<CoralEngine>().As<ICoralEngine>();
            builder.RegisterType<ConsoleLoggerFactory>().As<ILoggerFactory>();
            builder.RegisterType<DefaultScheduler>().As<IScheduler>();
            return builder.Build();
        }
    }
}
