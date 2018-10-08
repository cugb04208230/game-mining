using BaseClasses;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using BaseClasses.Data;
using BaseClasses.Extensions;
using BaseClasses.Filters;
using BaseClasses.Util;
using DataRepository.Entities;
using DataRepository.Enums;
using NHibernate.Linq;

namespace Bussiness
{
    public class MemberManager : BaseManager
    {
        private const string Salt = "7c2e5m%3*c";
	    public const string ChainStr = "->";
        public MemberManager(MiddleTier middleTier) : base(middleTier)
        {

        }

	    #region WebAPP

	    /// <summary>
	    /// 登录
	    /// </summary>
	    /// <param name="userName">用户名</param>
	    /// <param name="password">密码</param>
	    public void Login(string userName, string password)
	    {
		    var member = DataBase.Get<Member>(e => e.UserName == userName);
		    if (member == null)
		    {
			    throw new PlatformException(ErrorCode.ErrorUserNameOrPassword);
		    }
		    string toPassword = CypherUtility.Md5(password + Salt);
		    if (toPassword != member.Password)
		    {
			    throw new PlatformException(ErrorCode.ErrorUserNameOrPassword);
			}
		    if (member.Status == MemberStatus.UnActived)//未激活不让登录
			{
				throw new PlatformException(ErrorCode.AccountIsUnActived);
			}
		    //Locking
			if (member.Status== MemberStatus .Actived && member.LastModifiedAt < DateTime.Now.AddDays(-10))
			{
				member.Status = MemberStatus.Locking;
				DataBase.Update(member);
			}
			//Freezing
			if (member.Status == MemberStatus.Actived && member.LastModifiedAt < DateTime.Now.AddDays(-20))
		    {
			    member.Status = MemberStatus.Freezing;
			    DataBase.Update(member);
			    throw new PlatformException(ErrorCode.AccountIsFreezing);
			}
			if (member.Status == MemberStatus.Freezing)
			{
				throw new PlatformException(ErrorCode.AccountIsFreezing);
			}
			//SealUp
			if (member.Status == MemberStatus.InUse && member.ExpiredAt <= DateTime.Now)
		    {
			    member.Status = MemberStatus.SealUp;
			    DataBase.Update(member);
			    throw new PlatformException(ErrorCode.AccountIsSealUp);
			}
		    if (member.Status == MemberStatus.SealUp)
		    {
			    throw new PlatformException(ErrorCode.AccountIsSealUp);
		    }
			DataBase.Update(member);
		    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, member.UserName,DateTime.Now,DateTime.Now.AddDays(1),true, member.SerializeObject());
		    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName,FormsAuthentication.Encrypt(ticket))
		    {
			    HttpOnly = true,
		    };
		    HttpContext.Current.Response.Cookies.Add(cookie);
	    }

	    /// <summary>
	    /// 登出
	    /// </summary>
	    public void LogOut()
	    {
		    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName)
		    {
			    HttpOnly = true,
			    Expires = DateTime.Now.AddDays(-1)
		    };
		    HttpContext.Current.Response.Cookies.Add(cookie);
	    }

	    /// <summary>
	    /// 注册
	    /// //TODO 必须要推荐人，初始为未激活
	    /// </summary>
	    /// <param name="inviter">必填</param>
	    /// <param name="userName"></param>
	    /// <param name="password"></param>
	    /// <param name="mobile"></param>
	    /// <param name="weChat"></param>
	    /// <param name="alipay"></param>
	    /// <param name="bitCoin"></param>
	    /// <returns></returns>
	    public void Register(string inviter, string userName, string password,string mobile,string bankName,string bankCode,string areaCode,string weChat="",string alipay="",string bitCoin="",string name = "")
	    {
		    lock (this)
		    {
			    var member = DataBase.Get<Member>(e => e.UserName == userName);
			    if (member != null)
			    {
				    throw new PlatformException(ErrorCode.UserNameIsExisted);
				}
			    if (DataBase.Count<Member>(e =>e.Mobile == mobile) > 0)
				{
					throw new PlatformException(ErrorCode.MobileIsExisted);
				}
			    //校验
			    var recommendUser = DataBase.Get<Member>(e => e.UserName == inviter);
			    if (recommendUser == null)
			    {
				    throw new PlatformException(ErrorCode.InviterError);
			    }
				ISession session = DataBase.Session;
			    ITransaction iTransaction = session.BeginTransaction(IsolationLevel.ReadCommitted);
			    try
				{
					//Todo
//					if (recommendUser.CopperBalance < MiddleTier.BusinessConfig.ActivePartnerNeedAmount)
//					{
//						throw new PlatformException(ErrorCode.InviterAccountBalanceNotEnough);
//					}
					//保存用户
					member = new Member
				    {
					    UserName = userName,
					    Password = CypherUtility.Md5(password + Salt),
					    Mobile = mobile,
					    Name = name,
					    NickName = "",
					    Status = MemberStatus.UnActived,
					    WeChat= weChat,
						Alipay = alipay,
						BitCoin = bitCoin,
						HeadPic = MemberHeadPicOption.A,
						RecommendUserName = inviter,
						Chain = $"{recommendUser.Chain}{ChainStr}{recommendUser.UserName}",
						DiamondBalance = 1,
						ExpiredAt = DateTime.Now.AddDays(30),
						MemberType = MemberType.Normal,
						GlobalAreaCode = areaCode,
						BankCode = bankCode,
						BankName = bankName
				    };
				    DataBase.Save(member, session);
//					MiddleTier.TransferBillManager.Initiate(TransferBillType.Active, TransferBillStatus.Completed, recommendUser.UserName, member.UserName, MiddleTier.BusinessConfig.ActivePartnerNeedAmount);
					//发放邀请奖励
//					var recommendCount = DataBase.Count<Member>(e => e.RecommendUserName == inviter,session);
//					var slagEquipmentCount = DataBase.Count<SlagRefiningEquipment>(e => e.MemberUserName == inviter&&e.SourceType==SlagRefiningEquipmentSourceType.InvitingAwards,session);
//					if (slagEquipmentCount < MiddleTier.BusinessConfig.SlagRefiningEquipmentCountLimit&& (int)(recommendCount/MiddleTier.BusinessConfig.SlagRefiningEquipmentRewardInviteMemberCount)>slagEquipmentCount)//每个推荐人精炼设备上限&&达到发放奖励的推荐人数
//					{
//						//矿渣提炼炉上限是10台,每推荐10名合伙人送一台，每台可精炼矿渣总量的0.3％黄金，前两台0.5％，每台使用期限一年！
//						DataBase.Save(new SlagRefiningEquipment
//						{
//							MemberUserName = inviter,
//							LastRefiningAt = DateTime.Now,
//							Percentage = slagEquipmentCount<MiddleTier.BusinessConfig.SlagRefiningEquipmentTopCount ? MiddleTier.BusinessConfig.SlagRefiningEquipmentTopPercentage:MiddleTier.BusinessConfig.SlagRefiningEquipmentNormalPercentage,
//							SourceType = SlagRefiningEquipmentSourceType.InvitingAwards,
//							ValidityTerm = DateTime.Now.AddDays(MiddleTier.BusinessConfig.SlagRefiningEquipmentUseTermDay)
//						},session);
//					}
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
	    }
		
	    /// <summary>
	    /// 邀请的二维码
	    /// </summary>
	    /// <returns></returns>
	    public FileContentResult InviterQrCode(string userName)
	    {
		    var url = $"{MiddleTier.BusinessConfig.WebAppBaseUrl}/Account/Regist?Inviter={userName}";
		    return url.FileContentResult();
	    }

	    /// <summary>
	    /// 邀请的二维码
	    /// </summary>
	    /// <returns></returns>
	    public string InviterQrCodeUrl(string userName)
	    {
//		    http://111.231.77.96:8002/index.html#/registered?code=1111&aa=22
			return $"{MiddleTier.BusinessConfig.WebAppBaseUrl}/index.html#/registered?RecommondUserName={userName}";
	    }

		/// <summary>
		/// 邀请的二维码
		/// </summary>
		/// <returns></returns>
		public string InviterQrCodeBase64Image(string userName)
	    {
		    var url = $"{MiddleTier.BusinessConfig.WebAppBaseUrl}/Account/Regist?Inviter={userName}";
		    return url.Base64ImageString();
	    }

		/// <summary>
		/// 重置密码
		/// </summary>
		/// <param name="memberUserName"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public void ResetPassword(string memberUserName,string password)
	    {
		    var member = DataBase.Get<Member>(e => e.UserName == memberUserName);
		    if (member == null)
		    {
			    throw new PlatformException(ErrorCode.AuthFailed);
			}
		    member.Password = CypherUtility.Md5(password + Salt);
		    DataBase.Update(member);
		}

	    /// <summary>
	    /// 找回密码
	    /// Todo
	    /// </summary>
	    public void FindPassword()
	    {
	    }

		/// <summary>
		/// 获取在线人数
		/// </summary>
		/// <returns></returns>
	    public int InUseMemberCount()
	    {
		    var count = MiddleTier.BusinessConfig.OnLineMemberCount;
		    if (count == 0)
		    {
				//Todo 缺少定义
			    count = DataBase.Count<Member>(e => e.Status == MemberStatus.Actived || e.Status == MemberStatus.InUse|| e.Status == MemberStatus.SealUp);
			}
		    return count;
	    }

		/// <summary>
		/// 获取排队人数
		/// </summary>
		/// <returns></returns>
	    public int ActiveMemberCount()
		{
			var count = MiddleTier.BusinessConfig.QueuesMemberCount;
			if (count == 0)
			{
				//Todo 缺少定义
				count = DataBase.Count<Member>(e => e.Status != MemberStatus.Actived&& e.Status != MemberStatus.InUse&& e.Status != MemberStatus.SealUp);
			}
			return count;
		}

		/// <summary>
		/// 获取合伙人列表
		/// </summary>
		/// <param name="memberUserName"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public QueryResult<Member> GetMemberPartner(string memberUserName,int pageIndex=1,int pageSize=10)
	    {
		    var session = DataBase.Session;
		    var queryable = session.Query<Member>().Where(e => e.RecommendUserName==memberUserName);
		    var restult =  new QueryResult<Member>
		    {
			    List = queryable.OrderByDescending(e => e.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(),
			    Count = queryable.Count()
		    };
		    session.Close();
		    return restult;
	    }

		/// <summary>
		/// 校验用户名和姓名
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="name"></param>
		/// <returns></returns>
	    public Member CheckMember(string userName, string name)
	    {
		    var member = DataBase.Get<Member>(e => e.UserName == userName);
		    if (member == null)
		    {
				throw new PlatformException(ErrorCode.UserNameIsNotExisted);
		    }
		    if (member.Name != name)
			{
				throw new PlatformException(ErrorCode.UserNameIsNotMatchName);
			}
		    return member;
	    }

		/// <summary>
		/// 激活好友
		/// </summary>
		/// <param name="fromUserName"></param>
		/// <param name="toUserName"></param>
	    public void ActivePartner(string fromUserName, string toUserName,string name)
		{
			var toMember = GetMember(toUserName);
			if (toMember.Status != MemberStatus.UnActived)
			{
				throw new PlatformException(ErrorCode.UserIsActived);
			}
			var fromMember = GetMember(fromUserName);
			var userNames = toMember.Chain.Split(new[] { ChainStr }, StringSplitOptions.RemoveEmptyEntries);
			if (!userNames.Contains(fromUserName))
			{
				throw new PlatformException(ErrorCode.ErrorInvitRelationship);
			}
			if (fromMember.CopperBalance < MiddleTier.BusinessConfig.ActivePartnerNeedAmount)
			{
				throw new PlatformException(ErrorCode.AccountBalanceNotEnough);
			}
			//2、用户注册后，默认是未激活状态（这个又改回来了），推荐人可以激活他下面的所有用户（往下N代都行），直推从好友列表中激活就可以了。不是直推的让他输入登录名和姓名，匹配上了就可以激活（要判断是否是A下面的用户），激活动作扣除激活人的100$的银，并且往被激活人账户里充100$的银（100这个参数做成可配的）。 冬冬要做个激活的页面。
			ISession session = DataBase.Session;
			ITransaction iTransaction = session.BeginTransaction(IsolationLevel.ReadCommitted);
			try
			{
				fromMember.CopperBalance -= MiddleTier.BusinessConfig.ActivePartnerNeedAmount;
				toMember.SilverBalance += MiddleTier.BusinessConfig.ActivePartnerNeedAmount;
				toMember.Status = MemberStatus.Actived;
				//6、激活成功送钻石一个(给推荐人)，每增加一个矿工送一个钻石，选金厂送5个，冶炼厂送10个！钻石以后有用！ （这个数字做成可配的）
				fromMember.DiamondBalance += MiddleTier.BusinessConfig.ActivePartnerRewardDiamondAmount;
				DataBase.Update(toMember,session);
				DataBase.Update(fromMember, session);

				//创建转账单
				var transferBill = new TransferBill
				{
					FromMemberUserName = fromUserName,
					FromAccountType = AccountType.Copper,
					ToMemberUserName = toUserName,
					ToAccountType = AccountType.Silver,
					Amount = MiddleTier.BusinessConfig.ActivePartnerNeedAmount,
					Status = TransferBillStatus.Completed,
					TransferBillType = TransferBillType.Active,
					ExpiredAt = DateTime.Now
				};
				DataBase.Save(transferBill, session);
				var ffMemberIncomeRecord = new MemberIncomeRecord
				{
					Amount = MiddleTier.BusinessConfig.ActivePartnerRewardDiamondAmount,
					MemberUserName = fromMember.UserName,
					DiamondAmount = MiddleTier.BusinessConfig.ActivePartnerRewardDiamondAmount,
					CopperAmount = -MiddleTier.BusinessConfig.ActivePartnerNeedAmount,
					Type = MemberIncomeType.InviteActive,
					ReferenceMemberUsername = toUserName
				};
//				var fffMemberIncomeRecord = new MemberIncomeRecord
//				{
//					Amount = MiddleTier.BusinessConfig.ActivePartnerNeedAmount,
//					MemberUserName = toUserName,
//					GoldAmount = MiddleTier.BusinessConfig.ActivePartnerNeedAmount,
//					Type = MemberIncomeType.InviteActiveCost,
//					ReferenceMemberUsername = fromUserName
//				};
				DataBase.Save(ffMemberIncomeRecord, session);
				//				DataBase.Save(fffMemberIncomeRecord, session);
				if (toMember.RecommendUserName.IsNotNullOrEmpty())
				{
					var recommendCount = DataBase.Count<Member>(e => e.RecommendUserName == toMember.RecommendUserName&&new []{MemberStatus.Actived, MemberStatus.InUse, MemberStatus.SealUp}.Contains(e.Status), session);
					var slagEquipmentCount = DataBase.Count<SlagRefiningEquipment>(e => e.MemberUserName == toMember.RecommendUserName && e.SourceType == SlagRefiningEquipmentSourceType.InvitingAwards, session);
					if (slagEquipmentCount < MiddleTier.BusinessConfig.SlagRefiningEquipmentCountLimit && (int)(recommendCount / MiddleTier.BusinessConfig.SlagRefiningEquipmentRewardInviteMemberCount) > slagEquipmentCount)//每个推荐人精炼设备上限&&达到发放奖励的推荐人数
					{
						//矿渣提炼炉上限是10台,每推荐10名合伙人送一台，每台可精炼矿渣总量的0.3％黄金，前两台0.5％，每台使用期限一年！
						DataBase.Save(new SlagRefiningEquipment
						{
							MemberUserName = toMember.RecommendUserName,
							LastRefiningAt = DateTime.Now,
							Percentage = slagEquipmentCount < MiddleTier.BusinessConfig.SlagRefiningEquipmentTopCount ? MiddleTier.BusinessConfig.SlagRefiningEquipmentTopPercentage : MiddleTier.BusinessConfig.SlagRefiningEquipmentNormalPercentage,
							SourceType = SlagRefiningEquipmentSourceType.InvitingAwards,
							ValidityTerm = DateTime.Now.AddDays(MiddleTier.BusinessConfig.SlagRefiningEquipmentUseTermDay)
						}, session);
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

	    public void ChangeHeadPic(string userName, MemberHeadPicOption option)
		{
			var member = MiddleTier.MemberManager.GetMember(userName);
			if (member == null)
			{
				throw new PlatformException(ErrorCode.UserNameIsNotExisted);
			}
			member.HeadPic = option;
			DataBase.Update(member);
		}


	    public QueryResult<MemberIncomeRecord> MemberIncomeQuery(MemberIncomeRecordQuery query)
	    {
		    return DataBase.Query(query);
	    }

		#endregion

		#region WebAdmin

		/// <summary>
		/// 管理员登录
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		public void SysLogin(string userName, string password)
		{
			if (userName != MiddleTier.BusinessConfig.BackEndUserName|| password!= MiddleTier.BusinessConfig.BackEndPassword)
			{
				throw new PlatformException(ErrorCode.ErrorUserNameOrPassword);
			}
			FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddDays(1), true, "");
			HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket))
			{
				HttpOnly = true,
			};
			HttpContext.Current.Response.Cookies.Add(cookie);
		}

	    /// <summary>
	    /// 登出
	    /// </summary>
	    public void SysLogOut()
	    {
		    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName)
		    {
			    HttpOnly = true,
			    Expires = DateTime.Now.AddDays(-1)
		    };
		    HttpContext.Current.Response.Cookies.Add(cookie);
	    }

	    public void SysMemberAdd(string userName, string password,string nickName)
	    {
		    if (DataBase.Count<Member>(e => e.UserName == userName) > 0)
		    {
				throw new PlatformException(ErrorCode.UserNameIsExisted);
		    }
		    var member = new Member
		    {
			    UserName = userName,
			    Password = CypherUtility.Md5(password + Salt),
			    Mobile = "",
			    Name = "",
			    NickName = nickName,
			    Status = MemberStatus.Actived,
			    WeChat = "",
			    HeadPic = MemberHeadPicOption.A,
				RecommendUserName = "",
				Chain = ""
		    };
		    DataBase.Save(member);
		}

	    public void SysMemberResetPassword(string userName, string password)
	    {
		    var member = DataBase.Get<Member>(e => e.UserName == userName);
		    if (member == null)
		    {
				throw new PlatformException(ErrorCode.UserNameIsNotExisted);
		    }
		    member.Password = CypherUtility.Md5(password + Salt);
			DataBase.Update(member);
		}

	    public void SysMemberUpdateInfo(string userName, MemberType memberType)
	    {
		    var member = DataBase.Get<Member>(e => e.UserName == userName);
		    if (member == null)
		    {
			    throw new PlatformException(ErrorCode.UserNameIsNotExisted);
		    }
		    member.MemberType = memberType;
		    DataBase.Update(member);
	    }

		public void SysMemberRelieve(string userName)
	    {
		    var member = DataBase.Get<Member>(e => e.UserName == userName);
		    if (member == null)
		    {
			    throw new PlatformException(ErrorCode.UserNameIsNotExisted);
		    }
		    switch (member.Status)
		    {
				case MemberStatus.Freezing:
					member.Status = MemberStatus.Actived;
					break;
			    case MemberStatus.Locking:
				    member.Status = MemberStatus.Actived;
					break;
			    case MemberStatus.SealUp:
				    member.Status = MemberStatus.InUse;
				    member.GoldBalance -= 50;
				    break;
			}
		    DataBase.Update(member);
		}

	    public void SysMemberSealUp(string userName)
	    {
		    var member = DataBase.Get<Member>(e => e.UserName == userName);
		    if (member == null)
		    {
			    throw new PlatformException(ErrorCode.UserNameIsNotExisted);
			}
			member.Status = MemberStatus.SealUp;
		    DataBase.Update(member);
	    }


		public QueryResult<Member> SysMemberQuery(MemberQuery query)
	    {
		    return DataBase.Query(query);
	    }

		public QueryResult<MemberIncomeRecord> SysMemberIncomeQuery(MemberIncomeRecordQuery query)
		{
			return DataBase.Query(query);
		}

		#endregion

		/// <summary>
		/// 根据用户名获取用户
		/// </summary>
		/// <param name="memberUserName"></param>
		/// <returns></returns>
		public Member GetMember(string memberUserName)
		{
			var member = DataBase.Get<Member>(e => e.UserName == memberUserName);
			return member;
		}

		/// <summary>
		/// 获取父级邀请用户
		/// </summary>
		/// <param name="member">当前用户</param>
		/// <param name="count">层级</param>
		/// <returns></returns>
	    public List<Member> GetInviters(Member member, int count=1)
	    {
		    var chain = member.Chain;
		    var userNames = chain.Split(new[] { MemberManager.ChainStr }, StringSplitOptions.RemoveEmptyEntries);
		    if (count < userNames.Length)
		    {
			    userNames = userNames.Skip(userNames.Length - count).Take(count).ToArray();
		    }
		    return DataBase.GetList<Member>(e => userNames.Contains(e.UserName));
	    }

	    /// <summary>
	    /// 获取直接邀请用户
	    /// </summary>
	    public List<Member> GetInvitedMembers(string userName)
	    {
		    return DataBase.GetList<Member>(e => e.RecommendUserName == userName);
	    }

	    /// <summary>
	    /// 获取直接邀请用户
	    /// </summary>
	    public QueryResult<Member> GetAllInvitedMembers(string userName,decimal gold,int pageIndex,int pageSize)
	    {
		    var session = DataBase.Session;
			var result = new QueryResult<Member>{List = new List<Member>()};
			var user = DataBase.Get<Member>(e => e.UserName == userName);
		    if (user == null)
		    {
			    session.Close();
				return result;
		    }
		    var query = session.Query<Member>();
		    query = query.Where(e =>
			    e.Chain.EndsWith($"{ChainStr}{userName}") || e.Chain.Contains($"{ChainStr}{userName}{ChainStr}"));
		    query = query.Where(e => e.GoldBalance >= gold);
		    result.Count = query.Count();
		    result.List = query.OrderByDescending(e => e.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
		    session.Close();
			return result;
		}

	    /// <summary>
	    /// 获取直接邀请用户
	    /// </summary>
	    public void ManageAllInvitedMembers(string userName, decimal gold, decimal percent)
	    {
		    var user = DataBase.Get<Member>(e => e.UserName == userName);
		    if (user == null)
		    {
				return;
		    }
			ISession session = DataBase.Session;
			ITransaction iTransaction = session.BeginTransaction(IsolationLevel.ReadCommitted);
			try
			{
				var query = session.Query<Member>();
				query = query.Where(e =>
					e.Chain.EndsWith($"{ChainStr}{userName}") || e.Chain.Contains($"{ChainStr}{userName}{ChainStr}"));
				query = query.Where(e => e.GoldBalance >= gold);
				var members = query.ToList();
				members.ForEach(member =>
				{
					member.GoldBalance = member.GoldBalance - (member.GoldBalance - gold) * percent;
					DataBase.Update(member,session);
				});
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

		public void UpdateMember(string userName, string alipay, string weChat, string bitCoin, string name, string mobile)
		{
			var member = DataBase.Get<Member>(e => e.UserName == userName);
			if (member == null)
			{
				throw new PlatformException(ErrorCode.UserNameIsNotExisted);
			}
			if (DataBase.Count<Member>(e => e.UserName != userName && e.Mobile == mobile) > 0)
			{
				throw new PlatformException(ErrorCode.MobileIsExisted);
			}
			member.Alipay = alipay;
			member.WeChat = weChat;
			member.BitCoin = bitCoin;
			member.Name = name;
			member.Mobile = mobile;
			DataBase.Update(member);
		}
	    public void UpdateMember(string userName, string arecCode, string bitCoin)
	    {
		    var member = DataBase.Get<Member>(e => e.UserName == userName);
		    if (member == null)
		    {
			    throw new PlatformException(ErrorCode.UserNameIsNotExisted);
		    }
		    member.BitCoin = bitCoin;
		    member.GlobalAreaCode = arecCode;
		    DataBase.Update(member);
	    }

	    public string GetUpdateBalanceSql(string userName, decimal goldBalance = 0, decimal silverBalance = 0,
		    decimal copperBalance = 0, decimal slagBalance = 0, decimal collectAmount = 0)
	    {
		    return
			    $"update Members set GoldBalance = GoldBalance + {goldBalance}, SilverBalance = SilverBalance + {silverBalance}, CopperBalance = CopperBalance + {copperBalance}, SlagBalance = SlagBalance + {slagBalance}, CollectAmount = CollectAmount + {collectAmount} where UserName='{userName}';";
	    }

    }
}
