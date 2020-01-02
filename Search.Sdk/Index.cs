using System.Collections.Generic;
using System.Linq;

namespace Search.Sdk
{
	public abstract class Index : IIndex
    {
		protected Dictionary<string, List<IShard>> Shards = new Dictionary<string, List<IShard>>();

		public IEnumerable<IShard> Search(SearchArgumentContext context)
		{	
			var orig = context.Text;
			var result = new List<IShard>();

			Word.ForEach(context.Text, word => {
				if(Shards.ContainsKey(word))
				{
					context.Text = word;
					result.AddRange(Shards[word].Where(x => x.IsMatch(context)));
				}
			});
			context.Text = orig;
			return result.OrderByDescending(x => x);
		}
    }
}