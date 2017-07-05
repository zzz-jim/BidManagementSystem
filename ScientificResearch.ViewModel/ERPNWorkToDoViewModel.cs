using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScientificResearch.ViewModel
{
    public class ERPNWorkToDoViewModel
    {
       

        public int NWorkToDoID { get; set; }
        public string WorkName { get; set; }

        [Required(ErrorMessage = "项目名称不能为空")]
        [Display(Name = "项目名称")]
        public string WenHao { get; set; }
        public Nullable<int> FormID { get; set; }
        public Nullable<int> WorkFlowID { get; set; }

        [Display(Name = "申请人")]
        public string UserName { get; set; }

        [Display(Name = "时间")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public Nullable<DateTime> TimeStr { get; set; }
        [Display(Name = "申请单")]
        public string FormContent { get; set; }
        [Display(Name = "附件")]
        public string FuJianList { get; set; }
        [Display(Name = "备注信息：")]
        public string ShenPiYiJian { get; set; }
        public Nullable<int> JieDianID { get; set; }
        public string JieDianName { get; set; }
        [Display(Name = "审批人")]
        public string ShenPiUserList { get; set; }
        public string OKUserList { get; set; }
        [Display(Name = "流程状态")]
        public string StateNow { get; set; }
        public Nullable<DateTime> LateTime { get; set; }
        [Display(Name = "项目编号")]
        public string BeiYong1 { get; set; }
        public string BeiYong2 { get; set; }

        [Display(Name = "申请书状态")]
        public string ApplicationStatus { get; set; }

        [Display(Name = "申请书Id")]
        public int ApplicationId { get; set; }
        public string FormKeys { get; set; }
        public string FormValues { get; set; }

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
        /// 项目进行状态
        /// </summary>
        [Display(Name = "项目状态")]
        public string ProjectStatus { get; set; }

        [Display(Name = "项目确立时间")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public Nullable<DateTime> ProjectEstablishTime { get; set; }

      //  public IList<FundsThresholdViewModel> FundsLimitsList { get; set; }
    }
}
