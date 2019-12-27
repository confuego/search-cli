using System;
using System.Collections.Generic;

namespace Search.Cli.Services
{
    public class NGram
    {
        public static List<string> Of(string word, int startingCount, int? endingCount = null)
		{
			if(string.IsNullOrWhiteSpace(word))
			{
				throw new NullReferenceException(nameof(word));
			}

			endingCount = endingCount.GetValueOrDefault(word.Length);

			if(startingCount < 0)
			{
				throw new NGramException("Starting count is less than 0.");
			}

			if(endingCount < 0)
			{
				throw new NGramException("Ending count is less than 0.");
			}

			if(startingCount > endingCount)
			{
				throw new NGramException("Starting count greater than ending count.");
			}

			if(word.Length < endingCount)
			{
				throw new NGramException("Word length is less than ending count.");
			}

			var strBuffer = string.Empty;
			var result = new List<string>(endingCount.Value - startingCount);
			for(var count = 0; count < word.Length; count++)
			{
				strBuffer += word[count];
				if(count >= startingCount && count <= endingCount)
				{
					result.Add(strBuffer);
				}
			}

			return result;
		}
    }
}