using System.Collections.Generic;

namespace Search.Cli
{
	public class Argument
	{
		public string Name { get; set; }

		public List<string> Aliases { get; set; } = new List<string>();
	}
}