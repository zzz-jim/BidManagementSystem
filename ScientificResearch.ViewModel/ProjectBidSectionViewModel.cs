using System;
using System.ComponentModel.DataAnnotations;

namespace ScientificResearch.ViewModel
{
    public class ProjectBidSectionViewModel
    {
        public int ID { get; set; }
        public int ApplicationId { get; set; }
        [Display(Name = "标段编号")]
        public string SectionNumber { get; set; }
        [Display(Name = "标段名称")]
        public string SectionName { get; set; }
        [Display(Name = "项目名称")]
        public string ProjectName { get; set; }
        [Display(Name = "序号")]
        public int Number { get; set; }

        public Nullable<System.DateTime> CreatedTime { get; set; }
    }
}
