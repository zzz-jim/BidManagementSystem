using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace PF.DomainModel.Identity
{
    // This is useful if you do not want to tear down the database each time you run the application.
    // public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    // This example shows you how to create a new database if the Model changes
    public class ApplicationDbInitializer :
       // DropCreateDatabaseAlways<ApplicationDbContext>
       DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
            //增加唯一键或唯一索引
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX CK_APPUSER_WORKID ON AspNetUsers (WorkId) WHERE WorkId IS NOT NULL");
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX CK_HOSPITAL_INSURANCECODE ON Hospitals (InsuranceCode) WHERE InsuranceCode IS NOT NULL");
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX CK_SECTION_INSURANCECODE ON Sections (InsuranceCode) WHERE InsuranceCode IS NOT NULL");
            //唯一键，也称(唯一约束)，和主键的区别是可以为有多个唯一键并且值可以为NULL，但NULL也不能重复，也就是说只能有一行的值为NULL。它会隐式的创建唯一索引。
            //alter table 表名
            //add constraint 约束名 unique(列名)
            //唯一索引，几乎和唯一键一样，但可以添加过滤器来允许重复某些值
            //CREATE UNIQUE INDEX 索引名 ON 表名 (列名)
            //--如果要允许NULL重复
            // WHERE 列名 IS NOT NULL 
            //--如果还要允许值为xx重复
            //and 列名!='xx'
        }

        //Create User=Admin@Admin.com with password=Admin@123456 in the Admin role        
        public static void InitializeIdentityForEF(ApplicationDbContext db)
        {

            Hospital ybb = new Hospital
            {
                Id = "h1",
                Name = "本部",
                InsuranceCode = "A8970"
            };
            db.Hospitals.Add(ybb);
            db.SaveChanges();

            Department bqbf = new Department
            {
                Id = "d1",
                Name = "病区病房",
                HospitalId="h1"
            };
            db.SaveChanges();
            Section xxk = new Section
            {
                Id = "s1",
                Name = "信息科",
                InsuranceCode = "000",
                DepartmentId="d1"
            };
            db.SaveChanges();
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            const string username = "admin";
            const string name = "工程师";
            const string workId = "001";
            const string mail = "admin@example.com";
            const string password = "1";
            const string roleName = "Admin";
            const int cate = 0;

            //Create Role Admin if it does not exist
            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new IdentityRole(roleName);
                var roleresult = roleManager.Create(role);
            }

            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName =username,
                    WorkId=workId,
                    Name=name,
                    Email = mail,
                    Qualification = name,
                    Degree = name,
                    Special = name,
                    TechnicalTitle = name,
                    Duty = name,
                     Category=cate
                    //Departments = name,
                    //LastUpdateDate=DateTime.Now
                };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            // Add user admin to Role Admin if not already added
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(user.Id, role.Name);
            }
            //初始化权限组信息
            var groupManager = new ApplicationGroupManager();
            var newGroup = new ApplicationGroup("SuperAdmins", "Full Access to All");

            groupManager.CreateGroup(newGroup);
            groupManager.SetUserGroups(user.Id, new string[] { newGroup.Id });
            groupManager.SetGroupRoles(newGroup.Id, new string[] { role.Name });
        }
    }

}
