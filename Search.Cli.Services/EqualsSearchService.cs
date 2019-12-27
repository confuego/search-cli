using System.Linq;
using Search.Cli.Repository;

namespace Search.Cli.Services
{
	public class EqualsSearchService : SearchService
	{
		public EqualsSearchService(IMedicalRecordRepository medicalRecordRepository) 
			: base(medicalRecordRepository)
		{
		}

		protected override void Filter(SearchFilterContext context)
		{
			context.Shards = context.Shards.Where(x => x.WordStart == 0 && x.WordEnd == context.SearchText.Length);
		}
	}
}