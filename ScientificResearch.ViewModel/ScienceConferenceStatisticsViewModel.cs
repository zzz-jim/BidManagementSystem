using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScientificResearch.ViewModel
{
    public class ScienceConferenceStatisticsViewModel
    {
        [Display(Name = "科室")]
        /// <summary>
        /// 科室
        /// </summary>
        public string Department { get; set; }

        [Display(Name = "学员")]
        /// <summary>
        /// 学员
        /// </summary>
        public int Students { get; set; }

        [Display(Name = "委员")]
        /// <summary>
        /// 委员
        /// </summary>
        public int Member { get; set; }

        [Display(Name = "其他")]
        /// <summary>
        /// 其他
        /// </summary>
        public int Others { get; set; }

        [Display(Name = "无")]
        /// <summary>
        /// 无
        /// </summary>
        public int No { get; set; }

        [Display(Name = "人数")]
        /// <summary>
        /// 人数
        /// </summary>
        public int Count { get; set; }

        [Display(Name = "经费")]
        /// <summary>
        /// 经费
        /// </summary>
        public double Funds { get; set; }
      
    }
}
