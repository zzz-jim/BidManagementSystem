using System;
using System.ComponentModel.DataAnnotations;

namespace ScientificResearch.ViewModel
{
    public class ContinuingEducationRecordViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public Nullable<int> NworkToDoId { get; set; }

        [Required(ErrorMessage = "姓名不能为空")]
        [Display(Name = "姓名")]
        public string UserName { get; set; }

        [Display(Name = "科室")]
        public string Department { get; set; }

        public Nullable<double> AccountCredit { get; set; }

        [Required(ErrorMessage = "学分不能为空")]
        [Display(Name = "学分")]
        public double Credit { get; set; }

        [Required(ErrorMessage = "学分级别不能为空")]
        [Display(Name = "学分级别")]
        public string CreditLevel { get; set; }

        [Required(ErrorMessage = "学分类型不能为空")]
        [Display(Name = "学分类型")]
        public string CreditType { get; set; }
        public bool IsProjectCredit { get; set; }

        [Required(ErrorMessage = "状态不能为空")]
        [Display(Name = "状态")]
        public bool IsGranted { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedTime { get; set; }

         [Display(Name = "学历")]
        public string UserDegree { get; set; }

         [Display(Name = "职称")]
        public string UserDuty { get; set; }
    }
}
