using PF.DomainModel.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace UI.ScientificResearch.Areas.ApplicationIdentity.Controllers
{
    public class SectionAdminController : Controller
    {
       
       

        public SectionAdminController()
        {
          
        }
        public SectionAdminController(SectionManager s,DepartmentManager d)
        {

            DepartmentManager= d ;
            SectionManager =s ;
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
        #region 科室
        public ActionResult Index()
        {
            List<Section> model = SectionManager.All().ToList();
            return View(model);
        }
        public async Task<ActionResult> Details(string id)
        {
            var model = await SectionManager.GetByIdAsync(id);
            return View(model);
        }

        public ActionResult Create()
        {
            ViewBag.Departments = (from h in DepartmentManager.All()
                                 select new SelectListItem()
                                 {
                                     Text = h.Name,
                                     Value = h.Id
                                 }).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Create(Section section)
        {
            if (ModelState.IsValid)
            {
                SectionManager.Create(section);
                return RedirectToAction("Index");
            }
            return View(section);
        }

        //[HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            Section h = await SectionManager.GetByIdAsync(id);
            await SectionManager.DeleteAsync(h);
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> Edit(string id)
        {
            Section model = await SectionManager.GetByIdAsync(id);
            ViewBag.Departments = (from h in DepartmentManager.All()
                                 select new SelectListItem()
                                 {
                                     Text = h.Name,
                                     Value = h.Id,
                                     Selected = h.Id == model.DepartmentId
                                 }).ToList();

            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(Section section)
        {
            if (ModelState.IsValid)
            {
                //string oldId=Request["oldId"].Trim();

                //if (string.Equals(oldId,hosptial.Id))
                //{
                await SectionManager.UpdateAsync(section);

                //}
                //else
                //{
                //    if (1 != await sectionManager.SqlAsync("UPDATE dbo.Departments SET Id ='" + hosptial.Id + "' WHERE Id ='" + oldId+"'"))
                //        return Content("Fail update PK");
                //}
                return RedirectToAction("Details", new { id = section.Id });
            }
            return View(section);
        }
        #endregion

    }
}