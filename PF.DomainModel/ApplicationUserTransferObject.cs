using System;
using System.Collections.Generic;

namespace PF.DomainModel
{
    public class ApplicationUserTransferObject
    {
        // public string RoleName { get; set; }
        public string Id { get; set; }

        //[Display(Name = "姓名")]
        public string Name { get; set; }
        //[MaxLength(256)]
        //[Display(Name = "工号")]//唯一、可空
        public string WorkId { get; set; }
        //[Display(Name = "拼音码")]
        public string PinYin { get; set; }
        //[Display(Name = "医师资格")]
        public string Qualification { get; set; }
        //[Display(Name = "学历")]
        public string Degree { get; set; }
        //[Display(Name = "所学专业")]
        public string Special { get; set; }
        //[Display(Name = "职称等级")]
        public string TechnicalTitle { get; set; }
        //[Display(Name = "职务")]
        public string Duty { get; set; }

        //[Display(Name = "类别")]
        //[Range(0, 6)]
        //[EnumDataType(typeof(PF.Common.Enums.HB.Identity.UserCategory))]
        public string Category { get; set; }

        //[Display(Name = "更新日期")]
        //[Column(TypeName = "Date")]
        public DateTime LastUpdateDate { get; set; }


        //public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        //{
        //    // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
        //    var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
        //    // Add custom user claims here
        //    return userIdentity;
        //}
    }

    ////用户所属于科室
    //public class ApplicationUserSection
    //{
    //    public string ApplicationUserId { get; set; }
    //    public string SectionId { get; set; }
    //}

    //科室
    public class SectionTransferObject
    {
        //[Key]
        //[Display(Name = "科室代码")]
        public string Id { get; set; }
        //[Display(Name = "科室名称")]
        public string Name { get; set; }
        //[Display(Name = "拼音码")]
        public string PinYin { get; set; }
        //[Display(Name = "部门代码")]
        public string DepartmentId { get; set; }
        //[Display(Name = "医保代码")]
        //[MaxLength(256)]
        public string InsuranceCode { get; set; }
        public IList<ApplicationUserTransferObject> ApplicationUsers { get; set; }
        //[Display(Name = "部门")]
        //public virtual Department Department { get; set; }
        // public string PinYin { get; set; }
        public bool HasChildren { get; set; }
    }

    //部门
    public class DepartmentTransferObject
    {
        //[Key]
        //[Required]
        //[Display(Name = "部门代码")]
        public string Id { get; set; }
        //[Display(Name = "部门名称")]
        public string Name { get; set; }
        //[Display(Name = "院区代码")]
        public string HospitalId { get; set; }
        public virtual IList<SectionTransferObject> Sections { get; set; }
        //[Display(Name = "院区")]
        //public virtual Hospital Hospital { get; set; }
        public bool HasChildren { get; set; }
    }

    //分院
    public class HospitalTransferObject
    {
        //[Key]
        //[Required]
        //[Display(Name = "院区代码")]
        public string Id { get; set; }
        // [Key, Column(Order = 1)]
        //[Required]       
        // public string Code { get; set; }
        //[Display(Name = "院区名称")]
        public string Name { get; set; }
        //[Display(Name = "医保代码")]
        //[MaxLength(256)]
        public string InsuranceCode { get; set; }
        public IList<DepartmentTransferObject> Departments { get; set; }
        public bool HasChildren { get; set; }
    }
}
