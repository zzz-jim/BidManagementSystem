using System.Collections.Generic;

using ScientificResearch.DataAccessImplement;
using ScientificResearch.DataTransferModel;
using ScientificResearch.IBusinessLogic;
using ScientificResearch.IDataAccess;

namespace ScientificResearch.BusinessLogicImplement
{
    public class ScienceResearchAmountOfFundStatisticServiceImplement : IScienceResearchAmountOfFundStatisticService
    {
        private IScienceResearchAmountOfFundStatisticRepository repository;
        public ScienceResearchAmountOfFundStatisticServiceImplement()
            : this(new ScienceResearchAmountOfFundStatisticRepository())
        { }
        public ScienceResearchAmountOfFundStatisticServiceImplement(IScienceResearchAmountOfFundStatisticRepository repository)
        {
            this.repository = repository;
        }
        public IList<ScienceResearchAmountOfFundStatisticTransferObject> GetScienceResearchAmountOfFundStatistic(string startTime, string endTime, string projectName,
            string departmentName, string reiburseName, string moduleName, int pageIndex, int pageSize, out int totalcount)
        {
            var result = repository.GetScienceResearchAmountOfFundStatistic(startTime, endTime, projectName,
             departmentName, reiburseName, moduleName, pageIndex, pageSize, out totalcount);

            return result;
        }
        public IList<ScienceResearchAmountOfFundStatisticTransferObject> GetAllScienceResearchAmountOfFundStatistic(string startTime, string endTime, string projectName,
            string departmentName, string reiburseName, string moduleName)
        {
            var result = repository.GetAllScienceResearchAmountOfFundStatistic(startTime, endTime, projectName,
            departmentName, reiburseName, moduleName);

            return result;
        }
    }
}
