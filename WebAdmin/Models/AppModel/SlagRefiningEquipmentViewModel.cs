namespace WebAdmin.Models.AppModel
{

	/// <summary>
	/// 用户精炼设备查询
	/// </summary>
	public class MemberSlagRefiningEquipmentViewModel
	{
		/// <summary>
		/// 分页参数-页码
		/// </summary>
		public int? PageIndex { set; get; }

		/// <summary>
		/// 分页参数-数量
		/// </summary>
		public int? PageSize { set; get; }
	}

	/// <summary>
	/// 用户精炼
	/// </summary>
	public class MemberRefiningModel
	{
		/// <summary>
		/// 精炼设备编号
		/// </summary>
		public long Id { get; set; }
	}
}