using System;
using System.Collections.Generic;

namespace ScientificResearch.DataTransferModel
{
    public class ScienceProjectStatisticsTransferObject
    {
        public ScienceProjectStatisticsTransferObject()
        {
            //TeamMemberList = new List<string>();
        }

        /// <summary>
        /// 序号
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 项目年份
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 项目类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 立项类型
        /// </summary>
        public string EstablishType { get; set; }

        /// <summary>
        /// 科室
        /// </summary>
        public string Department { get; set; }
        /// <summary>
        /// 项目负责人
        /// </summary>
        public string ProjectManager { get; set; }

        /// <summary>
        /// 项目参与人员
        /// </summary>
        public string TeamMemberList { get; set; }

        /// <summary>
        /// 下达经费
        /// </summary>
        public double ReleaseFunds { get; set; }

        /// <summary>
        /// 配套经费
        /// </summary>
        public double CounterpartFunds { get; set; }

        /// <summary>
        /// 费用总计经费
        /// </summary>
        public double TotalFunds { get; set; }

        /// <summary>
        /// 支出经费
        /// </summary>
        public double Payment { get; set; }

        /// <summary>
        /// 余额经费
        /// </summary>
        public double Balance { get; set; }

        /// <summary>
        /// FormId
        /// </summary>
        public int FormId { get; set; }
    }
}
