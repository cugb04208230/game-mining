using System.Web.Http;
using WebActivatorEx;
using WebAdmin;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace WebAdmin
{
	public class SwaggerConfig
	{
		public static void Register()
		{
			var thisAssembly = typeof(SwaggerConfig).Assembly;

			GlobalConfiguration.Configuration
				.EnableSwagger(c =>
				{

					c.SingleApiVersion("v1", "TestSwagger");
					//Ìí¼ÓXML½âÎö
					c.IncludeXmlComments(GetXmlCommentsPath());
					c.IncludeXmlComments(GetDataXmlPath());
					c.IncludeXmlComments(GetBasicDataXmlPath());

				})
				.EnableSwaggerUi(c =>
				{

				});
		}

		private static string GetXmlCommentsPath()
		{
			return string.Format("{0}/WebAdmin.XML", System.AppDomain.CurrentDomain.BaseDirectory);
		}

		private static string GetDataXmlPath()
		{
			return string.Format("{0}/DataRepository.xml", System.AppDomain.CurrentDomain.BaseDirectory);
		}
		private static string GetBasicDataXmlPath()
		{
			return string.Format("{0}/BaseClasses.xml", System.AppDomain.CurrentDomain.BaseDirectory);
		}
	}
}
