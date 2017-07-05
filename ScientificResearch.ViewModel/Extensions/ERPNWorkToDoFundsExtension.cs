using System.Linq;

using ScientificResearch.DataTransferModel;

namespace ScientificResearch.ViewModel
{
    /// <summary>
    /// Supply the conversition between view model and business logic model.
    /// </summary>
    public static class ERPNWorkToDoFundsExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static ERPNWorkToDoFundsTransferObject ToDataTransferObjectModel(this ERPNWorkToDoFundsViewModel viewModel)
        {
            if (null == viewModel)
            {
                return null;
            }

            return new ERPNWorkToDoFundsTransferObject()
            {
                ID = viewModel.NWorkToDoID,
                WorkName = viewModel.WorkName,
                WenHao = viewModel.WenHao,
                FormID = viewModel.FormID,
                WorkFlowID = viewModel.WorkFlowID,
                UserName = viewModel.UserName,
                TimeStr = viewModel.TimeStr,
                FormContent = viewModel.FormContent,
                FuJianList = viewModel.FuJianList,
                ShenPiYiJian = viewModel.ShenPiYiJian,
                JieDianID = viewModel.JieDianID,
                JieDianName = viewModel.JieDianName,
                ShenPiUserList = viewModel.ShenPiUserList,
                OKUserList = viewModel.OKUserList,
                StateNow = viewModel.StateNow,
                LateTime = viewModel.LateTime,
                BeiYong1 = viewModel.BeiYong1,
                BeiYong2 = viewModel.BeiYong2,
                ApplicationStatus = viewModel.ApplicationStatus,
                ApplicationId = viewModel.ApplicationId,
                FormKeys = viewModel.FormKeys,
                FormValues = viewModel.FormValues,
                IsRejected = viewModel.IsRejected,
                IsLocked = viewModel.IsLocked,
                IsTemporary = viewModel.IsTemporary,
                IsDeleted = viewModel.IsDeleted,
                ProjectStatus = viewModel.ProjectStatus,
               // ProjectEstablishTime = viewModel.ProjectEstablishTime,
                NWorkToDoFundsList = viewModel.NWorkToDoFundsList.Select(x => x.ToDataTransferObjectModel()).ToList(),
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ERPNWorkToDoFundsViewModel ToViewModel(this ERPNWorkToDoFundsTransferObject model)
        {
            return new ERPNWorkToDoFundsViewModel()
            {
                NWorkToDoID = model.ID,
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
               // ProjectEstablishTime = model.ProjectEstablishTime,
                NWorkToDoFundsList = model.NWorkToDoFundsList.Select(x => x.ToViewModel()).ToList(),
            };
        }
    }
}
