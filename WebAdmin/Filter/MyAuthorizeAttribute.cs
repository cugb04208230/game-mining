using System;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Security;
using BaseClasses;
using BaseClasses.Filters;
using DataRepository.Entities;
using DataRepository.Enums;
using Newtonsoft.Json;
using AllowAnonymousAttribute = System.Web.Http.AllowAnonymousAttribute;

namespace WebAdmin.Filter
{
	/// <inheritdoc />
	/// <summary>
	/// 我的校验模型
	/// </summary>
    public class MyAuthorizeAttribute : AuthorizeAttribute
    {

		/// <inheritdoc />
		/// <summary>
		/// OnAuthorization
		/// </summary>
		/// <param name="filterContext"></param>
		public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }

		/// <inheritdoc />
		/// <summary>
		/// AuthorizeCore
		/// </summary>
		/// <param name="httpContext"></param>
		/// <returns></returns>
		protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
			//每周日放假，无产量。每天晚上11点到早6点维护
//	        if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
//			{
//				throw new PlatformException(ErrorCode.SundayToRest);
//			}
//	        if (DateTime.Now.Hour >= 23 || DateTime.Now.Hour < 6)
//	        {
//				throw new PlatformException(ErrorCode.BusinessHoursError);
//	        }
	        var user = httpContext.User;
            if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
            {
                return true;
			}
			throw new PlatformException(ErrorCode.AuthFailed);
		}


		/// <inheritdoc />
		/// <summary>
		/// HandleUnauthorizedRequest
		/// </summary>
		/// <param name="filterContext"></param>
		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
            filterContext.Result = new RedirectResult(FormsAuthentication.LoginUrl);
        }
    }

	/// <inheritdoc />
	/// <summary>
	/// 我的Api校验模型
	/// </summary>
	public class MyApiAuthorizeAttribute : AuthorizationFilterAttribute
	{
		/// <inheritdoc />
		/// <summary>
		/// 校验模型
		/// </summary>
		/// <param name="actionContext"></param>
		public override void OnAuthorization(HttpActionContext actionContext)
		{
			try
			{
				if (((ReflectedHttpActionDescriptor)actionContext.ActionDescriptor).MethodInfo.GetCustomAttribute(typeof(AllowAnonymousAttribute))!=null)
				{
					return;
				}
				base.OnAuthorization(actionContext);
				//每周日放假，无产量。每天晚上11点到早6点维护
//			if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
//			{
//				throw new PlatformException(ErrorCode.SundayToRest);
//			}
//				if (DateTime.Now.Hour >= 23 || DateTime.Now.Hour < 6)
//				{
//					throw new PlatformException(ErrorCode.BusinessHoursError);
//				}
				var cookie = HttpContext.Current?.Request?.Cookies?[FormsAuthentication.FormsCookieName];
				if (cookie == null)
					throw new PlatformException(ErrorCode.AuthFailed);
				var ticket = FormsAuthentication.Decrypt(cookie.Value);
				if (ticket == null)
					throw new PlatformException(ErrorCode.AuthFailed);
				var member = JsonConvert.DeserializeObject<Member>(ticket.UserData);
				if (member.Status == MemberStatus.Locking)
				{
					throw new PlatformException(ErrorCode.AccountIsLocking);
				}
				if (member.Status == MemberStatus.Freezing)
				{
					throw new PlatformException(ErrorCode.AccountIsFreezing);
				}
			}
			catch (Exception e)
			{
				if (e is PlatformException)
				{
					throw e;
				}
				throw new PlatformException(ErrorCode.AuthFailed);
			}
		}
	}
}