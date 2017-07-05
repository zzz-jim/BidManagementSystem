using ScientificResearch.DataAccessImplement;
using ScientificResearch.DataTransferModel;
using ScientificResearch.IBusinessLogic;
using ScientificResearch.IDataAccess;
using System.Collections.Generic;
using ScientificResearch.DomainModel;

using System;
using System.Linq;

namespace ScientificResearch.BusinessLogicImplement
{
    public class ERPBuMenServiceImplement : IERPBuMenService
    {
        private IERPBuMenRepository repository;

        public ERPBuMenServiceImplement()
            // Calls the constructor that takes 1 argument
            : this(new ERPBuMenRepository())
        { }

        public ERPBuMenServiceImplement(IERPBuMenRepository repository)
        {
            this.repository = repository;
        }

        public int AddERPBuMen(ERPBuMenTransferObject model)
        {
            return repository.AddEntity(model.ToDomainModel());
        }

        public bool UpdateERPBuMen(ERPBuMenTransferObject model)
        {
            return repository.UpdateEntity(model.ToDomainModel());
        }
        public bool DeleteEntityById(int id)
        {
            return repository.DeleteEntityById(id);
        }
        public IList<ERPBuMenTransferObject> GetEntities(Func<ERPBuMen, bool> whereLambda)
        {

            var tempResult = repository.GetEntities(whereLambda);

            return tempResult.Select(x => x.ToDataTransferObjectModel()).ToList();
        }


        public ERPBuMenTransferObject GetEntityById(int id)
        {
            return repository.GetEntityById(id).ToDataTransferObjectModel();
        }
    }
}
