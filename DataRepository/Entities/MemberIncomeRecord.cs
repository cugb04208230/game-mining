using BaseClasses.Data;
using DataRepository.Enums;

namespace DataRepository.Entities
{
	/// <inheritdoc />
	/// <summary>
	/// 用户每日挖矿收益记录
	/// </summary>
	public class MemberIncomeRecord : BaseEntity
	{
		/// <summary>
		/// 用户名
		/// </summary>
		public string MemberUserName { get; set; }

		/// <summary>
		/// 总产量
		/// </summary>
		public decimal Amount { set; get; }

		/// <summary>
		/// 金账户金额-交易赠送/转银和铜
		/// </summary>
		public decimal GoldAmount { get; set; }
		/// <summary>
		/// 银账户金额-购买挖矿装备
		/// </summary>
		public decimal SilverAmount { get; set; }
		/// <summary>
		/// 铜账户金额-招募玩家
		/// </summary>
		public decimal CopperAmount { get; set; }
		/// <summary>
		/// 矿渣账户金额-用于精炼
		/// </summary>
		public decimal SlagAmount { get; set; }

		/// <summary>
		/// 钻石数量
		/// </summary>
		public decimal DiamondAmount { set; get; }

		/// <summary>
		/// 矿渣转金金额
		/// </summary>
		public decimal SlagToGoldAmount { get; set; }

		/// <summary>
		/// 收益类型
		/// </summary>
		public MemberIncomeType Type { set; get; }

		/// <summary>
		/// 关联用户Id
		/// </summary>
		public string ReferenceMemberUsername { set; get; }
	}

	/// <inheritdoc />
	/// <summary>
	/// 用户收益查询
	/// </summary>
	public class MemberIncomeRecordQuery : BaseQuery<MemberIncomeRecord>
	{
		/// <summary>
		/// 用户名
		/// </summary>
		public string MemberUserName { get; set; }


		/// <summary>
		/// 用户名
		/// </summary>
		public string MemberUserNameEqual { get; set; }
	}
}
