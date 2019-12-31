using System;
using Search.Sdk;

namespace Search.Cli
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var results = new SearchEngine()
				.Index("./data/sample_data.csv.gz")
				.Search(string.Concat(args));

			foreach (var id in results)
			{
				Console.WriteLine(id);
			}
		}
	}
}
