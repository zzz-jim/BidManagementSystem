using ScientificResearch.DomainModel;

namespace ScientificResearch.DataTransferModel
{
    public static class FundsThresholdExtension
    {
        public static FundsThresholdTransferObjectTransferObject ToDataTransferObjectModel(this FundsThreshold  domainModel)
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
                NWorkToDoID = domainModel.NWorkToDoID,
            };
        }

        public static FundsThreshold ToDomainModel(this FundsThresholdTransferObjectTransferObject model)
        {
            return new FundsThreshold
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

