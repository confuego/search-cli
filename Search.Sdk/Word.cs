using System;
using System.Collections.Generic;

namespace Search.Sdk
{
    public class Word
    {

		public static bool IsWordBreak(char c)
		{
			return Char.IsWhiteSpace(c)
					|| Char.IsPunctuation(c)
					|| Char.IsSeparator(c);
		}


		public static void ForEach(string text, Action<string> wordSelector)
		{
			ForEach(text, (word, start, end) => wordSelector(word));
		}

		public static void ForEach(string text, Action<string, int, int> wordSelector)
		{
			if(string.IsNullOrWhiteSpace(text))
			{
				throw new ArgumentNullException(nameof(text));
			}

			var buf = string.Empty;
			char curr;
			var wordStart = 0;
			var wordEnd = 0;

			for(var charIndex = 0; charIndex < text.Length; charIndex++)
			{
				curr = text[charIndex];

				if(IsWordBreak(curr))
				{
					wordSelector(buf, wordStart, wordEnd);
					wordStart = charIndex + 1;
					buf = string.Empty;
				}
				else
				{
					buf += curr;
					wordEnd = charIndex;
				}
			}
		}

        public static List<string> ParseAll(string text)
		{
			var result = new List<string>();
			
			ForEach(text, (word, start, end) => result.Add(word));

			return result;
		}
    }
}