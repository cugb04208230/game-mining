using System.ComponentModel;
using BaseClasses.Attributes;

namespace DataRepository.Enums
{
	public enum TradeStatus
	{
		[Description("初始")]
		Init=1,
		[Description("已完成")]
		Completed =2,
		[Description("已撤销")]
		Cancelled =4
	}
}
