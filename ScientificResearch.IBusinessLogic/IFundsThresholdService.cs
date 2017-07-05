using ScientificResearch.DataTransferModel;
using ScientificResearch.DomainModel;
using System;
using System.Collections.Generic;

namespace ScientificResearch.IBusinessLogic
{
    public interface IFundsThresholdService
    {
        bool DeleteEntityById(int id);
        int AddFundsThreshold(FundsThresholdTransferObjectTransferObject model);
        bool UpdateFundsThreshold(FundsThresholdTransferObjectTransferObject model);
        FundsThresholdTransferObjectTransferObject GetEntityById(int id);
        IList<FundsThresholdTransferObjectTransferObject> GetEntities(Func<FundsThreshold, bool> whereLambda);
    }

}
