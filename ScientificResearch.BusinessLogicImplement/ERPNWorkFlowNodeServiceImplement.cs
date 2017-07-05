using ScientificResearch.DataAccessImplement;
using ScientificResearch.DataTransferModel;
using ScientificResearch.IBusinessLogic;
using ScientificResearch.IDataAccess;
using ScientificResearch.DomainModel;
using System.Collections.Generic;

using System;
using System.Linq;


namespace ScientificResearch.BusinessLogicImplement
{
    public class ERPNWorkFlowNodeServiceImplement : IERPNWorkFlowNodeService
    {
        private IERPNWorkFlowNodeRepository repository;
        public ERPNWorkFlowNodeServiceImplement()
            : this(new ERPNWorkFlowNodeRepository())
        { }

        public ERPNWorkFlowNodeServiceImplement(IERPNWorkFlowNodeRepository repository)
        {
            this.repository = repository;
        }
        public int AddERPNWorkFlowNode(ERPNWorkFlowNodeTransferObject model)
        {
            return repository.AddEntity(model.ToDomainModel());
        }

        public bool UpdateERPNWorkFlowNode(ERPNWorkFlowNodeTransferObject model)
        {
            return repository.UpdateEntity(model.ToDomainModel());
        }

        public IList<ERPNWorkFlowNodeTransferObject> GetEntities(Func<ERPNWorkFlowNode, bool> whereLambda)
        {

            var tempResult = repository.GetEntities(whereLambda);

            return tempResult.Select(x => x.ToDataTransferObjectModel()).ToList();
        }

        public bool DeleteEntityById(int id)
        {
            return repository.DeleteEntityById(id);
        }
        public ERPNWorkFlowNodeTransferObject GetEntityById(int id)
        {
            return repository.GetEntityById(id).ToDataTransferObjectModel();
        }
       
    }
}
