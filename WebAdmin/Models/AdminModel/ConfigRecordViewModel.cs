using BaseClasses.Data;
using DataRepository.Entities;

namespace WebAdmin.Models
{
	/// <summary>
	/// 配置页面查询
	/// </summary>
	public class ConfigRecordViewModel
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
		/// 配置列表
		/// </summary>
		public QueryResult<ConfigRecord> ConfigRecords { set; get; }
	}

	/// <summary>
	/// 配置更新
	/// </summary>
	public class ConfigRecordUpdateModel
	{
		/// <summary>
		/// 编号
		/// </summary>
		public long Id { set; get; }

		/// <summary>
		/// 值
		/// </summary>
		public string Value { set; get; }
	}
}