using System;

namespace ScientificResearch.DataTransferModel
{
    public class FundsManageProgramStatisticsTransferObject
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 经费类型
        /// </summary>
        public string FundsType { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        public string Programtype { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        public string ApplyMan { get; set; }
        /// <summary>
        /// 所在科室
        /// </summary>
        public string LocalDepartment { get; set; }
        /// <summary>
        /// 上级拨款
        /// </summary>
        public double SuperiorFunds { get; set; }
        /// <summary>
        /// 院内拨款
        /// </summary>
        public double HospitalFunds { get; set; }
        /// <summary>
        /// 项目总金额
        /// </summary>
        public double ProjectTotalFunds { get; set; }
        /// <summary>
        /// 判断收支
        /// </summary>
        public Nullable<bool> IsIncome { get; set; }
        /// <summary>
        /// 收入
        /// </summary>
        public double Income { get; set; }
        /// <summary>
        /// 支出
        /// </summary>
        public double Expend { get; set; }
        /// <summary>
        /// 余额
        /// </summary>
        public double Balance { get; set; }
        /// <summary>
        /// 确立时间
        /// </summary>
        public DateTime EstablishTiem { get; set; }
        /// <summary>
        /// 项目进行时间
        /// </summary>
        public DateTime ProjecProcessTime { get; set; }

    }
}
