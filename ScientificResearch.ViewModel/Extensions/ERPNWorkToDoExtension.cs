using ScientificResearch.DataTransferModel;
using ScientificResearch.Utility.Enums;
using ScientificResearch.Utility.Constants;
using System.Linq;

namespace ScientificResearch.ViewModel
{
    /// <summary>
    /// Supply the conversition between view model and business logic model.
    /// </summary>
    public static class ERPNWorkToDoExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static ERPNWorkToDoTransferObject ToDataTransferObjectModel(this ERPNWorkToDoViewModel viewModel)
        {
            if (null == viewModel)
            {
                return null;
            }

            return new ERPNWorkToDoTransferObject()
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
                ProjectEstablishTime = viewModel.ProjectEstablishTime,
               // FundsLimitsList = viewModel.FundsLimitsList.Select(x => x.ToDataTransferObjectModel()).ToList(),
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ERPNWorkToDoViewModel ToViewModel(this ERPNWorkToDoTransferObject model)
        {
            return new ERPNWorkToDoViewModel()
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
                ProjectEstablishTime = model.ProjectEstablishTime,
               // FundsLimitsList = model.FundsLimitsList.Select(x => x.ToViewModel()).ToList(),
               // FundsLimitsList = null,
            };
        }

        public static ProjectStatisticViewModel ToProjectStatiticViewModel(this ERPNWorkToDoTransferObject model)
        {
            ProjectStatisticViewModel viewModel = new ProjectStatisticViewModel();

            viewModel.ApplicationTime = model.TimeStr;
            viewModel.ProjectName = model.WenHao;
            viewModel.UserName = model.UserName;

            //var indexOfSectionName = viewModel.SectionName = model.FormKeys.Split(Constant.SharpChar)[];

            if (model.FormID == (int)GoodSubjectTypeOfFormId.Application)
            {
                viewModel.ModuleName = Constant.GoodSubject;
                viewModel.SectionName = model.FormValues.Split(Constant.SharpChar)[4];
            }
            else if (model.FormID == (int)ScienceResearchTypeOfFormId.Application)
            {
                viewModel.ModuleName = Constant.ScienceResearch;
                viewModel.SectionName = model.FormValues.Split(Constant.SharpChar)[4];
            }
            else if (model.FormID == (int)ScienceConferenceTypeOfFormId.Application)
            {
                viewModel.ModuleName = Constant.ScienceConference;
                viewModel.SectionName = model.FormValues.Split(Constant.SharpChar)[1];
            }
            else if (model.FormID == (int)PaperPublishTypeOfFormId.Application)
            {
                viewModel.ModuleName = Constant.PaperPublish;
                viewModel.SectionName = model.FormValues.Split(Constant.SharpChar)[6];
            }
            else if (model.FormID == (int)SubjectLeaderTypeOfFormId.Application)
            {
                viewModel.ModuleName = Constant.SubjectLeader;
                viewModel.SectionName = model.FormValues.Split(Constant.SharpChar)[4];
            }
            else
            {
                viewModel.ModuleName = Constant.ResearchAward;
                viewModel.SectionName = model.FormValues.Split(Constant.SharpChar)[4];
            }

            return viewModel;

        }


    }
}
