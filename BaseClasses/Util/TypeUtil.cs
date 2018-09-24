using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.Util
{
	/// <summary>
	/// Type帮助工具
	/// </summary>
	public class TypeFinder
	{
		private TypeFinder() { }
		private IList<Assembly> _assemblies;

		/// <summary>
		/// 获取程序集
		/// </summary>
		/// <param name="assemblies"></param>
		/// <returns></returns>
		public static TypeFinder SetScope(params Assembly[] assemblies)
		{
			var finder = new TypeFinder { _assemblies = assemblies };
			return finder;
		}

		/// <summary>
		/// Type-Where
		/// </summary>
		/// <param name="selector"></param>
		/// <returns></returns>
		public IEnumerable<Type> Where(Func<Type, bool> selector)
		{
			var type = new List<Type>(0);
			foreach (var assemlby in _assemblies)
			{
				try
				{
					type.AddRange(assemlby.GetTypes().Where(selector));
				}
				catch (ReflectionTypeLoadException e)
				{
					type.AddRange(e.Types.Where(t => t != null && selector(t)));
				}

			}
			return type;
		}
	}
}
