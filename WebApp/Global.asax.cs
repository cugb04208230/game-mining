using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebApp
{
    public class MvcApplication : HttpApplication
    {
        public MvcApplication()
        {
            System.IO.FileInfo configPath = new System.IO.FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}/Config/log4net.xml");
            log4net.Config.XmlConfigurator.Configure(configPath);
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
