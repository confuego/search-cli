using System;

namespace Search.Cli
{
	public class SearchArgumentException : Exception
	{
		public SearchArgumentException(string message): base(message)
		{

		}

		public SearchArgumentException(string message, Exception inner): base(message, inner)
		{
			
		}
	}
}