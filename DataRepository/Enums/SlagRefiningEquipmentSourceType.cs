using System.ComponentModel;

namespace DataRepository.Enums
{
	/// <summary>
	/// 矿渣精炼设备来源
	/// </summary>
	public enum SlagRefiningEquipmentSourceType
	{
		/// <summary>
		/// 邀请奖励
		/// </summary>
		[Description("邀请奖励")] InvitingAwards = 1,
		/// <summary>
		/// 系统赠送
		/// </summary>
		[Description("系统赠送")] SystemGift = 2
	}
}
