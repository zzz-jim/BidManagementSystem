using ScientificResearch.DataTransferModel;
using ScientificResearch.DomainModel;
using System;
using System.Collections.Generic;

namespace ScientificResearch.IBusinessLogic
{
    public interface IERPNFormService
    {
        ERPNFormTransferObject GetEntityById(int id);
        int AddERPNForm(ERPNFormTransferObject model);
        bool UpdateERPNForm(ERPNFormTransferObject model);
        bool DeleteEntityById(int id);
        IList<ERPNFormTransferObject> GetEntities(Func<ERPNForm, bool> whereLambda);
    }
}
