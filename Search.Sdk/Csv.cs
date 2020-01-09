using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace Search.Sdk
{
	public class Csv
	{
		private static void AddRow(DataTable table, string row)
		{
			var buffer = "";
			var currentColumnCount = 0;
			var tableColumnCount = table.Columns.Count;
			var dataRow = table.NewRow();
			var items = new object[tableColumnCount];
			var hasQuotes = false;

			foreach (var character in row.Trim())
			{
				if(currentColumnCount >= tableColumnCount)
				{
					break;
				}
				if(character == '"')
				{
					hasQuotes = !hasQuotes;
					continue;
				}
				if(character == ',' && !hasQuotes)
				{
					items[currentColumnCount] = buffer;
					currentColumnCount++;
					buffer = "";
					continue;
				}

				buffer += character;
			}
			dataRow.ItemArray = items;
			table.Rows.Add(dataRow);
		}

		private static void AddColumns(DataTable table, string columns)
		{
			var buffer = string.Empty;
			var duplicates = new Dictionary<string, int>();
			var hasQuotes = false;
			foreach (var character in columns)
			{
				if(character == '"')
				{
					hasQuotes = !hasQuotes;
					continue;
				}
				if(character == ',' && !hasQuotes)
				{
					if(table.Columns.Contains(buffer) && !duplicates.ContainsKey(buffer))
					{
						duplicates.Add(buffer, 1);
						buffer += "_1";
					}
					else if(table.Columns.Contains(buffer))
					{
						duplicates[buffer]++;
						buffer += $"{duplicates[buffer]}";
					}
					table.Columns.Add(new DataColumn(buffer.ToLowerInvariant()));
					buffer = "";
					continue;
				}

				buffer += character;
			}
		}


		public static async Task<DataTable> ReadAsync(string filePath)
		{
			var result = new DataTable();
			var rowCount = 0;
			using(var fileStream = File.OpenRead(filePath))
			{
				using(var gzipStream = new GZipStream(fileStream, CompressionMode.Decompress))
				{
					using(var reader = new StreamReader(gzipStream))
					{
						while(!reader.EndOfStream)
						{
							var row = await reader.ReadLineAsync();

							if(rowCount == 0) 
							{
								AddColumns(result, row);
							}
							else 
							{
								AddRow(result, row);
							}

							rowCount++;
						}
					}
				}
			}

			return result;
		}

		public static async Task<DataSet> ReadAllAsync(params string[] paths)
		{
			var tasks = new List<Task<DataTable>>();

			tasks.AddRange(paths.ToList().Select(x => ReadAsync(x)));

			await Task.WhenAll(tasks);

			var set = new DataSet();

			tasks.ForEach(x => set.Tables.Add(x.Result));

			return set;
		}
	}
}