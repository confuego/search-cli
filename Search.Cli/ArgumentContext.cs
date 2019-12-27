using Search.Cli.Services;

namespace Search.Cli
{
	public class ArgumentContext
	{
		public string Text { get; set; }

		public string Column { get; set; }

		public Operator Operator { get; set; }
	}
}