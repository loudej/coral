using System.Collections.Generic;
using System.IO;
using NDesk.Options;

namespace Coral.Options
{
    public class DefaultOptions
    {
        protected readonly OptionSet _optionSet = new OptionSet();

        public DefaultOptions()
        {
            _optionSet
                .Add("h|?|help", "show this message and exit", _ => ShowHelp = true)
                .Add("v|verbose", "increase output verbosity", _ => ++Verbose);
        }

        public bool ShowHelp { get; set; }
        public int Verbose { get; set; }
        public List<string> Extra { get; set; }

        public void Parse(string[] arguments)
        {
            Extra = _optionSet.Parse(arguments);
        }

        public void WriteOptionDescriptions(TextWriter writer)
        {
            _optionSet.WriteOptionDescriptions(writer);
        }

    }
}