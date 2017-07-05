using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.ScientificResearch.Areas.Training.Controllers
{
    /// <summary>
    /// 网上培训
    /// </summary>
    public class OnlineTrainingController : Controller
    {
        // GET: Training/OnlineTraining
        public ActionResult Index()
        {
            return View();
        }
    }
}