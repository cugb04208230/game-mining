using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BaseClasses.Data;
using DataRepository.Entities;

namespace WebAdmin.Models.AdminModel
{

	/// <summary>
	/// 系统采矿设备查询
	/// </summary>
	public class MiningEquipmentViewModel
	{
		/// <summary>
		/// 关键字
		/// </summary>
		public string Text { set; get; }

		/// <summary>
		/// 分页参数-页码
		/// </summary>
		public int? PageIndex { set; get; }

		/// <summary>
		/// 分页参数-数量
		/// </summary>
		public int? PageSize { set; get; }

		/// <summary>
		/// 系统采矿设备列表
		/// </summary>
		public QueryResult<MiningEquipment> MiningEquipments { set; get; }
	}



	/// <summary>
	/// 挖矿设备更新
	/// </summary>
	public class MiningEquipmentPostModel
	{
		/// <summary>
		/// 编号
		/// </summary>
		[Required(ErrorMessage = "编号不能为空")]
		public long Id { get; set; }

		/// <summary>
		/// 价格
		/// </summary>
		[Required(ErrorMessage = "价格不能为空")]
		public decimal Price { get; set; }

		/// <summary>
		/// 产品有效期
		/// </summary>
		[Required(ErrorMessage = "产品有效期不能为空")]
		public int ExpirationDay { set; get; }

		/// <summary>
		/// 收益百分比
		/// </summary>
		[Required(ErrorMessage = "收益百分比不能为空")]
		public decimal Percentage { set; get; }

	}
}