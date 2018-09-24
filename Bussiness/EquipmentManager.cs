using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BaseClasses;
using BaseClasses.Data;
using BaseClasses.Extensions;
using BaseClasses.Filters;
using Common.Util;
using DataRepository.Entities;
using DataRepository.Enums;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Util;

namespace Bussiness
{
	public class EquipmentManager:BaseManager
	{
		public EquipmentManager(MiddleTier middleTier) : base(middleTier)
		{
		}

		/// <summary>
		/// 获取用户有效劳工数量
		/// </summary>
		/// <param name="memberUserName"></param>
		/// <returns></returns>
		public int MemberMiningEquipmentCount(string memberUserName)
		{
			return DataBase.Count<MemberMiningEquipment>(e=>e.ValidityTerm>DateTime.Now&&e.MemberUserName==memberUserName);
		}

		public QueryResult<MemberMiningEquipment> MemberMiningEquipmentQuery(string userName,int pageIndex,int pageSize)
		{
			var session = DataBase.Session;
			var queryable = session.Query<MemberMiningEquipment>().Where(e => e.MemberUserName == userName&&e.ValidityTerm>DateTime.Now);
			var restult =  new QueryResult<MemberMiningEquipment>
			{
				List = queryable.OrderByDescending(e => e.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(),
				Count = queryable.Count()
			};
			session.Close();
			return restult;
		}

		public Dictionary<string, int> GetMemberEquipmentCount(IEnumerable<string> members)
		{
			var session = DataBase.Session;
			var queryable = session.Query<MemberMiningEquipment>().Where(e =>
				members.Contains(e.MemberUserName) && e.Status != MemberMiningEquipmentStatus.Expired);
			var dic = new Dictionary<string,int>();
			members.ForEach(member => { dic.Add(member, queryable.Count(e => e.MemberUserName == member));});
			session.Close();
			return dic;
		}


		public QueryResult<MiningEquipment> MiningEquipmentQuery()
		{
			return DataBase.Query(new MiningEquipmentQuery());
		}

		/// <summary>
		/// 用户采集
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="id"></param>
		public void MemberCollect(string userName, long id)
		{
			//数据校验
			var member = MiddleTier.MemberManager.GetMember(userName);
			if (member == null)
			{
				throw new PlatformException(ErrorCode.UserNameIsNotExisted);
			}
			var memberMiningEquipment = DataBase.Get<MemberMiningEquipment>(e => e.Id == id && e.MemberUserName == userName);
			if (memberMiningEquipment == null)
			{
				throw new PlatformException(ErrorCode.ErrorId);
			}
			var collectStatus = GetMemberMiningEquipmentStatus(memberMiningEquipment.LastCollectAt);
			if (collectStatus != MemberMiningEquipmentStatus.CanCollected)
			{
				throw new PlatformException(ErrorCode.MiningEquipmentCollectError);
			}
			ISession session = DataBase.Session;
			ITransaction iTransaction = session.BeginTransaction(IsolationLevel.ReadCommitted);
			try
			{
				var amount = MemberMiningEquipmentCalculate(memberMiningEquipment);
				//新增采集收益记录
				var memberIncomeRecord = new MemberIncomeRecord
				{
					GoldAmount = amount * MiddleTier.BusinessConfig.CollectGoldPercentage / 100,
					CopperAmount = amount * MiddleTier.BusinessConfig.CollectCopperPercentage / 100,
					SilverAmount = amount * MiddleTier.BusinessConfig.CollectSilverPercentage / 100,
					SlagAmount = amount * MiddleTier.BusinessConfig.CollectSlagPercentage / 100,
					SlagToGoldAmount = 0,
					Amount = amount,
					MemberUserName = member.UserName,
					Type = MemberIncomeType.Collect
				};
				DataBase.Save(memberIncomeRecord,session);
				//更新账户余额
//				member.GoldBalance += memberIncomeRecord.GoldAmount;
//				member.SilverBalance += memberIncomeRecord.SilverAmount;
//				member.CopperBalance += memberIncomeRecord.CopperAmount;
//				member.SlagBalance += memberIncomeRecord.SlagAmount;
//				member.CollectAmount += memberIncomeRecord.Amount;
//				DataBase.Update(member, session);
				var sql = MiddleTier.MemberManager.GetUpdateBalanceSql(member.UserName, memberIncomeRecord.GoldAmount,
					memberIncomeRecord.SilverAmount, memberIncomeRecord.CopperAmount, memberIncomeRecord.SlagAmount,
					memberIncomeRecord.Amount);
				DataBase.ExecuteBySql(sql, session);
				//给邀请人发送奖励
				//原三层减为两层，推荐收益的3％直接显示累计在黄金里！
				//				var inviters = GetInviters(member, MiddleTier.BusinessConfig.ProfitTransmitTime,session);
				//				inviters.ForEach(inviter =>
				//				{
				//					var feedBackAmount = amount * MiddleTier.BusinessConfig.ProfitTransmitPercentage / 100;
				//					//Todo 是金账户么？
				//					inviter.GoldBalance += feedBackAmount;
				//					inviter.FeedBackAmount += feedBackAmount;
				//					DataBase.Update(inviter, session);
				//				});
				//刷新采矿机的状态
				//每5天收矿一次（用户手动点击劳工或矿机收矿），当天24点前必须收矿，错过了就不能收了，10天收两次，10天两次都错过收矿，将该劳工（或矿机）锁定10天！
				memberMiningEquipment.LastCollectAt=DateTime.Now;
				memberMiningEquipment.OverDueTime = 0;
				memberMiningEquipment.Status = MemberMiningEquipmentStatus.InUse;
				DataBase.Update(memberMiningEquipment, session);
				iTransaction.Commit();
			}
			catch (Exception ex)
			{
				iTransaction.Rollback();
				MiddleTier.LogManager.Error(ex);
				if (ex is PlatformException platformException)
				{
					throw platformException;
				}
				throw new PlatformException(ErrorCode.SystemError);
			}
			finally
			{
				session.Close();
			}
		}

		/// <summary>
		/// 获取父级邀请用户
		/// </summary>
		/// <param name="member">当前用户</param>
		/// <param name="count">层级</param>
		/// <returns></returns>
		public List<Member> GetInviters(Member member, int count, ISession session)
		{
			var chain = member.Chain;
			var userNames = chain.Split(new[] { MemberManager.ChainStr }, StringSplitOptions.RemoveEmptyEntries);
			if (count < userNames.Length)
			{
				userNames = userNames.Skip(userNames.Length - count).Take(count).ToArray();
			}
			return DataBase.GetList<Member>(e => userNames.Contains(e.UserName), session);
		}
		public void MemberBuyMiningEquipment(string userName, MiningEquipmentType type)
		{
			//数据校验
			var member = MiddleTier.MemberManager.GetMember(userName);
			if (member == null)
			{
				throw new PlatformException(ErrorCode.UserNameIsNotExisted);
			}
			var miningEquipment = DataBase.Get<MiningEquipment>(e => e.Type == type);
			if (miningEquipment == null)
			{
				throw new PlatformException(ErrorCode.ErrorId);
			}
			if (member.SilverBalance < miningEquipment.Price)
			{
				throw new PlatformException(ErrorCode.AccountBalanceNotEnough);
			}
			//11、可以设置购买劳工和矿机的上限（比如劳工设置最多可以买10个，半自动矿机最多可以买5个），这个数字后台可以调整。
			var count = DataBase.Count<MemberMiningEquipment>(e => e.MemberUserName == userName && e.Type == type);
			var limitCount = type == MiningEquipmentType.Miner
				? MiddleTier.BusinessConfig.MemberMiningEquipmentType1Limit
				: (
					type == MiningEquipmentType.GoldFactory
						? MiddleTier.BusinessConfig.MemberMiningEquipmentType2Limit
						: MiddleTier.BusinessConfig.MemberMiningEquipmentType3Limit);
			if (count >= limitCount)
			{
				throw new PlatformException(ErrorCode.EquipmentCountLimit);
			}
			ISession session = DataBase.Session;
			ITransaction iTransaction = session.BeginTransaction(IsolationLevel.ReadCommitted);
			try
			{
				var diamondAmount = type == MiningEquipmentType.Miner
					? MiddleTier.BusinessConfig.PartnerBuyEquipmentType1RewardDiamondAmount
					: (
						type == MiningEquipmentType.GoldFactory
							? MiddleTier.BusinessConfig.PartnerBuyEquipmentType2RewardDiamondAmount
							: MiddleTier.BusinessConfig.PartnerBuyEquipmentType3RewardDiamondAmount);
				//扣除银余额
				member.SilverBalance -= miningEquipment.Price;
				if (member.Status == MemberStatus.Actived)
				{
					member.Status = MemberStatus.InUse;
				}
				if(member.ExpiredAt< DateTime.Now.AddDays(miningEquipment.ExpirationDay + 20))
				{
					member.ExpiredAt = DateTime.Now.AddDays(miningEquipment.ExpirationDay + 20);
				}
				member.DiamondBalance += diamondAmount;
				DataBase.Update(member, session);
//				DataBase.ExecuteBySql($"UPDATE Members set SilverBalance = SilverBalance-{miningEquipment.Price}  Where UserName='{member.UserName}';");
				//新增用户挖矿设备
				var memberMiningEquipment = new MemberMiningEquipment
				{
					MemberUserName = userName,
					LastCollectAt = DateTime.Now,
					OverDueTime = 0,
					Percentage = miningEquipment.Percentage,
					PurchasePrice = miningEquipment.Price,
					Type = miningEquipment.Type,
					ValidityTerm = DateTime.Now.AddDays(miningEquipment.ExpirationDay),
					Status= MemberMiningEquipmentStatus.InUse
				};
				DataBase.Save(memberMiningEquipment, session);
//				2.用户购买矿工后，给她的直接上级奖励矿工价值的25 %（这25 % 中30 % 是金，70 % 是狂渣），她的间接推荐人（上级的上级）拿矿工价值的10 %。
//				限制条件：每种类型矿工的前30个，第31个开始就不给上面的奖励了。
				if (DataBase.Count<MemberMiningEquipment>(e => e.MemberUserName == member.UserName && e.Type == type,session) < 30)
				{
					if (member.RecommendUserName.IsNotNullOrEmpty())
					{
						var fRecommendUser = DataBase.Get<Member>(e => e.UserName == member.RecommendUserName,session);
						if (fRecommendUser != null)
						{
							//Todo 收益记录
							var fAmount = miningEquipment.Price / 4;
							fRecommendUser.GoldBalance += fAmount * 0.3m;
							fRecommendUser.FeedBackAmount += fAmount;
							fRecommendUser.FeedBackGoldAmount += fAmount * 0.3m;
							fRecommendUser.SlagBalance += fAmount * 0.7m;
							fRecommendUser.FeedBackSlagAmount += fAmount * 0.7m;
							DataBase.Update(fRecommendUser,session);
							var fMemberIncomeRecord = new MemberIncomeRecord
							{
								Amount = fAmount,
								GoldAmount = fAmount*0.3m,
								SlagAmount = fAmount*0.7m,
								MemberUserName = fRecommendUser.UserName,
								Type = MemberIncomeType.InviteFeedback,
								ReferenceMemberUsername = userName
							};
							DataBase.Save(fMemberIncomeRecord, session);
							var ffMemberIncomeRecord = new MemberIncomeRecord
							{
								Amount = diamondAmount,
								MemberUserName = userName,
								DiamondAmount = diamondAmount,
								Type = MemberIncomeType.EquipmentBuyFeedback,
								ReferenceMemberUsername = userName
							};
							DataBase.Save(ffMemberIncomeRecord, session);
							if (fRecommendUser.RecommendUserName.IsNotNullOrEmpty())
							{
								var sRecommendUser = DataBase.Get<Member>(e => e.UserName == fRecommendUser.RecommendUserName,session);
								if (sRecommendUser != null)
								{
									var sRecommendUserRefineEquipmentCount =
										DataBase.Count<SlagRefiningEquipment>(e => e.MemberUserName == sRecommendUser.UserName);
									if (sRecommendUserRefineEquipmentCount >= 4)
									{
										var sAmount = miningEquipment.Price / 10;
										sRecommendUser.GoldBalance += sAmount * 0.3m;
										sRecommendUser.FeedBackAmount += sAmount;
										sRecommendUser.FeedBackGoldAmount += sAmount * 0.3m;
										sRecommendUser.SlagBalance += sAmount * 0.7m;
										sRecommendUser.FeedBackSlagAmount += sAmount * 0.7m;
										DataBase.Update(sRecommendUser, session);
										var sMemberIncomeRecord = new MemberIncomeRecord
										{
											Amount = sAmount,
											GoldAmount = sAmount,
											MemberUserName = sRecommendUser.UserName,
											Type = MemberIncomeType.EquipmentBuyFeedback,
											ReferenceMemberUsername = userName
										};
										DataBase.Save(sMemberIncomeRecord, session);
									}
								}
							}
						}
					}
					//给邀请人发放奖励-矿渣
					//增加直接推荐奖励，推荐的玩家每购买一台矿机，推荐人都有矿机价值的10％奖励，累计到矿渣！
//					var inviters = GetInviters(member, 2, session);
//					inviters.ForEach(inviter =>
//					{
//						inviter.SlagBalance +=
//							memberMiningEquipment.PurchasePrice / 100 * MiddleTier.BusinessConfig.EquipmentBuyRewardPercentage;
//						//6、激活成功送钻石一个(给推荐人)，每增加一个矿工送一个钻石，选金厂送5个，冶炼厂送10个！钻石以后有用！ （这个数字做成可配的）
//						inviter.DiamondBalance += type == MiningEquipmentType.Type1
//							? MiddleTier.BusinessConfig.PartnerBuyEquipmentType1RewardDiamondAmount
//							: (
//								type == MiningEquipmentType.Type2
//									? MiddleTier.BusinessConfig.PartnerBuyEquipmentType2RewardDiamondAmount
//									: MiddleTier.BusinessConfig.PartnerBuyEquipmentType3RewardDiamondAmount);
//						DataBase.Update(inviter, session);
//					});
				}
				iTransaction.Commit();
			}
			catch (Exception ex)
			{
				iTransaction.Rollback();
				MiddleTier.LogManager.Error(ex);
				if (ex is PlatformException platformException)
				{
					throw platformException;
				}
				throw new PlatformException(ErrorCode.SystemError);
			}
			finally
			{
				session.Close();
			}
		}

		/// <summary>
		/// 计算采矿设备可采集数额
		/// </summary>
		/// <param name="equipment"></param>
		/// <param name="checkStatus"></param>
		/// <returns></returns>
		public decimal MemberMiningEquipmentCalculate(MemberMiningEquipment equipment,bool checkStatus=true)
		{
			var status = GetMemberMiningEquipmentStatus(equipment.LastCollectAt);
			if (checkStatus&& status != MemberMiningEquipmentStatus.CanCollected)
			{
				throw new PlatformException(ErrorCode.MiningEquipmentCollectError);
			}
			var collectTerm = MiddleTier.BusinessConfig.MiningEquipmentCollectDay;
			switch (status)
			{
				case MemberMiningEquipmentStatus.InUse:
					var days = (DateTime.Now.Date - equipment.LastCollectAt.Date).Days;
					collectTerm = days%5;
					break;
				case MemberMiningEquipmentStatus.CanCollected:
					break;
				case MemberMiningEquipmentStatus.Expired:
				case MemberMiningEquipmentStatus.Lock:
					collectTerm = 0;
					break;
			}
			var amount = equipment.PurchasePrice;
			decimal percentage = 0;
			//获取设备变动记录
			var changeRecords = DataBase.GetList<MiningEquipmentChangeRecord>(e=>e.Type== equipment.Type);
			for (int i = 0; i < collectTerm; i++)
			{
				var day = DateTime.Now.AddDays(0 - i);
				if (day.DayOfWeek == DayOfWeek.Sunday)
					continue;
				var changeRecord = changeRecords.Where(e => e.CreatedAt.Date <= day).OrderByDescending(e=>e.Id).FirstOrDefault();
				percentage += (changeRecord?.Percentage ?? equipment.Type.GetMiningEquipment().Percentage);
			}
			return amount * percentage/100;
		}

		public QueryResult<SlagRefiningEquipment> SlagRefiningEquipmentQuery(SlagRefiningEquipmentQuery query)
		{
			return DataBase.Query(query);
		}

		/// <summary>
		/// 用户精炼
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="id"></param>
		public void MemberRefining(string userName, long id)
		{
			var member = MiddleTier.MemberManager.GetMember(userName);
			if (member == null)
			{
				throw new PlatformException(ErrorCode.UserNameIsNotExisted);
			}
			var slagRefiningEquipment = DataBase.Get<SlagRefiningEquipment>(e => e.Id == id && e.MemberUserName == userName);
			if (slagRefiningEquipment == null)
			{
				throw new PlatformException(ErrorCode.ErrorId);
			}
			if (slagRefiningEquipment.LastRefiningAt.Date >= DateTime.Now.Date)
			{
				throw new PlatformException(ErrorCode.RefiningTooBusy);
			}
			ISession session = DataBase.Session;
			ITransaction iTransaction = session.BeginTransaction(IsolationLevel.ReadCommitted);
			try
			{
				//比如矿渣余额1000    黄金账户增加金额=1000*精炼百分比（0.3）*精炼数量
				//后台设个系数吧，矿渣和黄金的提炼比例。 默认1: 1。比如提炼出1G黄金就扣1G矿渣。 如果1: 2，就是提炼1G黄金，扣2G矿渣。
				var amount = member.SlagBalance * slagRefiningEquipment.Percentage/100;
				member.GoldBalance += amount;
				member.SlagBalance -= amount * MiddleTier.BusinessConfig.RefiningConsumeRatio;
				DataBase.Update(member,session);
				slagRefiningEquipment.LastRefiningAt=DateTime.Now;
				DataBase.Update(slagRefiningEquipment, session);
				//创建转账单
				var transferBill = new TransferBill
				{
					FromMemberUserName = userName,
					FromAccountType = AccountType.Slag,
					ToMemberUserName = userName,
					ToAccountType = AccountType.Gold,
					Amount = amount,
					Status = TransferBillStatus.Completed,
					TransferBillType = TransferBillType.SlagRefine,
					ExpiredAt = DateTime.Now,
					ServiceCharge = amount * MiddleTier.BusinessConfig.RefiningConsumeRatio-amount
				};
				DataBase.Save(transferBill, session);
				var memberIncomeRecord = new MemberIncomeRecord
				{
					Amount = amount,
					GoldAmount = amount,
					SlagAmount = -amount * MiddleTier.BusinessConfig.RefiningConsumeRatio,
					Type = MemberIncomeType.Refining,
					MemberUserName = userName
				};
				DataBase.Save(memberIncomeRecord, session);
				iTransaction.Commit();
			}
			catch (Exception ex)
			{
				iTransaction.Rollback();
				MiddleTier.LogManager.Error(ex);
				if (ex is PlatformException platformException)
				{
					throw platformException;
				}
				throw new PlatformException(ErrorCode.SystemError);
			}
			finally
			{
				session.Close();
			}
		}

		public void UpdateMemberMiningEquipment(MemberMiningEquipment memberMiningEquipment)
		{
			DataBase.Update(memberMiningEquipment);
		}

		public void UpdateMiningEquipment(MiningEquipment miningEquipment)
		{
			var dbMiningEquipment = DataBase.Get<MiningEquipment>(e => e.Id == miningEquipment.Id);
			if (dbMiningEquipment == null)
			{
				throw new PlatformException(ErrorCode.ErrorId);
			}
			dbMiningEquipment.Price = miningEquipment.Price;
			dbMiningEquipment.ExpirationDay = miningEquipment.ExpirationDay;
			dbMiningEquipment.Percentage = miningEquipment.Percentage;
			DataBase.Save(ModelMapUtil.AutoMap(dbMiningEquipment, new MiningEquipmentChangeRecord()));
			DataBase.Update(dbMiningEquipment);
		}

		public QueryResult<MemberMiningEquipment> SysMemberMiningEquipmentQuery(string userName, int pageIndex, int pageSize)
		{
			var session = DataBase.Session;
			var queryable = session.Query<MemberMiningEquipment>().Where(e => e.MemberUserName.Contains(userName) && e.Status != MemberMiningEquipmentStatus.Expired);
			var restult =  new QueryResult<MemberMiningEquipment>
			{
				List = queryable.OrderByDescending(e => e.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(),
				Count = queryable.Count()
			};
			session.Close();
			return restult;
		}

		public MemberMiningEquipmentStatus GetMemberMiningEquipmentStatus(DateTime lastCollectedAt)
		{
//			0 - 4 inuse
//			5 cancollected
//			6 - 9 inuse
//			10 cancollected
//			11 - 20 lock
			var days = (DateTime.Now.Date - lastCollectedAt.Date).Days;
			var term = days / 5;//第几个周期
			var mod = days % 5;//在该周期的第几天
			var termMod = term % 5;//0,1,2,3,4---周期的周期
			if (mod == 0)//锁定和可采集
			{
				if (termMod == 1 || termMod == 2)//可采集
				{
					return MemberMiningEquipmentStatus.CanCollected;
				}
				if(termMod == 0)
				{
					return MemberMiningEquipmentStatus.InUse;
				}
				return MemberMiningEquipmentStatus.Lock;
			}
			//锁定和采集中
			if (termMod == 0||termMod == 1)//采集中
			{
				return MemberMiningEquipmentStatus.InUse;
			}
			return MemberMiningEquipmentStatus.Lock;

		}
	}
}
