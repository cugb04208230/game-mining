using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{

	public class LoginModel
	{
		[Required]
		[RegularExpression(@"^[0-9a-zA-Z]{6,20}$", ErrorMessage = "用户名长度为6-20位的数字和大小写字母")]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		
	}
}