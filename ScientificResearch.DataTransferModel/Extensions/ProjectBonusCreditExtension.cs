using ScientificResearch.DomainModel;

namespace ScientificResearch.DataTransferModel
{
    public static class ProjectBonusCreditExtension
    {
        public static ProjectBonusCreditTransferObject ToDataTransferObjectModel(this ProjectBonusCredit domainModel)
        {
            return new ProjectBonusCreditTransferObject
            {
                Id = domainModel.Id,
                ModuleName = domainModel.ModuleName,
                ProjectType = domainModel.ProjectType,
                ProjectLevel = domainModel.ProjectLevel,
                Credit = domainModel.Credit,
                IsDeleted = domainModel.IsDeleted,
                CreatedBy = domainModel.CreatedBy,
                CreateTime = domainModel.CreateTime,
                UpdateBy = domainModel.UpdateBy,
                UpdateTime = domainModel.UpdateTime,
            };
        }

        public static ProjectBonusCredit ToDomainModel(this ProjectBonusCreditTransferObject model)
        {
            return new ProjectBonusCredit
            {
                Id = model.Id,
                ModuleName = model.ModuleName,
                ProjectType = model.ProjectType,
                ProjectLevel = model.ProjectLevel,
                Credit = model.Credit,
                IsDeleted = model.IsDeleted,
                CreatedBy = model.CreatedBy,
                CreateTime = model.CreateTime,
                UpdateBy = model.UpdateBy,
                UpdateTime = model.UpdateTime,
            };
        }
    }
}
