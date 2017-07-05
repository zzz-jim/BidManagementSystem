using ScientificResearch.DataAccessImplement;
using ScientificResearch.DataTransferModel;
using ScientificResearch.IBusinessLogic;
using ScientificResearch.IDataAccess;
using System;
using System.Collections.Generic;

namespace ScientificResearch.BusinessLogicImplement
{
    public class FundsManageProgramStatisticsImplement : IFundsManageProgramStatisticsService
    {
        private IFundsManageProgramStatisticsRepository repository;

        public FundsManageProgramStatisticsImplement()
            // Calls the constructor that takes 1 argument
            : this(new FundsManageProgramStatisticsRepository())
        { }

        public FundsManageProgramStatisticsImplement(IFundsManageProgramStatisticsRepository repository)
        {
            this.repository = repository;
        }

        public IList<FundsManageProgramStatisticsTransferObject> GetFundsManageProgramStatistics(Func<FundsManageProgramStatisticsTransferObject, bool> whereLambda, int formid)
        {
            var result = repository.GetFundsManageProgramStatistics(whereLambda, formid);

            return result;
        }
    }
}
