using System.ComponentModel;

namespace DataRepository.Enums
{
	/// <summary>
	/// 转账单状态
	/// 买家求购->卖家确认赠送扣除卖家金额->买家确认增加买家余额（扣除手续费后）/买家取消卖家余额增加
	/// new 交易的流程： 1. 求购者发起求购 ->2.赠送者赠送->3.求购者确认接收->4.赠送者结束交易 （在求购者没有确认接收前都可以取消交易，走到第四步，才会将黄金真正打到求购者账户）
	/// </summary>
	public enum TransferBillStatus
	{
		[Description("求购者发起求购")]
		Original=1,
		[Description("求购者确认接收")]
		BuyerEnsure =2,
		[Description("赠送者赠送")]
		SellerEnsure = 3,
		[Description("赠送者结束交易")]
		Completed =4,
		[Description("已撤销")]
		Cancelled = 5,
		[Description("已过期")]
		Expired = 6
	}
}
