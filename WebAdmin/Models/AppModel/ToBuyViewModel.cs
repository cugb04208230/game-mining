using System.ComponentModel.DataAnnotations;

namespace WebAdmin.Models.AppModel
{
	/// <summary>
	/// 用户预购
	/// </summary>
	public class MemberToBuyModel
	{
		/// <summary>
		/// 金额
		/// </summary>
		[RegularExpression(@"^([1-9][0-9]*)*[0,5]0$", ErrorMessage = "AmountTypeError")]
		public decimal Amount { get; set; }
	}

	/// <summary>
	/// 用户求购列表
	/// </summary>
	public class MemberToBuyListModel
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
	/// 用户求购列表
	/// </summary>
	public class ToBuyListModel
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
	/// 用户赠送列表
	/// </summary>
	public class MemberGiveListModel
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
	/// 
	/// </summary>
	public class MemberToBuyCancelModel
	{
		/// <summary>
		/// 取消Id
		/// </summary>
		public long Id { get; set; }
	}


	/// <summary>
	/// 
	/// </summary>
	public class MemberToBuyEnsureModel
	{
		/// <summary>
		/// 
		/// </summary>
		public long Id { get; set; }
	}
}