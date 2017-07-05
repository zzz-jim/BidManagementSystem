using System;
using System.Collections.Generic;

using ScientificResearch.DomainModel;
using ScientificResearch.DataTransferModel;

namespace ScientificResearch.IDataAccess
{
    public interface IERPNWorkToDoRepository : IRepository<ERPNWorkToDo>
    {
        IList<ERPNWorkToDo> GetPageEntities(Func<ERPNWorkToDo, bool> whereLambda, int pageSize, int pageIndex, out int totalPage);

        IList<ERPNWorkToDoTransferObject> GetPageEntities(Func<ERPNWorkToDo, bool> whereLambda, string sortField, int pageSize, int pageIndex, out int totalPage);

        IList<ERPNWorkToDoTransferObject> GetAllPageEntities(Func<ERPNWorkToDo, bool> whereLambda, string sortField);
    }
}
