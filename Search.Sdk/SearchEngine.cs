using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Search.Sdk
{
	public class SearchEngine
	{
		private ConcurrentDictionary<string, IIndex> _indices = new ConcurrentDictionary<string, IIndex>();

		public SearchEngine Index(string filePath, string indexName = null)
		{
			var task = IndexAsync(filePath, indexName);
			Task.WaitAll(task);

			return task.Result;
		}

		public async Task<SearchEngine> IndexAsync(string filePath, string indexName = null)
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();
			var table = await Csv.ReadAsync(filePath);
			var index = IndexFactory.Create(table);

			if(string.IsNullOrWhiteSpace(indexName))
			{
				indexName = Guid.NewGuid().ToString();
			}

			_indices.TryAdd(indexName, index);

			stopwatch.Stop();
			Console.WriteLine($"Indexing '{filePath}' took {stopwatch.ElapsedMilliseconds}ms");

			return this;
		}

		public SearchEngine IndexAll(params string[] filePaths)
		{
			var task = IndexAllAsync(filePaths);
			Task.WaitAll(task);

			return task.Result;
		}

		public async Task<SearchEngine> IndexAllAsync(params string[] filePaths)
		{
			if(filePaths == null || filePaths.Length == 0)
			{
				throw new ArgumentNullException(nameof(filePaths));
			}

			var indexName = string.Empty;
			var lastArg = filePaths[filePaths.Length - 1];

			if(filePaths.Length >= 2 && !File.Exists(lastArg))
			{
				indexName = lastArg;
			}

			var tasks = filePaths.ToList().Select(x => IndexAsync(x, indexName));

			await Task.WhenAll(tasks);

			return this;
		}

		public IEnumerable<string> Search(string query, string indexPattern = null)
		{
			var task = SearchAsync(query, indexPattern);
			Task.WaitAll(task);

			return task.Result;
		}

		public async Task<IEnumerable<string>> SearchAsync(string query, string indexPattern = null)
		{
			var context = SearchArgumentParser.Parse(query);
			var stopwatch = new Stopwatch();
			stopwatch.Start();

			var indicesToSearch = _indices.ToDictionary(k => k.Key, v => v.Value);

			if(!string.IsNullOrWhiteSpace(indexPattern))
			{
				var regex = new Regex(indexPattern);
				indicesToSearch = indicesToSearch
					.Where(x => regex.IsMatch(x.Key))
					.ToDictionary(k => k.Key, v => v.Value);
			}

			var tasks = indicesToSearch.Select(x => Task.Factory.StartNew(() =>
			{
				return x.Value.Search(context);
			}));

			await Task.WhenAll(tasks);

			Console.WriteLine($"Searching for {context} took {stopwatch.ElapsedMilliseconds}ms");

			return tasks.SelectMany(x => x.Result).Select(x => x.ReadData<string>("Primary DI").TrimStart('0'));
		}
	}
}