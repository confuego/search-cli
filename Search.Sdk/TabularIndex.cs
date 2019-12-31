using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace Search.Sdk
{
	public class TabularIndex : Index
	{

		private void CreateShards(DataTable table)
		{
			var result = new Dictionary<string, List<IShard>>();
			var currCol = string.Empty;
			DataRow currRow;
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

					Word.ForEach(currCol, (word, wordStart, wordEnd) => {
						if(word.Length <= 2) 
						{
							return;
						}

						var shard = new TabularShard
						{
							Column = col,
							Row = row,
							Source = table,
							WordStart = wordStart,
							WordEnd = wordEnd
						};

						var ngrams = NGram.Of(word.ToLowerInvariant(), 2);
						foreach (var ngram in ngrams)
						{
							if(!result.ContainsKey(ngram))
							{
								result.Add(ngram, new List<IShard>());
							}

							result[ngram].Add(shard);
						}
					});
				}
			}

			stopwatch.Stop();
			Console.WriteLine($"Indexed {result.Count} after {stopwatch.ElapsedMilliseconds}ms");

			Shards = result;
		}

		public TabularIndex(DataTable table)
		{
			CreateShards(table);
		}
	}
}