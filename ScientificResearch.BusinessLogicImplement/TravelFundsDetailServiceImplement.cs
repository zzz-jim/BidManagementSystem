using ScientificResearch.DataAccessImplement;
using ScientificResearch.DataTransferModel;
using ScientificResearch.DomainModel;
using ScientificResearch.IBusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;



namespace ScientificResearch.BusinessLogicImplement
{
    public class TravelFundsDetailServiceImplement : ITravelFundsDetailService
    {
        private TravelFundsDetailRepository repository;

        public TravelFundsDetailServiceImplement()
            // Calls the constructor that takes 1 argument
            : this(new TravelFundsDetailRepository())
        { }

        public TravelFundsDetailServiceImplement(TravelFundsDetailRepository repository)
        {
            this.repository = repository;
        }

        public int AddERPRiZhi(TravelFundsDetailTransferObject model)
        {
            return repository.AddEntity(model.ToDomainModel());
        }
        public bool DeleteEntityById(int id)
        {
            return repository.DeleteEntityById(id);
        }
        public bool UpdateERPRiZhi(TravelFundsDetailTransferObject model)
        {
            return repository.UpdateEntity(model.ToDomainModel());
        }

        public TravelFundsDetailTransferObject GetEntityById(int id)
        {
            return repository.GetEntityById(id).ToDataTransferObjectModel();
        }
        public IList<TravelFundsDetailTransferObject> GetEntities(Func<TravelFundsDetail, bool> whereLambda)
        {
            var tempResult = repository.GetEntities(whereLambda);
            return tempResult.Select(x => x.ToDataTransferObjectModel()).ToList();
        }
    }
}
