using System.Web.Mvc;

namespace UI.ScientificResearch.Areas.Education.Controllers
{
    /// <summary>
    /// 教学统计
    /// </summary>
    public class EducationStatisticController : Controller
    {
        //
        // GET: /Education/EducationStatistic/
        public ActionResult Index()
        {
            return View();
        }
	}
}