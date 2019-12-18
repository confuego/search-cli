using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Search.Cli.Services;

namespace Search.Cli
{
	public class Program
	{
		public async static Task Main(string[] args)
		{
			var provider = new ServiceCollection()
				.AddServices()
				.BuildServiceProvider();

			var searchService = provider.GetRequiredService<ISearchService>();

			var result = await searchService.SearchAsync("", "", Operator.Contains);

			foreach (var id in result)
			{
				Console.WriteLine(id);
			}
		}
	}
}
