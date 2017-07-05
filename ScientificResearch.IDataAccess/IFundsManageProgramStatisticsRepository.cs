using ScientificResearch.DataTransferModel;
using System;
using System.Collections.Generic;

namespace ScientificResearch.IDataAccess
{
    public interface IFundsManageProgramStatisticsRepository
    {
        /// <summary>
        /// 获取资金管理项目统计分析结果
        /// </summary>
        /// <param name="whereLambda">申请书满足的条件</param>
        /// <returns></returns>
        IList<FundsManageProgramStatisticsTransferObject> GetFundsManageProgramStatistics(Func<FundsManageProgramStatisticsTransferObject, bool> whereLambda, int formid);
    }
}
