using System.Web.Mvc;

namespace jQueryTmpl.WebTest.ViewEngine
{
	public class TemplateViewEngine : Controller
	{
		public class NewLocationViewEngine : RazorViewEngine
		{
			public NewLocationViewEngine()
			{
				PartialViewLocationFormats = new[] {
	                "~/Views/Templates/{0}.cshtml"
	            };
			}
		}
	}
}