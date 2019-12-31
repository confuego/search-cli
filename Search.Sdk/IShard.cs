namespace Search.Sdk
{
    public interface IShard
	{
		bool IsMatch(SearchArgumentContext context);

		T ReadData<T>(string set);
	}
}