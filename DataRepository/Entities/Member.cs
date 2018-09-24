using System;
using BaseClasses.Data;
using DataRepository.Enums;

namespace DataRepository.Entities
{
	/// <inheritdoc />
	/// <summary>
	/// 用户
	/// </summary>
    public class Member:BaseEntity
    {
		/// <summary>
		/// 用户名
		/// </summary>
		public string UserName { set; get; }

		/// <summary>
		/// 密码
		/// </summary>
		public string Password { set; get; }
		

	    /// <summary>
	    /// 支付宝账号
	    /// </summary>
	    public string Alipay { set; get; }

	    /// <summary>
	    /// 微信账号
	    /// </summary>
	    public string WeChat { set; get; }

	    /// <summary>
	    /// 比特币账号
	    /// </summary>
	    public string BitCoin { set; get; }

		/// <summary>
		/// 用户状态
		/// </summary>
		public MemberStatus Status { set; get; }

		/// <summary>
		/// 名称
		/// </summary>
		public string Name { set; get; }

		/// <summary>
		/// 昵称
		/// </summary>
		public string NickName { set; get; }
		
		/// <summary>
		/// 手机号
		/// </summary>
		public string Mobile { set; get; }

		/// <summary>
		/// 头像-选项
		/// </summary>
		public MemberHeadPicOption HeadPic { set; get; }
		
		/// <summary>
		/// 推荐人用户名
		/// </summary>
	    public string RecommendUserName { get; set; }

		/// <summary>
		/// 用户关系链
		/// $"{RecommendUser.Chain}->{RecommendUserName}"
		/// </summary>
		public string Chain { get; set; }

		/// <summary>
		/// 账户总额
		/// </summary>
	    public decimal RawStoneBalance => GoldBalance + SilverBalance + CopperBalance + SlagBalance;

		/// <summary>
		/// 金账户余额-交易赠送/转银和铜
		/// </summary>
		public decimal GoldBalance { get; set; }
		/// <summary>
		/// 银账户余额-购买挖矿装备
		/// </summary>
	    public decimal SilverBalance { get; set; }
		/// <summary>
		/// 铜账户余额-招募玩家
		/// </summary>
	    public decimal CopperBalance { get; set; }
		/// <summary>
		/// 矿渣账户余额-用于精炼
		/// </summary>
	    public decimal SlagBalance { get; set; }


	    /// <summary>
	    /// 历次采集数量
	    /// </summary>
	    public decimal CollectAmount { set; get; }

		/// <summary>
		/// 钻石余额
		/// </summary>
		public decimal DiamondBalance { get; set; }

		/// <summary>
		/// 用户邀请回馈
		/// </summary>
		public decimal FeedBackAmount { set; get; }

	    /// <summary>
	    /// 用户邀请回馈金
	    /// </summary>
	    public decimal FeedBackGoldAmount { set; get; }
	    /// <summary>
	    /// 用户邀请回馈矿渣
	    /// </summary>
	    public decimal FeedBackSlagAmount { set; get; }
		/// <summary>
		/// 员工级别
		/// </summary>
		public MemberLevel Level { set; get; }

		/// <summary>
		/// 用户查封账户时间
		/// </summary>
		public DateTime ExpiredAt { set; get; }

		/// <summary>
		/// 荣誉值
		/// </summary>
		public decimal Honor { set; get; }

		/// <summary>
		/// 国际区号
		/// </summary>
		public string GlobalAreaCode { set; get; }

		/// <summary>
		/// 银行名称
		/// </summary>
		public string BankName { set; get; }

		/// <summary>
		/// 银行卡号
		/// </summary>
		public string BankCode { set; get; }

		/// <summary>
		/// 会员类型
		/// </summary>
		public MemberType MemberType { set; get; }
	}


	/// <inheritdoc />
	/// <summary>
	/// 用户
	/// </summary>
	public class MemberQuery : BaseQuery<Member>
	{
		/// <summary>
		/// 用户名
		/// </summary>
		public string UserName { set; get; }
		
		/// <summary>
		/// 用户状态
		/// </summary>
		public MemberStatus? Status { set; get; }
		
		/// <summary>
		/// 昵称
		/// </summary>
		public string NickName { set; get; }
		
		/// <summary>
		/// 用户名完全匹配
		/// </summary>
		public string UserNameEqual { set; get; }
		/// <summary>
		/// 推荐人用户名
		/// </summary>
		public string Chain { get; set; }
		
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
}
