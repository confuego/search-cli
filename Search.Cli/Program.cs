using System;
using System.Threading.Tasks;
using Search.Cli.Services;

namespace Search.Cli
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var argumentContext = SearchArgumentParser.Parse(args);
			Console.WriteLine($"Arguments created ({argumentContext.Column}, {argumentContext.Operator}, {argumentContext.Text})");
			Console.WriteLine();

			var searchService = SearchFactory.Create(argumentContext.Operator);

			var result = await searchService.SearchAsync(argumentContext.Column, argumentContext.Text);

			foreach (var id in result)
			{
				Console.WriteLine(id);
			}
		}
	}
}
