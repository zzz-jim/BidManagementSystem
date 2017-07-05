using ScientificResearch.DataAccessImplement;
using ScientificResearch.DomainModel;
using ScientificResearch.IBusinessLogic;
using ScientificResearch.IDataAccess;
using ScientificResearch.DataTransferModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScientificResearch.BusinessLogicImplement
{
    public class ProjectRecordServiceImplement : IProjectRecordService
    {
         private IProjectRecordRepository repository;

        public ProjectRecordServiceImplement()
            // 调用构造函数接受参数
            : this(new ProjectRecordRepository())
        {}
        public ProjectRecordServiceImplement(IProjectRecordRepository repository)
        {
            this.repository = repository;
        }
        public int AddProjectRecord(ProjectRecordTransferObject model)
        {
            return repository.AddEntity(model.ToDomainModel());
        }
        public bool UpdateProjectRecord(ProjectRecordTransferObject model)
        {
            return repository.UpdateEntity(model.ToDomainModel());
        }
        public ProjectRecordTransferObject GetEntityById(int id)
        {
            return repository.GetEntityById(id).ToDataTransferObjectModel();
        }
        public bool DeleteEntityById(int id)
        {
            return repository.DeleteEntityById(id);
        }
        public IList<ProjectRecordTransferObject> GetAllEntities()
        {
            var tempResult = repository.GetAllEntities();
            return tempResult.Select(x => x.ToDataTransferObjectModel()).ToList();
        }
        public IList<ProjectRecordTransferObject> GetEntities(Func<ProjectRecord, bool> whereLambda)
        {
            var tempResult = repository.GetEntities(whereLambda);
            return tempResult.Select(x => x.ToDataTransferObjectModel()).ToList();
        }
    }
}
