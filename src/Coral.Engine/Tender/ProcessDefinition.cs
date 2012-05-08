using System.Collections.Generic;

namespace Coral.Engine.Tender
{
    public class ProcessDefinition
    {
        public string Name { get; set; }
        public string Program { get; set; }
        public string Command { get; set; }
        public string WorkingDirectory { get; set; }
        public string Identity { get; set; }
        public IDictionary<string, string> Environment { get; set; }
    }
}
