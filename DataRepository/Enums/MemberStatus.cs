using System.ComponentModel;

namespace DataRepository.Enums
{
	/// <summary>
	/// 用户账号状态
	/// </summary>
	public enum MemberStatus
	{
		/// <summary>
		/// 正常
		/// 排队矿主
		/// </summary>
		[Description("已激活")] Actived = 1,
		/// <summary>
		/// 锁定
		/// 新用户{10}天不开工
		/// </summary>
		[Description("锁定")] Locking = 2,
		/// <summary>
		/// 冻结
		/// 新用户{30}天不开工
		/// </summary>
		[Description("冻结")] Freezing = 3,

		/// <summary>
		/// 未激活
		/// 排队矿主
		/// </summary>
		[Description("未激活")] UnActived = 4,

		/// <summary>
		/// 努力挖矿中
		/// 矿主
		/// </summary>
		[Description("努力挖矿中")] InUse = 5,

		/// <summary>
		/// 1、超过20天如果账号没有设备在运转就被查封（封号），解封扣50（做成可配的）黄金（后台管理员解封，如果黄金不够可以扣成负数）！也就是说冻结和封号就停止收益（推荐奖继续有效）！，这个封号跟新注册的用户未买设备要区分开，这里封号是针对老用户的。
		//比较简单的解决方式：用户购买设备的时候，就更新一个到期时间（以最远的这个为准），后面用户登录的时候就判断这个值，满足条件就查封。如果解封后过10天（做成可配的）还是没有设备，那再封号。
		/// </summary>
		[Description("查封中")]SealUp=6

	}
}
