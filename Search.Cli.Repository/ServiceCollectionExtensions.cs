using Microsoft.Extensions.DependencyInjection;

namespace Search.Cli.Repository
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection collection)
		{
			return collection.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
		}
    }
}