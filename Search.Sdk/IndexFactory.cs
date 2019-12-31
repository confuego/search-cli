using System;
using System.Collections.Generic;
using System.Data;

namespace Search.Sdk
{
    public class IndexFactory
    {

		private static readonly Dictionary<Type, Type> _typeMap = new Dictionary<Type, Type>
		{
			{ typeof(DataTable), typeof(TabularIndex) }
		};

		public static IIndex Create(object data)
		{
			var type  = data.GetType();

			if(_typeMap.ContainsKey(type))
			{
				return (IIndex) Activator.CreateInstance(_typeMap[type], data);
			}

			throw new TypeLoadException($"Type {type} not found");
		}

		public static T Create<T>(params object[] args)
			where T : IIndex
		{
			return (T) Activator.CreateInstance(typeof(T), args);
		}
    }
}