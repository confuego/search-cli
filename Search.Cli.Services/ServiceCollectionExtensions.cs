using Microsoft.Extensions.DependencyInjection;
using Search.Cli.Repository;

namespace Search.Cli.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection collection)
		{
			return collection.AddScoped<ISearchService, SearchService>().AddRepositories();
		}
    }
}