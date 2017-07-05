using ScientificResearch.DataAccessImplement;
using ScientificResearch.DataTransferModel;
using ScientificResearch.DomainModel;
using ScientificResearch.IBusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;



namespace ScientificResearch.BusinessLogicImplement
{
    public class ContinuingEducationRecordServiceImplement : IContinuingEducationRecordService
    {
        private ContinuingEducationRecordRepository repository;

        public ContinuingEducationRecordServiceImplement()
            // Calls the constructor that takes 1 argument
            : this(new ContinuingEducationRecordRepository())
        { }

        public ContinuingEducationRecordServiceImplement(ContinuingEducationRecordRepository repository)
        {
            this.repository = repository;
        }

        public int AddContinuingEducationRecord(ContinuingEducationRecordTransferObject model)
        {
            return repository.AddEntity(model.ToDomainModel());
        }
        public bool DeleteEntityById(int id)
        {
            return repository.DeleteEntityById(id);
        }
        public bool UpdateContinuingEducationRecord(ContinuingEducationRecordTransferObject model)
        {
            return repository.UpdateEntity(model.ToDomainModel());
        }

        public ContinuingEducationRecordTransferObject GetEntityById(int id)
        {
            return repository.GetEntityById(id).ToDataTransferObjectModel();
        }
        public IList<ContinuingEducationRecordTransferObject> GetEntities(Func<ContinuingEducationRecord, bool> whereLambda)
        {
            var tempResult = repository.GetEntities(whereLambda);
            return tempResult.Select(x => x.ToDataTransferObjectModel()).ToList();
        }

        public IList<ContinuingEducationRecordTransferObject> GetPageEntities(Func<ContinuingEducationRecord, bool> whereLambda, string sortField, int pageSize, int pageIndex, out int totalPage)
        {
            return repository.GetPageEntities(whereLambda, sortField, pageSize, pageIndex, out totalPage).ToList();
        }

        public IList<ContinuingEducationRecordTransferObject> GetPageNotProjectEntities(Func<ContinuingEducationRecord, bool> whereLambda, string sortField, int pageSize, int pageIndex, out int totalPage)
        {
            return repository.GetPageNotProjectEntities(whereLambda, sortField, pageSize, pageIndex, out totalPage).ToList();
        }
    }
}
