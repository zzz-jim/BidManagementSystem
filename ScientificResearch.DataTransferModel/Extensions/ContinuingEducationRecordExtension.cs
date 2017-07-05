using ScientificResearch.DomainModel;

namespace ScientificResearch.DataTransferModel
{
    public static class ContinuingEducationRecordExtension
    {
        public static ContinuingEducationRecordTransferObject ToDataTransferObjectModel(this ContinuingEducationRecord domainModel)
        {
            return new ContinuingEducationRecordTransferObject
            {
                Id = domainModel.Id,
                UserId = domainModel.UserId,
                NworkToDoId = domainModel.NworkToDoId,
                UserName = domainModel.UserName,
                Department = domainModel.Department,
                AccountCredit = domainModel.AccountCredit,
                Credit=domainModel.Credit,
                CreditLevel=domainModel.CreditLevel,
                CreditType = domainModel.CreditType,
                IsProjectCredit = domainModel.IsProjectCredit,
                IsGranted = domainModel.IsGranted,
                CreatedBy = domainModel.CreatedBy,
                CreatedTime = domainModel.CreatedTime,
                UpdatedBy = domainModel.UpdatedBy,
                UpdatedTime = domainModel.UpdatedTime,
                UserDegree = domainModel.UserDegree,
                UserDuty = domainModel.UserDuty,
            };
        }

        public static ContinuingEducationRecord ToDomainModel(this ContinuingEducationRecordTransferObject model)
        {
            return new ContinuingEducationRecord
            {
                Id = model.Id,
                UserId = model.UserId,
                NworkToDoId = model.NworkToDoId,
                UserName = model.UserName,
                Department = model.Department,
                AccountCredit = model.AccountCredit,
                Credit = model.Credit,
                CreditLevel = model.CreditLevel,
                CreditType = model.CreditType,
                IsProjectCredit = model.IsProjectCredit,
                IsGranted = model.IsGranted,
                CreatedBy = model.CreatedBy,
                CreatedTime = model.CreatedTime,
                UpdatedBy = model.UpdatedBy,
                UpdatedTime = model.UpdatedTime,
                UserDegree = model.UserDegree,
                UserDuty = model.UserDuty,
            };
        }
    }
}

