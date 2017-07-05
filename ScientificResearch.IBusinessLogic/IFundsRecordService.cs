using System;
using System.Collections.Generic;

using ScientificResearch.DataTransferModel;
using ScientificResearch.DomainModel;

namespace ScientificResearch.IBusinessLogic
{
    public interface IFundsRecordService
    {
        bool DeleteEntityById(int id);

        bool travelDeleteEntityById(int id);
        int AddFundsRecord(FundsRecordTransferObject model);

        bool UpdateFundsRecord(FundsRecordTransferObject model);

        FundsRecordTransferObject GetEntityById(int id);

        IList<FundsRecordTransferObject> GetAllEntities();

        FundsRecordTransferObject GetAllById(int id);

        IList<FundsRecordTransferObject> GetEntities(Func<FundsRecord, bool> whereLambda);
        //IList<TravelFundsRecordTransferObject> GetEntities(Func<TravelFundsDetail, bool> whereLambda);

        int AddTravelFundsRecord(FundsRecordTransferObject model);

        TravelFundsRecordTransferObject GetEntityWithTravelDetailListById(int id);

        int AddFundsRecord(TravelFundsRecordTransferObject model);

        bool UpdateFundsRecord(TravelFundsRecordTransferObject model);


        IList<FundsRecordTransferObject> GetPageEntities(Func<FundsRecord, bool> whereLambda, string sortField, int pageSize, int pageIndex, out int totalPage);
    }
}
