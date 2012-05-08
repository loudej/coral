using System;
using System.IO;
using System.Linq;
using Coral.Engine.Tender;
using Coral.Options;
using Coral.Util;

namespace Coral
{
    class Program
    {
        public static int Main(string[] args)
        {
            using (new ChangeErrorMode(ErrorModes.FailCriticalErrors | ErrorModes.NoGpFaultErrorBox | ErrorModes.NoOpenFileErrorBox))
            {
                try
                {
                    switch (args.FirstOrDefault(arg => !arg.StartsWith("-") && !arg.StartsWith("/")))
                    {
                        case "start":
                            return DoStart(args);
                        default:
                            return DoDefault(args);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unhandled exception: " + ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    return 1;
                }
            }
        }

        static int DoDefault(string[] args)
        {
            var options = new DefaultOptions();
            options.Parse(args);

            Console.WriteLine(
@"Usage: Coral [command] [options]

Commands:
  start    Execute multiple console applications

Options:"
);
            options.WriteOptionDescriptions(Console.Out);

            return 0;
        }

        static int DoStart(string[] args)
        {
            // Parse options
            var options = new StartOptions();
            options.Parse(args);

            // Show help if needed
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

            // Determine process file
            var procfile = options.Procfile ?? "Coral.txt";

            // Split process file contents into "name:command" pairs
            var lines = File.ReadAllText(procfile)
                .Split(new[] { "\n" }, StringSplitOptions.None)
                .Select(line => line.Trim('\r', ' ', '\t'))
                .Select(line => line.Split(new[] { ":" }, 2, StringSplitOptions.None));

            // Switch to provided directory, or use procfile location as default
            var directory = options.Directory ?? Path.GetDirectoryName(Path.Combine(Directory.GetCurrentDirectory(), procfile));
            Directory.SetCurrentDirectory(directory);


            // Create components and add process definitions
            var engine = Starter.CreateEngine();
            engine.Start();
            engine.Scheduler.Post("Init", () =>
            {
                foreach (var line in lines.Where(x => x.Length == 2))
                {
                    engine.ProcessTender.AddProcess(
                        new ProcessDefinition
                            {
                                Name = line[0] + ".1",
                                Command = line[1],
                                WorkingDirectory = options.Directory,
                            });
                }
            });
            Console.ReadLine();

            engine.Stop().Wait();
            return 0;
        }
    }
}
