using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
	public class ResetPasswordModel
	{
		[Required]
		[DataType(DataType.Password)]
		[RegularExpression(@"^[0-9a-zA-Z]{6,20}$", ErrorMessage = "用户名长度为6-20位的数字和大小写字母")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
		public string ConfirmPassword { get; set; }
		
	}
}