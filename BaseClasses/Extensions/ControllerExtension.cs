using System.Web.Http;
using System.Web.Mvc;
using BaseClasses.Data;

namespace BaseClasses.Extensions
{
	/// <summary>
	/// 控制器扩展函数
	/// </summary>
	public static class ControllerExtension
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="me"></param>
		/// <returns></returns>
		public static JsonResult Success(this Controller me)
		{
			return me.Result(true, string.Empty, string.Empty);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="me"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static JsonResult Success<T>(this Controller me, T data)
		{
			return me.Result<T>(true, "", data);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="me"></param>
		/// <param name="data"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public static JsonResult PageSuccess<T>(this Controller me, T data, int count)
		{
			return me.PageResult<T>(true, "", data, count);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="me"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		public static JsonResult Fail(this Controller me, string message)
		{
			return me.Result(false, message, string.Empty);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="me"></param>
		/// <param name="success"></param>
		/// <returns></returns>
		public static JsonResult Result(this Controller me, bool success)
		{
			return me.Result(success, string.Empty, string.Empty);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="me"></param>
		/// <param name="success"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		public static JsonResult Result(this Controller me, bool success, string message)
		{
			return me.Result(success, message, string.Empty);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="me"></param>
		/// <param name="success"></param>
		/// <param name="message"></param>
		/// <param name="data"></param>
		/// <param name="code"></param>
		/// <returns></returns>
		public static JsonResult Result<T>(this Controller me, bool success, string message, T data, int code = 200)
		{
			var responseModel = new CommonResult<T>
			{
				Success = success,
				Data = data,
				Message = message,
				Code = code
			};
			return new CustomJson(responseModel);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="me"></param>
		/// <param name="success"></param>
		/// <param name="message"></param>
		/// <param name="data"></param>
		/// <param name="count"></param>
		/// <param name="code"></param>
		/// <returns></returns>
		public static JsonResult PageResult<T>(this Controller me, bool success, string message, T data, int count, int code = 200)
		{
			var responseModel = new CommonResult<T>
			{
				Success = success,
				Data = data,
				Message = message,
				Code = code,
				Count = count
			};
			return new CustomJson(responseModel);
		}


	}
}
