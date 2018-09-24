using System;
using BaseClasses.Extensions;
using DataRepository.Enums;

namespace WebAdmin.Models.AppModel
{
	/// <summary>
	/// 矿场中心设备模型
	/// </summary>
	public class EuipmentViewModel
	{
		/// <summary>
		/// 设备类型
		/// 1.矿工，2.选金厂，3.冶炼厂,4.精炼设备
		/// </summary>
		public int EquipmentType { set; get; }
		
		/// <summary>
		/// 主键
		/// </summary>
		public long Id { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime CreatedAt { set; get; }

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
		/// 产量
		/// </summary>
		public string Yield { set; get; }

		/// <summary>
		/// 最后一次采集时间
		/// </summary>
		public DateTime LastCollectAt { set; get; }

		/// <summary>
		/// 挖矿设备状态
		/// 1.准备中，2.锁定中,3.可采集
		/// </summary>
		public MemberMiningEquipmentStatus Status { set; get; }
	}

}