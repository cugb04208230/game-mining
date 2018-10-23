using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using BaseClasses;
using BaseClasses.Data;
using BaseClasses.Extensions;
using BaseClasses.Filters;
using BaseClasses.Util;
using Bussiness;
using Common.Util;
using DataRepository;
using DataRepository.Entities;
using DataRepository.Enums;
using Newtonsoft.Json;
using Swashbuckle.Swagger;
using WebAdmin.Filter;
using WebAdmin.Models.AppModel;
using WebGrease.Css.Extensions;
using Color = System.Windows.Media.Color;

namespace WebAdmin.Controllers
{
	/// <inheritdoc />
	/// <summary>
	/// APP接口
	/// </summary>
	[AllowCrossSiteJson]
	[MyApiAuthorize]
	[ModelCheckFilter]
	public class AppController:ApiController
	{

		#region Ctor.

		private string UserName
		{
			get
			{
				var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
				if (cookie == null) return "";
				var ticket = FormsAuthentication.Decrypt(cookie.Value);
				if (ticket == null) return "";
				return ticket.Name;
			}
		}

		/// <summary>
		/// 用户
		/// </summary>
		private Member Member
		{
			get
			{
				var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
				if (cookie == null) return null;
				var ticket = FormsAuthentication.Decrypt(cookie.Value);
				if (ticket == null) return null;
				return JsonConvert.DeserializeObject<Member>(ticket.UserData);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected MiddleTier MiddleTier { get; set; }
		
		/// <inheritdoc />
		/// <summary>
		/// ctor.
		/// </summary>
		public AppController()
		{
			MiddleTier = MiddleTier.Instance;
		}
		

			#endregion

		private void CheckVerifyCode(string code)
		{
			var cookie = HttpContext.Current?.Request?.Cookies?[captchaCookieKey];
			if (cookie == null)
			{
				throw new PlatformException(ErrorCode.VerifyCodeError);
			}
			var ticket = FormsAuthentication.Decrypt(cookie.Value);
			if (ticket == null || ticket.UserData != code)
			{
				throw new PlatformException(ErrorCode.VerifyCodeError);
			}
		}

		/// <summary>
		/// 登录
		/// </summary>
		/// <returns></returns>
		[System.Web.Http.HttpPost]
		[System.Web.Http.AllowAnonymous]
		public CommonResult Login(LoginModel model)
		{
			CheckVerifyCode(model.Code);
			MiddleTier.MemberManager.Login(model.UserName, model.Password);
			return this.Success();
		}

		/// <summary>
		/// 退出登录
		/// </summary>
		/// <returns></returns>
		[System.Web.Http.HttpPost]
		public CommonResult Logout()
		{
			MiddleTier.Instance.MemberManager.LogOut();
			return this.Success();
		}

		/// <summary>
		/// 注册
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[System.Web.Http.HttpPost]
		[System.Web.Http.AllowAnonymous]
		public CommonResult Register(RegisterModel model)
		{
			CheckVerifyCode(model.Code);
			if (model.GlobalAreaCode == "86")
			{
				if (model.Mobile.Length != 11 || !model.Mobile.StartsWith("1"))
				{
					throw new PlatformException(ErrorCode.MobileRegexError);
				}
			}
			MiddleTier.MemberManager.Register(model.RecommondUserName,model.UserName,model.Password,model.Mobile,model.BankName,model.BankCode,model.GlobalAreaCode,model.WeChat??"",model.Alipay??"",model.BitCoin??"",model.Name??"");
			return this.Success();
		}


		/// <summary>
		/// 忘记密码--用户登录前
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[System.Web.Http.HttpPost]
		[System.Web.Http.AllowAnonymous]
		public CommonResult ForgotPassword(ForgotPasswordModel model)
		{
			return this.Success();
		}

		/// <summary>
		/// 重置密码-用户登录后
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[System.Web.Http.HttpPost]
		public CommonResult ResetPassword(ResetPasswordModel model)
		{
			MiddleTier.MemberManager.ResetPassword(UserName,model.Password);
			return this.Success();
		}

		/// <summary>
		/// 首页信息
		/// </summary>
		/// <returns></returns>
		[System.Web.Http.HttpPost]
		public CommonResult<HomeViewModel> Index()
		{
			var member = MiddleTier.MemberManager.GetMember(Member.UserName);
			var viewModel = new HomeViewModel { Member = ModelMapUtil.AutoMap(member, new HomeViewMemberModel()),GlobalGoldPrice = MiddleTier.BusinessConfig.GlobalGoldPrice};
			Parallel.Invoke(() =>
			{
				viewModel.InUseMemberCount = MiddleTier.MemberManager.InUseMemberCount();//矿主数量
//				viewModel.InUseMemberCount = MiddleTier.MemberManager.ActiveMemberCount();//排队矿主数量
			}, () =>
			{
//				viewModel.ActiveMemberCount = MiddleTier.MemberManager.InUseMemberCount();//矿主数量
				viewModel.ActiveMemberCount = MiddleTier.MemberManager.ActiveMemberCount();//排队矿主数量
			}, () =>
			{
				viewModel.EquipmentCount = MiddleTier.EquipmentManager.MemberMiningEquipmentCount(viewModel.Member.UserName);//劳工数量
			}, () =>
			{
				viewModel.Notices = MiddleTier.NoticeManager
					.Query(new NoticeQuery { PageSize = 5, OrderBys = new List<OrderField> { new OrderField() } }).List;//通知列表
			}, () =>
			{
				viewModel.QrCode = MiddleTier.MemberManager.InviterQrCodeUrl(viewModel.Member.UserName);//邀请二维码
			});
			return this.Success(viewModel);
		}

		/// <summary>
		/// 获取合伙人列表
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[System.Web.Http.HttpPost]
		public CommonResult<PartnerQueryResult> Partner(PartnerViewModel model)
		{
			var list = MiddleTier.MemberManager.GetMemberPartner(UserName, model.PageIndex ?? 1, model.PageSize ?? 10);
			var partner = list.ToPartner();
			var memberEquipmentCount = MiddleTier.EquipmentManager.GetMemberEquipmentCount(partner.List.Select(e => e.UserName));
			partner.List.ForEach(e => e.EquipmentCount = memberEquipmentCount[e.UserName]);
			partner.List.ForEach(e => e.ActiveCost = MiddleTier.BusinessConfig.ActivePartnerNeedAmount);
			var member = MiddleTier.Instance.MemberManager.GetMember(UserName);
			var language = LanguageHelper.GetLanguage();
			if (language == LanguageType.SimplifiedChinese)
				partner.Title = $"分红收益：金{member.FeedBackGoldAmount}$,矿渣{member.FeedBackSlagAmount}$";
			else
				partner.Title = $"dividend：{member.FeedBackGoldAmount}$ gold,{member.FeedBackSlagAmount}$ slag";
			return this.Success(partner);
		}

		/// <summary>
		/// 激活合伙人
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public CommonResult ActivePartner(ActivePartnerViewModel model)
		{
			MiddleTier.MemberManager.ActivePartner(UserName,model.UserName,model.UserName);
			return this.Success();
		}

		/// <summary>
		/// 我要求购 - 用户发布求购
		/// </summary>
		/// <returns></returns>
		[System.Web.Http.HttpPost]
		public CommonResult MemberToBuy(MemberToBuyModel model)
		{
			MiddleTier.TransferBillManager.MemberToBuy(UserName,model.Amount);
			return this.Success();
		}

		/// <summary>
		/// 求购中心-所有用户求购列表
		/// </summary>
		/// <returns></returns>
		[System.Web.Http.HttpPost]
		public CommonResult<QueryResult<ToBuyModel>> ToBuyList(ToBuyListModel model)
		{
			var rst = MiddleTier.TransferBillManager.ToBuyList(new TransferBillQuery
			{
				PageIndex = model.PageIndex,
				PageSize = model.PageSize,
				ExpiredAtFrom = DateTime.Now,
				Status = TransferBillStatus.Original,
				TransferBillType = TransferBillType.GoldTransfer,
				ToMemberUserNameNot = UserName,
				OrderBys = new List<OrderField>{new OrderField("Id",OrderDirection.Asc)}
			});
			var result = rst.ToToBuy();
			result.List.ForEach(item => item.Honor = item.To?.Honor ?? 0);
			return this.Success(result);
		}

		/// <summary>
		/// 我的求购列表：就是只显示自己的求购信息。
		/// 状态为3卖家已确认的时候，可以确认
		/// 状态为1（初始状态），3（卖家已确认）时可以取消
		/// </summary>
		/// <returns></returns>
		[System.Web.Http.HttpPost]
		public CommonResult<QueryResult<ToBuyModel>> MemberToBuyList(MemberToBuyListModel model)
		{
			var rst = MiddleTier.TransferBillManager.ToBuyList(new TransferBillQuery
			{
				PageIndex = model.PageIndex,
				PageSize = model.PageSize,
				ToMemberUserName = UserName,
				TransferBillType = TransferBillType.GoldTransfer
			});
			var result = rst.ToToBuy();
			result.List.ForEach(item =>item.Honor=item.From?.Honor??0);
			return this.Success(result);
		}

		/// <summary>
		/// 赠送列表：就是看到别人的求购信息，然后点击赠送之后，就出现在该列表中，赠送也要加个限制，这个规则跟求购类似。
		/// </summary>
		/// <returns></returns>
		[System.Web.Http.HttpPost]
		public CommonResult<QueryResult<ToBuyModel>> MemberGiveList(MemberGiveListModel model)
		{
			var rst = MiddleTier.TransferBillManager.ToBuyList(new TransferBillQuery
			{
				PageIndex = model.PageIndex,
				PageSize = model.PageSize,
				FromMemberUserName = UserName,
				TransferBillType = TransferBillType.GoldTransfer
			});
			var result = rst.ToToBuy();
			result.List.ForEach(item => item.Honor = item.To?.Honor ?? 0);
			return this.Success(result);
		}

		/// <summary>
		/// 用户取消求购
		/// </summary>
		/// <returns></returns>
		[System.Web.Http.HttpPost]
		public CommonResult MemberCancelToBuy(MemberToBuyCancelModel model)
		{
			MiddleTier.TransferBillManager.MemberCancelToBuy(UserName,model.Id);
			return this.Success();
		}

		/// <summary>
		/// 用户确认求购
		/// </summary>
		/// <returns></returns>
		[System.Web.Http.HttpPost]
		public CommonResult MemberEnsureToBuy(MemberToBuyEnsureModel model)
		{
			MiddleTier.TransferBillManager.MemberEnsureToBuy(UserName, model.Id);
			return this.Success();
		}

		/// <summary>
		/// 用户求购记录统计
		/// </summary>
		/// <returns></returns>
		[System.Web.Http.HttpPost]
		public CommonResult<ToBuyCollectModel> MemberToBuyCollect()
		{
			var result = MiddleTier.TransferBillManager.MemberToBuyCollect(UserName);
			return this.Success(new ToBuyCollectModel {TotalAmount = result.Item1, MonthAmount = result.Item2});
		}


		/// <summary>
		/// 资源中心
		/// </summary>
		/// <returns></returns>
		public CommonResult<QueryResult<MiningEquipmentModel>> MiningEquipment()
		{
			var list = MiddleTier.EquipmentManager.MiningEquipmentQuery();
			return this.Success(list.ToMiningEquipmentModel());
		}

		/// <summary>
		/// 用户购买采矿设备
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[System.Web.Http.HttpPost]
		public CommonResult MemberBuyMiningEquipment(MemberBuyMiningEquipmentModel model)
		{
			MiddleTier.EquipmentManager.MemberBuyMiningEquipment(UserName, model.MiningEquipmentType);
			return this.Success();
		}


		/// <summary>
		/// 用户挖矿设备分页
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[System.Web.Http.HttpPost]
		public CommonResult<List<EuipmentViewModel>> MemberMiningEquipment(MemberMiningEquipmentModel model)
		{
			var result = new CommonResult<List<EuipmentViewModel>>();
			var userName = UserName;
			var slagEuipments =
				MiddleTier.EquipmentManager.SlagRefiningEquipmentQuery(
					new SlagRefiningEquipmentQuery {MemberUserName = userName, PageSize = Int32.MaxValue,ValidityTermFrom = DateTime.Now});
			var miningEquipments = MiddleTier.EquipmentManager.MemberMiningEquipmentQuery(userName, 1, Int32.MaxValue);
			var list = new List<EuipmentViewModel>();
			if (slagEuipments.List.Count > 0)
			{
				slagEuipments.List.ForEach(slagEuipment =>
				{
					list.Add(new EuipmentViewModel
					{
						Id = slagEuipment.Id,
						CreatedAt = slagEuipment.CreatedAt,
						EquipmentType = 4,
						LastCollectAt = slagEuipment.LastRefiningAt,
						Percentage = slagEuipment.Percentage,
						PurchasePrice = Member.SlagBalance,
						Status = slagEuipment.LastRefiningAt.Date >= DateTime.Now.Date?MemberMiningEquipmentStatus.InUse:MemberMiningEquipmentStatus.CanCollected,
						ValidityTerm = slagEuipment.ValidityTerm,
						Yield = (slagEuipment.LastRefiningAt.Date==DateTime.Now.Date?0m:(Member.SlagBalance * slagEuipment.Percentage / 100)).ToString("0.##")
					});
				});
			}
			if (miningEquipments.List.Count > 0)
			{
				miningEquipments.List.ForEach(miningEquipment =>
				{
					var status = MiddleTier.EquipmentManager.GetMemberMiningEquipmentStatus(miningEquipment.LastCollectAt);
					list.Add(new EuipmentViewModel
					{
						Id = miningEquipment.Id,
						CreatedAt = miningEquipment.CreatedAt,
						EquipmentType = (int)miningEquipment.Type,
						LastCollectAt = miningEquipment.LastCollectAt,
						Percentage = miningEquipment.Percentage,
						PurchasePrice = miningEquipment.PurchasePrice,
						Status = status,
						ValidityTerm = miningEquipment.ValidityTerm,
						Yield = MiddleTier.EquipmentManager.MemberMiningEquipmentCalculate(miningEquipment,false).ToString("0.##")
					});
				});
			}
			result.Data = list;
			result.Count = miningEquipments.Count;
			return result;
		}


		/// <summary>
		/// 用户采集
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[System.Web.Http.HttpPost]
		public CommonResult MemberCollect(MemberCollectModel model)
		{
			if (model.EquipmentType == 4)
			{
				MiddleTier.EquipmentManager.MemberRefining(UserName, model.Id);
			}
			else
			{
				MiddleTier.EquipmentManager.MemberCollect(UserName, model.Id);
			}
			return this.Success();
		}
		
		/// <summary>
		/// 用户留言板数据分页
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[System.Web.Http.HttpPost]
		public CommonResult<IList<MessageBoard>> MemberMessageBoard(MemberMessageBoardModel model)
		{
			//五天之内
			var rst = MiddleTier.MessageBoardManager.Query(new MessageBoardQuery
			{
				CreatedAtFrom = DateTime.Now.Date.AddDays(-5),
				MemberUserNameEqual = UserName,
				OrderBys = new List<OrderField> { new OrderField() }
			});
			return this.Success(rst.List);
		}

		/// <summary>
		/// 用户提交留言板
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[System.Web.Http.HttpPost]
		public CommonResult MemberSubMessageBoard(MemberMessageBoardSubModel model)
		{
			if (!model.Title.ProcessStr() || !model.Content.ProcessStr())
			{
				throw new PlatformException(ErrorCode.DangerousContent);
			}
			MiddleTier.MessageBoardManager.SubMessageBoard(UserName, model.Title, model.Content);
			return this.Success();
		}
		
		/// <summary>
		/// 用户修改头像
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[System.Web.Http.HttpPost]
		public CommonResult MemberSelectHeadPic(MemberHeadPicViewModel model)
		{
			MiddleTier.MemberManager.ChangeHeadPic(UserName, model.Option);
			return this.Success();
		}

		/// <summary>
		/// 用户黄金转换
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[System.Web.Http.HttpPost]
		public CommonResult MemberTransfer(MemberTransferModel model)
		{
			MiddleTier.TransferBillManager.Initiate(model.TransferBillType,TransferBillStatus.Completed,UserName,UserName,model.Amount);
			return this.Success();
		}

		/// <summary>
		/// 转换记录查询
		/// </summary>
		/// <returns></returns>
		[System.Web.Http.HttpPost]
		public CommonResult<QueryResult<MemberTransferItemModel>> MemberTransferList(MemberTransferListModel model)
		{
			var queryResult =
				MiddleTier.TransferBillManager.MemberTransferList(UserName, model.PageIndex ?? 1, model.PageSize ?? 10);
			return this.Success(new QueryResult<MemberTransferItemModel>
			{
				Count = queryResult.Count,
				List = queryResult.List.Select(e =>
						new MemberTransferItemModel {CreatedAt = e.CreatedAt, Amount = e.Amount, TransferBillType = e.TransferBillType})
					.ToList()
			});
		}


		/// <summary>
		/// 用户收益记录
		/// </summary>
		/// <returns></returns>
		[System.Web.Http.HttpPost]
		public CommonResult<QueryResult<MemberIncomeListItemModel>> MemberIncomeList(MemberIncomeListViewModel model)
		{
			var queryResult = MiddleTier.MemberManager.MemberIncomeQuery(new MemberIncomeRecordQuery
			{
				PageIndex = model.PageIndex ?? 1,
				PageSize = model.PageSize ?? 10,
				MemberUserNameEqual = UserName
			});
			return this.Success(new QueryResult<MemberIncomeListItemModel>
			{
				Count = queryResult.Count,
				List = queryResult.List.Select(e=>new MemberIncomeListItemModel
				{
					Type = e.Type,
					Content = e.ToContent(),
					CreatedAt = e.CreatedAt
				}).ToList()
			});
		}

		/// <summary>
		/// 更新个人信息
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[System.Web.Http.HttpPost]
		public CommonResult UpdateUserInfo(MemberInfoUpdateViewModel model)
		{
			MiddleTier.Instance.MemberManager.UpdateMember(UserName,model.GlobalAreaCode??"",model.BitCoin??"");
			return this.Success();
		}


		/// <summary>
		/// 获取个人信息
		/// </summary>
		/// <returns></returns>
		[System.Web.Http.HttpPost]
		public CommonResult<HomeViewMemberModel> MemberInfo()
		{
			var member = MiddleTier.MemberManager.GetMember(Member.UserName);
			var viewModel = ModelMapUtil.AutoMap(member, new HomeViewMemberModel());
			FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, member.UserName, DateTime.Now, DateTime.Now.AddDays(1), true, member.SerializeObject());
			HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket))
			{
				HttpOnly = true,
			};
			HttpContext.Current.Response.Cookies.Add(cookie);
			return this.Success(viewModel);
		}

		/// <summary>
		/// 修改语言
		/// </summary>
		/// <param name="language">1.中文，2.英文</param>
		/// <returns></returns>
		[System.Web.Http.HttpPost]
		[System.Web.Http.AllowAnonymous]
		public CommonResult SelectLanguage(SelectLanguageModel model)
		{
			LanguageHelper.SetLanguage(model.Language);
			return this.Success();
		}

		private readonly string captchaCookieKey = "captchaKey";

		/// <summary>
		/// 生成验证码
		/// </summary>
		/// <returns></returns>
		[System.Web.Http.AllowAnonymous]
		public HttpResponseMessage GetCaptcha()
		{
			Random rd = new Random(int.Parse(DateTime.Now.ToString("HHmmssfff")));
			string str = rd.Next(0, 10000).ToString("0000");
			MemoryStream ms = GenerateImageStream(str);
			FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, captchaCookieKey, DateTime.Now, DateTime.Now.AddDays(1), true, str);
			HttpCookie cookie = new HttpCookie(captchaCookieKey, FormsAuthentication.Encrypt(ticket))
			{
				HttpOnly = true,
			};
			HttpContext.Current.Response.Cookies.Add(cookie);
			var resp = new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = new StreamContent(new MemoryStream(ms.GetBuffer()))
			};
			resp.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
			return resp;
//			return new FileContentResult(ms.GetBuffer(), "image/Jpeg");
		}


		private MemoryStream GenerateImageStream(string str)
		{
			Bitmap Img = null;
			Graphics g = null;

			int width = str.Length * 12;
			Img = new Bitmap(width, 25);
			g = Graphics.FromImage(Img);
			//背景颜色
			g.Clear(System.Drawing.Color.White);
			//文字字体
			Font f = new Font("Arial Black", 10);
			//文字颜色
			SolidBrush s = new SolidBrush(System.Drawing.Color.Black);
			g.DrawString(str, f, s, 3, 3);
			MemoryStream ms = new MemoryStream();
			Img.Save(ms, ImageFormat.Jpeg);

			//回收资源
			g.Dispose();
			Img.Dispose();
			return ms;
		}
	}


}