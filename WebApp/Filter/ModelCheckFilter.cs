

using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using BaseClasses.Filters;

namespace WebApp.Filter
{
	public class ModelCheckFilter:ActionFilterAttribute
	{
		public override void OnActionExecuting(HttpActionContext filterContext)
		{
			if (filterContext.ModelState.IsValid)
			{
				throw new PlatformException(filterContext.ModelState.Values.First(e => e.Errors.Count > 0).Errors[0].ErrorMessage);
			}
		}
	}
}