using System.Text.RegularExpressions;

namespace BaseClasses.Extensions
{
	/// <summary>
	/// 字符串的扩展函数
	/// </summary>
    public static class StringExtension
    {

	    /// <summary>
	    /// 单词变成单数形式
	    /// </summary>
	    /// <param name="word"></param>
	    /// <returns></returns>
	    public static string ToSingular(this string word)
	    {
		    Regex plural1 = new Regex("(?<keep>[^aeiou])ies$");
		    Regex plural2 = new Regex("(?<keep>[aeiou]y)s$");
		    Regex plural3 = new Regex("(?<keep>[sxzh])es$");
		    Regex plural4 = new Regex("(?<keep>[^sxzhyu])s$");

		    if (plural1.IsMatch(word))
			    return plural1.Replace(word, "${keep}y");
		    if (plural2.IsMatch(word))
			    return plural2.Replace(word, "${keep}");
		    if (plural3.IsMatch(word))
			    return plural3.Replace(word, "${keep}");
		    if (plural4.IsMatch(word))
			    return plural4.Replace(word, "${keep}");
		    return word;
	    }

	    /// <summary>
	    /// 单词变成复数形式
	    /// </summary>
	    /// <param name="word"></param>
	    /// <returns></returns>
	    public static string ToPlural(this string word)
	    {
		    Regex plural1 = new Regex("(?<keep>[^aeiou])y$");
		    Regex plural2 = new Regex("(?<keep>[aeiou]y)$");
		    Regex plural3 = new Regex("(?<keep>[sxzh])$");
		    Regex plural4 = new Regex("(?<keep>[^sxzhy])$");

		    if (plural1.IsMatch(word))
			    return plural1.Replace(word, "${keep}ies");
		    if (plural2.IsMatch(word))
			    return plural2.Replace(word, "${keep}s");
		    if (plural3.IsMatch(word))
			    return plural3.Replace(word, "${keep}es");
		    if (plural4.IsMatch(word))
			    return plural4.Replace(word, "${keep}s");
		    return word;
	    }

		/// <summary>
		/// 转换
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="me"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static T To<T>(this object me, T defaultValue = default(T)) where T : struct
		{
			if (me == null)
			{
				return defaultValue;
			}
			try
			{
				return (T)System.Convert.ChangeType(me, typeof(T));
			}
			catch
			{
				return defaultValue;
			}
		}

		/// <summary>
		/// 转换
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="me"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static T? Nullable<T>(this object me, T? defaultValue = null) where T : struct
		{
			if (me == null)
			{
				return defaultValue;
			}
			try
			{
				return (T)System.Convert.ChangeType(me, typeof(T));
			}
			catch
			{
				return defaultValue;
			}
		}

		/// <summary>
		/// 是否为空
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool IsNullOrEmpty(this string value)
		{
			return string.IsNullOrEmpty(value);
		}

		/// <summary>
		/// 是否不为空
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool IsNotNullOrEmpty(this string value)
		{
			return !string.IsNullOrEmpty(value);
		}

		/// <summary>
		/// 格式化输出
		/// </summary>
		/// <param name="value"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public static string Formats(this string value, params object[] args)
		{
			return string.Format(value, args);
		}

		/// <summary>
		/// Url编码
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string UrlEncode(this string value)
		{
			return System.Web.HttpUtility.UrlEncode(value);
		}

		/// <summary>
		/// Url解码
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string UrlDecode(this string value)
		{
			return System.Web.HttpUtility.UrlDecode(value);
		}
	}
}
