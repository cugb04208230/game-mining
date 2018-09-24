using System.ComponentModel;

namespace DataRepository.Enums
{
	/// <summary>
	/// 余额账户类型
	/// </summary>
	public enum AccountType
	{
		/// <summary>
		/// 原石账号
		/// </summary>
		[Description("原石账号")] RawStone = 1,
		/// <summary>
		/// 金账号
		/// </summary>
		[Description("金账号")] Gold = 2,
		/// <summary>
		/// 银账号
		/// </summary>
		[Description("银账号")] Silver = 3,
		/// <summary>
		/// 铜账号
		/// </summary>
		[Description("铜账号")] Copper = 4,
		/// <summary>
		/// 矿渣账号
		/// </summary>
		[Description("矿渣账号")] Slag = 5,
	}
}
