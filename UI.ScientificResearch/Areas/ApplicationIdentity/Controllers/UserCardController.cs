using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PF.DomainModel.Identity;


namespace UI.ScientificResearch.Areas.ApplicationIdentity.Controllers
{
    public class UserCardController : Controller
    {
        UserCardManager userCardManager = new UserCardManager();
        public ActionResult Index()
        {
            var model = userCardManager.All().ToList();
            return View(model);
        }
	}
}