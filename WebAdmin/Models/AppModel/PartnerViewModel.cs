using System.ComponentModel.DataAnnotations;

namespace WebAdmin.Models.AppModel
{
	/// <summary>
	/// 合伙人
	/// </summary>
	public class PartnerViewModel
	{
		/// <summary>
		/// 分页参数-页码
		/// </summary>
		public int? PageIndex { set; get; }

		/// <summary>
		/// 分页参数-数量
		/// </summary>
		public int? PageSize { set; get; }
	}

	/// <summary>
	/// 激活合伙人
	/// </summary>
	public class ActivePartnerViewModel
	{
		/// <summary>
		/// 用户名
		/// </summary>
		[Required(ErrorMessage = "UserNameCanNotBeEmpty")]
		public string UserName { set; get; }

		/// <summary>
		/// 用户姓名
		/// </summary>
		public string Name { set; get; }
	}




}