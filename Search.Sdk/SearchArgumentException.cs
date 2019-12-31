using System;

namespace Search.Sdk
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