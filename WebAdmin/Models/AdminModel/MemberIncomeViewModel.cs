using System;
using BaseClasses.Data;
using DataRepository.Entities;

namespace WebAdmin.Models.AdminModel
{
	/// <summary>
	/// 用户收入查询
	/// </summary>
	public class MemberIncomeViewModel
	{
		/// <summary>
		/// 关键字
		/// </summary>
		public string Text { set; get; }

		/// <summary>
		/// 开始时间
		/// </summary>
		public DateTime? CreatedAtFrom { set; get; }

		/// <summary>
		/// 结束时间
		/// </summary>
		public DateTime? CreatedAtTo { set; get; }



		/// <summary>
		/// 分页参数-页码
		/// </summary>
		public int? PageIndex { set; get; }

		/// <summary>
		/// 分页参数-数量
		/// </summary>
		public int? PageSize { set; get; }

		/// <summary>
		/// 用户收入列表
		/// </summary>
		public QueryResult<MemberIncomeRecord> MemberIncomeRecords { set; get; }
	}
}