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
					result = context.Text.Length == WordEnd - WordStart + 1;
					break;
				case Operator.StartsWith:
					result = WordStart == 0;
					break;
			}

			return result && columnIndex == Column;
		}
	}
}