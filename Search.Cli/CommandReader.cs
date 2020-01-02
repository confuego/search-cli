using System.Collections.Generic;
using System.Linq;

namespace Search.Cli
{
	public class CommandReader
	{
		private enum State
		{
			ReadFlag,
			ReadArg,
			Done,
			AddFlag
		}

		private readonly Dictionary<string, Argument> _arguments = new Dictionary<string, Argument>();

		public CommandReader(params string[] flags) 
			: this(flags.Select(x => new Argument { Name = x }).ToArray())
		{
		}

		public CommandReader(params Argument[] arguments)
		{
			foreach(var arg in arguments)
			{
				_arguments.Add(arg.Name, arg);

				foreach(var alias in arg.Aliases)
				{
					_arguments.Add(alias, arg);
				}
			}
		}

		public IEnumerable<Command> Parse(string[] args)
		{

			if(args == null || args.Length == 0)
			{
				throw new CommandReaderException("No arguments were found.");
			}

			var result = new Dictionary<string, Command>();
			var state = State.ReadFlag;
			var currIdx = 0;
			Argument currFlag = null;

			while(state != State.Done && currIdx < args.Length)
			{
				switch(state)
				{
					case State.ReadArg:
						if(_arguments.ContainsKey(args[currIdx]))
						{
							throw new CommandReaderException("Expected argument got flag.");
						}

						if(!result.ContainsKey(currFlag.Name))
						{
							result.Add(currFlag.Name, new Command
							{
								Name = currFlag.Name
							});
						}
						result[currFlag.Name].Arguments.Add(args[currIdx]);
						currIdx++;
						state = currIdx < args.Length? State.AddFlag : State.Done;
						break;
					case State.ReadFlag:
						var curr = args[currIdx];
						if(_arguments.ContainsKey(curr))
						{
							currFlag = _arguments[curr];
							state = State.ReadArg;
							currIdx++;
						}
						else
						{
							throw new CommandReaderException("No arguments found.");
						}
						break;
				}
			}

			if(state != State.Done)
			{
				throw new CommandReaderException($"Parsing did not complete. Last state was {state}");
			}

			return result.Select(x => x.Value);
		}
	}
}