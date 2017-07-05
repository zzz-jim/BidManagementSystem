
using System;
using System.Collections.Generic;

using ScientificResearch.DataTransferModel;
using ScientificResearch.DomainModel;

namespace ScientificResearch.IBusinessLogic
{
    public interface IProjectBonusCreditService
    {
        bool DeleteEntityById(int id);
        int AddProjectBonusCredit(ProjectBonusCreditTransferObject model);
        bool UpdateProjectBonusCredit(ProjectBonusCreditTransferObject model);
        ProjectBonusCreditTransferObject GetEntityById(int id);
        IList<ProjectBonusCreditTransferObject> GetAllEntities();
        IList<ProjectBonusCreditTransferObject> GetEntities(Func<ProjectBonusCredit, bool> whereLambda);
    }
}
