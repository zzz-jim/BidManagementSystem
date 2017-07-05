using ScientificResearch.DataTransferModel;
using ScientificResearch.DomainModel;
using System;
using System.Collections.Generic;

namespace ScientificResearch.IBusinessLogic
{
    public interface IProjectRecordService
    {
        bool DeleteEntityById(int id);
        int AddProjectRecord(ProjectRecordTransferObject model);
        bool UpdateProjectRecord(ProjectRecordTransferObject model);
        ProjectRecordTransferObject GetEntityById(int id);
        IList<ProjectRecordTransferObject> GetAllEntities();
        IList<ProjectRecordTransferObject> GetEntities(Func<ProjectRecord, bool> whereLambda);
    }
}
