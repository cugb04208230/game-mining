using System.ComponentModel.DataAnnotations;

namespace WebAdmin.Models.AppModel
{
	/// <summary>
	/// 注册
	/// </summary>
	public class RegisterViewModel
	{
		/// <summary>
		/// 推荐人用户名
		/// </summary>
		[Required]
		public string RecommondUserName { set; get; }
	}

	/// <summary>
	/// 注册模型
	/// </summary>
	public class RegisterModel
	{
		/// <summary>
		/// 用户名
		/// </summary>
		[RegularExpression(@"^[0-9a-zA-Z]{6,20}$", ErrorMessage = "UserNameLengthError")]
		public string UserName { get; set; }

		/// <summary>
		/// 密码
		/// </summary>
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{6,20}$", ErrorMessage = "PasswordRegexError")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		/// <summary>
		/// 确认密码
		/// </summary>
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "PasswordNotMatch")]
		public string ConfirmPassword { get; set; }
		
		/// <summary>
		/// 推荐人用户名
		/// </summary>
		public string RecommondUserName { set; get; }

		/// <summary>
		/// 用户名称
		/// </summary>
		[Required(ErrorMessage = "UserNameCanNotBeEmpty")]
		public string Name { set; get; }

		/// <summary>
		/// 手机号
		/// </summary>
		[Required(ErrorMessage = "MobileCanNotBeEmpty")]
		[RegularExpression(@"^[1][\d]{10}$", ErrorMessage = "MobileRegexError")]
		public string Mobile { set; get; }

		/// <summary>
		/// 支付宝账号
		/// </summary>
		public string Alipay { set; get; }

		/// <summary>
		/// 微信账号
		/// </summary>
		public string WeChat { set; get; }

		/// <summary>
		/// 钱包地址
		/// </summary>
		[Required(ErrorMessage = "WalletCanNotBeEmpty")]
		public string BitCoin { set; get; }


		/// <summary>
		/// 国际区号
		/// </summary>
		[Required(ErrorMessage = "GlobalAreaCodeCanNotBeEmpty")]
		public string GlobalAreaCode { set; get; }

		/// <summary>
		/// 银行名称
		/// </summary>
		[Required(ErrorMessage = "BankNameCanNotBeEmpty")]
		public string BankName { set; get; }

		/// <summary>
		/// 银行卡号
		/// </summary>
		[Required(ErrorMessage = "BankcardNoCanNotBeEmpty")]
		public string BankCode { set; get; }
		/// <summary>
		/// 验证码
		/// </summary>
		[RegularExpression(@"^[0-9]{4}$", ErrorMessage = "InvalidVerificationCode")]
		[DataType(DataType.Password)]
		public string Code { set; get; }
	}
}