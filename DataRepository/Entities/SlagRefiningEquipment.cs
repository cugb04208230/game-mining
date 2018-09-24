using System;
using BaseClasses.Data;
using DataRepository.Enums;

namespace DataRepository.Entities
{
	/// <inheritdoc />
	/// <summary>
	/// 矿渣精炼设备
	/// </summary>
    public class SlagRefiningEquipment:BaseEntity
    {
		/// <summary>
		/// 用户名
		/// </summary>
		public string MemberUserName { set; get; }

		/// <summary>
	    /// 收益百分比
	    /// </summary>
	    public decimal Percentage { set; get; }

		/// <summary>
		/// 设备来源
		/// </summary>
		public SlagRefiningEquipmentSourceType SourceType { set; get; }

	    /// <summary>
	    /// 有效截止日期
	    /// </summary>
	    public DateTime ValidityTerm { set; get; }

	    /// <summary>
	    /// 最后精炼时间
	    /// </summary>
	    public DateTime LastRefiningAt { set; get; }
	}
	/// <inheritdoc />
	/// <summary>
	/// 矿渣精炼设备
	/// </summary>
	public class SlagRefiningEquipmentQuery : BaseQuery<SlagRefiningEquipment>
	{
		/// <summary>
		/// 用户名
		/// </summary>
		public string MemberUserName { set; get; }

		/// <summary>
		/// 有效截至时间
		/// </summary>
		public DateTime? ValidityTermFrom { set; get; }

		/// <summary>
		/// 设备来源
		/// </summary>
		public SlagRefiningEquipmentSourceType? SourceType { set; get; }
	}
}
