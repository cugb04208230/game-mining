using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
	public class RegisterViewModel
	{
		[Required]
		public string RecommondUserName { set; get; }
	}

	public class RegisterModel
	{
		[Required]
		[RegularExpression(@"^[0-9a-zA-Z]{6,20}$", ErrorMessage = "用户名长度为6-20位的数字和大小写字母")]
		public string UserName { get; set; }

		[Required]
		[RegularExpression(@"^[0-9a-zA-Z]{6,20}$", ErrorMessage = "密码长度为6-20位的数字和大小写字母")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
		public string ConfirmPassword { get; set; }

		[Required]
		public string RecommondUserName { set; get; }
	}
}