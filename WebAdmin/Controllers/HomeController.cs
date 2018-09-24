using System;
using System.Linq;
using System.Web.Mvc;
using BaseClasses.Extensions;
using Common.Util;
using DataRepository.Entities;
using WebAdmin.Models;
using WebAdmin.Models.AdminModel;

namespace WebAdmin.Controllers
{
	/// <inheritdoc />
	/// <summary>
	/// 管理后台
	/// </summary>
	[Authorize]
    public class HomeController : BaseController
	{

		#region Login

		/// <summary>
		/// 登陆页
		/// </summary>
		/// <returns></returns>
		[AllowAnonymous]
		public ActionResult Login()
		{
			return View();
		}

		/// <summary>
		/// 登录
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpPost]
		public ActionResult Login(AdminLoginModel model)
		{
			if (!ModelState.IsValid)
			{
				return this.Fail(ModelState.Values.First(e=>e.Errors.Count>0).Errors[0].ErrorMessage);
			}
			MiddleTier.MemberManager.SysLogin(model.UserName, model.Password);
			return this.Success();
		}

		/// <summary>
		/// 登出
		/// </summary>
		/// <returns></returns>
		[AllowAnonymous]
		public ActionResult LogOut()
		{
			MiddleTier.MemberManager.SysLogOut();
			return RedirectToAction("Login","Home");
		}

		#endregion

		/// <summary>
		/// 首页
		/// </summary>
		/// <returns></returns>
		public ActionResult Index()
        {
            return View();
        }


		#region ConfigRecord

		/// <summary>
		/// 配置页
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public ActionResult ConfigRecord(ConfigRecordViewModel model)
		{
			var queryRst = MiddleTier.ConfigManager.Query(new ConfigRecordQuery { PageIndex = model.PageIndex, PageSize = model.PageSize,Description = model.Text,IsEnabled = true});
			model.ConfigRecords = queryRst;
			return View(model);
		}

		/// <summary>
		/// 配置更新
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult ConfigRecord(ConfigRecordUpdateModel model)
		{
			if (!ModelState.IsValid)
			{
				return this.Fail(ModelState.Values.First(e=>e.Errors.Count>0).Errors[0].ErrorMessage);
			}
			MiddleTier.ConfigManager.Update(model.Id,model.Value);
			return this.Success();
		}

		/// <summary>
		/// 初始化配置
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Synchronize()
		{
			MiddleTier.ConfigManager.Synchronize();
			return this.Success();
		}

		#endregion

		#region MessageBoard

		/// <summary>
		/// 留言板页
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public ActionResult MessageBoard(MessageBoardViewModel model)
		{
			var queryRst = MiddleTier.MessageBoardManager.Query(new MessageBoardQuery { PageIndex = model.PageIndex, PageSize = model.PageSize, Title = model.Text});
			model.MessageBoards = queryRst;
			return View(model);
		}

		/// <summary>
		/// 留言回复
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult MessageBoard(MessageBoardReplyModel model)
		{
			if (!ModelState.IsValid)
			{
				return this.Fail(ModelState.Values.First(e=>e.Errors.Count>0).Errors[0].ErrorMessage);
			}
			MiddleTier.MessageBoardManager.Reply(model.Id, model.Reply);
			return this.Success();
		}


		#endregion

		#region Notice

		/// <summary>
		/// 系统通知页
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public ActionResult Notice(NoticeViewModel model)
		{
			var queryRst = MiddleTier.NoticeManager.Query(new NoticeQuery { PageIndex = model.PageIndex, PageSize = model.PageSize, Title = model.Text });
			model.Notices = queryRst;
			return View(model);
		}

		/// <summary>
		/// 发布系统通知
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Notice(NoticePostModel model)
		{
			if (!ModelState.IsValid)
			{
				return this.Fail(ModelState.Values.First(e=>e.Errors.Count>0).Errors[0].ErrorMessage);
			}
			MiddleTier.NoticeManager.AddOrUpdate(model.Id, model.Title,model.Content);
			return this.Success();
		}

		/// <summary>
		/// 删除通知
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult NoticeDelete(long id)
		{
			MiddleTier.NoticeManager.Delete(id);
			return this.Success();
		}


		#endregion

		#region Member

		/// <summary>
		/// 用户列表页
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public ActionResult Member(MemberViewModel model)
		{
			var queryrst = MiddleTier.MemberManager.SysMemberQuery(ModelMapUtil.AutoMap(model,new MemberQuery()));
			model.Members = queryrst;
			return View(model);
		}

		/// <summary>
		/// 用户新增
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Member(MemberPostModel model)
		{
			if (!ModelState.IsValid)
			{
				return this.Fail(ModelState.Values.First(e => e.Errors.Count > 0).Errors[0].ErrorMessage);
			}
			MiddleTier.MemberManager.SysMemberAdd(model.UserName,model.Password,model.NickName);
			return this.Success();
		}

		/// <summary>
		/// 用户重置密码
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult MemberResetPassword(MemberResetPasswordModel model)
		{
			if (!ModelState.IsValid)
			{
				return this.Fail(ModelState.Values.First(e => e.Errors.Count > 0).Errors[0].ErrorMessage);
			}
			MiddleTier.MemberManager.SysMemberResetPassword(model.UserName,model.Password);
			return this.Success();
		}


		/// <summary>
		/// 用户更新信息
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult MemberUpdateInfo(MemberUpdateInfoModel model)
		{
			if (!ModelState.IsValid)
			{
				return this.Fail(ModelState.Values.First(e => e.Errors.Count > 0).Errors[0].ErrorMessage);
			}
			MiddleTier.MemberManager.SysMemberUpdateInfo(model.UserName, model.MemberType);
			return this.Success();
		}


		/// <summary>
		/// 用户解除状态
		/// 锁定，冻结，查封
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult MemberRelieve(string username)
		{
			//Todo
			MiddleTier.MemberManager.SysMemberRelieve(username);
			return this.Success();
		}

		/// <summary>
		/// 查封用户解除状态
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult MemberSealUp(string username)
		{
			//Todo
			MiddleTier.MemberManager.SysMemberSealUp(username);
			return this.Success();
		}

		/// <summary>
		/// 用户合伙人
		/// </summary>
		/// <returns></returns>
		public ActionResult MemberPartner(MemberPartnerViewModel model)
		{
			model.Members =
				MiddleTier.MemberManager.GetAllInvitedMembers(model.RecommendUserName,model.Gold??0, model.PageIndex ?? 1, model.PageSize ?? 10);
			return View(model);
		}

		/// <summary>
		/// 控制合伙人金余额
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult ManageMemberPartner(MemberPartnerManageModel model)
		{
			MiddleTier.MemberManager.ManageAllInvitedMembers(model.RecommendUserName,model.Gold,model.Percent/100);
			return this.Success();
		}


		#endregion

		#region MemberIncome

		/// <summary>
		/// 用户每日收益页
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public ActionResult MemberIncome(MemberIncomeViewModel model)
		{
			var queryRst = MiddleTier.MemberManager.SysMemberIncomeQuery(new MemberIncomeRecordQuery { PageIndex = model.PageIndex, PageSize = model.PageSize, MemberUserName = model.Text,CreatedAtFrom = model.CreatedAtFrom,CreatedAtTo = model.CreatedAtTo});
			model.MemberIncomeRecords = queryRst;
			return View(model);
		}



		#endregion

		#region MemberTransferBill

		/// <summary>
		/// 预约单页面
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public ActionResult TransferBill(TransferBillViewModel model)
		{
			var queryRst = MiddleTier.TransferBillManager.Query(new TransferBillQuery
			{
				PageSize = model.PageSize,
				PageIndex = model.PageIndex,
				MemberUserName = model.Text
			});
			model.TransferBills = queryRst;
			return View(model);
		}


		/// <summary>
		/// 用户求购奖励，每月28日可用
		/// </summary>
		/// <returns></returns>
		[System.Web.Http.HttpPost]
		public ActionResult MemebrToBuyReward()
		{
			var day = DateTime.Now.Day;
			if (day < 28&&day >1)
			{
				return this.Fail("每月28日-1日为求购奖励结算日");
			}
			MiddleTier.TransferBillManager.MemberToBuyReward();
			return this.Success();
		}

		#endregion

		#region MemberMiningEquipment
		/// <summary>
		/// 用户采矿设备页
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public ActionResult MemberMiningEquipment(MemberMiningEquipmentViewModel model)
		{
			var queryRst = MiddleTier.EquipmentManager.SysMemberMiningEquipmentQuery(model.Text??"", model.PageSize??1,model.PageIndex??10);
			model.MemberMiningEquipments = queryRst;
			return View(model);
		}

		#endregion

		/// <summary>
		/// 采矿设备页
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public ActionResult MiningEquipment(Models.AdminModel.MiningEquipmentViewModel model)
		{
			var queryRst = MiddleTier.EquipmentManager.MiningEquipmentQuery();
			model.MiningEquipments = queryRst;
			return View(model);
		}

		/// <summary>
		/// 采矿设备更新
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult MiningEquipment(MiningEquipmentPostModel model)
		{
			MiddleTier.EquipmentManager.UpdateMiningEquipment(ModelMapUtil.AutoMap(model,new MiningEquipment()));
			return this.Success();
		}

		/// <summary>
		/// 精炼设备页
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public ActionResult SlagRefiningEquipment(SlagRefiningEquipmentViewModel model)
		{
			var queryRst = MiddleTier.EquipmentManager.SlagRefiningEquipmentQuery(new SlagRefiningEquipmentQuery
			{
				MemberUserName = model.Text,
				PageSize = model.PageSize,
				PageIndex = model.PageIndex
			});
			model.SlagRefiningEquipments = queryRst;
			return View(model);
		}

		/// <summary>
		/// 错误页
		/// </summary>
		/// <returns></returns>
		public ActionResult Error()
		{
			return View();
		}
	}

	#region Models


	#endregion
	
}