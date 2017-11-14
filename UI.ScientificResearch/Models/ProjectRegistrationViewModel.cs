using System;
using System.ComponentModel.DataAnnotations;

namespace UI.ScientificResearch.Models
{
    public class ProjectRegistrationViewModel
    {
        public int ID { get; set; }
        [Display(Name = "项目Id")]
        public int ApplicationId { get; set; }
        [Required]
        [Display(Name = "公司名称")]
        public string CompanyName { get; set; }
        [Required]
        [Display(Name = "联系人")]
        public string ContactName { get; set; }
        [Required]
        [Display(Name = "联系电话")]
        public string Phone { get; set; }
        [Required]
        [Display(Name = "邮箱地址")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "标段")]
        public string BidSection { get; set; }
        [Required]
        [Display(Name = "标段Id")]
        public int BidSectionId { get; set; }
        [Required]
        [Display(Name = "是否交报名费")]
        public bool IsSubmitRegistrationFee { get; set; }
        [Required]
        [Display(Name = "报名费金额")]
        public long RegistrationFee { get; set; }

        [Display(Name = "是否交保证金")]
        public bool IsSubmitBidBondFee { get; set; }
        [Display(Name = "保证金")]
        public long BidBondFee { get; set; }
        [Display(Name = "备注说明")]
        public string Remark { get; set; }
        [Display(Name = "报名时间")]
        public Nullable<DateTime> RegisterTime { get; set; }

        [Display(Name = "是否退保证金")]
        public bool IsRefundBidBondFee { get; set; }

        [Required]
        [Display(Name = "操作人")]
        public string OperatorName { get; set; }
        [Required]
        [Display(Name = "操作人ID")]
        public string OperatorId { get; set; }

        [Display(Name = "创建时间")]
        public Nullable<DateTime> CreatedTime { get; set; }

        [Display(Name = "名次")]
        public Nullable<int> Rank { get; set; }

        [Display(Name = "名次")]
        public string RankDescription { get; set; }

        [Display(Name = "综合评标等分")]
        public Nullable<decimal> Score { get; set; }

        [Display(Name = "投标报价")]
        public Nullable<long> TenderOffer { get; set; }

        [Display(Name = "是否上榜")]
        public bool IsShow { get; set; }

        public int Number { get; set; }
    }
}