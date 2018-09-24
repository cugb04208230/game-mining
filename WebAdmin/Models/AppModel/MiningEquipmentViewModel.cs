using System.Collections.Generic;
using DataRepository.Entities;
using DataRepository.Enums;

namespace WebAdmin.Models.AppModel
{
	/// <summary>
	/// 设备列表
	/// </summary>
	public class MiningEquipmentViewModel
	{
		/// <summary>
		/// 列表
		/// </summary>
		public List<MiningEquipment> List { set; get; }
	}

	/// <summary>
	/// 设备购买
	/// </summary>
	public class MemberBuyMiningEquipmentModel
	{
		/// <summary>
		/// 设备类型
		/// </summary>
		public MiningEquipmentType MiningEquipmentType { set; get; }
	}

	/// <summary>
	/// 用户设备查询
	/// </summary>
	public class MemberMiningEquipmentModel
	{
	}

	/// <summary>
	/// 设备采集
	/// </summary>
	public class MemberCollectModel
	{
		/// <summary>
		/// 用户编号
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// 设备类型
		/// 1.矿工，2.选金厂，3.冶炼厂,4.精炼设备
		/// </summary>
		public int EquipmentType { set; get; }
	}

}