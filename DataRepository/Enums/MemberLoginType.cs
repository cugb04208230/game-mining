using System.ComponentModel;

namespace DataRepository.Enums
{
	/// <summary>
	/// 用户登录类型
	/// </summary>
	public enum MemberLoginType
	{
		/// <summary>
		/// 用户名密码
		/// </summary>
		[Description("用户名密码")] UserNameAndPassword = 1,
		/// <summary>
		/// 授权微信
		/// </summary>
		[Description("授权微信")] AuthWeChat = 2
	}
}
