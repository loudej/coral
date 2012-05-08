using Autofac;
using Coral.Engine;
using Coral.Engine.Logger;
using Coral.Engine.Tender;

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
            builder.RegisterType<CoralEngine>().As<ICoralEngine>().SingleInstance();
            builder.RegisterType<Scheduler>().As<IScheduler>().SingleInstance();
            builder.RegisterType<ProcessTender>().As<IProcessTender>().SingleInstance();
            builder.RegisterType<ConsoleLoggerFactory>().As<ILoggerFactory>();
            return builder.Build();
        }
    }
}
