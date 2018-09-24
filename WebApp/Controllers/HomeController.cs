using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using BaseClasses.Data;
using BaseClasses.Extensions;
using DataRepository.Entities;
using DataRepository.Enums;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : BaseController
    {
	    #region Login

	    [AllowAnonymous]
	    public ActionResult Login()
	    {
		    return View();
	    }

	    [HttpPost]
	    [AllowAnonymous]
	    [ValidateAntiForgeryToken]
	    public ActionResult Login(LoginModel model)
	    {
		    if (!ModelState.IsValid)
		    {
			    return View(model);
		    }
		    MiddleTier.MemberManager.Login(model.UserName, model.Password);
		    return this.Success();
	    }

	    #endregion

	    #region Register

	    [AllowAnonymous]
	    public ActionResult Register(RegisterViewModel model)
	    {
		    if (!ModelState.IsValid)
		    {
			    ViewBag.ErrorMessage = ModelState.Values.First().Errors[0].ErrorMessage;
			    return RedirectToAction("Login");
		    }
		    return View(model);
	    }

	    [HttpPost]
	    [AllowAnonymous]
	    [ValidateAntiForgeryToken]
	    public ActionResult Register(RegisterModel model)
	    {
		    if (ModelState.IsValid)
		    {
			    var message = ModelState.Values.First().Errors[0].ErrorMessage;
			    return this.Fail(message);
		    }
		    return this.Success();
	    }

	    #endregion

	    #region ForgotPassword

	    [AllowAnonymous]
	    public ActionResult ForgotPassword()
	    {
		    return View();
	    }

	    [HttpPost]
	    [AllowAnonymous]
	    [ValidateAntiForgeryToken]
	    public ActionResult ForgotPassword(ForgotPasswordModel model)
	    {
		    if (ModelState.IsValid)
		    {
			    var message = ModelState.Values.First().Errors[0].ErrorMessage;
			    return this.Fail(message);
		    }
		    return this.Success();
	    }


	    #endregion

	    #region ResetPassword

	    public ActionResult ResetPassword()
	    {
		    return View();
	    }

	    [HttpPost]
	    [ValidateAntiForgeryToken]
	    public ActionResult ResetPassword(ResetPasswordModel model)
	    {
		    if (!ModelState.IsValid)
		    {
			    var message = ModelState.Values.First().Errors[0].ErrorMessage;
			    return this.Fail(message);
		    }
		    return this.Success();
	    }

		#endregion

		#region Index

		public ActionResult Index()
	    {
		    var model = new HomeViewModel {Member = Member};
		    Parallel.Invoke(() =>
		    {
			    model.InUseMemberCount = MiddleTier.MemberManager.InUseMemberCount();//矿主数量
		    }, () =>
		    {
			    model.ActiveMemberCount = MiddleTier.MemberManager.ActiveMemberCount();//排队矿主数量
		    }, () =>
		    {
			    model.EquipmentCount = MiddleTier.EquipmentManager.MemberMiningEquipmentCount(model.Member.UserName);//劳工数量
		    }, () =>
		    {
			    model.Notices = MiddleTier.NoticeManager
				    .Query(new NoticeQuery {PageSize = 5, OrderBys = new List<OrderField> {new OrderField()}}).List;//通知列表
		    }, () =>
		    {
			    model.QrCode = MiddleTier.MemberManager.InviterQrCodeBase64Image(model.Member.UserName);//邀请二维码
		    });
		    return View(model);
	    }
	    
		#endregion
		
	    #region Partner

	    public ActionResult Partner()
	    {
		    //ToDo 是否获取第一页数据
		    return View();
	    }

	    [HttpPost]
	    public ActionResult Partner(PartnerViewModel model)
		{
			if (!ModelState.IsValid)
			{
				var message = ModelState.Values.First().Errors[0].ErrorMessage;
				return this.Fail(message);
			}
			var list = MiddleTier.MemberManager.GetMemberPartner(UserName, model.PageIndex ?? 1, model.PageSize ?? 10);
		    return this.Success(list);
	    }

	    #endregion

	    #region TransferBill
		
		[HttpPost]
	    public ActionResult Transfer(TransferModel model)
		{
			MiddleTier.TransferBillManager.Initiate(TransferBillType.GoldTransfer, TransferBillStatus.Original, UserName,
				model.ToMemberUserName, model.Amount);
			return this.Success();
	    }

	    public ActionResult TransferList()
	    {
			//Todo 是否获取首页数据
		    return View();
	    }

		[HttpPost]
	    public ActionResult TransferList(TransferListViewModel model)
		{
			if (!ModelState.IsValid)
			{
				var message = ModelState.Values.First().Errors[0].ErrorMessage;
				return this.Fail(message);
			}
			var userName = UserName;
			var list = MiddleTier.TransferBillManager.Query(new TransferBillQuery
			{
				MemberUserName = userName,
				PageSize = model.PageSize,
				PageIndex = model.PageIndex
			});
			return this.Success(list);
		}

		[HttpPost]
	    public ActionResult TransferBillEnsure(TransferBillEnsureModel model)
		{
			MiddleTier.TransferBillManager.EnsureTransferBill(model.Id,UserName);
			return this.Success();
		}

	    #endregion

	    #region MiningEquipment

		/// <summary>
		/// 用户挖矿设备页
		/// </summary>
		/// <returns></returns>
	    public ActionResult MemberMiningEquipment()
	    {
			return View();
	    }

		/// <summary>
		/// 用户挖矿设备分页
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
	    [HttpPost]
	    public ActionResult MemberMiningEquipment(MemberMiningEquipmentModel model)
		{
			var rst = MiddleTier.EquipmentManager.SysMemberMiningEquipmentQuery(UserName, model.PageIndex ?? 1,
				model.PageSize ?? 10);
		    return this.Success(rst.List);
	    }

		/// <summary>
		/// 用户挖矿设备采集
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
	    [HttpPost]
		public ActionResult MemberCollect(MemberCollectModel model)
		{
			MiddleTier.EquipmentManager.MemberCollect(UserName,model.Id);
			return this.Success();
		}

		/// <summary>
		/// 资源中心
		/// </summary>
		/// <returns></returns>
	    public ActionResult MiningEquipment()
	    {
		    var list = MiddleTier.EquipmentManager.MiningEquipmentQuery();
		    return View(list);
	    }


	    [HttpPost]
	    public ActionResult MemberBuyMiningEquipment(MemberBuyMiningEquipmentModel model)
	    {
		    MiddleTier.EquipmentManager.MemberBuyMiningEquipment(UserName,model.MiningEquipmentType);
			return this.Success();
	    }

		#endregion

		#region SlagRefiningEquipment

		/// <summary>
		/// 用户的精炼设备页
		/// </summary>
		/// <returns></returns>
		public ActionResult MemberSlagRefiningEquipment()
	    {
		    return View();
		}

		/// <summary>
		/// 用户的精炼设备数据分页
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
	    public ActionResult MemberSlagRefiningEquipment(MemberSlagRefiningEquipmentViewModel model)
	    {
		    var list = MiddleTier.EquipmentManager.SlagRefiningEquipmentQuery(new SlagRefiningEquipmentQuery
		    {
			    MemberUserName = UserName,
			    OrderBys = new List<OrderField> {new OrderField()}
		    });
		    return this.Success(list);
	    }

		/// <summary>
		/// 用户精炼
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
	    [HttpPost]
		public ActionResult MemberRefining(MemberRefiningModel model)
	    {
			MiddleTier.EquipmentManager.MemberRefining(UserName,model.Id);
		    return this.Success();
	    }

		#endregion

		#region MessageBoard

		/// <summary>
		/// 用户留言板页
		/// </summary>
		/// <returns></returns>
		public ActionResult MemberMessageBoard()
	    {
		    return View();
	    }

		/// <summary>
		/// 用户留言板数据分页
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
	    public ActionResult MemberMessageBoard(MemberMessageBoardModel model)
	    {
			//五天之内
		    var rst = MiddleTier.MessageBoardManager.Query(new MessageBoardQuery
		    {
			    CreatedAtFrom = DateTime.Now.Date.AddDays(-5),
			    MemberUserNameEqual = UserName,
			    OrderBys = new List<OrderField> {new OrderField()}
		    });
		    return this.Success(rst.List);
	    }

		/// <summary>
		/// 用户提交留言板
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
	    [HttpPost]
	    public ActionResult MemberSubMessageBoard(MemberMessageBoardSubModel model)
	    {
			MiddleTier.MessageBoardManager.SubMessageBoard(UserName,model.Title,model.Content);
		    return this.Success();
	    }

		#endregion

		#region HeadPic

		/// <summary>
		/// 用户修改头像页
		/// </summary>
		/// <returns></returns>
	    [HttpPost]
	    public ActionResult MemberHeadPic()
	    {
		    var member = Member;
		    return View(member);
	    }

		/// <summary>
		/// 用户修改头像
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
	    public ActionResult MemberSelectHeadPic(MemberHeadPicViewModel model)
		{
			MiddleTier.MemberManager.ChangeHeadPic(UserName,model.Option);
			return this.Success();
		}

	    #endregion
	}
}