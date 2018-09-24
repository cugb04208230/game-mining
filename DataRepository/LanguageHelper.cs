using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using DataRepository.Enums;

namespace DataRepository
{
	public static class LanguageHelper
	{
		public static readonly string SessionLanguageCookieKey = "SessionLanguageCookieKey";
		public static void SetLanguage(LanguageType language)
		{
			FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, "LanguageHelper.SessionLanguageCookieKey", DateTime.Now, DateTime.Now.AddDays(1), true, ((int)language).ToString());
			HttpCookie cookie = new HttpCookie(LanguageHelper.SessionLanguageCookieKey, FormsAuthentication.Encrypt(ticket))
			{
				HttpOnly = true,
			};
			HttpContext.Current.Response.Cookies.Add(cookie);
		}

		public static LanguageType GetLanguage()
		{
			var languageType = LanguageType.SimplifiedChinese;
			var cookie = HttpContext.Current?.Request?.Cookies?[SessionLanguageCookieKey];
			if (cookie != null)
			{
				var ticket = FormsAuthentication.Decrypt(cookie.Value);
				if (ticket !=null)
				{
					languageType = (LanguageType) int.Parse(ticket.UserData);
				}
			}
			return languageType;
		}

		public static string GetLanguageDesc(string key)
		{
			var type = GetLanguage() == LanguageType.English ? typeof(En) : typeof(ZhCn);
			ResourceManager  manager = new ResourceManager(type);
			return manager.GetString(key)??key;
		}
	}
}
