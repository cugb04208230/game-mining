using System;
using System.Web.Mvc;
using BaseClasses.Filters;

namespace WebApp.Filter
{
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
		        message = error.Message;
	        }
	        else
	        {
		        message = "打盹儿了，请稍后重试";
	        }
	        message = $"<script type='text/javascript'>alert('{message}');</script >";
			filterContext.ExceptionHandled = true;
			filterContext.Result = new JsonResult{Data = new {message}};
        }
    }
}