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
    public class HospitalAdminController : Controller
    {
      
        public HospitalAdminController()
        {
         }
        public HospitalAdminController(HospitalManager h, DepartmentManager d)
        {
            HospitalManager = h;
            DepartmentManager = d;
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
        #region 院区
        public ActionResult Index()
        {
            List<Hospital> model = HospitalManager.All().ToList();
            return View(model);
        }
          public async Task<ActionResult> Details(string id)
        {
            var model = await HospitalManager.GetByIdAsync(id);
            return View(model);
        }

          public ActionResult Create()
          {
              return View();
          }
          [HttpPost]
          public ActionResult Create(Hospital hospital)
          {
              if (ModelState.IsValid)
              {
                  HospitalManager.Create(hospital);
                  return RedirectToAction("Index");
              }
              return View(hospital);
          }

        //[HttpPost]
          public async Task<ActionResult> Delete(string id)
        {
            Hospital h = await HospitalManager.GetByIdAsync(id);
            await HospitalManager.DeleteAsync(h);
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> Edit(string id)
        {
            Hospital model = await HospitalManager.GetByIdAsync(id);
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(Hospital hospital)
        {
            if(ModelState.IsValid)
            {
                string oldId=Request["oldId"].Trim();
               
               if (string.Equals(oldId,hospital.Id))
               {
                   await HospitalManager.UpdateAsync(hospital);
                 
               }
               else
               {
                   //由于外键约束，必须新建院区
                  await HospitalManager.CreateAsync(hospital);
                   //更新所属部门
                  var departs = DepartmentManager.Filter(h => h.HospitalId == oldId).ToList();
                   foreach (var d in departs)
                   {
                       d.HospitalId = hospital.Id;
                       await DepartmentManager.UpdateAsync(d);
                   }
                   //最后删除原院区
                  var oldHosp= await HospitalManager.GetByIdAsync(oldId);
                  await HospitalManager.DeleteAsync(oldHosp);
                   //强制更新PK
                   //if (1 != await HospitalManager.SqlAsync("UPDATE dbo.Hospitals SET Id ='" + hosptial.Id + "' WHERE Id ='" + oldId+"'"))
                   //    return Content("Fail update PK");
               }
               return RedirectToAction("Details", new { id = hospital.Id });
            }
            return View(hospital);
        }
        #endregion
      
	}
}