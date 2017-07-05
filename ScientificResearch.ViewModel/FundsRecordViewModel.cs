using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScientificResearch.ViewModel
{
    public class FundsRecordViewModel
    {
        public int FundsRecordID { get; set; }
        public int ApplicationId { get; set; }
        public int WorkflowId { get; set; }

        [Required(ErrorMessage = "事由名称不能为空")]
        [Display(Name = "事由")]
        public string Name { get; set; }
        [Display(Name = "类型")]
        public string Type { get; set; }
        [Display(Name = "经费描述")]
        public string Description { get; set; }

        [Required(ErrorMessage = "项目不能为空")]
        [Display(Name = "项目名称")]
        public string ProjectName { get; set; }

        [Required(ErrorMessage = "单据数不能为空")]
        [Display(Name = "单据数")]
        public int CountOfBill { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }

        [Required(ErrorMessage = "金额不能为空")]
        [Display(Name = "金额")]
        public double TotalPrice { get; set; }
        public Nullable<bool> IsIncome { get; set; }
        public bool IsPrint { get; set; }
        public Nullable<System.DateTime> LastPrintTime { get; set; }
        [Display(Name = "操作人")]
        public string UserName { get; set; }

        
        [Display(Name = "操作时间")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public Nullable<System.DateTime> TimeStr { get; set; }
         [Display(Name = "附件")]
        public string FuJianList { get; set; }
        [Display(Name = "备注信息")]
        public string ShenPiYiJian { get; set; }
        public Nullable<int> JieDianID { get; set; }

        [Display(Name = "状态")]
        public string JieDianName { get; set; }
        public string OKUserList { get; set; }
        public string ShenPiUserList { get; set; }
        [Display(Name = "状态")]
        public string StateNow { get; set; }
        public Nullable<System.DateTime> LateTime { get; set; }
        [Display(Name = "备注")]
        public string Comment { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedTime { get; set; }

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
        /// 模块名称
        /// </summary>
        [Display(Name = "模块名称")]
        public string ModuleName { get; set; }

        public IList<TravelFundsDetailViewModel> TravelFundsList { get; set; }
    }
}
