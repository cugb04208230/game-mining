using Bussiness;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using DataRepository.Entities;
using Newtonsoft.Json;
using WebClasses;

namespace WebApp.Controllers
{
    public class BaseController : Controller
    {
        protected MiddleTier MiddleTier { get; set; }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            MiddleTier = WebUtilities.GetMiddleTier(HttpContext);
        }


	    protected string UserName
	    {
		    get
		    {
			    var cookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
			    if (cookie == null) return "";
			    var ticket = FormsAuthentication.Decrypt(cookie.Value);
			    if (ticket == null) return "";
			    return ticket.Name;
		    }
	    }


	    protected Member Member
	    {
		    get
		    {
			    var cookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
			    if (cookie == null) return null;
			    var ticket = FormsAuthentication.Decrypt(cookie.Value);
			    if (ticket == null) return null;
			    return JsonConvert.DeserializeObject<Member>(ticket.UserData);
		    }
	    }
	}
}