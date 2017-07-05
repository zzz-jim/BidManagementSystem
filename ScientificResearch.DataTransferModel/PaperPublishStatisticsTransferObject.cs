
namespace ScientificResearch.DataTransferModel
{
    public class PaperPublishStatisticsTransferObject
    {
        ///// <summary>
        ///// 项目年份
        ///// </summary>
        //public DateTime Time { get; set; }

        /// <summary>
        /// 项目年份
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 特I类期刊
        /// </summary>
        public int Superjournal { get; set; }

        /// <summary>
        /// I类期刊
        /// </summary>
        public int Onejournal { get; set; }

        /// <summary>
        /// II类期刊
        /// </summary>
        public int Twojournal { get; set; }

        /// <summary>
        /// III类期刊
        /// </summary>
        public int Threejournal { get; set; }

        /// <summary>
        /// 当年所有类型项目总数
        /// </summary>
        public int TotalCount { get; set; }
    }
}
