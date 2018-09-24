using System.ComponentModel;

namespace DataRepository.Enums
{
	public enum MemberMiningEquipmentStatus
	{
		[Description("挖矿中")] InUse =1,
		[Description("锁定中")] Lock =2,
		[Description("可采集")] CanCollected=3,
		[Description("已过期")] Expired=4
	}
}
