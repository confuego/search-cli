namespace Search.Cli.Services
{
	public class Shard
	{
		public long Row { get; set; }

		public long Column { get; set; }

		public long WordStart { get; set; }

		public long WordEnd { get; set; }
	}
}