using Search.Cli.Repository;

namespace Search.Cli.Services
{
	public class GeneralSearchService : SearchService
	{
		public GeneralSearchService(IMedicalRecordRepository medicalRecordRepository) 
			: base(medicalRecordRepository)
		{
		}

		protected override void Filter(SearchFilterContext context)
		{
			return;
		}
	}
}