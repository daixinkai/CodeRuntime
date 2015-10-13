using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeRuntime.TestWeb.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            DebugLog.WriteLine("test");
            return Content(DebugLog.Message);
        }
    }
}