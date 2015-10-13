﻿using System;
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
            return View();
            DebugLog.WriteLine("test");
            return Content(DebugLog.Message);
        }

        [HttpPost]
        public ActionResult Index(string code, string reference = null)
        {
            ViewBag.Code = code;
            ViewBag.Reference = reference;
            object result = null;
            try
            {

                if (!string.IsNullOrEmpty(reference))
                {
                    var rr = reference.Split(new char[] { ';', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    var te = MethodConstruct.CreateMethod<object>(code, rr);

                    result = te();
                }
                else
                {
                    var te = MethodConstruct.CreateMethod<object>(code);

                    result = te();
                }
                string debugger = DebugLog.Message;
                result = debugger + "\r\n" + result;
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex;
            }
            return View(result);
        }

    }
}