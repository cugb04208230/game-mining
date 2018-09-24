using System;
using System.Linq;
using BaseClasses.Data;
using BaseClasses.Extensions;
using Bussiness;
using Common.Util;
using DataRepository;
using DataRepository.Entities;
using DataRepository.Enums;

namespace WebAdmin.Models.AppModel
{
	/// <summary>
	/// 客户端模型
	/// </summary>
	public static class AppModelUtil
	{
		public static PartnerQueryResult ToPartner(this QueryResult<Member> source)
		{
			return new PartnerQueryResult
			{
				Count = source.Count,
				List = source.List.Select(e=>ModelMapUtil.AutoMap(e,new PartnerModel())).ToList()
			};
		}

		public static QueryResult<ToBuyModel> ToToBuy(this QueryResult<TransferBill> source,string currentUsername="")
		{
			return new QueryResult<ToBuyModel>
			{
				Count = source.Count,
				List = source.List.Select(e => ModelMapUtil.AutoMap(e, new ToBuyModel{From = ModelMapUtil.AutoMap(e.FromMember,new TransferMemberModel()),To = ModelMapUtil.AutoMap(e.ToMember, new TransferMemberModel()) })).ToList()
			};
		}

		public static QueryResult<MiningEquipmentModel> ToMiningEquipmentModel(this QueryResult<MiningEquipment> source)
		{
			return new QueryResult<MiningEquipmentModel>
			{
				Count = source.Count,
				List = source.List.Select(e => ModelMapUtil.AutoMap(e, new MiningEquipmentModel())).ToList()
			};
		}

		public static QueryResult<MemberMiningEquipmentItemModel> ToMemberMiningEquipmentItemModel(
			this QueryResult<MemberMiningEquipment> source)
		{
			return new QueryResult<MemberMiningEquipmentItemModel>
			{
				Count = source.Count,
				List = source.List.Select(e => ModelMapUtil.AutoMap(e, new MemberMiningEquipmentItemModel())).ToList()
			};
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="record"></param>
		/// <returns></returns>
		public static string ToContent(this MemberIncomeRecord record)
		{
			string content = "";
			var language = LanguageHelper.GetLanguage();
			if (language == LanguageType.English)
			{
				switch (record.Type)
				{
					case MemberIncomeType.ToBuyReward:
					case MemberIncomeType.Collect:
						content =
							$"{record.GoldAmount}$ gold，{record.SilverAmount}$ silver，{record.CopperAmount}$ copper，{record.SlagAmount}$ slag";
						break;
					case MemberIncomeType.InviteFeedback:
						content = $"[{record.ReferenceMemberUsername}]  {record.GoldAmount}$ gold,{record.SlagAmount}$ slag";
						break;
					case MemberIncomeType.Refining:
						content = $"{record.GoldAmount}$ gold,{record.SlagAmount}$ slag";
						break;
					case MemberIncomeType.EquipmentBuyFeedback:
						content = $"{record.DiamondAmount} diamond";
						break;
					case MemberIncomeType.InviteActive:
						content = $"{record.DiamondAmount} diamond,{record.CopperAmount}$ copper";
						break;
					case MemberIncomeType.InviteActiveCost:
						content = $"{record.GoldAmount}$ gold";
						break;
				}
			}
			else
			{
				switch (record.Type)
				{
					case MemberIncomeType.ToBuyReward:
					case MemberIncomeType.Collect:
						content =
							$"{record.GoldAmount}$金，{record.SilverAmount}$银，{record.CopperAmount}$铜，{record.SlagAmount}$矿渣";
						break;
					case MemberIncomeType.InviteFeedback:
						content = $"[{record.ReferenceMemberUsername}]  {record.GoldAmount}$金,{record.SlagAmount}$矿渣";
						break;
					case MemberIncomeType.Refining:
						content = $"{record.GoldAmount}$金,{record.SlagAmount}$矿渣";
						break;
					case MemberIncomeType.EquipmentBuyFeedback:
						content = $"{record.DiamondAmount}枚钻石";
						break;
					case MemberIncomeType.InviteActive:
						content = $"{record.DiamondAmount}枚钻石,{record.CopperAmount}$铜";
						break;
					case MemberIncomeType.InviteActiveCost:
						content = $"{record.GoldAmount}$金";
						break;
				}
			}
			return content;
		}
	}

	/// <inheritdoc />
	/// <summary>
	/// 合伙人返回结果
	/// </summary>
	public class PartnerQueryResult : QueryResult<PartnerModel>
	{
		/// <summary>
		/// 
		/// </summary>
		public string Title { set; get; }
	}

	/// <summary>
	/// 用户合伙人
	/// </summary>
	public class PartnerModel
	{

		/// <summary>
		/// 用户名
		/// </summary>
		public string UserName { set; get; }
		
		/// <summary>
		/// 用户状态
		/// 1.激活，2.锁定，3.冻结，4.未激活，5.努力挖矿中，6.查封中
		/// 未激活状态才可以激活
		/// </summary>
		public MemberStatus Status { set; get; }

		/// <summary>
		/// 用户状态描述
		/// </summary>
		public string StatusDesc
		{
			get
			{
				var language = LanguageHelper.GetLanguage();
				return language == LanguageType.English  ? Status.ToString(): Status.GetDescription();
			}
		}

		/// <summary>
		/// 名称
		/// </summary>
		public string Name { set; get; }
		
		/// <summary>
		/// 头像-选项
		/// 1-7,默认七个头像，按客户端的图片命名来
		/// </summary>
		public MemberHeadPicOption HeadPic { set; get; }

		/// <summary>
		/// 劳工数量
		/// </summary>
		public int EquipmentCount { set; get; }

		/// <summary>
		/// 激活消耗的铜的数量
		/// </summary>
		public decimal ActiveCost { set; get; }
	}

	/// <summary>
	/// 求购中心
	/// </summary>
	public class ToBuyModel
	{
		/// <summary>
		/// 编号
		/// </summary>
		public long Id { get; set; }


		/// <summary>
		/// 入账账户用户名
		/// </summary>
		public string FromMemberUserName { set; get; }

		/// <summary>
		/// 入账账户用户名
		/// </summary>
		public string ToMemberUserName { set; get; }
		
		/// <summary>
		/// 金额
		/// </summary>
		public decimal Amount { set; get; }

		/// <summary>
		/// 状态
		/// 1.初始，3.卖家确认，4.已完成，5.已撤销，6.已过期
		/// </summary>
		public TransferBillStatus Status { set; get; }

		/// <summary>
		/// 状态描述
		/// </summary>
		public string StatusDesc
		{
			get
			{
				var language = LanguageHelper.GetLanguage();
				return language == LanguageType.English ? Status.ToString() : Status.GetDescription();
			}
		}

		/// <summary>
		/// 时间
		/// </summary>
		public DateTime CreatedAt { set; get; }

		/// <summary>
		/// 荣誉值
		/// </summary>
		public decimal Honor { set; get; }

		/// <summary>
		/// 是否是自己
		/// </summary>
		public bool IsSelf { set; get; }

		/// <summary>
		/// 赠送者
		/// </summary>
		public TransferMemberModel From { set; get; }

		/// <summary>
		/// 求购者
		/// </summary>
		public TransferMemberModel To { set; get; }


		/// <summary>
		/// 求购时间
		/// </summary>
		public DateTime? ToBuyAt { set; get; }

		/// <summary>
		/// 赠送时间
		/// </summary>
		public DateTime? GivedAt { set; get; }

		/// <summary>
		/// 接收时间
		/// </summary>
		public DateTime? ReceivedAt { set; get; }

		/// <summary>
		/// 结束时间
		/// </summary>
		public DateTime? CompletedAt { set; get; }
	}

	/// <summary>
	/// 求购用户模型
	/// </summary>
	public class TransferMemberModel
	{

		/// <summary>
		/// 用户名
		/// </summary>
		public string UserName { set; get; }

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
		/// 名称
		/// </summary>
		public string Name { set; get; }
		/// <summary>
		/// 手机号
		/// </summary>
		public string Mobile { set; get; }

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
	}


	/// <summary>
	/// 资源中心，挖矿设备
	/// </summary>
	public class MiningEquipmentModel
	{
		/// <summary>
		/// 设备类型
		/// 1.矿工，2.选金厂，3.冶炼厂
		/// </summary>
		public MiningEquipmentType Type { set; get; }

		/// <summary>
		/// 挖矿设备描述
		/// </summary>
		public string TypeDesc
		{
			get
			{
				var language = LanguageHelper.GetLanguage();
				return language == LanguageType.English ? Type.ToString() : Type.GetDescription();
			}
		}

		/// <summary>
		/// 价格
		/// </summary>
		public decimal Price { get; set; }

		/// <summary>
		/// 产品有效期
		/// </summary>
		public int ExpirationDay { set; get; }

		/// <summary>
		/// 收益百分比
		/// </summary>
		public decimal Percentage { set; get; }
	}

	/// <summary>
	/// 个人挖矿设备模型
	/// </summary>
	public class MemberMiningEquipmentItemModel
	{
		/// <summary>
		/// 主键
		/// </summary>
		public long Id { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime CreatedAt { set; get; }

		/// <summary>
		/// 挖矿设备类型
		/// 1.矿工，2.选金厂，3.冶炼厂
		/// </summary>
		public MiningEquipmentType Type { set; get; }

		/// <summary>
		/// 挖矿设备描述
		/// </summary>
		public string TypeDesc
		{
			get
			{
				var language = LanguageHelper.GetLanguage();
				return language == LanguageType.English ? Status.ToString() : Status.GetDescription();
			}
		}

		/// <summary>
		/// 设备采购价格
		/// </summary>
		public decimal PurchasePrice { set; get; }

		/// <summary>
		/// 收益百分比
		/// </summary>
		public decimal Percentage { set; get; }

		/// <summary>
		/// 有效截止日期
		/// </summary>
		public DateTime ValidityTerm { set; get; }

		/// <summary>
		/// 已经使用的天数
		/// </summary>
		public int UsedDay => (DateTime.Now.Date - CreatedAt.Date).Days;


		/// <summary>
		/// 剩余使用的天数
		/// </summary>
		public int LeftDay => (ValidityTerm.Date - DateTime.Now.Date).Days;

		/// <summary>
		/// 预计每日产量
		/// </summary>
		public decimal Yield => PurchasePrice * Percentage;

		/// <summary>
		/// 最后一次采集时间
		/// </summary>
		public DateTime LastCollectAt { set; get; }

		/// <summary>
		/// 逾期未收取次数
		/// </summary>
		public int OverDueTime { set; get; }

		/// <summary>
		/// 挖矿设备状态
		/// 1.挖矿中，2.锁定中,3.可采集
		/// </summary>
		public MemberMiningEquipmentStatus Status { set; get; }

		/// <summary>
		/// 锁定时间
		/// </summary>
		public DateTime? LockedAt { set; get; }
	}

	/// <summary>
	/// 求购统计
	/// </summary>
	public class ToBuyCollectModel
	{
		/// <summary>
		/// 所有求购成交额
		/// </summary>
		public decimal TotalAmount { set; get; }

		/// <summary>
		/// 本月求购成交额
		/// </summary>
		public decimal MonthAmount { set; get; }
	}
}