using ScientificResearch.DataTransferModel;
using System;
using System.Collections.Generic;

namespace ScientificResearch.IBusinessLogic
{
    public interface IScienceResearchAmountOfApplicationStatisticService
    {
        IList<ScienceResearchAmountOfApplicationStatisticTransferObject> GetScienceResearchAmountOfApplicationStatistics(DateTime startTime, DateTime endTime);
    }
}
