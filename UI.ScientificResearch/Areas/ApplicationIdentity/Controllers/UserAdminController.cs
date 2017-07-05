using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PF.DomainModel.Identity;
using UI.ScientificResearch.Areas.ApplicationIdentity.Models;

namespace UI.ScientificResearch.Areas.ApplicationIdentity.Controllers
{
    [Authorize(Roles = "超级管理员")]
    public class UsersAdminController : Controller
    {
        public UsersAdminController()
        {
        }

        public UsersAdminController(ApplicationUserManager userManager,
            ApplicationRoleManager roleManager, SectionManager sectionManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            SectionManager = sectionManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // Add the Group Manager (NOTE: only access through the public
        // Property, not by the instance variable!)
        private ApplicationGroupManager _groupManager;
        public ApplicationGroupManager GroupManager
        {
            get
            {
                return _groupManager ?? new ApplicationGroupManager();
            }
            private set
            {
                _groupManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext()
                    .Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        private SectionManager _sectionManager;
        public SectionManager SectionManager
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

        public async Task<ActionResult> Index()
        {
            List<ApplicationUser> model = await UserManager.Users.ToListAsync();
            return View(model);
        }


        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);

            // Show the groups the user belongs to:
            var userGroups = await this.GroupManager.GetUserGroupsAsync(id);
            ViewBag.GroupNames = userGroups.Select(u => u.Name).ToList();
            return View(user);
        }


        public ActionResult Create()
        {
            // Show a list of available groups:
            ViewBag.GroupsList =
                new SelectList(this.GroupManager.Groups, "Id", "Name");
            // Show a list of available sections:
            ViewBag.SectionList =
                new SelectList(SectionManager.All(), "Id", "Name");
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel user, string[] selectedSections,
            params string[] selectedGroups)
        {
            if (ModelState.IsValid)
            {
                var newUser = new ApplicationUser
                {
                    UserName = user.UserName,
                    WorkId = user.WorkerId,
                    Email = user.Email,
                    Name = user.Name,
                    Qualification = user.Qualification,
                    Degree = user.Degree,
                    Special = user.Special,
                    TechnicalTitle = user.TechnicalTitle,
                    Duty = user.Duty,
                    Category = user.Category,
                    //SectionId=user.Sections,
                    LastUpdateDate = DateTime.UtcNow
                };
                //var adminresult= UserManager.Create(newUser, user.Password);
                var adminresult = await UserManager.CreateAsync(newUser, user.Password);

                if (adminresult.Succeeded)
                {
                    //Add User to the selected Groups 
                    if (selectedGroups != null)
                    {
                        selectedGroups = selectedGroups ?? new string[] { };
                        await this.GroupManager.SetUserGroupsAsync(newUser.Id, selectedGroups);
                    }
                    //Add User to the selected Sections 
                    if (selectedSections != null)
                    {
                        await SectionManager.SetUserSectionsAsync(newUser.Id, selectedSections);
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    // string err="错误：";
                    foreach (var e in adminresult.Errors)
                    {
                        //err += e; 
                        ModelState.AddModelError("", e);
                    }
                    // throw new Exception(err);
                }
            }
            ViewBag.GroupsList = new SelectList(this.GroupManager.Groups, "Id", "Name");
            ViewBag.SectionList = new SelectList(SectionManager.All(), "Id", "Name");
            return View();
        }


        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var model = new EditUserViewModel()
            {
                Name = user.Name,
                WorkId = user.WorkId,
                Id = user.Id,
                Qualification = user.Qualification,
                Degree = user.Degree,
                Special = user.Special,
                TechnicalTitle = user.TechnicalTitle,
                Duty = user.Duty,
                UserName = user.UserName,
                Email = user.Email,

            };
            await GetSelectedGroupListAndSectionList(id, model);
            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(//[Bind(Include = "Email,Id")]
           EditUserViewModel editUser, string[] selectedSections,
            params string[] selectedGroups)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(editUser.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                // 不能更改工号:
                user.UserName = editUser.UserName;
                user.Email = editUser.Email;
                user.Name = editUser.Name;
                user.Qualification = editUser.Qualification;
                user.Degree = editUser.Degree;
                user.Special = editUser.Special;
                user.TechnicalTitle = editUser.TechnicalTitle;
                user.Duty = editUser.Duty;
                user.LastUpdateDate = DateTime.UtcNow;

                var adminresult = await this.UserManager.UpdateAsync(user);
                if (!adminresult.Succeeded)
                {
                    foreach (var e in adminresult.Errors)
                    {
                        //err += e; 
                        ModelState.AddModelError("", e);
                    }
                    await GetSelectedGroupListAndSectionList(editUser.Id, editUser);
                    return View(editUser);
                }
                // Update the Groups:
                selectedGroups = selectedGroups ?? new string[] { };
                await this.GroupManager.SetUserGroupsAsync(user.Id, selectedGroups);
                // Update the Sections:
                selectedSections = selectedSections ?? new string[] { };
                await SectionManager.SetUserSectionsAsync(user.Id, selectedSections);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something failed.");
            await GetSelectedGroupListAndSectionList(editUser.Id, editUser);
            return View(editUser);
        }


        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                // Remove all the User Group references:
                await this.GroupManager.ClearUserGroupsAsync(id);

                // Then Delete the User:
                var result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        /// <summary>
        /// 为视图模型添加用户的组别和科室
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="model">编辑用户ViewModel</param>
        /// <returns></returns>
        private async Task GetSelectedGroupListAndSectionList(string id, EditUserViewModel model)
        {
            var allGroups = this.GroupManager.Groups;
            var userGroups = await this.GroupManager.GetUserGroupsAsync(id);
            var allSections = await SectionManager.All().ToListAsync();
            var userSections = await SectionManager.GetUserSectionsAsync(id);
            foreach (var group in allGroups)
            {
                var listItem = new SelectListItem()
                {
                    Text = group.Name,
                    Value = group.Id,
                    Selected = userGroups.Any(g => g.Id == group.Id)
                };
                model.GroupsList.Add(listItem);
            }

            foreach (var section in allSections)
            {
                var listItem = new SelectListItem()
                {
                    Text = section.Name,
                    Value = section.Id,
                    Selected =
                    userSections.Any(g => g.Id == section.Id)
                };
                model.SectionList.Add(listItem);
            }
        }
    }

    //[Authorize(Roles = "超级管理员")]
    //public class UsersAdminController : Controller
    //{
    //    public UsersAdminController()
    //    {
    //    }

    //    public UsersAdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
    //    {
    //        UserManager = userManager;
    //        RoleManager = roleManager;
    //    }

    //    private ApplicationUserManager _userManager;
    //    public ApplicationUserManager UserManager
    //    {
    //        get
    //        {
    //            return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
    //        }
    //        private set
    //        {
    //            _userManager = value;
    //        }
    //    }

    //    private ApplicationRoleManager _roleManager;
    //    public ApplicationRoleManager RoleManager
    //    {
    //        get
    //        {
    //            return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
    //        }
    //        private set
    //        {
    //            _roleManager = value;
    //        }
    //    }

    //    //
    //    // GET: /Users/
    //    public async Task<ActionResult> Index()
    //    {
    //        return View(await UserManager.Users.ToListAsync());
    //    }

    //    //
    //    // GET: /Users/Details/5
    //    public async Task<ActionResult> Details(string id)
    //    {
    //        if (id == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        var user = await UserManager.FindByIdAsync(id);

    //        ViewBag.RoleNames = await UserManager.GetRolesAsync(user.Id);

    //        return View(user);
    //    }

    //    //
    //    // GET: /Users/Create
    //    public async Task<ActionResult> Create()
    //    {
    //        //Get the list of Roles
    //        ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
    //        return View();
    //    }

    //    //
    //    // POST: /Users/Create
    //    [HttpPost]
    //    public async Task<ActionResult> Create(RegisterViewModel userViewModel, params string[] selectedRoles)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            var user = new ApplicationUser 
    //            { UserName = userViewModel.WorkerId, 
    //                Email = userViewModel.Email,
    //             Name=userViewModel.Name,
    //             Qualification=userViewModel.Qualification,
    //             Degree=userViewModel.Degree,
    //             TechnicalTitle=userViewModel.TechnicalTitle,
    //             Duty=userViewModel.Duty,
    //             Departments=userViewModel.Departments,
    //             LockoutEnabled=false,
    //             Special=userViewModel.Special,
    //             LastUpdateDate=DateTime.UtcNow


    //            };
    //            var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);

    //            //Add User to the selected Roles 
    //            if (adminresult.Succeeded)
    //            {
    //                if (selectedRoles != null)
    //                {
    //                    var result = await UserManager.AddToRolesAsync(user.Id, selectedRoles);
    //                    if (!result.Succeeded)
    //                    {
    //                        ModelState.AddModelError("", result.Errors.First());
    //                        ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
    //                        return View();
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                ModelState.AddModelError("", adminresult.Errors.First());
    //                ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
    //                return View();

    //            }
    //            return RedirectToAction("Index");
    //        }
    //        ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
    //        return View();
    //    }

    //    //
    //    // GET: /Users/Edit/1
    //    public async Task<ActionResult> Edit(string id)
    //    {
    //        if (id == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        var user = await UserManager.FindByIdAsync(id);
    //        if (user == null)
    //        {
    //            return HttpNotFound();
    //        }

    //        var userRoles = await UserManager.GetRolesAsync(user.Id);

    //        return View(new EditUserViewModel()
    //        {
    //            Id = user.Id,
    //            Email = user.Email,
    //            Name=user.Name,
    //            WorkId=user.UserName,
    //            Qualification=user.Qualification,
    //            Degree=user.Degree,
    //            Special=user.Special,
    //            TechnicalTitle=user.TechnicalTitle,
    //            Duty=user.Duty,
    //            Departments=user.Departments,
    //            RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
    //            {
    //                Selected = userRoles.Contains(x.Name),
    //                Text = x.Name,
    //                Value = x.Name
    //            })
    //        });
    //    }

    //    //
    //    // POST: /Users/Edit/5
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<ActionResult> Edit( EditUserViewModel editUser, params string[] selectedRole)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            var user = await UserManager.FindByIdAsync(editUser.Id);
    //            if (user == null)
    //            {
    //                return HttpNotFound();
    //            }

    //            user.UserName = editUser.Email;
    //            user.Email = editUser.Email;
    //            user.Name=editUser.Name;
    //            user.UserName=editUser.WorkId;
    //           user.Qualification=editUser.Qualification;
    //           user.Degree=editUser.Degree;
    //        user.Special=editUser.Special;
    //            user.TechnicalTitle=editUser.TechnicalTitle;
    //           user.Duty=editUser.Duty;
    //            user.Departments=editUser.Departments;
    //            user.LastUpdateDate = DateTime.UtcNow;
    //            var userRoles = await UserManager.GetRolesAsync(user.Id);

    //            selectedRole = selectedRole ?? new string[] { };

    //            var result = await UserManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray<string>());

    //            if (!result.Succeeded)
    //            {
    //                ModelState.AddModelError("", result.Errors.First());
    //                return View();
    //            }
    //            result = await UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray<string>());

    //            if (!result.Succeeded)
    //            {
    //                ModelState.AddModelError("", result.Errors.First());
    //                return View();
    //            }
    //            return RedirectToAction("Index");
    //        }
    //        ModelState.AddModelError("", "Something failed.");
    //        return View();
    //    }

    //    //
    //    // GET: /Users/Delete/5
    //    public async Task<ActionResult> Delete(string id)
    //    {
    //        if (id == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        var user = await UserManager.FindByIdAsync(id);
    //        if (user == null)
    //        {
    //            return HttpNotFound();
    //        }
    //        return View(user);
    //    }

    //    //
    //    // POST: /Users/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public async Task<ActionResult> DeleteConfirmed(string id)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            if (id == null)
    //            {
    //                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //            }

    //            var user = await UserManager.FindByIdAsync(id);
    //            if (user == null)
    //            {
    //                return HttpNotFound();
    //            }
    //            var result = await UserManager.DeleteAsync(user);
    //            if (!result.Succeeded)
    //            {
    //                ModelState.AddModelError("", result.Errors.First());
    //                return View();
    //            }
    //            return RedirectToAction("Index");
    //        }
    //        return View();
    //    }
    //}
}
