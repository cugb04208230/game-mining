using System;
using System.ComponentModel.DataAnnotations;
using DataRepository.Enums;

namespace WebAdmin.Models.AppModel
{
	/// <summary>
	/// 用户转账
	/// </summary>
	public class MemberTransferModel
	{
		/// <summary>
		/// 转账类型
		/// 3.黄金转银 4.黄金转铜
		/// </summary>
		[Required]
		public TransferBillType TransferBillType { get; set; }

		/// <summary>
		/// 金额
		/// </summary>
		[Required]
		public decimal Amount { set; get; }
	}

	/// <summary>
	/// 转换记录查询
	/// </summary>
	public class MemberTransferListModel
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
	/// 用户转换记录
	/// </summary>
	public class MemberTransferItemModel
	{
		/// <summary>
		/// 转换时间
		/// </summary>
		public DateTime CreatedAt { set; get; }

		/// <summary>
		/// 转换类型
		/// 3.黄金转银 4.黄金转铜
		/// </summary>
		public TransferBillType TransferBillType { get; set; }

		/// <summary>
		/// 转换金额
		/// </summary>
		public decimal Amount { set; get; }
	}
}