using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace UI.ScientificResearch.Areas.ApplicationIdentity.Models
{
  
    public class GroupViewModel
    {
        public GroupViewModel()
        {
            this.UsersList = new List<SelectListItem>();
            this.RolesList = new List<SelectListItem>();
        }
        [Required(AllowEmptyStrings = false)]
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<SelectListItem> UsersList { get; set; }
        public ICollection<SelectListItem> RolesList { get; set; }
    }

    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "权限")]
        public string Name { get; set; }
    }

    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            this.GroupsList=new List<SelectListItem>();
            this.SectionList = new List<SelectListItem>();
        }
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false)]

        [EmailAddress]
        public string Email { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }

        [Display(Name = "姓名")]
        public string Name { get; set; }
        [Display(Name = "工号")]
        public string WorkId { get; set; }
        [Display(Name = "用户名")]
        public string UserName
        {
            get;set;
        }
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
        [Display(Name = "所在科室")]
    
        public ICollection<SelectListItem> GroupsList { get; set; }
        public ICollection<SelectListItem> SectionList { get; set; }

    }
}