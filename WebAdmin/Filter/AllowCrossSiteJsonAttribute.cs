using System.Web.Mvc;

namespace WebAdmin.Filter
{
	/// <inheritdoc />
	/// <summary>
	/// 跨域属性
	/// </summary>
	public class AllowCrossSiteJsonAttribute : ActionFilterAttribute
	{
		private readonly string[] _domains;
		/// <inheritdoc />
		/// <summary>
		/// </summary>
		/// <param name="domain"></param>
		public AllowCrossSiteJsonAttribute(string domain)
		{
			_domains = new[] { domain };
		}
		/// <inheritdoc />
		/// <summary>
		/// </summary>
		/// <param name="domains"></param>
		public AllowCrossSiteJsonAttribute(string[] domains)
		{
			_domains = domains;
		}
		/// <inheritdoc />
		/// <summary>
		/// </summary>
		public AllowCrossSiteJsonAttribute()
		{
			_domains = new []{""};
		}
		/// <inheritdoc />
		/// <summary>
		/// </summary>
		/// <param name="filterContext"></param>
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
//			var context = filterContext.RequestContext.HttpContext;
//			var host = context.Request.UrlReferrer?.Host;
			filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
			base.OnActionExecuting(filterContext);
		}
	}
}