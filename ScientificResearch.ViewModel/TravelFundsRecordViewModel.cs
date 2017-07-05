using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScientificResearch.ViewModel
{
    public class TravelFundsRecordViewModel
    {
        public TravelFundsRecordViewModel()
        {
            TravelFundsList = new List<TravelFundsDetailViewModel>();
        }

        public int FundsRecordID { get; set; }
        public int ApplicationId { get; set; }
        public int WorkflowId { get; set; }

        [Required(ErrorMessage = "报销单不能为空")]
        [Display(Name = "费用单报销")]
        public string Name { get; set; }
        [Display(Name = "类型")]
        public string Type { get; set; }
        [Display(Name = "差旅描述")]
        public string Description { get; set; }
        [Display(Name = "项目")]

        public string ProjectName { get; set; }
        [Required(ErrorMessage = "单据数不能为空")]
        [Display(Name = "单据数")]
        public int CountOfBill { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        [Display(Name = "总金额")]
        public double TotalPrice { get; set; }
        public Nullable<bool> IsIncome { get; set; }
        public bool IsPrint { get; set; }
        public Nullable<System.DateTime> LastPrintTime { get; set; }
        public string UserName { get; set; }
        public Nullable<System.DateTime> TimeStr { get; set; }
        public string FuJianList { get; set; }
        public string ShenPiYiJian { get; set; }
        public Nullable<int> JieDianID { get; set; }
        public string JieDianName { get; set; }
        public string OKUserList { get; set; }
        public string ShenPiUserList { get; set; }
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
