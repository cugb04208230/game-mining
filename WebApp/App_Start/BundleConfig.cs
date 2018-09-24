using System.Web.Optimization;

namespace WebApp
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
				"~/Scripts/jquery-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
				"~/Scripts/n/jquery/dist/jquery.js",
				"~/Scripts/n/jquery-validation/dist/additional-methods.js",
				"~/Scripts/n/jquery-validation/dist/jquery.validate.js",
				"~/Scripts/n/jquery-validation-unobtrusive/jquery-validation-unobtrusive.js"));
			
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
				"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
				"~/Scripts/bootstrap.js",
				"~/Scripts/respond.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
				"~/Content/bootstrap.css",
				"~/Content/site.css"));
		}
	}
}