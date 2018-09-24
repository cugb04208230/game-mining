using BaseClasses.Attributes;

namespace DataRepository.Enums
{
	/// <summary>
	/// 交易单类型
	/// </summary>
    public enum TradeType
    {
	    [TradeSummary("转账单冻结", "转账单冻结")]
	    TransferBillFrozen = 1
	}
}
