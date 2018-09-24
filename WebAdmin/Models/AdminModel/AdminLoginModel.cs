using System.ComponentModel.DataAnnotations;

namespace WebAdmin.Models.AdminModel
{
	
	/// <summary>
	/// 登录
	/// </summary>
	public class AdminLoginModel
	{
		/// <summary>
		/// 用户名
		/// </summary>
		[Required(ErrorMessage = "请输入用户名")]
		public string UserName { get; set; }

		/// <summary>
		/// 密码
		/// </summary>
		[Required(ErrorMessage = "请输入密码")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

	}
}