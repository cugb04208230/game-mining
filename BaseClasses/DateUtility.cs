using System;

namespace BaseClasses
{
    public class DateUtility
    {
        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        public static DateTime GetTime(long timeStamp)
        {
	        var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            var time = startTime.AddMilliseconds(timeStamp);
            return time;
        }

		/// <summary>
		/// DateTime时间格式转换为Unix时间戳格式
		/// </summary>
		/// <param name="dateTime"> DateTime时间格式</param>
		/// <returns>Unix时间戳格式</returns>
		public static long ConvertDateTimeLong(DateTime dateTime)
        {
	        DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            var result = (long)(dateTime - startTime).TotalMilliseconds;
            return result;
        }
    }
}
