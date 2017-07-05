using ScientificResearch.DataTransferModel;

namespace ScientificResearch.ViewModel
{
    public static class FundsThresholdExtension
    {
        public static FundsThresholdTransferObjectTransferObject ToDataTransferObjectModel(this FundsThresholdViewModel domainModel)
        {
            return new FundsThresholdTransferObjectTransferObject
            {
                Id = domainModel.Id,
                ModuleName = domainModel.ModuleName,
                ProjectType = domainModel.ProjectType,
                FundsType = domainModel.FundsType,
                Threshold = domainModel.Threshold,
                IsDeleted = domainModel.IsDeleted,
                CreatedBy = domainModel.CreatedBy,
                CreatedTime = domainModel.CreatedTime,
                UpdatedBy = domainModel.UpdatedBy,
                UpdatedTime = domainModel.UpdatedTime,
                NWorkToDoID=domainModel.NWorkToDoID,
            };
        }

        public static FundsThresholdViewModel ToViewModel(this FundsThresholdTransferObjectTransferObject model)
        {
            return new FundsThresholdViewModel
            {
                Id = model.Id,
                ModuleName = model.ModuleName,
                ProjectType = model.ProjectType,
                FundsType = model.FundsType,
                Threshold = model.Threshold,
                IsDeleted = model.IsDeleted,
                CreatedBy = model.CreatedBy,
                CreatedTime = model.CreatedTime,
                UpdatedBy = model.UpdatedBy,
                UpdatedTime = model.UpdatedTime,
                NWorkToDoID = model.NWorkToDoID,
            };
        }
    }
}