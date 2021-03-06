﻿using ScientificResearch.DataTransferModel;

namespace ScientificResearch.ViewModel
{
    public static class ERPRiZhiExtension
    {
        public static ERPRiZhiTransferObject ToDataTransferObjectModel(this ERPRiZhiViewModel domainModel)
        {
            return new ERPRiZhiTransferObject
            {
                ID = domainModel.ID,
                UserName = domainModel.UserName,
                TimeStr = domainModel.TimeStr,
                DoSomething = domainModel.DoSomething,
                IpStr = domainModel.IpStr,
                NotificationContent = domainModel.NotificationContent,
                FkFormName = domainModel.FkFormName,
                FKAction = domainModel.FKAction,
                FKApplicationID = domainModel.FKApplicationID,
                ModuleName=domainModel.ModuleName,
            };
        }

        public static ERPRiZhiViewModel ToViewModel(this ERPRiZhiTransferObject model)
        {
            return new ERPRiZhiViewModel
            {
                ID = model.ID,
                UserName = model.UserName,
                TimeStr = model.TimeStr,
                DoSomething = model.DoSomething,
                IpStr = model.IpStr,
                NotificationContent = model.NotificationContent,
                FkFormName = model.FkFormName,
                FKAction = model.FKAction,
                FKApplicationID = model.FKApplicationID,
                ModuleName = model.ModuleName,
            };
        }
    }
}