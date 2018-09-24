using Newtonsoft.Json;

namespace BaseClasses.Data
{
	/// <summary>
	/// 通用数据返回模型
	/// </summary>
	public class CommonResult
	{
		/// <summary>
		/// 请求结果是否成功
		/// </summary>
		[JsonProperty("success")]
		public bool Success { get; set; }

		/// <summary>
		/// 请求返回描述信息
		/// </summary>
		[JsonProperty("message")]
		public string Message { get; set; }

		/// <summary>
		/// 状态码
		/// </summary>
		[JsonProperty("code")]
		public int Code { set; get; }

		/// <summary>
		/// 请求返回的数据模型
		/// </summary>
		[JsonProperty("data")]
		public object Data { get; set; }

		public object ExactMessage { set; get; }

		#region ctor

		/// <inheritdoc />
		/// <summary>
		/// ctor.
		/// </summary>
		public CommonResult() : this(true, "")
		{
		}

		/// <inheritdoc />
		/// <summary>
		/// ctor.
		/// </summary>
		public CommonResult(bool success) : this(success, "")
		{
		}

		/// <summary>
		/// ctor.
		/// </summary>
		public CommonResult(bool success, string message)
		{
			Success = success;
			Message = message;
		}

		/// <inheritdoc />
		/// <summary>
		/// ctor.
		/// </summary>
		public CommonResult(object data) : this()
		{
			Data = data;
		}

		#endregion

	}

	/// <inheritdoc />
	/// <summary>
	/// 通用泛型数据返回模型
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class CommonResult<T> : CommonResult
	{
		/// <summary>
		/// 请求返回的数据模型
		/// </summary>
		[JsonProperty("data")]
		public new T Data { get; set; }

		/// <summary>
		/// 数量
		/// </summary>
		public int Count { set; get; }

		#region ctor

		/// <inheritdoc />
		/// <summary>
		/// ctor
		/// </summary>
		public CommonResult() : this(true, "")
		{
		}

		/// <inheritdoc />
		/// <summary>
		/// ctor
		/// </summary>
		/// <param name="success"></param>
		public CommonResult(bool success) : this(success, "")
		{
		}

		/// <inheritdoc />
		/// <summary>
		/// ctor
		/// </summary>
		/// <param name="success"></param>
		/// <param name="message"></param>
		public CommonResult(bool success, string message)
		{
			Success = success;
			Message = message;
		}

		/// <inheritdoc />
		/// <summary>
		/// ctor
		/// </summary>
		/// <param name="data"></param>
		public CommonResult(T data) : this()
		{
			Data = data;
		}

		#endregion

	}
}
