using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PF.DomainModel.Identity;


namespace UI.ScientificResearch.Areas.ApplicationIdentity.Controllers
{
    public class PatinetController : Controller
    {
        PatientManager patiManager = new PatientManager();
        public ActionResult Index()
        {
            var model = patiManager.All().ToList();
            return View(model);
        }
	}
}