using System;
using System.Collections;

namespace ScientificResearch.DataTransferModel
{
    public class ScienceConferenceStatisticsTransferObject
    {
        /// <summary>
        /// 科室
        /// </summary>
        public string Department { get; set; }
        /// <summary>
        /// 学员
        /// </summary>
        public int Students { get; set; }
        /// <summary>
        /// 委员
        /// </summary>
        public int Member { get; set; }
        /// <summary>
        /// 其他
        /// </summary>
        public int Others { get; set; }
        /// <summary>
        /// 无
        /// </summary>
        public int No { get; set; }
        /// <summary>
        /// 人数
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 经费
        /// </summary>
        public double Funds { get; set; }

    }
}
