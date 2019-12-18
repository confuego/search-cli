using System.Collections.Generic;
using System.Threading.Tasks;
using Search.Cli.Repository;

namespace Search.Cli.Services
{
	public class SearchService : ISearchService
	{
		private readonly IMedicalRecordRepository _medicalRecordRepository;
		public SearchService(IMedicalRecordRepository medicalRecordRepository)
		{
			_medicalRecordRepository = medicalRecordRepository;
		}

		public async Task<List<string>> SearchAsync(string column, string keyword, Operator op)
		{
			var data = await _medicalRecordRepository.ReadAsync("./data/sample_data.csv.gz");
			var result = new List<string>();
			foreach (var row in data.Select())
			{
				var id = row["Primary DI"];
				result.Add(id.ToString());
			}

			return result;
		}
	}
}