using System.Collections.Generic;

namespace Search.Cli.Services
{
	public class SearchFilterContext
	{
		public long ColumnIndex { get; set; }

		public string SearchText { get; set; }

		public IEnumerable<Shard> Shards { get; set; }
	}
}