using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using BaseClasses;
using BaseClasses.Data;
using BaseClasses.Extensions;
using BaseClasses.Filters;
using DataRepository.Entities;
using DataRepository.Enums;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Util;

namespace Bussiness
{
	public class TransferBillManager:BaseManager
	{
		public TransferBillManager(MiddleTier middleTier) : base(middleTier)
		{
		}

		/// <summary>
		/// 发起转账单
		/// </summary>
		/// <param name="transferBillType">转账单类型</param>
		/// <param name="status"></param>
		/// <param name="fromMemberUserName">发起转账用户Id</param>
		/// <param name="toMemberUserName">购买人用户名</param>
		/// <param name="amount">转账金额</param>
		public void Initiate(TransferBillType transferBillType, TransferBillStatus status, string fromMemberUserName, string toMemberUserName, decimal amount)
		{
			var session = DataBase.Session;
			ITransaction iTransaction = session.BeginTransaction(IsolationLevel.ReadCommitted);
			try
			{
				//创建转账单
				var transferBill = new TransferBill
				{
					FromMemberUserName = fromMemberUserName,
					FromAccountType = transferBillType.FromAccount(),
					ToMemberUserName = toMemberUserName,
					ToAccountType = transferBillType.ToAccount(),
					Amount = amount,
					Status = status,
					TransferBillType= transferBillType,
					ExpiredAt = DateTime.Now
				};
				DataBase.Save(transferBill,session);
				//扣除出账账户余额
				var fromMember = GetFromMember(transferBill,session);
				if (fromMember != null)
				{
					DataBase.Update(fromMember, session);
				}
				//完成状态的交易单增加入账账户余额
				if (transferBill.Status == TransferBillStatus.Completed)
				{
					if (transferBill.FromMemberUserName == transferBill.ToMemberUserName)
					{
						var propertyName = $"{transferBill.ToAccountType.ToString()}Balance";
						var balance = fromMember.GetPropertyValue(propertyName).To<decimal>();
						balance += transferBill.Amount;
						fromMember.SetPropertyValue(propertyName, balance);
						DataBase.Update(fromMember, session);
					}
					else
					{
						var toMember = GetToMember(transferBill, session);
						DataBase.Update(toMember, session);
					}
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
		

		private Member GetFromMember(TransferBill transferBill,ISession session)
		{
			if (transferBill.FromMemberUserName.IsNullOrEmpty())
			{
				return null;
			}
			//1.检查用户
			var fromMember = DataBase.Get<Member>(e=>e.UserName==transferBill.FromMemberUserName,session);
			if (fromMember == null)
			{
				throw new PlatformException(ErrorCode.UserNameIsNotExisted);
			}
			var propertyName = $"{transferBill.FromAccountType.ToString()}Balance";
			var balance = fromMember.GetPropertyValue(propertyName).To<decimal>();
			if (balance < transferBill.Amount)//出账账户余额不足
			{
				throw new PlatformException(ErrorCode.AccountBalanceNotEnough);
			}
			balance -= transferBill.Amount;
			fromMember.SetPropertyValue(propertyName,balance);
			return fromMember;
		}
		
		private Member GetToMember(TransferBill transferBill, ISession session)
		{
			if (transferBill.ToMemberUserName.IsNullOrEmpty())
			{
				return null;
			}
			var toMember = DataBase.Get<Member>(e => e.UserName == transferBill.ToMemberUserName, session);
			if (toMember == null)
			{
				throw new PlatformException(ErrorCode.UserNameIsNotExisted);
			}
			var propertyName = $"{transferBill.FromAccountType.ToString()}Balance";
			var balance = toMember.GetPropertyValue(propertyName).To<decimal>();
			balance += transferBill.Amount;
			toMember.SetPropertyValue(propertyName, balance);
			return toMember;
		}

		public QueryResult<TransferBill> Query(TransferBillQuery query)
		{
			var expr = QueryWhere(query);
			var session = DataBase.Session;
			var rst = new QueryResult<TransferBill>
			{
				Count = DataBase.Count(expr),
				List = session.Query<TransferBill>().Where(expr).OrderByDescending(e=>e.Id).Skip(query.Skip).Take(query.Take).ToList()
			};
			session.Close();
			return rst;
		}

		private Expression<Func<TransferBill, bool>> QueryWhere(TransferBillQuery query)
		{
			Expression<Func<TransferBill, bool>> expr = query.Where();
			if (query.MemberUserName.IsNotNullOrEmpty())
			{
				expr = expr.And(e => e.ToMemberUserName == query.MemberUserName || e.FromMemberUserName == query.MemberUserName);
			}
			return expr;
		}

		#region ToBuy

		/// <summary>
		/// 1.买家发起求购
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="amount"></param>
		public void MemberToBuy(string userName, decimal amount)
		{
			if (amount % 50 > 0 || amount > MiddleTier.BusinessConfig.ToBuyAmountLimit)
			{
				throw new PlatformException(ErrorCode.ToBuyAmountLimitError);
			}
			var user = DataBase.Get<Member>(e => e.UserName == userName);
			if (user == null)
			{
				throw new PlatformException(ErrorCode.UserNameIsNotExisted);
			}
			if (user.Status != MemberStatus.InUse)
			{
				throw new PlatformException(ErrorCode.MustBuyMiningEquipment);
			}
			var count = DataBase.Count<TransferBill>(e =>
				e.ToMemberUserName == userName && (e.Status == TransferBillStatus.Original ||
				                                   e.Status == TransferBillStatus.SellerEnsure ||
				                                   e.Status == TransferBillStatus.BuyerEnsure) &&
				e.TransferBillType == TransferBillType.GoldTransfer);
			if (count >= MiddleTier.BusinessConfig.CurrentNormalToBuyLimitTime && user.MemberType == MemberType.Normal)
			{
				throw new PlatformException(ErrorCode.CurrentToBuyTimeLimit);
			}
			if (count >= MiddleTier.BusinessConfig.CurrentCallToBuyLimitTime && user.MemberType == MemberType.CallCenter)
			{
				throw new PlatformException(ErrorCode.CurrentToBuyTimeLimit);
			}
			var existCount = DataBase.Count<TransferBill>(e =>
				e.ToMemberUserName == userName && (e.Status == TransferBillStatus.Original|| e.Status == TransferBillStatus.SellerEnsure||e.Status==TransferBillStatus.BuyerEnsure||e.Status == TransferBillStatus.Completed) && 
				e.TransferBillType == TransferBillType.GoldTransfer&&e.CreatedAt>DateTime.Now.Date);
			if ((user.MemberType==MemberType.Normal&&existCount >= MiddleTier.BusinessConfig.ToBuyLimitTime)|| (user.MemberType == MemberType.CallCenter && existCount >= MiddleTier.BusinessConfig.CallCenterToBuyLimitTime))
			{
				throw new PlatformException(ErrorCode.ToBuyTimeLimit);
			}
			//创建转账单
			var transferBill = new TransferBill
			{
				FromMemberUserName = "",
				FromAccountType = TransferBillType.GoldTransfer.FromAccount(),
				ToMemberUserName = userName,
				ToAccountType = TransferBillType.GoldTransfer.ToAccount(),
				Amount = amount,
				Status = TransferBillStatus.Original,
				TransferBillType = TransferBillType.GoldTransfer,
				ExpiredAt = DateTime.Now.AddDays(MiddleTier.BusinessConfig.ToBuyExpiredTime),
				ToBuyAt = DateTime.Now
			};
			DataBase.Save(transferBill);
		}
		
		/// <summary>
		/// 用户取消求购
		/// 求购者才可以取消求购
		/// SellerEnsure，Original
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="id"></param>
		public void MemberCancelToBuy(string userName,long id)
		{
			var transferBill = DataBase.Get<TransferBill>(e => e.Id == id&&(e.ToMemberUserName == userName|| e.FromMemberUserName == userName));
			if (transferBill == null)
			{
				throw new PlatformException(ErrorCode.ErrorId);
			}
			if (transferBill.Status != TransferBillStatus.Original)
			{
				throw new PlatformException(ErrorCode.ErrorCancelStatus);
			}
			CancelToBuy(transferBill);
		}

		/// <summary>
		/// 管理员取消求购
		/// </summary>
		/// <param name="id"></param>
		public void AdminCancelToBuy(long id)
		{
			var transferBill = DataBase.Get<TransferBill>(e => e.Id == id);
			if (transferBill == null)
			{
				throw new PlatformException(ErrorCode.ErrorId);
			}
			CancelToBuy(transferBill);
		}

		/// <summary>
		/// 取消求购
		/// </summary>
		/// <param name="transferBill"></param>
		private void CancelToBuy(TransferBill transferBill)
		{
			var session = DataBase.Session;
			ITransaction iTransaction = session.BeginTransaction(IsolationLevel.ReadCommitted);
			try
			{
				var fromMember = DataBase.Get<Member>(e=>e.UserName ==transferBill.FromMemberUserName,session);
				var toMember = DataBase.Get<Member>(e => e.UserName == transferBill.ToMemberUserName,session);
				switch (transferBill.Status)
				{
					case TransferBillStatus.Original:
						break;
					case TransferBillStatus.SellerEnsure:
					case TransferBillStatus.BuyerEnsure:
						if (fromMember == null)
						{
							throw new PlatformException(ErrorCode.SystemError);
						}
						fromMember.GoldBalance += (transferBill.Amount + transferBill.ServiceCharge);
						DataBase.Update(fromMember, session);
						break;
					case TransferBillStatus.Completed:
						if (fromMember == null||toMember==null)
						{
							throw new PlatformException(ErrorCode.SystemError);
						}
						if (toMember.GoldBalance < transferBill.Amount-transferBill.ServiceCharge)
						{
							throw new PlatformException(ErrorCode.AccountBalanceNotEnough);
						}
						fromMember.GoldBalance += (transferBill.Amount+ transferBill.ServiceCharge);
						toMember.GoldBalance -= transferBill.Amount;
						if (toMember.GoldBalance < 0)
						{
							throw new PlatformException(ErrorCode.AccountBalanceNotEnough);
						}
						DataBase.Update(fromMember, session);
						DataBase.Update(toMember, session);
						break;
					default:
						throw new PlatformException(ErrorCode.ErrorCancelStatus);
				}
				transferBill.ExpiredAt = DateTime.Now.AddYears(1);
				transferBill.Status = TransferBillStatus.Cancelled;
				DataBase.Update(transferBill,session);
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
		/// 用户确认求购
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="id"></param>
		public void MemberEnsureToBuy(string userName, long id)
		{
			var session = DataBase.Session;
			ITransaction iTransaction = session.BeginTransaction(IsolationLevel.ReadCommitted);
			try
			{
				var transferBill = DataBase.Get<TransferBill>(e => e.Id == id, session);
				if (transferBill == null)
				{
					throw new PlatformException(ErrorCode.ErrorId);
				}
				var member = DataBase.Get<Member>(e => e.UserName == userName, session);
				if (member == null)
				{
					throw new PlatformException(ErrorCode.UserNameIsNotExisted);
				}
				switch (transferBill.Status)
				{
					case TransferBillStatus.Original://卖家可以确认,卖家确认修改交易单状态为卖家确认，并且减掉卖家金额
						GetTransferBillToAmount(transferBill);
						if (member.GoldBalance < (transferBill.Amount + transferBill.ServiceCharge))
						{
							throw new PlatformException(ErrorCode.AccountBalanceNotEnough);
						}
						if (transferBill.ToMemberUserName == userName)
						{
							throw new PlatformException(ErrorCode.ErrorId);
						}
						transferBill.FromMemberUserName = userName;
						transferBill.Status = TransferBillStatus.SellerEnsure;
						transferBill.GivedAt=DateTime.Now;
						member.GoldBalance -= (transferBill.Amount + transferBill.ServiceCharge);
						DataBase.Update(member, session);
						break;
					case TransferBillStatus.SellerEnsure://买家可以确认
						if (transferBill.ToMemberUserName != userName)
						{
							throw new PlatformException(ErrorCode.ErrorId);
						}
						transferBill.Status = TransferBillStatus.BuyerEnsure;
						transferBill.ReceivedAt = DateTime.Now;
						break;
					case TransferBillStatus.BuyerEnsure:
						if (transferBill.FromMemberUserName != userName)
						{
							throw new PlatformException(ErrorCode.ErrorId);
						}
						transferBill.Status = TransferBillStatus.Completed;
						var toMember = DataBase.Get<Member>(e => e.UserName == transferBill.ToMemberUserName, session);
						if (toMember == null)
						{
							throw new PlatformException(ErrorCode.UserNameIsNotExisted);
						}
						toMember.GoldBalance += transferBill.Amount;
						transferBill.CompletedAt = DateTime.Now;
						member.Honor += MiddleTier.BusinessConfig.TransferGiveHonor;
						toMember.Honor += MiddleTier.BusinessConfig.TransferHonor;
						DataBase.Update(member, session);
						DataBase.Update(toMember, session);
						break;
					default:
						throw new PlatformException(ErrorCode.TransferBillStatusError);
				}
				DataBase.Update(transferBill, session);
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
		public List<Member> GetInviters(Member member, int count,ISession session)
		{
			var chain = member.Chain;
			var userNames = chain.Split(new[] { MemberManager.ChainStr }, StringSplitOptions.RemoveEmptyEntries);
			if (count < userNames.Length)
			{
				userNames = userNames.Skip(userNames.Length - count).Take(count).ToArray();
			}
			return DataBase.GetList<Member>(e => userNames.Contains(e.UserName), session);
		}

		public QueryResult<TransferBill> ToBuyList(TransferBillQuery query)
		{
			var result =  DataBase.Query(query);
			var memberuserNames =
				result.List.Select(e => e.FromMemberUserName).Union(result.List.Select(e => e.ToMemberUserName));
			var members = DataBase.GetList<Member>(e => memberuserNames.Contains(e.UserName));
			result.List.ForEach(item =>
			{
				item.FromMember = members.FirstOrDefault(e => e.UserName == item.FromMemberUserName);
				item.ToMember = members.FirstOrDefault(e => e.UserName == item.ToMemberUserName);
			});
			return result;
		}

		/// <summary>
		/// 计算求购单的入账金额，免除手续费
		/// </summary>
		/// <param name="transferBill"></param>
		/// <returns></returns>
		private decimal GetTransferBillToAmount(TransferBill transferBill)
		{
			//Todo 手续费收买家还是卖家
			var feePercent = 0.1m;
			var equipmentCount = DataBase.Count<MemberMiningEquipment>(e =>
				e.MemberUserName == transferBill.ToMemberUserName && e.Type == MiningEquipmentType.Smelter);
			if (equipmentCount >= 5)
			{
				feePercent = 0;
			}
			else if(equipmentCount>=2)
			{
				feePercent = 0.05m;
			}
			transferBill.ServiceCharge = transferBill.Amount * feePercent;
			return transferBill.Amount * (1 - feePercent);
		}

		public QueryResult<TransferBill> MemberTransferList(string userName, int pageIndex, int pageSize)
		{
			var session = DataBase.Session;
			var queryable = session.Query<TransferBill>().Where(e =>
				e.FromMemberUserName == userName && (e.TransferBillType == TransferBillType.GoldToCopper ||
				                                     e.TransferBillType == TransferBillType.GoldToSilver)).OrderByDescending(e=>e.Id);
			var result =  new QueryResult<TransferBill>
			{
				Count = queryable.Count(),
				List = queryable.Skip((pageIndex-1)*pageSize).Take(pageSize).ToList()
			};
			session.Close();
			return result;
		}

		public Tuple<decimal, decimal> MemberToBuyCollect(string username)
		{
			var session = DataBase.Session;
			var lastDay = new DateTime(DateTime.Now.Year,DateTime.Now.Month,1);
			var queryable = session.Query<TransferBill>().Where(e =>
				e.ToMemberUserName == username && e.Status==TransferBillStatus.Completed&&e.TransferBillType == TransferBillType.GoldTransfer).ToList();
			var sum1 = queryable.Sum(e => e.Amount);
			var sum2 = queryable.Where(e => e.CompletedAt >= lastDay).Sum(e => e.Amount);
			var result = new Tuple<decimal, decimal>(sum1, sum2);
			session.Close();
			return result;
		}

		public void MemberToBuyReward()
		{
			var session = DataBase.Session;
			var endDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 28);
			var lastDay = endDay.AddMonths(-1);
			if (DateTime.Now.Day == 1)
			{
				endDay = endDay.AddMonths(-1);
				lastDay = lastDay.AddMonths(-1);
			}
			if (session.Query<MemberIncomeRecord>().Any(e => e.CreatedAt > lastDay))
			{
				session.Close();
				throw new PlatformException("本月已经奖励过了啊");
			}
			var queryable = session.Query<TransferBill>().Where(e =>
					e.Status == TransferBillStatus.Completed && e.TransferBillType == TransferBillType.GoldTransfer &&
					e.CompletedAt >= lastDay&&e.CompletedAt< endDay).Select(
					e => new {e.ToMemberUserName, e.Amount}).GroupBy(e => e.ToMemberUserName)
				.Select(e => new KeyValuePair<string,decimal>(e.Key, e.Sum(x => x.Amount))).ToList();
			var usernames = queryable.Select(kv => kv.Key).ToList();
			var users = session.Query<Member>().Where(e => usernames.Contains(e.UserName));
			foreach (var member in users)
			{
				ITransaction iTransaction = session.BeginTransaction(IsolationLevel.ReadCommitted);
				try
				{
					var amount = queryable.First(e => e.Key == member.UserName).Value * 0.03m;
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
						Type = MemberIncomeType.ToBuyReward
					};
					DataBase.Save(memberIncomeRecord, session);
					//更新账户余额
//					member.GoldBalance += memberIncomeRecord.GoldAmount;
//					member.SilverBalance += memberIncomeRecord.SilverAmount;
//					member.CopperBalance += memberIncomeRecord.CopperAmount;
//					member.SlagBalance += memberIncomeRecord.SlagAmount;
//					member.CollectAmount += memberIncomeRecord.Amount;
//					DataBase.Update(member, session);
					var sql = MiddleTier.MemberManager.GetUpdateBalanceSql(member.UserName, memberIncomeRecord.GoldAmount,
						memberIncomeRecord.SilverAmount, memberIncomeRecord.CopperAmount, memberIncomeRecord.SlagAmount,
						memberIncomeRecord.Amount);
					DataBase.ExecuteBySql(sql, session);
					iTransaction.Commit();
				}
				catch (Exception ex)
				{
					iTransaction.Rollback();
					MiddleTier.LogManager.Error(ex);
					session.Close();
					if (ex is PlatformException platformException)
					{
						throw platformException;
					}
					throw new PlatformException(ErrorCode.SystemError);
				}
			}
//			users.ForEach(member =>
//			{
//				
//			});
			session.Close();
		}

		#endregion
	}
}

