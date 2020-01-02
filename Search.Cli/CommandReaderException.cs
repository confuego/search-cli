using System;
using System.Runtime.Serialization;

namespace Search.Cli
{
	[System.Serializable]
	public class CommandReaderException : Exception
	{
		public CommandReaderException() { }
		public CommandReaderException(string message) : base(message) { }
		public CommandReaderException(string message, Exception inner) : base(message, inner) { }
		protected CommandReaderException(
			SerializationInfo info,
			StreamingContext context) : base(info, context) { }
	}
}