using BaseClasses.Data;
using DataRepository.Entities;

namespace WebAdmin.Models.AdminModel
{

	/// <summary>
	/// 用户采矿设备查询
	/// </summary>
	public class MemberMiningEquipmentViewModel
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
		/// 用户采矿设备列表
		/// </summary>
		public QueryResult<MemberMiningEquipment> MemberMiningEquipments { set; get; }
	}
}