using System;
using System.ComponentModel.DataAnnotations;

namespace UI.ScientificResearch.Models
{
    /// <summary>
    /// 中标通知
    /// </summary>
    public class BidWinnerNoticeViewModel
    {
        public int ID { get; set; }

        [Display(Name = "项目名称")]
        public string ProjectName { get; set; }
        [Display(Name = "项目编号")]
        public string ProjectNumber { get; set; }
        [Required]
        [Display(Name = "标段名称")]
        public string SectionName { get; set; }
        [Required]
        [Display(Name = "标段编号")]
        public string SectionNumber { get; set; }
        [Required]
        [Display(Name = "标段ID")]
        public int BidSectionId { get; set; }

        [Required]
        [Display(Name = "项目ID")]
        public int ApplicationId { get; set; }

        /// <summary>
        ///  第1名
        /// </summary>
        public int CompanyId1 { get; set; }
        [Required]
        [Display(Name = "公司名称")]
        public string CompanyName1 { get; set; }

        [Display(Name = "综合评标等分")]
        public decimal Score1 { get; set; }

        [Display(Name = "投标报价")]
        public long TenderOffer1 { get; set; }


        /// <summary>
        ///  第2名
        /// </summary>
        public int CompanyId2 { get; set; }
        [Required]
        [Display(Name = "公司名称")]
        public string CompanyName2 { get; set; }

        [Display(Name = "综合评标等分")]
        public decimal Score2 { get; set; }

        [Display(Name = "投标报价")]
        public long TenderOffer2 { get; set; }


        /// <summary>
        ///  第3名
        /// </summary>
        public int CompanyId3 { get; set; }
        [Required]
        [Display(Name = "公司名称")]
        public string CompanyName3 { get; set; }

        [Display(Name = "综合评标等分")]
        public decimal Score3 { get; set; }

        [Display(Name = "投标报价")]
        public long TenderOffer3 { get; set; }
    }
}