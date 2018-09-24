using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Web.Http.Filters;
using System.Web.Mvc;
using BaseClasses;
using BaseClasses.Data;
using BaseClasses.Extensions;
using BaseClasses.Filters;
using DataRepository;
using NLog;
using NLog.Config;
using WebAdmin.Filter;
using ActionFilterAttribute = System.Web.Mvc.ActionFilterAttribute;
using IExceptionFilter = System.Web.Mvc.IExceptionFilter;

namespace WebAdmin
{
    /// <summary>
    /// 
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
	        filters.Add(new MyAuthorizeAttribute());
			filters.Add(new ExceptionAttribute());
		}
    }
	/// <summary>
	/// 
	/// </summary>
	public class ExceptionAttribute : ActionFilterAttribute, IExceptionFilter
	{
		/// <inheritdoc />
		/// <summary>
		/// 异常
		/// </summary>
		/// <param name="filterContext"></param>
		public void OnException(ExceptionContext filterContext)
		{
			Exception error = filterContext.Exception;
			string message;//错误信息
			if (error is PlatformException)
			{
				message = ((PlatformException)error).ErrorCode.GetDescription();
			}
			else
			{
				message = "打盹儿了，请稍后重试";
			}
			filterContext.ExceptionHandled = true;
			filterContext.Controller.ViewBag.Message = message;
			var header = filterContext.HttpContext.Request.Headers["x-requested-with"];
			if (header != null && header.Equals("XMLHttpRequest"))
			{
				filterContext.Result = new JsonResult { Data = new {
					Success = false,
					Message = message,
				}, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
			}
			else
			{
				if (error is PlatformException&&((PlatformException)error).ErrorCode == ErrorCode.AuthFailed)
				{
					filterContext.Result = new RedirectResult("/home/login");
				}
				else
				{
					LogManager.Configuration = new XmlLoggingConfiguration($"{System.AppDomain.CurrentDomain.BaseDirectory}config/log.config");
					Logger logger = LogManager.GetCurrentClassLogger();
					logger.Info(error.Message);
					filterContext.Controller.ViewBag.Exception = error;
					filterContext.Result = new RedirectResult("/home/error");
				}
			}
			filterContext.ExceptionHandled = true;
			filterContext.HttpContext.Response.Clear();
			filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
		}
	}

	public class WebApiExceptionFilterAttribute : ExceptionFilterAttribute
	{
		//重写基类的异常处理方法
		public override void OnException(HttpActionExecutedContext actionExecutedContext)
		{
			Exception error = actionExecutedContext.Exception;
			string message;//错误信息
			var code = 200;
			if (error is PlatformException)
			{
				var errCode = (PlatformException) error;
				message = errCode.ErrorCode==ErrorCode.LogicError?errCode.Message:LanguageHelper.GetLanguageDesc(errCode.ErrorCode.ToString());
				code = (int) ((PlatformException) error).ErrorCode;

			}
			else
			{
				LogManager.Configuration = new XmlLoggingConfiguration($"{System.AppDomain.CurrentDomain.BaseDirectory}config/log.config");
				Logger logger = LogManager.GetCurrentClassLogger();
				logger.Info(error);
				message = "打盹儿了，请稍后重试";
			}
			var oResponse = new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = new  ObjectContent(typeof(CommonResult), new CommonResult<string>
				{
					Success = false,
					Message = message,
					Code = code,
					ExactMessage = error
				}, new JsonMediaTypeFormatter())
			};
			actionExecutedContext.Response = oResponse;
		}
	}

}
