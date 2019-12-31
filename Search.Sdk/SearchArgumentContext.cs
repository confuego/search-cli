namespace Search.Sdk
{
	public class SearchArgumentContext
	{
		public string Text { get; set; }

		public string Column { get; set; }

		public Operator Operator { get; set; }

		public override string ToString()
		{
			return $"(text: {Text}, op: {Operator}, col: {Column})";
		}
	}
}