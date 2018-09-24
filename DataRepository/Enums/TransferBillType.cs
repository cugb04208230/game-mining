using System.ComponentModel;
using BaseClasses;
using BaseClasses.Filters;

namespace DataRepository.Enums
{
	/// <summary>
	/// 转账单类型
	/// </summary>
	public enum TransferBillType
	{
		/// <summary>
		/// A的铜账户转到B的金账户
		/// </summary>
		[Description("激活合伙人")]
		Active = 1,
		/// <summary>
		/// A的金账户转到B的金账户
		/// </summary>
		[Description("黄金交易")]
		GoldTransfer =2,
		/// <summary>
		/// A的金账户转到A的银账户
		/// </summary>
		[Description("黄金转银")]
		GoldToSilver =3,
		/// <summary>
		/// A的金账户转到B的铜账户
		/// </summary>
		[Description("黄金转铜")]
		GoldToCopper =4,
		/// <summary>
		/// A的矿渣账户转到A的金账户
		/// </summary>
		[Description("矿渣精炼")]
		SlagRefine = 5
	}

	public static class TransferBillTypeExtension
	{
		public static AccountType FromAccount(this TransferBillType type)
		{
			switch (type)
			{
				case TransferBillType.Active:
					return AccountType.Copper;
				case TransferBillType.GoldTransfer:
					return AccountType.Gold;
				case TransferBillType.GoldToCopper:
					return AccountType.Gold;
				case TransferBillType.GoldToSilver:
					return AccountType.Gold;
				case TransferBillType.SlagRefine:
					return AccountType.Slag;
				default:
					throw new PlatformException(ErrorCode.TransferBillTypeError);
			}
		}

		public static AccountType ToAccount(this TransferBillType type)
		{
			switch (type)
			{
				case TransferBillType.Active:
					return AccountType.Gold;
				case TransferBillType.GoldTransfer:
					return AccountType.Gold;
				case TransferBillType.GoldToCopper:
					return AccountType.Copper;
				case TransferBillType.GoldToSilver:
					return AccountType.Silver;
				case TransferBillType.SlagRefine:
					return AccountType.Gold;
				default:
					throw new PlatformException(ErrorCode.TransferBillTypeError);
			}
		}
	}
}
