using System.Linq;
using Search.Cli.Repository;

namespace Search.Cli.Services
{
	public class ContainsSearchService : SearchService
	{
		public ContainsSearchService(IMedicalRecordRepository medicalRecordRepository) 
			: base(medicalRecordRepository)
		{
		}

		protected override void Filter(SearchFilterContext context)
		{
			context.Shards = context.Shards.Where(x => x.Column == context.ColumnIndex).Distinct();
		}
	}
}