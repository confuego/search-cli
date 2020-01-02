using System.Collections.Generic;

namespace Search.Cli
{
    public class Command
    {
        public string Name { get; set; }

		public List<string> Arguments { get; set; } = new List<string>();
    }
}