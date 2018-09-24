using BaseClasses.Data;
using DataRepository.Entities;

namespace WebAdmin.Models.AdminModel
{

	/// <summary>
	/// 精炼设备查询
	/// </summary>
	public class SlagRefiningEquipmentViewModel
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
		/// 精炼设备列表
		/// </summary>
		public QueryResult<SlagRefiningEquipment> SlagRefiningEquipments { set; get; }

	}
}