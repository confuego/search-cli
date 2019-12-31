namespace Search.Sdk
{
	public class NGramException : System.Exception
	{
		public NGramException() { }
		public NGramException(string message) : base(message) { }
		public NGramException(string message, System.Exception inner) : base(message, inner) { }
		protected NGramException(
			System.Runtime.Serialization.SerializationInfo info,
			System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}