using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Bussiness;
using WebAdmin.Filter;

namespace WebAdmin
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
	        GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
			AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
	        GlobalConfiguration.Configure(WebApiConfig.Register);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
	        GlobalConfiguration.Configuration.Filters.Add(new WebApiExceptionFilterAttribute());
	        GlobalConfiguration.Configuration.Filters.Add(new MyApiAuthorizeAttribute());

			string path = System.Configuration.ConfigurationManager.AppSettings["log4net_path"];
            System.IO.FileInfo configPath = new System.IO.FileInfo(path);
            log4net.Config.XmlConfigurator.Configure(configPath);

//			MiddleTier.Instance.ConfigManager.Synchronize();
        }
    }

}
