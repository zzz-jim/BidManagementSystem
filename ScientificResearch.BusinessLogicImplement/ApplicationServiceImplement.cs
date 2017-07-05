using System;
using System.Collections.Generic;
using System.Linq;

using ScientificResearch.DataAccessImplement;
using ScientificResearch.DataTransferModel;
using ScientificResearch.DomainModel;
using ScientificResearch.IBusinessLogic;
using ScientificResearch.IDataAccess;

namespace ScientificResearch.BusinessLogicImplement
{
    public class ApplicationServiceImplement : IApplicationService
    {
        private IERPNWorkToDoRepository repository;

        public ApplicationServiceImplement()
            // Calls the constructor that takes 1 argument
            : this(new ERPNWorkToDoRepository())
        { }

        public ApplicationServiceImplement(IERPNWorkToDoRepository repository)
        {
            this.repository = repository;
        }

        public int AddApplication(ERPNWorkToDoTransferObject model)
        {
            return repository.AddEntity(model.ToDomainModel());
        }

        public bool UpdateApplication(ERPNWorkToDoTransferObject model)
        {
            return repository.UpdateEntity(model.ToDomainModel());
        }

        public ERPNWorkToDoTransferObject GetEntityById(int id)
        {
            return repository.GetEntityById(id).ToDataTransferObjectModel();
        }

        public bool DeleteEntityById(int id)
        {
            return repository.DeleteEntityById(id);
        }
        public IList<ERPNWorkToDoTransferObject> GetAllEntities()
        {
            var tempResult = repository.GetAllEntities();

            return tempResult.Select(x => x.ToDataTransferObjectModel()).ToList();
        }

        public IList<ERPNWorkToDoTransferObject> GetEntities(Func<ERPNWorkToDo, bool> whereLambda)
        {
            var tempResult = repository.GetEntities(whereLambda);

            return tempResult.Select(x => x.ToDataTransferObjectModel()).ToList();
        }

        public IList<ERPNWorkToDoTransferObject> GetPageEntities(Func<ERPNWorkToDo, bool> whereLambda, int pageSize, int pageIndex, out int totalPage)
        {
            return repository.GetPageEntities(whereLambda, pageSize, pageIndex, out totalPage).Select(x => x.ToDataTransferObjectModel()).ToList();
        }

        public IList<ERPNWorkToDoTransferObject> GetPageEntities(Func<ERPNWorkToDo, bool> whereLambda, string sortField, int pageSize, int pageIndex, out int totalPage)
        {
            return repository.GetPageEntities(whereLambda, sortField, pageSize, pageIndex, out totalPage).ToList();
        }
        public IList<ERPNWorkToDoTransferObject> GetAllPageEntities(Func<ERPNWorkToDo, bool> whereLambda, string sortField)
        {
            return repository.GetAllPageEntities(whereLambda, sortField);
        }
        
    }
}
