using System.Web.Mvc;
using System.Web.Routing;
using jQueryTmpl.WebTest.ViewEngine;

namespace jQueryTmpl.WebTest
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

        public static void RegisterViewEngines(ViewEngineCollection viewEngines)
        {
            viewEngines.Clear();
            viewEngines.Add(new TemplateViewEngine.NewLocationViewEngine());
        }

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"Default", // Route name
				"", // URL with parameters
				new { controller = "WebTest", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);

			routes.MapRoute(
				"webtests", // Route name
				"test1", // URL with parameters
				new { controller = "WebTest", action = "Test1" } // Parameter defaults
			);
		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterViewEngines(ViewEngines.Engines);
			RegisterRoutes(RouteTable.Routes);			
		}
	}
}