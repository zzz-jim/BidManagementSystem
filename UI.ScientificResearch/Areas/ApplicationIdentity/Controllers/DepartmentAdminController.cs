using PF.DomainModel.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;

namespace UI.ScientificResearch.Areas.ApplicationIdentity.Controllers
{
    public class DepartmentAdminController : Controller
    {

        public DepartmentAdminController()
        {
        }
        public DepartmentAdminController(HospitalManager h, DepartmentManager d, SectionManager s)
        {
            HospitalManager = h;
            DepartmentManager = d;
            SectionManager = s;
        }
        private HospitalManager _hosManager;
        public HospitalManager HospitalManager
        {
            get
            {
                return _hosManager ?? HttpContext.GetOwinContext()
                    .Get<HospitalManager>();
            }
            private set
            {
                _hosManager = value;
            }
        }
        private DepartmentManager _departManager;
        public DepartmentManager DepartmentManager
        {
            get
            {
                return _departManager ?? HttpContext.GetOwinContext()
                    .Get<DepartmentManager>();
            }
            private set
            {
                _departManager = value;
            }
        }
        private ISectionManager _sectionManager;
        public ISectionManager SectionManager
        {
            get
            {
                return _sectionManager ?? HttpContext.GetOwinContext()
                    .Get<SectionManager>();
            }
            private set
            {
                _sectionManager = value;
            }
        }
        #region 部门
        public ActionResult Index()
        {
            // TODO: jim test
            var result = HttpContext.GetOwinContext();
            //var a=result.
            var dep = result.Get<DepartmentManager>();

            List<Department> model = DepartmentManager.All().ToList();
            return View(model);
        }
        public async Task<ActionResult> Details(string id)
        {
            var model = await DepartmentManager.GetByIdAsync(id);
            return View(model);
        }

        public ActionResult Create()
        {
            ViewBag.Hospitals = (from h in HospitalManager.All()
                                 select new SelectListItem()
                                 {
                                     Text = h.Name,
                                     Value = h.Id
                                 }).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                DepartmentManager.Create(department);
                return RedirectToAction("Index");
            }
            return View(department);
        }

        //[HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            Department h = await DepartmentManager.GetByIdAsync(id);
            await DepartmentManager.DeleteAsync(h);
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> Edit(string id)
        {
            Department model = await DepartmentManager.GetByIdAsync(id);
            ViewBag.Hospitals = (from h in HospitalManager.All()
                                 select new SelectListItem()
                                 {
                                     Text = h.Name,
                                     Value = h.Id,
                                     Selected = h.Id == model.HospitalId
                                 }).ToList();

            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(Department department)
        {

            if (ModelState.IsValid)
            {
                string oldId = Request["oldId"].Trim();

                if (string.Equals(oldId, department.Id))
                {
                    await DepartmentManager.UpdateAsync(department);

                }
                else
                {
                    //由于外键约束，必须新建部门
                    //await DepartmentManager.CreateAsync(department);
                    ////更新所属科室
                    //var sections = SectionManager.Filter(h => h.DepartmentId == oldId).ToList();
                    //foreach (var d in sections)
                    //{
                    //    d.DepartmentId = department.Id;
                    //    await SectionManager.UpdateAsync(d);
                    //}
                    ////最后删除原部门
                    //var oldDpart = await DepartmentManager.GetByIdAsync(oldId);
                    //await DepartmentManager.DeleteAsync(oldDpart);
                }
                return RedirectToAction("Details", new { id = department.Id });
            }
            return View(department);
        }
        #endregion

    }
}