using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScientificResearch.ViewModel
{
    public class ScienceProjectStatisticsViewModel
    {
        public ScienceProjectStatisticsViewModel()
        {
            //TeamMemberList = new List<string>();
        }

        /// <summary>
        /// 序号
        /// </summary>
        [Display(Name = "序号")]
        public int Number { get; set; }

        /// <summary>
        /// 项目年份
        /// </summary>
        [Display(Name = "项目年份")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Time { get; set; }

        /// <summary>
        /// 项目类型
        /// </summary>
       [Display(Name = "项目类型")]
        public string Type { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
       [Display(Name = "项目名称")]
        public string Name { get; set; }

        /// <summary>
        /// 立项类型
        /// </summary>
       [Display(Name = "立项类型")]
        public string EstablishType { get; set; }

        /// <summary>
        /// 科室
        /// </summary>
        [Display(Name = "科室")]
        public string Department { get; set; }
        /// <summary>
        /// 项目负责人
        /// </summary>
        [Display(Name = "项目负责人")]
        public string ProjectManager { get; set; }

        /// <summary>
        /// 项目参与人员
        /// </summary>
        [Display(Name = "项目参与人员")]
        public string TeamMemberList { get; set; }

        /// <summary>
        /// 下达经费
        /// </summary>
       [Display(Name = "下达经费")]
        public double ReleaseFunds { get; set; }

        /// <summary>
        /// 配套经费
        /// </summary>
       [Display(Name = "配套经费")]
        public double CounterpartFunds { get; set; }

        /// <summary>
        /// 费用总计经费
        /// </summary>
        [Display(Name = "费用总计")]
        public double TotalFunds { get; set; }

        /// <summary>
        /// 支出经费
        /// </summary>
       [Display(Name = "支出")]
        public double Payment { get; set; }

        /// <summary>
        /// 余额经费
        /// </summary>
       [Display(Name = "余额")]
        public double Balance { get; set; }
    }
}
