using ScientificResearch.DataAccessImplement;
using ScientificResearch.DataTransferModel;
using ScientificResearch.DomainModel;
using ScientificResearch.IBusinessLogic;
using ScientificResearch.IDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScientificResearch.BusinessLogicImplement
{
    public class StatisticServiceImplement : IStatisticService
    {
        private IStatisticRepository repository;

        public StatisticServiceImplement()
            // Calls the constructor that takes 1 argument
            : this(new StatisticRepository())
        { }

        public StatisticServiceImplement(IStatisticRepository repository)
        {
            this.repository = repository;
        }

        public IList<ScienceProjectStatisticsTransferObject> GetScienceProjectStatistics(Func<ScienceProjectStatisticsTransferObject, bool> whereLambda, int formId)
        {
            var result = repository.GetScienceProjectStatistics(whereLambda, formId);

            return result;
        }

        public IList<ScienceProjectEstablishTimeStatisticsTransferObject> GetScienceProjectEstablishTimeStatistics(Func<ScienceProjectEstablishTimeStatisticsTransferObject, bool> whereLambda)
        {
            var result = repository.GetScienceProjectEstablishTimeStatistics(whereLambda);

            return result;
        }

        public IList<PaperPublishStatisticsTransferObject> GetPaperPublishStatistics(Func<PaperPublishStatisticsTransferObject, bool> whereLambda)
        {
            var result = repository.GetPaperPublishStatistics(whereLambda);

            return result;
        }


        public IList<ScienceConferenceStatisticsTransferObject> GetScienceConferenceStatistics(Func<ScienceConferenceStatisticsTransferObject, bool> whereLambda, int formId)
        {
            var result = repository.GetScienceConferenceStatistics(whereLambda, formId);

            return result;
        }
    }
}
