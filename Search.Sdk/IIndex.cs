using System.Collections.Generic;

namespace Search.Sdk
{
	public interface IIndex
	{
		 IEnumerable<IShard> Search(SearchArgumentContext context);
	}
}