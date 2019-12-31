using System.Collections.Generic;
using System.Linq;

namespace Search.Sdk
{
	public abstract class Index : IIndex
    {
		protected Dictionary<string, List<IShard>> Shards = new Dictionary<string, List<IShard>>();

		public IEnumerable<IShard> Search(SearchArgumentContext context)
		{
			var lower = context.Text.ToLowerInvariant();
			if(Shards.ContainsKey(lower))
			{
				var shards = Shards[lower];
				return shards.Where(x => x.IsMatch(context));
			}

			return Enumerable.Empty<IShard>();
		}
    }
}