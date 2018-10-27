using System;
using System.ComponentModel.DataAnnotations;
using BaseClasses.Data;
using DataRepository.Entities;
using DataRepository.Enums;

namespace WebAdmin.Models.AdminModel
{
	/// <summary>
	/// 用户列表查询
	/// </summary>
	public class MemberViewModel
	{
		/// <summary>
		/// 用户名
		/// </summary>
		public string UserName { set; get; }
		/// <summary>
		/// 推荐人用户名
		/// </summary>
		public string Chain { get; set; }

		/// <summary>
		/// 分页参数-页码
		/// </summary>
		public int? PageIndex { set; get; }

		/// <summary>
		/// 分页参数-数量
		/// </summary>
		public int? PageSize { set; get; }

		/// <summary>
		/// 注册时间
		/// </summary>
		public DateTime? CreatedAtFrom { set; get; }

		/// <summary>
		/// 注册时间
		/// </summary>
		public DateTime? CreatedAtTo { set; get; }

		/// <summary>
		/// 用户列表
		/// </summary>
		public QueryResult<Member> Members { set; get; }
		/// <summary>
		/// 金账户余额-交易赠送/转银和铜
		/// </summary>
		public decimal? GoldBalanceFrom { get; set; }
		/// <summary>
		/// 银账户余额-购买挖矿装备
		/// </summary>
		public decimal? SilverBalanceFrom { get; set; }
		/// <summary>
		/// 铜账户余额-招募玩家
		/// </summary>
		public decimal? CopperBalanceFrom { get; set; }
		/// <summary>
		/// 矿渣账户余额-用于精炼
		/// </summary>
		public decimal? SlagBalanceFrom { get; set; }
		/// <summary>
		/// 金账户余额-交易赠送/转银和铜
		/// </summary>
		public decimal? GoldBalanceTo { get; set; }
		/// <summary>
		/// 银账户余额-购买挖矿装备
		/// </summary>
		public decimal? SilverBalanceTo { get; set; }
		/// <summary>
		/// 铜账户余额-招募玩家
		/// </summary>
		public decimal? CopperBalanceTo { get; set; }
		/// <summary>
		/// 矿渣账户余额-用于精炼
		/// </summary>
		public decimal? SlagBalanceTo { get; set; }


	}

	/// <summary>
	/// 细致用户
	/// </summary>
	public class MemberPostModel
	{
		/// <summary>
		/// 用户名
		/// </summary>
		[Required(ErrorMessage = "用户名必须填写", AllowEmptyStrings = false)]
		[RegularExpression(@"^[0-9a-zA-Z]{6,20}$", ErrorMessage = "用户名长度为6-20位的数字和大小写字母")]
		public string UserName { get; set; }

		/// <summary>
		/// 密码
		/// </summary>
		[Required(ErrorMessage = "密码必须填写", AllowEmptyStrings = false)]
		[RegularExpression(@"^[0-9a-zA-Z]{6,20}$", ErrorMessage = "密码长度为6-20位的数字和大小写字母")]
		public string Password { set; get; }

		/// <summary>
		/// 昵称
		/// </summary>

		[Required(ErrorMessage = "昵称必须填写", AllowEmptyStrings = false)]
		public string NickName { set; get; }
	}

	/// <summary>
	/// 用户重置密码
	/// </summary>
	public class MemberResetPasswordModel
	{
		/// <summary>
		/// 用户名
		/// </summary>
		[Required(ErrorMessage = "用户名不能为空", AllowEmptyStrings = false)]
		[RegularExpression(@"^[0-9a-zA-Z]{6,20}$", ErrorMessage = "用户名长度为6-20位的数字和大小写字母")]
		public string UserName { get; set; }

		/// <summary>
		/// 密码
		/// </summary>
		[Required(ErrorMessage = "密码必须填写", AllowEmptyStrings = false)]
		[RegularExpression(@"^[0-9a-zA-Z]{6,20}$", ErrorMessage = "密码长度为6-20位的数字和大小写字母")]
		public string Password { set; get; }

		/// <summary>
		/// 用户类型
		/// </summary>
		public MemberType MemberType { set; get; }
	}


	/// <summary>
	/// 用户重置密码
	/// </summary>
	public class MemberUpdateInfoModel
	{
		/// <summary>
		/// 用户名
		/// </summary>
		[Required(ErrorMessage = "用户名必须填写", AllowEmptyStrings = false)]
		[RegularExpression(@"^[0-9a-zA-Z]{6,20}$", ErrorMessage = "用户名长度为6-20位的数字和大小写字母")]
		public string UserName { get; set; }
		
		/// <summary>
		/// 用户名称
		/// </summary>
		[Required(ErrorMessage = "UserNameCanNotBeEmpty")]
		public string Name { set; get; }

		/// <summary>
		/// 手机号
		/// </summary>
		[Required(ErrorMessage = "MobileCanNotBeEmpty")]
		[RegularExpression(@"^[\d]{6,20}$", ErrorMessage = "MobileRegexError")]
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
		/// 用户类型
		/// </summary>
		public MemberType MemberType { set; get; }
	}

	public class MemberPartnerViewModel
	{
		/// <summary>
		/// 推荐人用户名
		/// </summary>
		public string RecommendUserName { get; set; }

		/// <summary>
		/// 分页参数-页码
		/// </summary>
		public int? PageIndex { set; get; }

		/// <summary>
		/// 分页参数-数量
		/// </summary>
		public int? PageSize { set; get; }
		/// <summary>
		/// 黄金余额基数
		/// </summary>
		public decimal? Gold { set; get; }

		/// <summary>
		/// 用户列表
		/// </summary>
		public QueryResult<Member> Members { set; get; }
	}

	public class MemberPartnerManageModel
	{
		/// <summary>
		/// 推荐人用户名
		/// </summary>
		public string RecommendUserName { get; set; }

		/// <summary>
		/// 黄金余额基数
		/// </summary>
		public decimal Gold { set; get; }

		/// <summary>
		/// 扣减百分比
		/// </summary>
		public decimal Percent { set; get; }

		/// <summary>
		/// 币种类型
		/// </summary>
		public AccountType CoinType { set; get; }

		/// <summary>
		/// 范围
		/// </summary>
		public int Area { set; get; }
	}



	/// <summary>
	/// 用户重置密码
	/// </summary>
	public class MemberUpdateBalanceModel
	{
		/// <summary>
		/// 用户名
		/// </summary>
		[Required(ErrorMessage = "用户名必须填写", AllowEmptyStrings = false)]
		[RegularExpression(@"^[0-9a-zA-Z]{6,20}$", ErrorMessage = "用户名长度为6-20位的数字和大小写字母")]
		public string UserName { get; set; }


		public decimal? Gold { set; get; }
		public decimal? Silver { set; get; }
		public decimal? Copper { set; get; }
		public decimal? Slag { set; get; }
		public decimal? Diamond { set; get; }
	}

	/// <summary>
	/// 用户列表查询
	/// </summary>
	public class MemberBalanceUpdateViewModel
	{
		/// <summary>
		/// 用户名
		/// </summary>
		public string UserName { set; get; }
		

		/// <summary>
		/// 分页参数-页码
		/// </summary>
		public int? PageIndex { set; get; }

		/// <summary>
		/// 分页参数-数量
		/// </summary>
		public int? PageSize { set; get; }

		/// <summary>
		/// 注册时间
		/// </summary>
		public DateTime? CreatedAtFrom { set; get; }

		/// <summary>
		/// 注册时间
		/// </summary>
		public DateTime? CreatedAtTo { set; get; }


		/// <summary>
		/// 用户列表
		/// </summary>
		public QueryResult<MemberBalanceUpdateInfo> MemberBalanceUpdateInfos { set; get; }


	}
}