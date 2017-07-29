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

        private IProjectBidSectionRepository sectionRepository;
        public ApplicationServiceImplement()
            // Calls the constructor that takes 1 argument
            : this(new ERPNWorkToDoRepository(), new ProjectBidSectionRepository())
        { }

        public ApplicationServiceImplement(IERPNWorkToDoRepository repository, IProjectBidSectionRepository sectionRepository)
        {
            this.repository = repository;
            this.sectionRepository = sectionRepository;
        }

        public int AddApplication(ERPNWorkToDoTransferObject model)
        {
            return repository.AddEntity(model.ToDomainModel());
        }

        public int AddApplication(ERPNWorkToDoTransferObject model, List<ProjectBidSection> sectionList)
        {
            var tempModel = model.ToDomainModel();
            tempModel.ProjectBidSection = sectionList;
            return repository.AddEntity(tempModel);
        }

        public bool UpdateApplication(ERPNWorkToDoTransferObject model)
        {
            return repository.UpdateEntity(model.ToDomainModel());
        }

        public bool UpdateApplication(ERPNWorkToDoTransferObject model, List<ProjectBidSection> sectionList)
        {
            var tempModel = model.ToDomainModel();
            tempModel.ProjectBidSection = sectionList;
            return repository.UpdateEntity(tempModel);
        }

        public ERPNWorkToDoTransferObject GetEntityById(int id)
        {
            var result = repository.GetEntityById(id).ToDataTransferObjectModel();

            result.ProjectBidSection = sectionRepository.GetEntities(x => x.ApplicationId==id).ToList();

            return result;
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
