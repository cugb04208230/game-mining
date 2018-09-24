using System;
using System.Web.Security;
using System.Web;
using BaseClasses;
using BaseClasses.Filters;

namespace Bussiness
{

	/// <summary>
	/// 管理后台的逻辑
	/// </summary>
    public class SystemManager : BaseManager
    {
        public SystemManager(MiddleTier middleTier) : base(middleTier)
        {

        }
		/// <summary>
		/// 登录
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="password"></param>
        public void Login(string userId, string password)
        {
            string uid = MiddleTier.BusinessConfig.BackEndUserName;
            string pwd = MiddleTier.BusinessConfig.BackEndPassword;
            if(userId!= uid || password !=pwd)
            {
                throw new PlatformException(ErrorCode.ErrorUserNameOrPassword);
            }
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,userId,DateTime.Now,DateTime.Now.Add(FormsAuthentication.Timeout),true,"");
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
		
    }
}
