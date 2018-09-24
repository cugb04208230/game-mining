using System.ComponentModel.DataAnnotations;

namespace WebAdmin.Models.AppModel
{
	/// <summary>
	/// 更新个人信息
	/// </summary>
	public class MemberInfoUpdateViewModel
	{
		/// <summary>
		/// 比特币账号
		/// </summary>
		public string BitCoin { set; get; }

		/// <summary>
		/// 国际区号
		/// </summary>
		public string GlobalAreaCode { set; get; }

	}
}