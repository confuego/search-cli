using Search.Cli.Repository;

namespace Search.Cli.Services
{
	public static class SearchFactory
	{
		public static ISearchService Create(Operator op)
		{
			var repo = new MedicalRecordRepository();
			switch(op)
			{
				case Operator.StartsWith:
					return new StartsWithSearchService(repo);
				case Operator.Equals:
					return new EqualsSearchService(repo);
				case Operator.Contains:
					return new ContainsSearchService(repo);
				default:
					return new GeneralSearchService(repo);
			}
		}
	}
}