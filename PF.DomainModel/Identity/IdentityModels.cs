using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;


namespace PF.DomainModel.Identity
{
    //public class UserLogin : IdentityUserLogin<Guid>
    //{
    //}

    //public class UserRole : IdentityUserRole<Guid>
    //{
    //}

    //public class UserClaim : IdentityUserClaim<Guid>
    //{
    //}

    //public class Role : IdentityRole<Guid, UserRole>
    //{
    //}

    //public class User : IdentityUser<Guid, UserLogin, UserRole, UserClaim>
    //{
    //}
    //登陆用户类， 在此类里增加公共属性以扩展用户字段, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
       // public string RoleName { get; set; }
        //public string Id { get; set; }
        //[Display(Name = "用户头像")]
        //public string AccountPicture { get; set; }
        [Display(Name = "姓名")]
        public string Name { get; set; }
        [MaxLength(256)]
        [Display(Name = "工号")]//唯一、可空
        public string WorkId { get; set; }
        [Display(Name = "拼音码")]
        public string PinYin { get; set; }
        [Display(Name = "医师资格")]
        public string Qualification { get; set; }
        [Display(Name = "学历")]
        public string Degree { get; set; }
        [Display(Name = "所学专业")]
        public string Special { get; set; }
        [Display(Name = "职称等级")]
        public string TechnicalTitle { get; set; }
        [Display(Name = "职务")]
        public string Duty { get; set; }

        [Display(Name = "类别")]
        [Range(0,6)]
        [EnumDataType(typeof(PF.Common.Enums.HB.Identity.UserCategory))]
        public PF.Common.Enums.HB.Identity.UserCategory Category { get; set; }

        [Display(Name = "更新日期")]
        [Column(TypeName = "Date")]
        public DateTime LastUpdateDate{get;set; }

     
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
    //用户所属于科室
    public class ApplicationUserSection
    {
        public string ApplicationUserId { get; set; }
        public string SectionId { get; set; }
    }
    //科室
    public class Section
    {
        [Key]
        [Display(Name = "科室代码")]
        public string Id { get; set; }
         [Display(Name = "科室名称")]
        public string Name { get; set; }
         [Display(Name = "拼音码")]
         public string PinYin { get; set; }
         [Display(Name = "部门代码")]
        public string DepartmentId { get; set; }
         [Display(Name = "医保代码")]
         [MaxLength(256)]
        public string InsuranceCode { get; set; }
         public virtual ICollection<ApplicationUserSection> ApplicationUsers { get; set; }
         [Display(Name = "部门")]
        public virtual Department Department { get; set; }
       // public string PinYin { get; set; }

    }
    //部门
    public class Department
    {
        [Key]
        [Required]
        [Display(Name = "部门代码")]
        public string Id { get; set; }
        [Display(Name = "部门名称")]
        public string Name { get; set; }
        [Display(Name = "院区代码")]
        public string HospitalId { get; set; }
        public virtual ICollection<Section> Sections { get; set; }
        [Display(Name = "院区")]
        public virtual Hospital Hospital { get; set; }
    }
    //分院
    public class Hospital
    {
        [Key]
        [Required]
        [Display(Name = "院区代码")]
        public string Id { get; set; }
       // [Key, Column(Order = 1)]
        //[Required]       
       
       // public string Code { get; set; }
        [Display(Name = "院区名称")]
        public string Name { get; set; }
        [Display(Name = "医保代码")]
        [MaxLength(256)]
        public string InsuranceCode { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
    }

    # region 分组模型
    public class ApplicationGroup
    {
        public ApplicationGroup()
        {
            this.Id = Guid.NewGuid().ToString();
            this.ApplicationRoles = new List<ApplicationGroupRole>();
            this.ApplicationUsers = new List<ApplicationUserGroup>();
        }

        public ApplicationGroup(string name)
            : this()
        {
            this.Name = name;
        }

        public ApplicationGroup(string name, string description)
            : this(name)
        {
            this.Description = description;
        }

        [Key]
        public string Id { get; set; }
        [Display(Name="组名")]
        public string Name { get; set; }
        [Display(Name = "描述")]
        public string Description { get; set; }
        public virtual ICollection<ApplicationGroupRole> ApplicationRoles { get; set; }
        public virtual ICollection<ApplicationUserGroup> ApplicationUsers { get; set; }
    }


    public class ApplicationUserGroup
    {
        public string ApplicationUserId { get; set; }
        public string ApplicationGroupId { get; set; }
    }

    public class ApplicationGroupRole
    {
        public string ApplicationGroupId { get; set; }
        public string ApplicationRoleId { get; set; }
    }

#endregion
}