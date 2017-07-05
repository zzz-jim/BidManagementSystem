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
    public class ProjectBonusCreditServiceImplement : IProjectBonusCreditService
    {
        private IProjectBonusCreditRepository repository;
      
        public ProjectBonusCreditServiceImplement()
            // 调用构造函数接受参数
            : this(new ProjectBonusCreditRepository())
        { }

        public ProjectBonusCreditServiceImplement(IProjectBonusCreditRepository repository)
        {
            this.repository = repository;
        }

        public int AddProjectBonusCredit(ProjectBonusCreditTransferObject model)
        {
            return repository.AddEntity(model.ToDomainModel());
        }
        public bool DeleteEntityById(int id)
        {
            return repository.DeleteEntityById(id);
        }
        public bool UpdateProjectBonusCredit(ProjectBonusCreditTransferObject model)
        {
            return repository.UpdateEntity(model.ToDomainModel());
        }

        public ProjectBonusCreditTransferObject GetEntityById(int id)
        {
            return repository.GetEntityById(id).ToDataTransferObjectModel();
        }
        public IList<ProjectBonusCreditTransferObject> GetAllEntities()
        {

            var tempResult = repository.GetAllEntities();

            return tempResult.Select(x => x.ToDataTransferObjectModel()).ToList();
        }
        public IList<ProjectBonusCreditTransferObject> GetEntities(Func<ProjectBonusCredit, bool> whereLambda)
        {

            var tempResult = repository.GetEntities(whereLambda);

            return tempResult.Select(x => x.ToDataTransferObjectModel()).ToList();
        }
    }
}
