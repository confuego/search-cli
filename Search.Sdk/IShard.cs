using System;

namespace Search.Sdk
{
    public interface IShard : IComparable<IShard>, IEquatable<IShard>
	{
		bool IsMatch(SearchArgumentContext context);

		T ReadData<T>(string set);
	}
}