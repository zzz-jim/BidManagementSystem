using System;
using System.Collections.Generic;

using ScientificResearch.DataTransferModel;
using ScientificResearch.DomainModel;

namespace ScientificResearch.IBusinessLogic
{
    public interface IApplicationService
    {
        int AddApplication(ERPNWorkToDoTransferObject model);
        int AddApplication(ERPNWorkToDoTransferObject model, List<ProjectBidSection> sectionList);
        bool UpdateApplication(ERPNWorkToDoTransferObject model);
        bool DeleteEntityById(int id);
        ERPNWorkToDoTransferObject GetEntityById(int id);
        IList<ERPNWorkToDoTransferObject> GetAllEntities();
        IList<ERPNWorkToDoTransferObject> GetEntities(Func<ERPNWorkToDo, bool> whereLambda);
        IList<ERPNWorkToDoTransferObject> GetPageEntities(Func<ERPNWorkToDo, bool> whereLambda, int pageSize, int pageIndex, out int totalPage);
        IList<ERPNWorkToDoTransferObject> GetPageEntities(Func<ERPNWorkToDo, bool> whereLambda, string sortField, int pageSize, int pageIndex, out int totalPage);
        IList<ERPNWorkToDoTransferObject> GetAllPageEntities(Func<ERPNWorkToDo, bool> whereLambda, string sortField);
    }
}
