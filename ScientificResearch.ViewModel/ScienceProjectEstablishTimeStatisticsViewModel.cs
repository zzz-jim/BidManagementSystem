using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScientificResearch.ViewModel
{
    public class ScienceProjectEstablishTimeStatisticsViewModel
    {
        public ScienceProjectEstablishTimeStatisticsViewModel()
        {
            //TeamMemberList = new List<string>();
        }

        ///// <summary>
        ///// 项目年份
        ///// </summary>
        //public DateTime Time { get; set; }

        /// <summary>
        /// 项目年份
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 立项类型(国家级)
        /// </summary>
        public int CountryLevel { get; set; }

        /// <summary>
        /// 立项类型(省级)
        /// </summary>
        public int ProvinceLevel { get; set; }

        /// <summary>
        /// 立项类型(市级)
        /// </summary>
        public int CityLevel { get; set; }

        /// <summary>
        /// 立项类型(局级)
        /// </summary>
        public int CountyLevel { get; set; }

        /// <summary>
        /// 立项类型(院级)
        /// </summary>
        public int HospitalLevel { get; set; }

        /// <summary>
        /// 当年所有类型项目总数
        /// </summary>
        public int TotalCount { get; set; }
    }
}
