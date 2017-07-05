using ScientificResearch.DataAccessImplement;
using ScientificResearch.DataTransferModel;
using ScientificResearch.DomainModel;
using ScientificResearch.IBusinessLogic;
using ScientificResearch.IDataAccess;
using System.Collections.Generic;

using System;
using System.Linq;


namespace ScientificResearch.BusinessLogicImplement
{
    public class ERPNWorkFlowServiceImplement : IERPNWorkFlowService
    {
        private IERPNWorkFlowRepository repository;
        public ERPNWorkFlowServiceImplement()
            : this(new ERPNWorkFlowRepository())
        { }

        public ERPNWorkFlowServiceImplement(IERPNWorkFlowRepository repository)
        {
            this.repository = repository;
        }
        public int AddERPNWorkFlow(ERPNWorkFlowTransferObject model)
        {
            return repository.AddEntity(model.ToDomainModel());
        }

        public bool UpdateERPNWorkERPNWorkFlow(ERPNWorkFlowTransferObject model)
        {
            return repository.UpdateEntity(model.ToDomainModel());
        }

        public IList<ERPNWorkFlowTransferObject> GetEntities(Func<ERPNWorkFlow, bool> whereLambda)
        {

            var tempResult = repository.GetEntities(whereLambda);

            return tempResult.Select(x => x.ToDataTransferObjectModel()).ToList();
        }

        public bool DeleteEntityById(int id)
        {
            return repository.DeleteEntityById(id);
        }
        public ERPNWorkFlowTransferObject GetEntityById(int id)
        {
            return repository.GetEntityById(id).ToDataTransferObjectModel();
        }
    }
}

