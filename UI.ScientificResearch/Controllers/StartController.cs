using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.ScientificResearch.Controllers
{
    public class StartController : Controller
    {
        //程序启动跳转
        public ActionResult Index()
        {
            return RedirectToAction("Login", "Account", new { area = "ApplicationIdentity" });
        }
	}
}