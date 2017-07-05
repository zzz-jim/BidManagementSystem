using ScientificResearch.DataTransferModel;
using ScientificResearch.DomainModel;
using System;
using System.Collections.Generic;

namespace ScientificResearch.IBusinessLogic
{
    public interface ITravelFundsDetailService
    {
        bool DeleteEntityById(int id);
        int AddERPRiZhi(TravelFundsDetailTransferObject model);
        bool UpdateERPRiZhi(TravelFundsDetailTransferObject model);
        TravelFundsDetailTransferObject GetEntityById(int id);
        IList<TravelFundsDetailTransferObject> GetEntities(Func<TravelFundsDetail, bool> whereLambda);
    }

}
