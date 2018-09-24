using System;

namespace BaseClasses.Util
{
	public static class RandomUtil
	{

		/// <summary>
		/// 生成随机验证码
		/// </summary>
		/// <returns></returns>
		public static string GenerateVerfiyCode(long verifyCodeLength=6)
		{
			long tick = DateTime.Now.Ticks;
			Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
			string verifyCode = "";
			for (int i = 0; i < verifyCodeLength; i++)
			{
				if (i == 0)
				{
					verifyCode += ran.Next(1, 9).ToString();
				}
				else
				{
					verifyCode += ran.Next(0, 9).ToString();
				}
			}
			return verifyCode;
		}

	}
}
