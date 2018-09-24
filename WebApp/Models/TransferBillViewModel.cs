using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
	public class TransferModel
	{
		[Required(ErrorMessage = "对方账号必须填写")]
		public string ToMemberUserName { set; get; }

		[RegularExpression("^[1-9][0-9]*00$", ErrorMessage = "转账金额必须为100的整数倍")]
		public decimal Amount { set; get; }
	}

	public class TransferListViewModel
	{
		public int? PageIndex { set; get; }

		public int? PageSize { set; get; }
	}

	public class TransferBillEnsureModel
	{
		public long Id { set; get; }
	}
}