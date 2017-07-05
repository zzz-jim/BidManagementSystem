using ScientificResearch.DataAccessImplement;
using ScientificResearch.DataTransferModel;
using ScientificResearch.DomainModel;
using ScientificResearch.IBusinessLogic;
using ScientificResearch.IDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScientificResearch.BusinessLogicImplement
{
    public class ERPNFormServiceImplement : IERPNFormService
    {
        private IERPNFormRepository repository;
        public ERPNFormServiceImplement()
            : this(new ERPNFormRepository())
        { }

        public ERPNFormServiceImplement(IERPNFormRepository repository)
        {
            this.repository = repository;
        }
        public ERPNFormTransferObject GetEntityById(int id)
        {
            return repository.GetEntityById(id).ToDataTransferObjectModel();
        }
        public int AddERPNForm(ERPNFormTransferObject model)
        {
            return repository.AddEntity(model.ToDomainModel());
        }

        public bool UpdateERPNForm(ERPNFormTransferObject model)
        {
            return repository.UpdateEntity(model.ToDomainModel());
        }
        public bool DeleteEntityById(int id)
        {
            return repository.DeleteEntityById(id);
        }
        public IList<ERPNFormTransferObject> GetEntities(Func<ERPNForm, bool> whereLambda)
        {
            var tempResult = repository.GetEntities(whereLambda);
            return tempResult.Select(x => x.ToDataTransferObjectModel()).ToList();
        }
    }
}
