using ScientificResearch.DataTransferModel;
using System.Collections.Generic;

namespace ScientificResearch.IBusinessLogic
{
    public interface IScienceResearchAmountOfFundStatisticService
    {
        /// <summary>
        /// 政府采购经费统计分页后的数据
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="projectName"></param>
        /// <param name="departmentName"></param>
        /// <param name="moduleName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IList<ScienceResearchAmountOfFundStatisticTransferObject> GetScienceResearchAmountOfFundStatistic(string startTime, string endTime, string projectName,
           string departmentName, string reiburseName, string moduleName, int pageIndex, int pageSize, out int totalcount);
        /// <summary>
        /// 政府采购经费统计全部数据
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="projectName"></param>
        /// <param name="departmentName"></param>
        /// <param name="reiburseName"></param>
        /// <param name="moduleName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalcount"></param>
        /// <returns></returns>
        IList<ScienceResearchAmountOfFundStatisticTransferObject> GetAllScienceResearchAmountOfFundStatistic(string startTime, string endTime, string projectName,
          string departmentName, string reiburseName, string moduleName);
    }
}
