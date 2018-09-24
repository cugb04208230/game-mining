using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using BaseClasses.Data;

namespace BaseClasses.Extensions
{
	public static class ApiControllerExtension
	{

		/// <summary>
		/// 
		/// </summary>
		/// <param name="me"></param>
		/// <returns></returns>
		public static CommonResult<string> Success(this ApiController me)
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
		public static CommonResult<T> Success<T>(this ApiController me, T data)
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
		public static CommonResult<T> PageSuccess<T>(this ApiController me, T data, int count)
		{
			return me.PageResult<T>(true, "", data, count);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="me"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		public static CommonResult<string> Fail(this ApiController me, string message)
		{
			return me.Result(false, message, string.Empty);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="me"></param>
		/// <param name="success"></param>
		/// <returns></returns>
		public static CommonResult<string> Result(this ApiController me, bool success)
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
		public static CommonResult<string> Result(this ApiController me, bool success, string message)
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
		public static CommonResult<T> Result<T>(this ApiController me, bool success, string message, T data, int code = 200)
		{
			var responseModel = new CommonResult<T>
			{
				Success = success,
				Data = data,
				Message = message,
				Code = code
			};
			return responseModel;
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
		public static CommonResult<T> PageResult<T>(this ApiController me, bool success, string message, T data, int count, int code = 200)
		{
			var responseModel = new CommonResult<T>
			{
				Success = success,
				Data = data,
				Message = message,
				Code = code,
				Count = count
			};
			return responseModel;
		}
	}
}
