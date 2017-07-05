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
    public class FundsRecordServiceImplement : IFundsRecordService
    {
        private IFundsRecordRepository repository;
        private ITravelFundsDetailRepository travelRepository;

        public FundsRecordServiceImplement()
            // 调用构造函数接受参数
            : this(new FundsRecordRepository(),
                new TravelFundsDetailRepository())
        { }

        public FundsRecordServiceImplement(IFundsRecordRepository repository, ITravelFundsDetailRepository travelRepository)
        {
            this.repository = repository;
            this.travelRepository = travelRepository;
        }

        public int AddFundsRecord(FundsRecordTransferObject model)
        {
            return repository.AddEntity(model.ToDomainModel());
        }

        public bool UpdateFundsRecord(FundsRecordTransferObject model)
        {
            return repository.UpdateEntity(model.ToDomainModel());
        }
        public bool DeleteEntityById(int id)
        {
            return repository.DeleteEntityById(id);
        }
        public bool travelDeleteEntityById(int id)
        {
            return travelRepository.DeleteEntityById(id);
        }
        public FundsRecordTransferObject GetEntityById(int id)
        {
            return repository.GetEntityById(id).ToDataTransferObjectModel();
        }

        public IList<FundsRecordTransferObject> GetAllEntities()
        {
            var tempResult = repository.GetAllEntities();
            return tempResult.Select(x => x.ToDataTransferObjectModel()).ToList();
        }

        //public IList<TravelFundsDetailTransferObject> GetAllEntities1()
        //{
        //    var tempResult = travelRepository.GetAllEntities1();
        //    return tempResult.Select(x => x.ToDataTransferObjectModel).ToList();
        //}
        public IList<FundsRecordTransferObject> GetEntities(Func<FundsRecord, bool> whereLambda)
        {
            var tempResult = repository.GetEntities(whereLambda);
            return tempResult.Select(x => x.ToDataTransferObjectModel()).ToList();
        }

        public int AddTravelFundsRecord(FundsRecordTransferObject model)
        {
            return repository.AddEntity(model.ToDomainModel());
        }

        public TravelFundsRecordTransferObject GetEntityWithTravelDetailListById(int id)
        {
            var fundsRecord = repository.GetEntityById(id).ToTravelFundsDataTransferObjectModel();
            var travelList = travelRepository.GetEntities(x => x.FundsRecordId == id);

            fundsRecord.TravelFundsList = travelList.Select(x => x.ToDataTransferObjectModel()).ToList();

            return fundsRecord;
        }
        public FundsRecordTransferObject GetAllById(int id)
        {
            var fundsRecord = repository.GetEntityById(id);
            var travelList = travelRepository.GetEntities(x => x.FundsRecordId == id);

            var result = fundsRecord.ToDataTransferObjectModel();
            result.TravelFundsList = travelList.Select(x => x.ToDataTransferObjectModel()).ToList();

            return result;

        }
        public int AddFundsRecord(TravelFundsRecordTransferObject model)
        {
            return repository.AddEntity(model.ToDomainModel());
        }

        public bool UpdateFundsRecord(TravelFundsRecordTransferObject model)
        {
            return repository.UpdateEntity(model.ToDomainModel());
        }



        public IList<FundsRecordTransferObject> GetPageEntities(Func<FundsRecord, bool> whereLambda, string sortField, int pageSize, int pageIndex, out int totalPage)
        {
            return repository.GetPageEntities(whereLambda, sortField, pageSize, pageIndex, out totalPage).Select(x => x.ToDataTransferObjectModel()).ToList();
        }
    }
}
