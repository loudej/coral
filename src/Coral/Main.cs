using System;
using System.Linq;
using Coral.Options;

namespace Coral
{
    class Program
    {
        public static int Main (string[] args)
        {
            switch (args.FirstOrDefault(arg => !arg.StartsWith("-") && !arg.StartsWith("/")))
            {
                case "start":
                    return DoStart(args);
                default:
                    return DoDefault(args);
            }
        }

        static int DoDefault(string[] args)
        {
            var options = new DefaultOptions();
            options.Parse(args);

            if (options.ShowHelp)
            {
                Console.WriteLine(
@"Usage: Coral [command] [options]

Commands:
  start    Execute multiple console applications

Options:"
);
                options.WriteOptionDescriptions(Console.Out);
                return 0;
            }

            return 0;
        }

        static int DoStart(string[] args)
        {
            var options = new StartOptions();
            options.Parse(args);

            if (options.ShowHelp)
            {
                Console.WriteLine(
@"Usage: Coral start [options] [process [process [...]]]
Execute multiple console applications

Options:"
);
                options.WriteOptionDescriptions(Console.Out);
                return 0;
            }

            Console.WriteLine("Concurrency " + options.Concurrency);
            return 0;
        }
    }
}
