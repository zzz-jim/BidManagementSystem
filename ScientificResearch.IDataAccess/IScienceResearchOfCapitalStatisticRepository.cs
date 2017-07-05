using ScientificResearch.DataTransferModel;
using System.Collections.Generic;

namespace ScientificResearch.IDataAccess
{
    public interface IScienceResearchOfCapitalStatisticRepository
    {
        /// <summary>
        /// 政府采购经费统计
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="projectName"></param>
        /// <param name="departmentName"></param>
        /// <param name="moduleName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IList<ScienceResearchOfCapitalStatisticTransferObject> GetScienceResearchOfCapitalStatistic(string startTime, string endTime,
            string projectName, string moduleName, int pageIndex, int pageSize, out int totalCount);
        /// <summary>
        /// 科研资金统计全部数据
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="projectName"></param>
        /// <param name="moduleName"></param>
        /// <returns></returns>
        IList<ScienceResearchOfCapitalStatisticTransferObject> GetAllScienceResearchOfCapitalStatistic(string startTime, string endTime,
            string projectName, string moduleName);
    }
}
