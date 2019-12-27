using System;
using System.Collections.Generic;
using Search.Cli.Services;

namespace Search.Cli
{
	public static class SearchArgumentParser
	{

		private static Dictionary<string, Operator> Operators = new Dictionary<string, Operator>
		{
			{ "=", Operator.Equals },
			{ "@>", Operator.Contains },
			{ "^=", Operator.StartsWith }
		};

		public static ArgumentContext Parse(string[] args)
		{
			var result = new ArgumentContext();
			try
			{
				if(args.Length == 1)
				{
					result.Operator = Operator.General;
					result.Text = args[0].ToLowerInvariant();

					return result;
				}
				else if(args.Length == 3)
				{
					result.Column = args[0].ToLowerInvariant();
					result.Operator = Operators[args[1]];
					result.Text = args[2].ToLowerInvariant();

					return result;
				}

				throw new SearchArgumentException($"Argument count was {args.Length}. (must be 1 or 3)");
			}
			catch(SearchArgumentException)
			{
				throw;
			}
			catch (Exception e) when (!(e is SearchArgumentException))
			{
				throw new SearchArgumentException("Unhandled exception", e);
			}
		}
	}
}