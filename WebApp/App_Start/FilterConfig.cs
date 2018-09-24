using System.Web.Mvc;
using WebApp.Filter;

namespace WebApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new MyAuthorizeAttribute());
            filters.Add(new ExceptionAttribute());
        }
    }
}
