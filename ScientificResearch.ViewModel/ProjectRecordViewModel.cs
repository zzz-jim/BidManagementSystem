using System;
using System.ComponentModel.DataAnnotations;

namespace ScientificResearch.ViewModel
{
    public class ProjectRecordViewModel
    {
        public ERPNWorkToDoViewModel Application { get; set; }
        public int PojectEstablishID { get; set; }
        public int ApplicationId { get; set; }
        public int WorkflowId { get; set; }

        [Required(ErrorMessage = "此处不能为空")]
        [Display(Name = "上级拨款")]
        public double SuperiorFunds { get; set; }

        [Required(ErrorMessage = "此处不能为空")]
        [Display(Name = "院内拨款")]
        public double HospitalFunds { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Required(ErrorMessage = "时间不能为空")]
        [Display(Name = "拨款时间")]
        public System.DateTime FundsTime { get; set; }

        [Display(Name = "项目开始时间")]
        public Nullable<System.DateTime> StartTime { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Required(ErrorMessage = "时间不能为空")]
        [Display(Name = "项目结束时间")]
        public System.DateTime EndTime { get; set; }

        [Display(Name = "总金额")]
        public double Total { get; set; }
        public string CreatedBy { get; set; }

        
        [Display(Name = "项目确立时间")]
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedTime { get; set; }

        [Display(Name = "上级拨款时间")]
        public Nullable<System.DateTime> SuperiorFundTime { get; set; }
        /// <summary>
        /// 项目立项类型级别(国家级，省级，市级，县级，院级)
        /// </summary>
        public string ProjectLevel { get; set; }

        /// <summary>
        /// 是否为驳回申请
        /// </summary>
        [Display(Name = "驳回")]
        public Nullable<bool> IsRejected { get; set; }

        /// <summary>
        /// 是否冻结
        /// </summary>
        [Display(Name = "冻结")]
        public Nullable<bool> IsLocked { get; set; }

        /// <summary>
        /// 是否为暂存数据
        /// </summary>
        [Display(Name = "暂存")]
        public Nullable<bool> IsTemporary { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [Display(Name = "删除")]
        public Nullable<bool> IsDeleted { get; set; }

        /// <summary>
        /// 附件列表
        /// </summary>
        [Display(Name = "附件")]
        public string FuJianList { get; set; }
    }
}
