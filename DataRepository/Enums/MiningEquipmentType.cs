using System.ComponentModel;
using BaseClasses.Attributes;

namespace DataRepository.Enums
{
	/// <summary>
	/// 3、设备劳工、全半矿机名字改成采矿工、选金厂、冶炼厂（每种设备的收益比例都可以后台调整，每天的收益可能都有变动，是全局变量）。系统要做个定时任务，记录三种类型的旷工每天的收益比例（用户收矿的时候按这个收益去算的）
	/// </summary>
	public enum MiningEquipmentType
	{
		/// <summary>
		/// 100-375-0.8
		/// </summary>
		[MiningEquipment(100,375,0.8)]
		[Description("矿工")] Miner = 1,
		/// <summary>
		/// 500-300-1.2
		/// </summary>
		[MiningEquipment(500, 300, 1.2)]
		[Description("选金厂")] GoldFactory = 2,
		/// <summary>
		/// 1000-200-1.5
		/// </summary>
		[MiningEquipment(1000, 200, 1.5)]
		[Description("冶炼厂")] Smelter = 3
	}
}
