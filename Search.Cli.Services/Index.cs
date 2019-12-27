using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace Search.Cli.Services
{
    public class Index
    {
		private readonly Dictionary<string, List<Shard>> _shards = new Dictionary<string, List<Shard>>();

		public Index(Dictionary<string, List<Shard>> shards)
		{
			_shards = shards;
		}


		public IEnumerable<Shard> Search(string keyword)
		{
			if(_shards.ContainsKey(keyword))
			{
				return _shards[keyword.ToLowerInvariant()];
			}

			return Enumerable.Empty<Shard>();
		}

		public static Index Create(DataTable table)
		{
			var result = new Dictionary<string, List<Shard>>();
			var strBuffer = string.Empty;
			var currCol = string.Empty;
			DataRow currRow;
			char currChar;
			var startChar = 0;
			var stopwatch = new Stopwatch();

			stopwatch.Start();


			for(var row = 0; row < table.Rows.Count; row++)
			{
				currRow = table.Rows[row];

				for(var col = 0; col < currRow.ItemArray.Length; col++)
				{
					currCol = currRow.ItemArray[col] as string;

					if(string.IsNullOrWhiteSpace(currCol))
					{
						continue;
					}

					for (var charCount = 0; charCount < currCol.Length; charCount++)
					{
						currChar = currCol[charCount];
						var isWordBreak = Char.IsWhiteSpace(currChar)
							|| Char.IsPunctuation(currChar)
							|| Char.IsSeparator(currChar);

						if(isWordBreak 
							&& !string.IsNullOrWhiteSpace(strBuffer)
							&& strBuffer.Length > 2)
						{
							var shard = new Shard
							{
								Column = col,
								Row = row,
								WordStart = startChar,
								WordEnd = charCount
							};

							var ngrams = NGram.Of(strBuffer.ToLowerInvariant(), 2);
							foreach (var ngram in ngrams)
							{
								if(!result.ContainsKey(ngram))
								{
									//Console.WriteLine($"{lower}");
									result.Add(ngram, new List<Shard>());
								}

								result[ngram].Add(shard);
							}

							strBuffer = string.Empty;
							startChar = charCount + 1;

							continue;
						}
						else
						{
							strBuffer += currChar;
						}
					}
					startChar = 0;
				}
			}

			stopwatch.Stop();
			Console.WriteLine($"Completed index after {stopwatch.Elapsed}.");
			Console.WriteLine($"Index count is {result.Count}.");
			Console.WriteLine();

			return new Index(result);
		}
    }
}