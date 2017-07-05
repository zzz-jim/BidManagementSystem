
using System;
using System.Collections.Generic;

using ScientificResearch.DataTransferModel;
using ScientificResearch.DomainModel;

namespace ScientificResearch.IBusinessLogic
{
    public interface IERPNWorkFlowService
    {
        bool DeleteEntityById(int id);
        ERPNWorkFlowTransferObject GetEntityById(int id);
        IList<ERPNWorkFlowTransferObject> GetEntities(Func<ERPNWorkFlow, bool> whereLambda);
    }
}
