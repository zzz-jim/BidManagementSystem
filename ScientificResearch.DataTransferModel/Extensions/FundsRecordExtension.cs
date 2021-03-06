﻿using System.Linq;

using ScientificResearch.DomainModel;

namespace ScientificResearch.DataTransferModel
{
    /// <summary>
    /// Supply the conversition between domain model and business logic model.
    /// </summary>
    public static class FundsRecordExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainModel"></param>
        /// <returns></returns>
        public static FundsRecordTransferObject ToDataTransferObjectModel(this FundsRecord domainModel)
        {
            return new FundsRecordTransferObject()
            {
                ID = domainModel.ID,
                ApplicationId = domainModel.ApplicationId,
                WorkflowId = domainModel.WorkflowId,
                Name = domainModel.Name,
                Type = domainModel.Type,
                Description = domainModel.Description,
                ProjectName = domainModel.ProjectName,
                CountOfBill = domainModel.CountOfBill,
                UnitPrice = domainModel.UnitPrice,
                Quantity = domainModel.Quantity,
                Unit = domainModel.Unit,
                TotalPrice = domainModel.TotalPrice,
                IsIncome = domainModel.IsIncome,
                IsPrint = domainModel.IsPrint,
                LastPrintTime = domainModel.LastPrintTime,
                UserName = domainModel.UserName,
                TimeStr = domainModel.TimeStr,
                FuJianList = domainModel.FuJianList,
                ShenPiYiJian = domainModel.ShenPiYiJian,
                JieDianID = domainModel.JieDianID,
                JieDianName = domainModel.JieDianName,
                OKUserList = domainModel.OKUserList,
                ShenPiUserList = domainModel.ShenPiUserList,
                StateNow = domainModel.StateNow,
                LateTime = domainModel.LateTime,
                Comment = domainModel.Comment,
                CreatedBy = domainModel.CreatedBy,
                CreatedTime = domainModel.CreatedTime,
                UpdatedBy=domainModel.UpdatedBy,
                UpdatedTime = domainModel.UpdatedTime,
                IsRejected = domainModel.IsRejected,
                IsLocked = domainModel.IsLocked,
                IsTemporary = domainModel.IsTemporary,
                IsDeleted = domainModel.IsDeleted,
                ModuleName = domainModel.ModuleName,
                //TravelFundsList=domainModel.TravelFundsDetail.Select(x=>x.ToDataTransferObjectModel()).ToList(),
            };
        }

        public static FundsRecord ToDomainModel(this FundsRecordTransferObject model)
        {
            return new FundsRecord()
            {
                ID = model.ID,
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
                //TravelFundsDetail=model.TravelFundsList.Select(x=>x.ToDomainModel()).ToList(),
            };
        }
    }
}
