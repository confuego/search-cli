using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Search.Cli.Repository;

namespace Search.Cli.Services
{
	public abstract class SearchService : ISearchService
	{
		private readonly IMedicalRecordRepository _medicalRecordRepository;
		public SearchService(IMedicalRecordRepository medicalRecordRepository)
		{
			_medicalRecordRepository = medicalRecordRepository;
		}

		protected abstract void Filter(SearchFilterContext context);

		private long FindIndex(string column, DataTable table)
		{
			return table.Columns[column].Ordinal;
		}

		public async Task<List<string>> SearchAsync(string column, string keyword)
		{
			var data = await _medicalRecordRepository.ReadAsync("./data/sample_data.csv.gz");
			var index = Index.Create(data);

			Console.WriteLine($"Searching for {keyword}...");

			var filterContext = new SearchFilterContext
			{
				SearchText = keyword,
				Shards = index.Search(keyword)
			};

			if(filterContext.Shards.Any())
			{
				var columnIndex = FindIndex(column, data);
				filterContext.ColumnIndex = columnIndex;
				Filter(filterContext);
			}

			Console.WriteLine($"Found {filterContext.Shards.LongCount()} matches.");
			Console.WriteLine();

			var result = new List<string>();
			var rows = data.Select();
			foreach (var row in filterContext.Shards.Select(x => rows[x.Row]))
			{
				var id = row["Primary DI"];
				result.Add(id.ToString().TrimStart('0'));
			}

			return result;
		}
	}
}