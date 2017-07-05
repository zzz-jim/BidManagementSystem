using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PF.DomainModel.Identity;

namespace UI.ScientificResearch.Areas.ApplicationIdentity.Controllers
{
    public class AccessSystemController : Controller
    {
        private AccessSystemManager accSysManager = new AccessSystemManager();
        public ActionResult Index()
        {
            var model = accSysManager.All().ToList();
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create([Bind(Include = "AppName,CompanyName,Note")] AccessSystem accessSystem)
        {
            accessSystem.Id = Guid.NewGuid();
            accessSystem.AccessDate = DateTime.Now;
            accSysManager.Create(accessSystem);
            return RedirectToAction("Index");
        }
	}
}