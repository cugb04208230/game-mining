using Bussiness;
using System.Web.Mvc;
using System.Web.Routing;
using WebAdmin.Filter;
using WebClasses;

namespace WebAdmin.Controllers
{
	/// <inheritdoc />
	/// <summary>
	/// Ctor.
	/// </summary>
    public class BaseController : Controller
    {
		/// <summary>
		/// 
		/// </summary>
        protected MiddleTier MiddleTier { get; set; }

		/// <inheritdoc />
		/// <summary>
		/// </summary>
		/// <param name="requestContext"></param>
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            MiddleTier = WebUtilities.GetMiddleTier(HttpContext);
        }
    }


}