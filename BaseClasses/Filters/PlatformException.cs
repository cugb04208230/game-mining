using System;
using BaseClasses.Extensions;

namespace BaseClasses.Filters
{
	/// <inheritdoc />
	/// <summary>
	/// 全局错误
	/// </summary>
	public class PlatformException : ApplicationException
	{
		/// <summary>
		/// error code
		/// </summary>
		public ErrorCode ErrorCode { get; set; }
		
		/// <inheritdoc />
		/// <summary>
		/// ctor.
		/// </summary>
		public PlatformException(ErrorCode code) : base(code.GetDescription(),new Exception(code.GetDescription()))
		{
			ErrorCode = code;
		}


		/// <inheritdoc />
		/// <summary>
		/// ctor.
		/// </summary>
		public PlatformException(string message) : base(message, new Exception(message))
		{
			ErrorCode = ErrorCode.LogicError;
		}
	}
}
