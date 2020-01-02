using System.Collections.Generic;

namespace Search.Sdk
{
	public static class SearchArgument
	{

		private static Dictionary<string, Operator> _operators = new Dictionary<string, Operator>
		{
			{ "=", Operator.Equals },
			{ "@>", Operator.Contains },
			{ "^=", Operator.StartsWith }
		};

		private static Dictionary<char, char?> _states = new Dictionary<char, char?>
		{
			{ '=', null },
			{ '@', '>' },
			{ '>', null },
			{ '^', '=' }
		};

		public static SearchArgumentContext Parse(string query)
		{
			var result = new SearchArgumentContext
			{
				Text = query.Trim(),
				Operator = Operator.General
			};
			var buf = string.Empty;
			string operatorBuf = null;
			char? nextState = null;
			char? currState = null;

			foreach (var character in query)
			{
				if(nextState.HasValue && character != nextState.Value)
				{
					throw new SearchArgumentException($"Expected '{nextState.Value}' after {currState.Value}");
				}

				if(_states.ContainsKey(character))
				{
					currState = character;
					nextState = _states[character];
					operatorBuf += currState;

					if(!nextState.HasValue)
					{
						if(_operators.ContainsKey(operatorBuf))
						{
							result.Operator = _operators[operatorBuf];
							result.Column = buf.Trim();
						}

						buf = null;
					}
				}
				else
				{
					buf += character;
				}
			}

			result.Text = buf;
			return result;
		}
	}
}