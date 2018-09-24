using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClasses.Util
{
	/// <summary>
	/// 值校验工具
	/// </summary>
	public static class ValueCheckUtil
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static bool ProcessStr(this string str)
		{
			var dangerousStr = "exec |insert |select |delete |update |count |chr |mid |master |truncate |char |declare |and|exec|insert|select|delete|update|count|*|chr|mid|master|truncate|char|declare|script";
			bool returnValue = true;
			try
			{
				if (str != "")
				{
					string[] anyStr = dangerousStr.Split('|');
					foreach(string ss in anyStr)
					{
						if(str.IndexOf(ss, StringComparison.Ordinal)>=0)
						{
							returnValue = false;
						}
					}
				}
			}
			catch
			{
				returnValue = false;
			}
			return returnValue;
		}
	}
}
