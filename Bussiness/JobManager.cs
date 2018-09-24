using System;
using System.Collections;
using System.Collections.Generic;
using BaseClasses.Attributes;
using DataRepository.Entities;
using DataRepository.Enums;
using NHibernate.Util;

namespace Bussiness
{
	/// <summary>
	/// 
	/// </summary>
	public class JobManager
	{
		public static Dictionary<Action, string> Actions = new Dictionary<Action, string>()
		{
			{LockMember, "0 1 * * *"},
			{FrozenMember, "0 2 * * *"},
			{CheckMemberMiningEquipment, "0 3 * * *"},
			{ExpireTransferBillStatus, "*/1 * * * *"},
			{RecordMiningEquipmentInfo, "50 23 * * *"}
		};

		/// <summary>
		/// 锁定用户
		/// 第二天执行，锁定前一天到期的用户
		/// </summary>
		[Job(Cron = "0 1 * * *")]
		public static void LockMember()
		{
			//激活状态的矿主，10天没开工，状态改为锁定
			var sql = $"UPDATE Members set Status = {(int)MemberStatus.Locking} WHERE Status = {(int)MemberStatus.Actived} and datediff(day, LastModifiedAt, getdate())> {10}";
			MiddleTier.Instance.TransferBillManager.ExecuteBySql(sql);
		}

		/// <summary>
		/// 冻结用户
		/// 第二天执行，冻结前一天的用户
		/// </summary>
		[Job(Cron = "0 2 * * *")]
		public static void FrozenMember()
		{
			//Todo
			//锁定状态的矿主，20天没开工，状态改为冻结
			var sql = $"UPDATE Members set Status = {(int)MemberStatus.Freezing} WHERE Status = {(int)MemberStatus.Locking} and datediff(day, LastModifiedAt, getdate())> {30}";
			MiddleTier.Instance.TransferBillManager.ExecuteBySql(sql);
		}

		/// <summary>
		/// 冻结用户挖矿设备
		/// 第二天执行，冻结前一天到期的挖矿设备
		/// </summary>
		[Job(Cron = "0 3 * * *")]
		public static void CheckMemberMiningEquipment()
		{
			MiddleTier.Instance.LogManager.Info("CheckMemberMiningEquipment");
			//重新计算昨天到达采集周期的采矿周期
//			var miningEquipments = MiddleTier.Instance.EquipmentManager.MemberMiningEquipmentQuery(new MemberMiningEquipmentQuery
//			{
//				LastCollectAtFrom = DateTime.Now.Date.AddDays(0 - MiddleTier.Instance.BusinessConfig.MiningEquipmentCollectDay),
//				LastCollectAtTo = DateTime.Now.Date.AddDays(1 - MiddleTier.Instance.BusinessConfig.MiningEquipmentCollectDay),
//				PageSize = int.MaxValue
//			}).List;
//			miningEquipments.ForEach(miningEquipment =>
//			{
//				//每5天收矿一次（用户手动点击劳工或矿机收矿），当天24点前必须收矿，错过了就不能收了，10天收两次，10天两次都错过收矿，将该劳工（或矿机）锁定10天！
//				if (miningEquipment.OverDueTime < MiddleTier.Instance.BusinessConfig.MiningEquipmentCollectOverDueTime - 1)
//				{
//					miningEquipment.OverDueTime += 1;
//				}
//				else
//				{
//					miningEquipment.OverDueTime = 0;
//					miningEquipment.LastCollectAt = DateTime.Now.AddDays(10);
//					miningEquipment.Status = MemberMiningEquipmentStatus.Lock;
//				}
//				MiddleTier.Instance.EquipmentManager.UpdateMemberMiningEquipment(miningEquipment);
//			});
		}

		/// <summary>
		/// 2. 求购中心：就是所有用户的求购列表（像是一个超市），这里系统要设置个超时时间（可以配再webconfig里面），超时了就不显示了，另外被别的用户点击赠送了也不显示了。当一直未有人赠送，求购者可以选择取消求购。
		/// 每分钟一次
		/// </summary>
		[Job(Cron = "*/1 * * * *")]
		public static void ExpireTransferBillStatus()
		{
			MiddleTier.Instance.LogManager.Info("ExpireTransferBillStatus");
			var sql =
				$"UPDATE TransferBills set Status={(int) TransferBillStatus.Expired},ExpiredAt=null WHERE Status={(int) TransferBillStatus.Original} and ExpiredAt<= GETDATE()";
			MiddleTier.Instance.TransferBillManager.ExecuteBySql(sql);
		}

		/// <summary>
		/// 每日记录设备价格信息
		/// 每日23：50点执行，记录当日设备信息
		/// </summary>
		[Job(Cron = "50 23 * * *")]
		public static void RecordMiningEquipmentInfo()
		{
			var sql =
				"INSERT INTO MiningEquipmentChangeRecords ( CreatedAt, ExpirationDay, IsEnabled, LastModifiedAt, Percentage, Price, Type ) SELECT getdate() as CreatedAt, ExpirationDay, IsEnabled, getdate() as LastModifiedAt, Percentage, Price, Type FROM MiningEquipments";
			MiddleTier.Instance.TransferBillManager.ExecuteBySql(sql);
			MiddleTier.Instance.LogManager.Info("RecordMiningEquipmentInfo");
		}
	}
}
