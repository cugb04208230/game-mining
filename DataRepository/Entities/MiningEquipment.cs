using BaseClasses.Data;
using DataRepository.Enums;

namespace DataRepository.Entities
{
	/// <inheritdoc />
	/// <summary>
	/// 采矿设备
	/// </summary>
    public class MiningEquipment:BaseEntity
    {
		/// <summary>
		/// 描述
		/// 1.矿工，2.选金厂，3.冶炼厂
		/// </summary>
		public MiningEquipmentType Type { set; get; }

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

	/// <inheritdoc />
	/// <summary>
	/// 采矿设备
	/// </summary>
	public class MiningEquipmentQuery : BaseQuery<MiningEquipment>
	{
	}
}
