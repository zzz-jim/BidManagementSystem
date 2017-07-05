using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ScientificResearch.ViewModel
{
    public class ProjectBonusCreditViewModel
    {
        public int Id { get; set; }

        [DisplayName("模块名称")]
        public string ModuleName { get; set; }
        [Required(ErrorMessage = "项目类型不能为空")]
        [DisplayName("项目类型")]
        public string ProjectType { get; set; }

        [DisplayName("立项类型")]
        public string ProjectLevel { get; set; }

        [DisplayName("奖励分数")]
        public double Credit { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
    }
}
