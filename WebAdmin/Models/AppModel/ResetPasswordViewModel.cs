using System.ComponentModel.DataAnnotations;

namespace WebAdmin.Models.AppModel
{
	/// <summary>
	/// 重置密码
	/// </summary>
	public class ResetPasswordModel
	{
		/// <summary>
		/// 密码
		/// </summary>
		[DataType(DataType.Password)]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{6,20}$", ErrorMessage = "PasswordRegexError")]
		public string Password { get; set; }

		/// <summary>
		/// 密码确认
		/// </summary>
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "PasswordNotMatch")]
		public string ConfirmPassword { get; set; }
		
	}
}