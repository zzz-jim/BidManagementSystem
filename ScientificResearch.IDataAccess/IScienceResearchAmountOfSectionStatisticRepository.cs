using ScientificResearch.DataTransferModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScientificResearch.IDataAccess
{
    public interface IScienceResearchAmountOfSectionStatisticRepository
    {
        /// <summary>
        /// 科室科研数量统计
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        IList<ScienceResearchAmountOfSectionStatisticTransferObject> GetScienceResearchAmountOfSectionStatistics(DateTime startTime, DateTime endTime);
    }
}
