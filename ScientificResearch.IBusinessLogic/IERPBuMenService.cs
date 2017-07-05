
using System;
using System.Collections.Generic;

using ScientificResearch.DomainModel;
using ScientificResearch.DataTransferModel;


namespace ScientificResearch.IBusinessLogic
{
    public interface IERPBuMenService
    {
        bool DeleteEntityById(int id);
        ERPBuMenTransferObject GetEntityById(int id);
        IList<ERPBuMenTransferObject> GetEntities(Func<ERPBuMen, bool> whereLambda);
    }
}
