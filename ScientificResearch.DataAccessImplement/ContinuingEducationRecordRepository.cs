using ScientificResearch.DataTransferModel;
using ScientificResearch.DomainModel;
using ScientificResearch.IDataAccess;
using ScientificResearch.Utility.Enums;
using ScientificResearch.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ScientificResearch.DataAccessImplement
{
    public class ContinuingEducationRecordRepository : IContinuingEducationRecordRepository
    {
        public int AddEntity(ContinuingEducationRecord entity)
        {
            int result = 0;

            using (var context = new CSPostOAEntities())
            {
                context.ContinuingEducationRecord.Add(entity);

                // 已写入基础数据库的对象的数目
                if (1 == context.SaveChanges())
                {
                    result = entity.Id;
                }
                else
                {
                    result = 0;
                }

                return result;
            }
        }

        public bool UpdateEntity(ContinuingEducationRecord entity)
        {
            bool result = false;

            using (var context = new CSPostOAEntities())
            {
                context.ContinuingEducationRecord.Attach(entity);
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
                var deleteEntity = context.ContinuingEducationRecord.FirstOrDefault(u => u.Id == intId);

                if (deleteEntity != null)
                {
                    context.ContinuingEducationRecord.Remove(deleteEntity);
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

        public ContinuingEducationRecord GetEntityById(object id)
        {
            ContinuingEducationRecord entity = new ContinuingEducationRecord();

            using (var context = new CSPostOAEntities())
            {
                entity = context.ContinuingEducationRecord.Find(id);

                return entity;
            }
        }

        public IList<ContinuingEducationRecord> GetAllEntities()
        {
            using (var context = new CSPostOAEntities())
            {
                var result = context.ContinuingEducationRecord;

                return result.ToList<ContinuingEducationRecord>();
            }
        }

        public IList<ContinuingEducationRecord> GetEntities(Func<ContinuingEducationRecord, bool> whereLambda)
        {
            using (var context = new CSPostOAEntities())
            {
                var result = context.ContinuingEducationRecord.Where(whereLambda);

                return result.ToList<ContinuingEducationRecord>();
            }
        }

        public IList<ContinuingEducationRecordTransferObject> GetPageEntities(Func<ContinuingEducationRecord, bool> whereLambda, string sortField, int pageSize, int pageIndex, out int totalPage)
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
                var result = context.ContinuingEducationRecord
                    .Where<ContinuingEducationRecord>(whereLambda);

                totalPage = (int)Math.Ceiling(result.Count() / (double)pageSize);

                ContinuingRecordSortField sorter = EnumToStringHelper.ContinuingConvertStringToEnum(sortField);

                switch (sorter)
                {
                    case ContinuingRecordSortField.Id:
                        break;
                    case ContinuingRecordSortField.UserId:
                        break;
                    case ContinuingRecordSortField.NworkToDoId:
                        break;
                    case ContinuingRecordSortField.UserName:
                        break;
                    case ContinuingRecordSortField.Department:
                        break;
                    case ContinuingRecordSortField.AccountCredit:
                        break;
                    case ContinuingRecordSortField.Credit:
                        break;
                    case ContinuingRecordSortField.CreditLevel:
                        break;
                    case ContinuingRecordSortField.CreditType:
                        break;
                    case ContinuingRecordSortField.IsProjectCredit:
                        break;
                    case ContinuingRecordSortField.IsGranted:
                        break;
                    case ContinuingRecordSortField.UserDegree:
                        break;
                    case ContinuingRecordSortField.UserDuty:
                        break;
                    case ContinuingRecordSortField.CreatedBy:
                        break;
                    case ContinuingRecordSortField.CreatedTime:
                        result = result.OrderBy(u => u.CreatedTime);
                        break;
                    case ContinuingRecordSortField.UpdatedBy:
                        break;
                    case ContinuingRecordSortField.UpdatedTime:
                        break;
                    case ContinuingRecordSortField.Id_Desc:
                        break;
                    case ContinuingRecordSortField.UserId_Desc:
                        break;
                    case ContinuingRecordSortField.NworkToDoId_Desc:
                        break;
                    case ContinuingRecordSortField.UserName_Desc:
                        break;
                    case ContinuingRecordSortField.Department_Desc:
                        break;
                    case ContinuingRecordSortField.AccountCredit_Desc:
                        break;
                    case ContinuingRecordSortField.Credit_Desc:
                        break;
                    case ContinuingRecordSortField.CreditLevel_Desc:
                        break;
                    case ContinuingRecordSortField.CreditType_Desc:
                        break;
                    case ContinuingRecordSortField.IsProjectCredit_Desc:
                        break;
                    case ContinuingRecordSortField.IsGranted_Desc:
                        break;
                    case ContinuingRecordSortField.UserDegree_Desc:
                        break;
                    case ContinuingRecordSortField.UserDuty_Desc:
                        break;
                    case ContinuingRecordSortField.CreatedBy_Desc:
                        break;
                    case ContinuingRecordSortField.CreatedTime_Desc:
                        result = result.OrderBy(u => u.CreatedTime);
                        break;
                    case ContinuingRecordSortField.UpdatedBy_Desc:
                        break;
                    case ContinuingRecordSortField.UpdatedTime_Desc:
                        break;
                    default:
                        result = result.OrderByDescending(u => u.Id);
                        break;
                }

                result = result.Skip<ContinuingEducationRecord>(pageSize * (pageIndex - 1))
                    .Take<ContinuingEducationRecord>(pageSize);
                var collection = result.ToList<ContinuingEducationRecord>();

                IList<ContinuingEducationRecordTransferObject> resultList = new List<ContinuingEducationRecordTransferObject>();
                var tempResult = collection.GroupBy(x => x.UserId);
                var count = tempResult.Count();
                foreach (var item in tempResult)
                {
                    var temp =new ContinuingEducationRecordTransferObject();
                    temp.Id = item.FirstOrDefault().Id;
                    temp.UserName = item.FirstOrDefault().UserName;
                    temp.Department = item.FirstOrDefault().Department;
                    temp.Credit= item.Sum(x => x.Credit);
                    temp.UserDegree = item.FirstOrDefault().UserDegree;

                    resultList.Add(temp);
                }
                return resultList;
            }
        }

        public IList<ContinuingEducationRecordTransferObject> GetPageNotProjectEntities(Func<ContinuingEducationRecord, bool> whereLambda, string sortField, int pageSize, int pageIndex, out int totalPage)
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
                var result = context.ContinuingEducationRecord
                    .Where<ContinuingEducationRecord>(whereLambda);

                totalPage = (int)Math.Ceiling(result.Count() / (double)pageSize);

                ContinuingRecordSortField sorter = EnumToStringHelper.ContinuingConvertStringToEnum(sortField);

                switch (sorter)
                {
                    case ContinuingRecordSortField.Id:
                        break;
                    case ContinuingRecordSortField.UserId:
                        break;
                    case ContinuingRecordSortField.NworkToDoId:
                        break;
                    case ContinuingRecordSortField.UserName:
                        break;
                    case ContinuingRecordSortField.Department:
                        break;
                    case ContinuingRecordSortField.AccountCredit:
                        break;
                    case ContinuingRecordSortField.Credit:
                        break;
                    case ContinuingRecordSortField.CreditLevel:
                        break;
                    case ContinuingRecordSortField.CreditType:
                        break;
                    case ContinuingRecordSortField.IsProjectCredit:
                        break;
                    case ContinuingRecordSortField.IsGranted:
                        break;
                    case ContinuingRecordSortField.UserDegree:
                        break;
                    case ContinuingRecordSortField.UserDuty:
                        break;
                    case ContinuingRecordSortField.CreatedBy:
                        break;
                    case ContinuingRecordSortField.CreatedTime:
                        result = result.OrderBy(u => u.CreatedTime);
                        break;
                    case ContinuingRecordSortField.UpdatedBy:
                        break;
                    case ContinuingRecordSortField.UpdatedTime:
                        break;
                    case ContinuingRecordSortField.Id_Desc:
                        break;
                    case ContinuingRecordSortField.UserId_Desc:
                        break;
                    case ContinuingRecordSortField.NworkToDoId_Desc:
                        break;
                    case ContinuingRecordSortField.UserName_Desc:
                        break;
                    case ContinuingRecordSortField.Department_Desc:
                        break;
                    case ContinuingRecordSortField.AccountCredit_Desc:
                        break;
                    case ContinuingRecordSortField.Credit_Desc:
                        break;
                    case ContinuingRecordSortField.CreditLevel_Desc:
                        break;
                    case ContinuingRecordSortField.CreditType_Desc:
                        break;
                    case ContinuingRecordSortField.IsProjectCredit_Desc:
                        break;
                    case ContinuingRecordSortField.IsGranted_Desc:
                        break;
                    case ContinuingRecordSortField.UserDegree_Desc:
                        break;
                    case ContinuingRecordSortField.UserDuty_Desc:
                        break;
                    case ContinuingRecordSortField.CreatedBy_Desc:
                        break;
                    case ContinuingRecordSortField.CreatedTime_Desc:
                        result = result.OrderBy(u => u.CreatedTime);
                        break;
                    case ContinuingRecordSortField.UpdatedBy_Desc:
                        break;
                    case ContinuingRecordSortField.UpdatedTime_Desc:
                        break;
                    default:
                        result = result.OrderByDescending(u => u.Id);
                        break;
                }

                result = result.Skip<ContinuingEducationRecord>(pageSize * (pageIndex - 1))
                    .Take<ContinuingEducationRecord>(pageSize);
                var collection = result.ToList<ContinuingEducationRecord>();

                IList<ContinuingEducationRecordTransferObject> resultList = new List<ContinuingEducationRecordTransferObject>();
                foreach (var item in collection)
                {
                    var temp = new ContinuingEducationRecordTransferObject();
                    temp.Id = item.Id;
                    temp.UserName = item.UserName;
                    temp.Credit = item.Credit;
                    temp.CreditLevel = item.CreditLevel;
                    temp.CreditType = item.CreditType;
                    temp.CreatedTime = item.CreatedTime;
                    temp.IsGranted = item.IsGranted;
                    resultList.Add(temp);
                }
                return resultList;
            }
        }
    }
}
