using System.Linq;

using ScientificResearch.DataTransferModel;

namespace ScientificResearch.ViewModel
{
    /// <summary>
    /// Supply the conversition between view model and business logic model.
    /// </summary>
    public static class FundsRecordExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static FundsRecordTransferObject ToDataTransferObjectModel(this FundsRecordViewModel viewModel)
        {
            if (null == viewModel)
            {
                return null;
            }

            return new FundsRecordTransferObject()
            {
                ID = viewModel.FundsRecordID,
                ApplicationId = viewModel.ApplicationId,
                WorkflowId = viewModel.WorkflowId,
                Name = viewModel.Name,
                Type = viewModel.Type,
                Description = viewModel.Description,
                ProjectName = viewModel.ProjectName,
                CountOfBill = viewModel.CountOfBill,
                UnitPrice = viewModel.UnitPrice,
                Quantity = viewModel.Quantity,
                Unit = viewModel.Unit,
                TotalPrice = viewModel.TotalPrice,
                IsIncome = viewModel.IsIncome,
                IsPrint = viewModel.IsPrint,
                LastPrintTime = viewModel.LastPrintTime,
                UserName = viewModel.UserName,
                TimeStr = viewModel.TimeStr,
                FuJianList = viewModel.FuJianList,
                ShenPiYiJian = viewModel.ShenPiYiJian,
                JieDianID = viewModel.JieDianID,
                JieDianName = viewModel.JieDianName,
                OKUserList = viewModel.OKUserList,
                ShenPiUserList = viewModel.ShenPiUserList,
                StateNow = viewModel.StateNow,
                LateTime = viewModel.LateTime,
                Comment = viewModel.Comment,
                CreatedBy = viewModel.CreatedBy,
                CreatedTime = viewModel.CreatedTime,
                UpdatedBy = viewModel.UpdatedBy,
                UpdatedTime = viewModel.UpdatedTime,
                IsRejected = viewModel.IsRejected,
                IsLocked = viewModel.IsLocked,
                IsTemporary = viewModel.IsTemporary,
                IsDeleted = viewModel.IsDeleted,
                ModuleName = viewModel.ModuleName,
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static FundsRecordViewModel ToViewModel(this FundsRecordTransferObject model)
        {
            return new FundsRecordViewModel()
            {
                FundsRecordID = model.ID,
                ApplicationId = model.ApplicationId,
                WorkflowId = model.WorkflowId,
                Name = model.Name,
                Type = model.Type,
                Description = model.Description,
                ProjectName = model.ProjectName,
                CountOfBill = model.CountOfBill,
                UnitPrice = model.UnitPrice,
                Quantity = model.Quantity,
                Unit = model.Unit,
                TotalPrice = model.TotalPrice,
                IsIncome = model.IsIncome,
                IsPrint = model.IsPrint,
                LastPrintTime = model.LastPrintTime,
                UserName = model.UserName,
                TimeStr = model.TimeStr,
                FuJianList = model.FuJianList,
                ShenPiYiJian = model.ShenPiYiJian,
                JieDianID = model.JieDianID,
                JieDianName = model.JieDianName,
                OKUserList = model.OKUserList,
                ShenPiUserList = model.ShenPiUserList,
                StateNow = model.StateNow,
                LateTime = model.LateTime,
                Comment = model.Comment,
                CreatedBy = model.CreatedBy,
                CreatedTime = model.CreatedTime,
                UpdatedBy = model.UpdatedBy,
                UpdatedTime = model.UpdatedTime,
                IsRejected = model.IsRejected,
                IsLocked = model.IsLocked,
                IsTemporary = model.IsTemporary,
                IsDeleted = model.IsDeleted,
                ModuleName = model.ModuleName,
                TravelFundsList = model.TravelFundsList == null ? null : model.TravelFundsList.Select(x => x.ToViewModel()).ToList(),
            };
        }
    }
}
