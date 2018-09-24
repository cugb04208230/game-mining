using System.ComponentModel.DataAnnotations;

namespace WebAdmin.Models.AppModel
{
	public class TransferModel
	{
		[Required(ErrorMessage = "对方账号必须填写")]
		public string ToMemberUserName { set; get; }

		[RegularExpression("^[1-9][0-9]*00$", ErrorMessage = "转账金额必须为100的整数倍")]
		public decimal Amount { set; get; }
	}

	/// <summary>
	/// 求购列表
	/// </summary>
	public class TransferListViewModel
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
	/// 确认求购单
	/// </summary>
	public class TransferBillEnsureModel
	{
		/// <summary>
		/// 编号
		/// </summary>
		public long Id { set; get; }
	}
}