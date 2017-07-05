using ScientificResearch.DataTransferModel;
using System;
using System.Collections.Generic;

namespace ScientificResearch.IDataAccess
{
    public interface IScienceResearchAmountOfApplicationStatisticRepository
    {
        IList<ScienceResearchAmountOfApplicationStatisticTransferObject> GetScienceResearchAmountOfApplicationStatistics(DateTime startTime, DateTime endTime);
    }
}
