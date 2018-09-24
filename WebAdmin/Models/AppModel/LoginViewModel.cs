using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace WebAdmin.Models.AppModel
{

	/// <summary>
	/// 登录
	/// </summary>
	public class LoginModel
	{
		/// <summary>
		/// 用户名
		/// </summary>
		[RegularExpression(@"^[0-9a-zA-Z]{6,20}$", ErrorMessage = "UserNameLengthError")]
		public string UserName { get; set; }

		/// <summary>
		/// 密码
		/// </summary>
		[RegularExpression(@"^[0-9a-zA-Z]{6,20}$", ErrorMessage = "PasswordRegexError")]
		[DataType(DataType.Password)]
		public string Password { get; set; }


		/// <summary>
		/// 验证码
		/// </summary>
		[RegularExpression(@"^[0-9]{4}$", ErrorMessage = "VerifyCodeError")]
		[DataType(DataType.Password)]
		public string Code { set; get; }
		
	}
}