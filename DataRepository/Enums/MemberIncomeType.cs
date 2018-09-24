using System.ComponentModel;
using NHibernate.Criterion;

namespace DataRepository.Enums
{
	/// <summary>
	/// 用户收益类型
	/// </summary>
	public enum MemberIncomeType
	{
		/// <summary>
		/// 采集
		/// </summary>
		[Description("采集")]Collect=1,
		/// <summary>
		/// 精炼
		/// </summary>
		[Description("精炼")]Refining=2,
		/// <summary>
		/// 邀请回馈
		/// </summary>
		[Description("好友购买设备")] InviteFeedback =3,
		/// <summary>
		/// 好友激活
		/// </summary>
		[Description("好友激活")]InviteActive=4,
		/// <summary>
		/// 购买设备
		/// </summary>
		[Description("购买设备")] EquipmentBuyFeedback = 5,

		/// <summary>
		/// 激活好友扣除
		/// </summary>
		[Description("激活好友")] InviteActiveCost=6,

		/// <summary>
		/// 每月求购奖励
		/// </summary>
		[Description("每月求购奖励")] ToBuyReward = 7

	}
}
