using ScientificResearch.DataAccessImplement;
using ScientificResearch.DataTransferModel;
using ScientificResearch.DomainModel;
using ScientificResearch.IBusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;



namespace ScientificResearch.BusinessLogicImplement
{
    public class FundsThresholdServiceImplement : IFundsThresholdService
    {
        private FundsThresholdRepository repository;

        public FundsThresholdServiceImplement()
            // Calls the constructor that takes 1 argument
            : this(new FundsThresholdRepository())
        { }

        public FundsThresholdServiceImplement(FundsThresholdRepository repository)
        {
            this.repository = repository;
        }

        public int AddFundsThreshold(FundsThresholdTransferObjectTransferObject model)
        {
            return repository.AddEntity(model.ToDomainModel());
        }
        public bool DeleteEntityById(int id)
        {
            return repository.DeleteEntityById(id);
        }
        public bool UpdateFundsThreshold(FundsThresholdTransferObjectTransferObject model)
        {
            return repository.UpdateEntity(model.ToDomainModel());
        }

        public FundsThresholdTransferObjectTransferObject GetEntityById(int id)
        {
            return repository.GetEntityById(id).ToDataTransferObjectModel();
        }
        public IList<FundsThresholdTransferObjectTransferObject> GetEntities(Func<FundsThreshold, bool> whereLambda)
        {
            var tempResult = repository.GetEntities(whereLambda);
            return tempResult.Select(x => x.ToDataTransferObjectModel()).ToList();
        }
    }
}
