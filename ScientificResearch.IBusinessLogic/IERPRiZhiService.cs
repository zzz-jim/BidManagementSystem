using ScientificResearch.DataTransferModel;
using ScientificResearch.DomainModel;
using System;
using System.Collections.Generic;

namespace ScientificResearch.IBusinessLogic
{
    public interface IERPRiZhiService
    {
        bool DeleteEntityById(int id);
        int AddERPRiZhi(ERPRiZhiTransferObject model);
        bool UpdateERPRiZhi(ERPRiZhiTransferObject model);
        ERPRiZhiTransferObject GetEntityById(int id);
        IList<ERPRiZhiTransferObject> GetEntities(Func<ERPRiZhi, bool> whereLambda);
    }

}
