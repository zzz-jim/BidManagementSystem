using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using ScientificResearch.Utility.Enums;

namespace UI.ScientificResearch.Models
{
    /// <summary>
    /// 押标金,投标押金,保证金
    /// </summary>
    public class BidBondInfoViewModels
    {
        [Required]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Display(Name = "项目Id")]
        public string ProjectId { get; set; }

        [Display(Name = "项目名称")]
        public string ProjectName { get; set; }

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
        [Display(Name = "付款人户名")]
        public string TendererAccountName { get; set; }

        [Required]
        [Display(Name = "付款人账号")]
        public string TendererAccount { get; set; }

        [Required]
        [Display(Name = "是否缴纳保证费")]
        public string IsSubmitBidBondFee { get; set; }

        [Required]
        [Display(Name = "保证金金额")]
        public long BidBondFee { get; set; }

        [Display(Name = "缴纳时间")]
        public DateTime RegisterTime { get; set; }

        [Display(Name = "创建时间")]
        public DateTime CreateTime { get; set; }

        [Required]
        [Display(Name = "备注说明")]
        public string Remark { get; set; }

        [Required]
        [Display(Name = "操作人")]
        public string OperatorName { get; set; }

        [Required]
        [Display(Name = "操作人ID")]
        public string OperatorId { get; set; }
    }
}