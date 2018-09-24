using System.Web.Mvc;

namespace BaseClasses.Data
{
	/// <inheritdoc />
	/// <summary>
	/// </summary>
	public class CustomJson : JsonResult
	{
		/// <inheritdoc />
		/// <summary>
		/// ctor.
		/// </summary>
		/// <param name="data"></param>
		public CustomJson(object data)
		{
			Data = data;
		}
	}
}
