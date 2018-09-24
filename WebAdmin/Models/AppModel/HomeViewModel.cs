using System.Collections.Generic;
using BaseClasses.Extensions;
using DataRepository;
using DataRepository.Entities;
using DataRepository.Enums;

namespace WebAdmin.Models.AppModel
{
	/// <summary>
	/// 首页用户
	/// </summary>
	public class HomeViewModel
	{
		/// <summary>
		/// 用户
		/// </summary>

		public HomeViewMemberModel Member { set; get; }

		/// <summary>
		/// 矿主数量
		/// </summary>
		public int InUseMemberCount { set; get; }

		/// <summary>
		/// 排队用户
		/// </summary>
		public int ActiveMemberCount { set; get; }

		/// <summary>
		/// 设备数量
		/// </summary>
		public int EquipmentCount { set; get; }

		/// <summary>
		/// 通知
		/// </summary>
		public IEnumerable<Notice> Notices { set; get; }

		/// <summary>
		/// 我的邀请二维码
		/// </summary>
		public string QrCode { set; get; }

		/// <summary>
		/// 全球金价
		/// </summary>
		public decimal GlobalGoldPrice { set; get; }
	}

	/// <summary>
	/// 首页用户模型
	/// </summary>
	public class HomeViewMemberModel
	{
		/// <summary>
		/// 用户名
		/// </summary>
		public string UserName { set; get; }

		/// <summary>
		/// 微信Id，预留
		/// </summary>
		public string WeChatId { set; get; }

		/// <summary>
		/// 用户状态
		/// 1.已激活，2.锁定，3.冻结，4.未激活，5.努力挖矿中，6.查封中
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
		/// 钻石余额
		/// </summary>
		public decimal DiamondBalance { get; set; }


		/// <summary>
		/// 历次采集数量
		/// </summary>
		public decimal CollectAmount { set; get; }

		/// <summary>
		/// 邀请回馈
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
		/// 比特币账号
		/// </summary>
		public string BitCoin { set; get; }
		/// <summary>
		/// 会员类型
		/// </summary>
		public MemberType MemberType { set; get; }

		/// <summary>
		/// 会员类型描述
		/// </summary>
		public string MemberTypeDesc
		{
			get
			{

				string content = "";
				var language = LanguageHelper.GetLanguage();
				switch (language)
				{
					case LanguageType.English:
						content = MemberType==MemberType.CallCenter?"服务中心":"普通用户";
						break;
					case LanguageType.SimplifiedChinese:
						content = MemberType.GetDescription();
						break;
				}
				return content;
			}
		}
	}
}