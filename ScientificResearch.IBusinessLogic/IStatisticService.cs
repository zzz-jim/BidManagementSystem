using System;
using System.Collections.Generic;

using ScientificResearch.DataTransferModel;

namespace ScientificResearch.IBusinessLogic
{
    public interface IStatisticService
    {
        /// <summary>
        /// 获取政府采购统计分析结果
        /// </summary>
        /// <param name="whereLambda">申请书满足的条件</param>
        /// <returns></returns>
        IList<ScienceProjectStatisticsTransferObject> GetScienceProjectStatistics(Func<ScienceProjectStatisticsTransferObject, bool> whereLambda, int formId);

        /// <summary>
        /// 科研立项时间按年份统计
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        IList<ScienceProjectEstablishTimeStatisticsTransferObject> GetScienceProjectEstablishTimeStatistics(Func<ScienceProjectEstablishTimeStatisticsTransferObject, bool> whereLambda);
        /// <summary>
        /// 论文管理
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        IList<PaperPublishStatisticsTransferObject> GetPaperPublishStatistics(Func<PaperPublishStatisticsTransferObject, bool> whereLambda);

        /// <summary>
        /// 获取学术会议统计分析结果
        /// </summary>
        /// <param name="whereLambda">申请书满足的条件</param>
        /// <returns></returns>
       IList<ScienceConferenceStatisticsTransferObject> GetScienceConferenceStatistics(Func<ScienceConferenceStatisticsTransferObject, bool> whereLambda, int formId);
       /// IList<ScienceConferenceStatisticsTransferObject> GetScienceConferenceStatistics( int formId);

    }
}
