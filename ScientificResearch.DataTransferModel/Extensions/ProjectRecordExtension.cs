using ScientificResearch.DomainModel;

namespace ScientificResearch.DataTransferModel
{
    public static class ProjectRecordExtension
    {
        public static ProjectRecordTransferObject ToDataTransferObjectModel(this ProjectRecord domainModel)
        {
            return new ProjectRecordTransferObject
            {
                ID = domainModel.ID,
                ApplicationId = domainModel.ApplicationId,
                WorkflowId = domainModel.WorkflowId,
                SuperiorFunds = domainModel.SuperiorFunds,
                HospitalFunds = domainModel.HospitalFunds,
                FundsTime = domainModel.FundsTime,
                StartTime = domainModel.StartTime,
                EndTime = domainModel.EndTime,
                Total = domainModel.Total,
                CreatedBy = domainModel.CreatedBy,
                CreatedTime = domainModel.CreatedTime,
                UpdatedBy = domainModel.UpdatedBy,
                UpdatedTime = domainModel.UpdatedTime,
                ProjectLevel = domainModel.ProjectLevel,
                IsRejected = domainModel.IsRejected,
                IsLocked = domainModel.IsLocked,
                IsTemporary = domainModel.IsTemporary,
                IsDeleted = domainModel.IsDeleted,
                FuJianList = domainModel.FuJianList,
            };
        }

        public static ProjectRecord ToDomainModel(this ProjectRecordTransferObject model)
        {
            return new ProjectRecord
            {
                ID = model.ID,
                ApplicationId = model.ApplicationId,
                WorkflowId = model.WorkflowId,
                SuperiorFunds = model.SuperiorFunds,
                HospitalFunds = model.HospitalFunds,
                FundsTime = model.FundsTime,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                Total = model.Total,
                CreatedBy = model.CreatedBy,
                CreatedTime = model.CreatedTime,
                UpdatedBy = model.UpdatedBy,
                UpdatedTime = model.UpdatedTime,
                ProjectLevel = model.ProjectLevel,
                IsRejected = model.IsRejected,
                IsLocked = model.IsLocked,
                IsTemporary = model.IsTemporary,
                IsDeleted = model.IsDeleted,
                FuJianList = model.FuJianList,
            };
        }
    }
}
