using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using BaseClasses.Filters;
using DataRepository;

namespace WebAdmin.Filter
{
	/// <inheritdoc />
	/// <summary>
	/// 模型校验
	/// </summary>
	public class ModelCheckFilter:ActionFilterAttribute
	{
		/// <inheritdoc />
		/// <summary>
		/// </summary>
		/// <param name="filterContext"></param>
		public override void OnActionExecuting(HttpActionContext filterContext)
		{
			if (!filterContext.ModelState.IsValid)
			{
				var message = filterContext.ModelState.Values.First(e => e.Errors.Count > 0).Errors[0].ErrorMessage;
				message = LanguageHelper.GetLanguageDesc(message);
				throw new PlatformException(message);
			}
		}
	}
}