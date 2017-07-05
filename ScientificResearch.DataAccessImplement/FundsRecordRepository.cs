using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using ScientificResearch.DomainModel;
using ScientificResearch.IDataAccess;
using ScientificResearch.Utility.Enums;
using ScientificResearch.Utility.Helper;

namespace ScientificResearch.DataAccessImplement
{
    public class FundsRecordRepository : IFundsRecordRepository
    {
        public int AddEntity(FundsRecord entity)
        {
            int result = 0;

            using (var context = new CSPostOAEntities())
            {
                context.FundsRecord.Add(entity);

                try
                {
                    // 已写入基础数据库的对象的数目
                    context.SaveChanges();

                    result = entity.ID;
                }
                catch (Exception ex)
                {
                    result = 0;
                    throw ex;
                }

                return result;
            }
        }

        public bool UpdateEntity(FundsRecord entity)
        {
            bool result = false;

            using (var context = new CSPostOAEntities())
            {
                context.FundsRecord.Attach(entity);
                context.Entry(entity).State = EntityState.Modified;

                if (1 == context.SaveChanges())
                {
                    result = true;
                }
                else
                {
                    result = false;
                }

                return result;
            }
        }

        public bool DeleteEntityById(object id)
        {
            bool result = false;

            using (var context = new CSPostOAEntities())
            {
                int intId = (int)id;
                var deleteEntity = context.FundsRecord.FirstOrDefault(u => u.ID == intId);

                if (deleteEntity != null)
                {
                    context.FundsRecord.Remove(deleteEntity);
                    context.Entry(deleteEntity).State = EntityState.Deleted;

                    if (1 == context.SaveChanges())
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }

                return result;
            }
        }

        public FundsRecord GetEntityById(object id)
        {
            FundsRecord entity = new FundsRecord();

            using (var context = new CSPostOAEntities())
            {
                entity = context.FundsRecord.Find(id);

                return entity;
            }
        }

        public IList<FundsRecord> GetAllEntities()
        {
            using (var context = new CSPostOAEntities())
            {
                var result = context.FundsRecord;

                return result.ToList<FundsRecord>();
            }
        }

        public IList<FundsRecord> GetEntities(Func<FundsRecord, bool> whereLambda)
        {
            using (var context = new CSPostOAEntities())
            {
                var result = context.FundsRecord.Where(whereLambda);

                return result.ToList<FundsRecord>();
            }
        }

        public IList<FundsRecord> GetPageEntities(Func<FundsRecord, bool> whereLambda, string sortField, int pageSize, int pageIndex, out int totalPage)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            else
            {
                // Empty
            }

            using (var context = new CSPostOAEntities())
            {
                var result = context.FundsRecord.Where<FundsRecord>(whereLambda);

                totalPage = (int)Math.Ceiling(result.Count() / (double)pageSize);

                ApplicationSortField sorter = EnumToStringHelper.ConvertStringToEnum(sortField);

                switch (sorter)
                {
                    case ApplicationSortField.ID:
                        break;
                    case ApplicationSortField.WorkName:
                        break;
                    case ApplicationSortField.WenHao:
                        break;
                    case ApplicationSortField.FormID:
                        break;
                    case ApplicationSortField.WorkFlowID:
                        break;
                    case ApplicationSortField.UserName:
                        break;
                    case ApplicationSortField.TimeStr:
                        result = result.OrderBy(u => u.TimeStr);
                        break;
                    case ApplicationSortField.FormContent:
                        break;
                    case ApplicationSortField.FuJianList:
                        break;
                    case ApplicationSortField.ShenPiYiJian:
                        break;
                    case ApplicationSortField.JieDianID:
                        break;
                    case ApplicationSortField.JieDianName:
                        break;
                    case ApplicationSortField.ShenPiUserList:
                        break;
                    case ApplicationSortField.OKUserList:
                        break;
                    case ApplicationSortField.StateNow:
                        break;
                    case ApplicationSortField.LateTime:
                        break;
                    case ApplicationSortField.BeiYong1:
                        break;
                    case ApplicationSortField.BeiYong2:
                        break;
                    case ApplicationSortField.ApplicationStatus:
                        break;
                    case ApplicationSortField.ApplicationId:
                        break;
                    case ApplicationSortField.FormKeys:
                        break;
                    case ApplicationSortField.FormValues:
                        break;
                    case ApplicationSortField.ID_Desc:
                        break;
                    case ApplicationSortField.WorkName_Desc:
                        break;
                    case ApplicationSortField.WenHao_Desc:
                        break;
                    case ApplicationSortField.FormID_Desc:
                        break;
                    case ApplicationSortField.WorkFlowID_Desc:
                        break;
                    case ApplicationSortField.UserName_Desc:
                        break;
                    case ApplicationSortField.TimeStr_Desc:
                        result = result.OrderByDescending(u => u.TimeStr);
                        break;
                    case ApplicationSortField.FormContent_Desc:
                        break;
                    case ApplicationSortField.FuJianList_Desc:
                        break;
                    case ApplicationSortField.ShenPiYiJian_Desc:
                        break;
                    case ApplicationSortField.JieDianID_Desc:
                        break;
                    case ApplicationSortField.JieDianName_Desc:
                        break;
                    case ApplicationSortField.ShenPiUserList_Desc:
                        break;
                    case ApplicationSortField.OKUserList_Desc:
                        break;
                    case ApplicationSortField.StateNow_Desc:
                        break;
                    case ApplicationSortField.LateTime_Desc:
                        break;
                    case ApplicationSortField.BeiYong1_Desc:
                        break;
                    case ApplicationSortField.BeiYong2_Desc:
                        break;
                    case ApplicationSortField.ApplicationStatus_Desc:
                        break;
                    case ApplicationSortField.ApplicationId_Desc:
                        break;
                    case ApplicationSortField.FormKeys_Desc:
                        break;
                    case ApplicationSortField.FormValues_Desc:
                        break;
                    default:
                        result = result.OrderByDescending(u => u.ID);
                        break;
                }

                result = result.Skip<FundsRecord>(pageSize * (pageIndex - 1))
                    .Take<FundsRecord>(pageSize);

                return result.ToList<FundsRecord>();
            }
        }
    }
}