using System;
using System.Data;

namespace Search.Sdk
{
	public class TabularShard : IShard
	{
		public int Row { get; set; }

		public int Column { get; set; }

		public DataTable Source { get; set; }

		public int WordStart { get; set; }

		public int WordEnd { get; set; }

		public T ReadData<T>(string column)
		{
			return (T) Source.Rows[Row][column];
		}

		public bool IsMatch(SearchArgumentContext context)
		{
			if(context.Operator == Operator.General)
			{
				return true;
			}

			var columnIndex = Source.Columns[context.Column].Ordinal;
			var result = true;

			switch(context.Operator)
			{
				case Operator.Equals:
					result = context.Text.Length == WordEnd - WordStart + 1 && WordStart == 0;
					break;
				case Operator.StartsWith:
					result = WordStart == 0;
					break;
			}

			return result && columnIndex == Column;
		}

		public int CompareTo(IShard other)
		{
			var tabularShard = other as TabularShard;

			var matchingCell = tabularShard.Row == Row
				&& tabularShard.Column == Column
				&& tabularShard.Source == Source;

			if(matchingCell 
				&& tabularShard.WordStart == WordStart 
				&& tabularShard.WordEnd == WordEnd)
			{
				return 0;
			}
			else if(matchingCell
				&& tabularShard.WordStart > WordStart)
			{
				return 1;
			}
			else if(matchingCell
				&& WordStart > tabularShard.WordStart)
			{
				return -1;
			}
			else if(WordStart > tabularShard.WordStart)
			{
				return -1;
			}
			else if(tabularShard.WordStart > WordStart)
			{
				return 1;
			}

			return 0;
		}

		public bool Equals(IShard other)
		{
			return CompareTo(other) == 0;
		}

		public override string ToString()
		{
			return $"(Row: {Row}, Column: {Column}, Start: {WordStart}, End: {WordEnd}, Data: {Source.Rows[Row][Column].ToString().Substring(WordStart, WordEnd - WordStart + 1)})";
		}
	}
}