using System;
using BaseClasses.Data;
using DataRepository.Enums;

namespace DataRepository.Entities
{
	/// <inheritdoc />
	/// <summary>
	/// 用户采矿设备
	/// </summary>
    public class MemberMiningEquipment:BaseEntity
    {
		/// <summary>
		/// 用户Id
		/// </summary>
		public string MemberUserName { set; get; }

		/// <summary>
		/// 挖矿设备类型
		/// </summary>
		public MiningEquipmentType Type { set; get; }

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
		/// 最后一次采集时间
		/// </summary>
		public DateTime LastCollectAt { set; get; }

		/// <summary>
		/// 逾期未收取次数
		/// </summary>
		public int OverDueTime { set; get; }

		/// <summary>
		/// 挖矿设备状态
		/// </summary>
		public MemberMiningEquipmentStatus Status { set; get; }

		/// <summary>
		/// 锁定时间
		/// </summary>
		public DateTime? LockedAt { set; get; }
	}


	/// <inheritdoc />
	/// <summary>
	/// 用户采矿设备
	/// </summary>
	public class MemberMiningEquipmentQuery : BaseQuery<MemberMiningEquipment>
	{
		/// <summary>
		/// 用户名
		/// </summary>
		public string MemberUserName { set; get; }

		/// <summary>
		/// 用户名完全匹配
		/// </summary>
		public string MemberUserNameEqual { set; get; }

		/// <summary>
		/// 挖矿设备类型
		/// </summary>
		public MiningEquipmentType? Type { set; get; }

		/// <summary>
		/// 挖矿设备状态
		/// </summary>
		public MemberMiningEquipmentStatus? Status { set; get; }

		/// <summary>
		/// 有效截止日期
		/// </summary>
		public DateTime? ValidityTermFrom { set; get; }
		/// <summary>
		/// 有效截止日期
		/// </summary>
		public DateTime? ValidityTermTo { set; get; }


		/// <summary>
		/// 最后一次采集时间-开始
		/// </summary>
		public DateTime LastCollectAtFrom { set; get; }
		/// <summary>
		/// 最后一次采集时间-结束
		/// </summary>
		public DateTime LastCollectAtTo { set; get; }

	}
}
