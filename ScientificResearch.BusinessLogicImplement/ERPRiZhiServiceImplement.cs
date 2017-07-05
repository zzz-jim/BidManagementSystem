using ScientificResearch.DataAccessImplement;
using ScientificResearch.DataTransferModel;
using ScientificResearch.DomainModel;
using ScientificResearch.IBusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;



namespace ScientificResearch.BusinessLogicImplement
{
    public class ERPRiZhiServiceImplement : IERPRiZhiService
    {
        private ERPRiZhiRepository repository;

        public ERPRiZhiServiceImplement()
            // Calls the constructor that takes 1 argument
            : this(new ERPRiZhiRepository())
        { }

        public ERPRiZhiServiceImplement(ERPRiZhiRepository repository)
        {
            this.repository = repository;
        }

        public int AddERPRiZhi(ERPRiZhiTransferObject model)
        {
            return repository.AddEntity(model.ToDomainModel());
        }
        public bool DeleteEntityById(int id)
        {
            return repository.DeleteEntityById(id);
        }
        public bool UpdateERPRiZhi(ERPRiZhiTransferObject model)
        {
            return repository.UpdateEntity(model.ToDomainModel());
        }

        public ERPRiZhiTransferObject GetEntityById(int id)
        {
            return repository.GetEntityById(id).ToDataTransferObjectModel();
        }
        public IList<ERPRiZhiTransferObject> GetEntities(Func<ERPRiZhi, bool> whereLambda)
        {
            var tempResult = repository.GetEntities(whereLambda);
            return tempResult.Select(x => x.ToDataTransferObjectModel()).ToList();
        }
    }
}
