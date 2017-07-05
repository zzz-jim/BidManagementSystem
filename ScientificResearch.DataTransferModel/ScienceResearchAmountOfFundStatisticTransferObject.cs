using System;

namespace ScientificResearch.DataTransferModel
{
    public class ScienceResearchAmountOfFundStatisticTransferObject
    {
        /// <summary>
        /// 模块
        /// </summary>
        public string Modeule { get; set; }
        /// <summary>
        /// 学科
        /// </summary>
        public string DepartmentName { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 申请人名字
        /// </summary>
        public string ApplicationMan { get; set; }
        /// <summary>
        /// 经费名称
        /// </summary>
        public string RemburseName { get; set; }
        /// <summary>
        /// 经费类型
        /// </summary>
        public string OutOrIn { get; set; }
        /// <summary>
        /// 经费金额
        /// </summary>
        public double Money { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
