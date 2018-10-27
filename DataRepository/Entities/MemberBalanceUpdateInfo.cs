using System;
using BaseClasses.Data;
using DataRepository.Enums;

namespace DataRepository.Entities
{
	/// <inheritdoc />
	/// <summary>
	/// 用户
	/// </summary>
	public class MemberBalanceUpdateInfo : BaseEntity
	{
		/// <summary>
		/// 用户名
		/// </summary>
		public string UserName { set; get; }

		/// <summary>
		/// 金账户余额-交易赠送/转银和铜
		/// </summary>
		public decimal Gold { get; set; }
		/// <summary>
		/// 银账户余额-购买挖矿装备
		/// </summary>
		public decimal Silver { get; set; }
		/// <summary>
		/// 铜账户余额-招募玩家
		/// </summary>
		public decimal Copper { get; set; }
		/// <summary>
		/// 矿渣账户余额-用于精炼
		/// </summary>
		public decimal Slag { get; set; }

		/// <summary>
		/// 金账户余额-交易赠送/转银和铜
		/// </summary>
		public decimal GoldBalanceFrom { get; set; }
		/// <summary>
		/// 银账户余额-购买挖矿装备
		/// </summary>
		public decimal SilverBalanceFrom { get; set; }
		/// <summary>
		/// 铜账户余额-招募玩家
		/// </summary>
		public decimal CopperBalanceFrom { get; set; }
		/// <summary>
		/// 矿渣账户余额-用于精炼
		/// </summary>
		public decimal SlagBalanceFrom { get; set; }
		/// <summary>
		/// 金账户余额-交易赠送/转银和铜
		/// </summary>
		public decimal GoldBalanceTo { get; set; }
		/// <summary>
		/// 银账户余额-购买挖矿装备
		/// </summary>
		public decimal SilverBalanceTo { get; set; }
		/// <summary>
		/// 铜账户余额-招募玩家
		/// </summary>
		public decimal CopperBalanceTo { get; set; }
		/// <summary>
		/// 矿渣账户余额-用于精炼
		/// </summary>
		public decimal SlagBalanceTo { get; set; }

	}


	/// <inheritdoc />
	/// <summary>
	/// 用户
	/// </summary>
	public class MemberBalanceUpdateInfoQuery : BaseQuery<MemberBalanceUpdateInfo>
	{
		/// <summary>
		/// 用户名
		/// </summary>
		public string UserName { set; get; }
	}
}
