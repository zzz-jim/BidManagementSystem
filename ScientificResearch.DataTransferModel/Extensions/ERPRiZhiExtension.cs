using ScientificResearch.DomainModel;

namespace ScientificResearch.DataTransferModel
{
    public static class ERPRiZhiExtension
    {
        public static ERPRiZhiTransferObject ToDataTransferObjectModel(this ERPRiZhi domainModel)
        {
            return new ERPRiZhiTransferObject
            {
                ID = domainModel.ID,
                UserName = domainModel.UserName,
                TimeStr = domainModel.TimeStr,
                DoSomething = domainModel.DoSomething,
                IpStr = domainModel.IpStr,
                NotificationContent = domainModel.NotificationContent,
                FkFormName=domainModel.FkFormName,
                FKAction=domainModel.FKAction,
                FKApplicationID = domainModel.FKApplicationID,
                ModuleName = domainModel.ModuleName,
            };
        }

        public static ERPRiZhi ToDomainModel(this ERPRiZhiTransferObject model)
        {
            return new ERPRiZhi
            {
                ID = model.ID,
                UserName = model.UserName,
                TimeStr = model.TimeStr,
                DoSomething = model.DoSomething,
                IpStr = model.IpStr,
                NotificationContent = model.NotificationContent,
                FkFormName=model.FkFormName,
                FKAction=model.FKAction,
                FKApplicationID = model.FKApplicationID,
                ModuleName=model.ModuleName,
            };
        }
    }
}

