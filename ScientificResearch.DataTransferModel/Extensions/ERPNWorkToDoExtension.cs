using System.Linq;

using ScientificResearch.DomainModel;

namespace ScientificResearch.DataTransferModel
{
    /// <summary>
    /// Supply the conversition between domain model and business logic model.
    /// </summary>
    public static class ERPNWorkToDoExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainModel"></param>
        /// <returns></returns>
        public static ERPNWorkToDoTransferObject ToDataTransferObjectModel(this ERPNWorkToDo domainModel)
        {
            return new ERPNWorkToDoTransferObject()
            {
                ID = domainModel.ID,
                WorkName = domainModel.WorkName,
                WenHao = domainModel.WenHao,
                FormID = domainModel.FormID,
                WorkFlowID = domainModel.WorkFlowID,
                UserName = domainModel.UserName,
                TimeStr = domainModel.TimeStr,
                FormContent = domainModel.FormContent,
                FuJianList = domainModel.FuJianList,
                ShenPiYiJian = domainModel.ShenPiYiJian,
                JieDianID = domainModel.JieDianID,
                JieDianName = domainModel.JieDianName,
                ShenPiUserList = domainModel.ShenPiUserList,
                OKUserList = domainModel.OKUserList,
                StateNow = domainModel.StateNow,
                LateTime = domainModel.LateTime,
                BeiYong1 = domainModel.BeiYong1,
                BeiYong2 = domainModel.BeiYong2,
                ApplicationStatus = domainModel.ApplicationStatus,
                ApplicationId = domainModel.ApplicationId.HasValue ? domainModel.ApplicationId.Value : 0,
                FormKeys = domainModel.FormKeys,
                FormValues = domainModel.FormValues,
                IsRejected = domainModel.IsRejected,
                IsLocked = domainModel.IsLocked,
                IsTemporary = domainModel.IsTemporary,
                IsDeleted = domainModel.IsDeleted,
                ProjectStatus = domainModel.ProjectStatus,
                //ProjectBidSection = domainModel.ProjectBidSection,
            };
        }

        public static ERPNWorkToDo ToDomainModel(this ERPNWorkToDoTransferObject model)
        {
            return new ERPNWorkToDo()
            {
                ID = model.ID,
                WorkName = model.WorkName,
                WenHao = model.WenHao,
                FormID = model.FormID,
                WorkFlowID = model.WorkFlowID,
                UserName = model.UserName,
                TimeStr = model.TimeStr,
                FormContent = model.FormContent,
                FuJianList = model.FuJianList,
                ShenPiYiJian = model.ShenPiYiJian,
                JieDianID = model.JieDianID,
                JieDianName = model.JieDianName,
                ShenPiUserList = model.ShenPiUserList,
                OKUserList = model.OKUserList,
                StateNow = model.StateNow,
                LateTime = model.LateTime,
                BeiYong1 = model.BeiYong1,
                BeiYong2 = model.BeiYong2,
                ApplicationStatus = model.ApplicationStatus,
                ApplicationId = model.ApplicationId,
                FormKeys = model.FormKeys,
                FormValues = model.FormValues,
                IsRejected = model.IsRejected,
                IsLocked = model.IsLocked,
                IsTemporary = model.IsTemporary,
                IsDeleted = model.IsDeleted,
                ProjectStatus = model.ProjectStatus,
            };
        }
    }
}
