using System;
using System.Collections.Generic;
using System.Linq;
using Search.Sdk;

namespace Search.Cli
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var reader = new CommandReader(new Argument
			{
				Name = "--index",
				Aliases = new List<string>
				{
					"-i"
				}
			});
			var engine = new SearchEngine();

			var command = reader.Parse(args).Where(x => x.Name == "--index").First();
			engine.IndexAll(command.Arguments.ToArray());

			ConsoleKeyInfo current;
			var commandBuf = string.Empty;
			var charsToIgnore = new HashSet<ConsoleKey>
			{
				ConsoleKey.Backspace
			};
			Console.Write("> ");
			do {
				current = Console.ReadKey();
				
				if(!charsToIgnore.Contains(current.Key))
				{
					commandBuf += current.KeyChar;
				}

				switch(current.Key)
				{
					case ConsoleKey.Backspace:
						if(commandBuf.Length > 0)
						{
							Console.Write("\b \b");
							commandBuf = commandBuf.Substring(0, commandBuf.Length - 1);
						}
						break;
					case ConsoleKey.Enter:
						Console.Clear();
						Console.WriteLine($"> {commandBuf}");
						var results = engine.Search(commandBuf.Trim());

						foreach(var result in results)
						{
							Console.WriteLine(result.ReadData<string>("Primary DI").TrimStart('0'));
						}

						commandBuf = string.Empty;
						Console.Write("> ");
						break;
				}
			} while(current.Key != ConsoleKey.Escape);
		}
	}
}
