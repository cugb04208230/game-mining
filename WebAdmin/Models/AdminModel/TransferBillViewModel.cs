using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseClasses.Data;
using DataRepository.Entities;

namespace WebAdmin.Models.AdminModel
{
	
	/// <summary>
	/// 交易单查询
	/// </summary>
	public class TransferBillViewModel
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
		/// 交易单列表
		/// </summary>
		public QueryResult<TransferBill> TransferBills { set; get; }
	}
}