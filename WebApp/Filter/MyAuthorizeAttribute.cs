using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace WebApp.Filter
{
    public class MyAuthorizeAttribute : AuthorizeAttribute
    {

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = httpContext.User;
            if (user?.Identity != null && user.Identity.IsAuthenticated)
            {
                return true;
            }
            return false;
        }


        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
            filterContext.Result = new RedirectResult(FormsAuthentication.LoginUrl);
        }
    }
}