using ScientificResearch.DataAccessImplement;
using ScientificResearch.DataTransferModel;
using ScientificResearch.IBusinessLogic;
using ScientificResearch.IDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScientificResearch.BusinessLogicImplement
{
    public class ScienceResearchOfCapitalStatisticServiceImplement:IScienceResearchOfCapitalStatisticService
    {
         private IScienceResearchOfCapitalStatisticRepository repository;
        public ScienceResearchOfCapitalStatisticServiceImplement()
            : this(new ScienceResearchOfCapitalStatisticRepository())
        { }
        public ScienceResearchOfCapitalStatisticServiceImplement(IScienceResearchOfCapitalStatisticRepository repository)
        {
            this.repository = repository;
        }
        public IList<ScienceResearchOfCapitalStatisticTransferObject> GetScienceResearchOfCapitalStatistic(string startTime, string endTime, 
            string projectName, string moduleName, int pageIndex, int pageSize, out int totalCount)
        {
            var result = repository.GetScienceResearchOfCapitalStatistic( startTime, endTime, 
            projectName, moduleName, pageIndex, pageSize, out totalCount);

            return result;
        }
        public IList<ScienceResearchOfCapitalStatisticTransferObject> GetAllScienceResearchOfCapitalStatistic(string startTime, string endTime,
            string projectName, string moduleName)
        {
            var result = repository.GetAllScienceResearchOfCapitalStatistic(startTime, endTime,
                projectName, moduleName);

            return result;
        }
    }
}
