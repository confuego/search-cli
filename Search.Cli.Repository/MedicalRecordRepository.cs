using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Search.Cli.Repository
{
	public class MedicalRecordRepository : IMedicalRecordRepository
	{
		public async Task<DataTable> ReadAsync(string filePath)
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

		private void AddRow(DataTable table, string row)
		{
			var buffer = "";
			var currentColumnCount = 0;
			var tableColumnCount = table.Columns.Count;
			var dataRow = table.NewRow();
			var items = new object[tableColumnCount];

			foreach (var character in row)
			{
				if(currentColumnCount >= tableColumnCount)
				{
					break;
				}
				if(character == ',')
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

		private void AddColumns(DataTable table, string columns)
		{
			var buffer = string.Empty;
			var duplicates = new Dictionary<string, int>();
			foreach (var character in columns)
			{
				if(character == ',')
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
					table.Columns.Add(new DataColumn(buffer));
					buffer = "";
					continue;
				}

				buffer += character;
			}
		}
	}
}