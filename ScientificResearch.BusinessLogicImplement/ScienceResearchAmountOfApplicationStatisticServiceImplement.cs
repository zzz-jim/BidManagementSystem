using ScientificResearch.DataAccessImplement;
using ScientificResearch.DataTransferModel;
using ScientificResearch.IBusinessLogic;
using ScientificResearch.IDataAccess;
using System;
using System.Collections.Generic;

namespace ScientificResearch.BusinessLogicImplement
{
    public class ScienceResearchAmountOfApplicationStatisticServiceImplement : IScienceResearchAmountOfApplicationStatisticService
    {
        private IScienceResearchAmountOfApplicationStatisticRepository repository;
        public ScienceResearchAmountOfApplicationStatisticServiceImplement()
            : this(new ScienceResearchAmountOfApplicationStatisticRepository())
        { }
        public ScienceResearchAmountOfApplicationStatisticServiceImplement(IScienceResearchAmountOfApplicationStatisticRepository repository)
        {
            this.repository = repository;
        }
        public IList<ScienceResearchAmountOfApplicationStatisticTransferObject> GetScienceResearchAmountOfApplicationStatistics(DateTime startTime, DateTime endTime)
        {
            var result = repository.GetScienceResearchAmountOfApplicationStatistics(startTime, endTime);
            return result;

        }
    }
}
