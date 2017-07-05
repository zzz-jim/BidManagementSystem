using ScientificResearch.DataTransferModel;

namespace ScientificResearch.ViewModel
{
    public static class ProjectRecordExtension
    {
        public static ProjectRecordTransferObject ToDataTransferObjectModel(this ProjectRecordViewModel viewModel)
        {
            return new ProjectRecordTransferObject
            {
                ID = viewModel.PojectEstablishID,
                ApplicationId = viewModel.ApplicationId,
                WorkflowId = viewModel.WorkflowId,
                SuperiorFunds = viewModel.SuperiorFunds,
                HospitalFunds = viewModel.HospitalFunds,
                FundsTime = viewModel.FundsTime,
                StartTime = viewModel.StartTime,
                EndTime = viewModel.EndTime,
                Total = viewModel.Total,
                CreatedBy = viewModel.CreatedBy,
                CreatedTime = viewModel.CreatedTime,
                UpdatedBy = viewModel.UpdatedBy,
                UpdatedTime = viewModel.UpdatedTime,
                ProjectLevel = viewModel.ProjectLevel,
                IsRejected = viewModel.IsRejected,
                IsLocked = viewModel.IsLocked,
                IsTemporary = viewModel.IsTemporary,
                IsDeleted = viewModel.IsDeleted,
                FuJianList = viewModel.FuJianList,
            };
        }

        public static ProjectRecordViewModel ToViewModel(this ProjectRecordTransferObject model)
        {
            return new ProjectRecordViewModel
            {
                PojectEstablishID = model.ID,
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