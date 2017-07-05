
using System;
using System.Collections.Generic;

using ScientificResearch.DataTransferModel;
using ScientificResearch.DomainModel;

namespace ScientificResearch.IBusinessLogic
{
    public interface IERPNWorkFlowNodeService
    {
        ERPNWorkFlowNodeTransferObject GetEntityById(int id);
        IList<ERPNWorkFlowNodeTransferObject> GetEntities(Func<ERPNWorkFlowNode, bool> whereLambda);
        bool DeleteEntityById(int id);
        int AddERPNWorkFlowNode(ERPNWorkFlowNodeTransferObject entity);
        bool UpdateERPNWorkFlowNode(ERPNWorkFlowNodeTransferObject model);
    }
}
