using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jQueryTmpl.WebTest.Controllers
{
    public class WebTestController : Controller
    {
        //
        // GET: /WebTest/

        public ActionResult Index()
        {
            return View();
        }

		public ActionResult Test1(string testName)
		{
			return View();
		}

    }
}
