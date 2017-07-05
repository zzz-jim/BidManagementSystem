using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PF.DomainModel.Identity.Mapping;

namespace PF.DomainModel.Identity
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            // TODO: jim
            //this.Configuration.LazyLoadingEnabled = false;
        }

        static ApplicationDbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            //  Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //增加属性（表） 
        public virtual IDbSet<ApplicationGroup> ApplicationGroups { get; set; }
        public virtual IDbSet<Hospital> Hospitals { get; set; }
        public virtual IDbSet<Department> Departments { get; set; }
        public virtual IDbSet<Section> Sections { get; set; }
        public virtual IDbSet<Patient> PatinetBasicInfoes { get; set; }
        public virtual IDbSet<UserCard> UserCards { get; set; }

        public virtual IDbSet<AccessSystem> AccessSystems { get; set; }

        // Override OnModelsCreating:
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Make sure to call the base method first:
            base.OnModelCreating(modelBuilder);
            // 配置实体类型映射到的表名            
            //modelBuilder.Entity<IdentityUser>().ToTable("User");
            //modelBuilder.Entity<IdentityRole>().ToTable("Role");
            //modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogin");
            //modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaim");
            //modelBuilder.Entity<IdentityUserRole>().ToTable("UserRole");

            //自增,一般含有ID的都会自增，如果不要自增请设置DatabaseGeneratedOption.None
            //     modelBuilder.Entity<Hospital>().Property(p => p.Id)
            //.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity); 


            // Map Users to Section:
            modelBuilder.Configurations.Add(new ApplicationGroupRoleMap());
            modelBuilder.Configurations.Add(new ApplicationGroupMap());
            modelBuilder.Configurations.Add(new ApplicationUserGroupMap());
            modelBuilder.Configurations.Add(new ApplicationUserSectionMap());

            modelBuilder.Configurations.Add(new SectionMap());


        }
    }
}
