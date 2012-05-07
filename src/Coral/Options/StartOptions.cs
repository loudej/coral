using System;
using System.Collections.Generic;
using System.Linq;

namespace Coral.Options
{
    public class StartOptions : DefaultOptions
    {
        public StartOptions()
        {
            _optionSet
                .Add("f|procfile:",
                     "Specify an alternate Procfile to load, implies -d at the Procfile root.",
                     value => Procfile = value)
                .Add("p|port:",
                     "Specify which port to use as the base for this application. Should be a multiple of 1000.",
                     (int value) => Port = value)
                .Add("c|concurrency:",
                     "Specify the number of each process type to run. The value passed in should be in the format process=num,process=num",
                     SetConcurrency)
                .Add("d|directory:",
                     "Specify an alternate application root. This defaults to the directory containing the Procfile.",
                     value => Directory = value)
                .Add("e|env:",
                     "Specify an alternate environment file. You can specify more than one file by using: --env file1,file2.",
                     SetEnvironmentFile);
        }

        public string Procfile { get; set; }
        public int Port { get; set; }
        public IDictionary<string, int> Concurrency { get; set; }
        public string Directory { get; set; }
        public IList<string> EnvironmentFiles { get; set; }

        public void SetConcurrency(string value)
        {
            if (Concurrency == null)
            {
                Concurrency = new Dictionary<string, int>();
            }

            foreach (var pair in value.Split(new[] { "," }, StringSplitOptions.None).Select(span => span.Split(new[] { "=" }, 2, StringSplitOptions.None)))
            {
                if (pair.Length != 2)
                {
                    throw new Exception("Concurrency must NAME=VALUE,NAME=VALUE format");
                }

                int number;
                if (!int.TryParse(pair[1], out number))
                {
                    throw new Exception("Concurrency values must be integer");
                }

                Concurrency[pair[0]] = number;
            }
        }

        public void SetEnvironmentFile(string value)
        {
            if (EnvironmentFiles == null)
            {
                EnvironmentFiles = new List<string>();
            }

            foreach (var file in value.Split(new[] { "," }, StringSplitOptions.None))
            {
                EnvironmentFiles.Add(file);
            }
        }
    }
}
