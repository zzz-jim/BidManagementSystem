using ScientificResearch.DataTransferModel;
using ScientificResearch.DomainModel;
using System;
using System.Collections.Generic;

namespace ScientificResearch.IBusinessLogic
{
    public interface IContinuingEducationRecordService
    {
        bool DeleteEntityById(int id);
        int AddContinuingEducationRecord(ContinuingEducationRecordTransferObject model);
        bool UpdateContinuingEducationRecord(ContinuingEducationRecordTransferObject model);
        ContinuingEducationRecordTransferObject GetEntityById(int id);
        IList<ContinuingEducationRecordTransferObject> GetEntities(Func<ContinuingEducationRecord, bool> whereLambda);

        IList<ContinuingEducationRecordTransferObject> GetPageEntities(Func<ContinuingEducationRecord, bool> whereLambda, string sortField, int pageSize, int pageIndex, out int totalPage);

        IList<ContinuingEducationRecordTransferObject> GetPageNotProjectEntities(Func<ContinuingEducationRecord, bool> whereLambda, string sortField, int pageSize, int pageIndex, out int totalPage);
        
    }

}
