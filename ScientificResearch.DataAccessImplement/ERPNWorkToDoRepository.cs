using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using ScientificResearch.DomainModel;
using ScientificResearch.IDataAccess;
using ScientificResearch.Utility.Enums;
using ScientificResearch.Utility.Helper;
using ScientificResearch.DataTransferModel;

namespace ScientificResearch.DataAccessImplement
{
    public class ERPNWorkToDoRepository : IERPNWorkToDoRepository
    {
        public int AddEntity(ERPNWorkToDo entity)
        {
            int result = 0;

            using (var context = new CSPostOAEntities())
            {
                context.ERPNWorkToDo.Add(entity);

                if (entity.ProjectBidSection.Any())
                {
                    context.ProjectBidSection.AddRange(entity.ProjectBidSection);
                }

                // 已写入基础数据库的对象的数目
                if (context.SaveChanges() > 0)
                {
                    result = entity.ID;
                }
                else
                {
                    result = 0;
                }

                return result;
            }
        }

        public bool UpdateEntity(ERPNWorkToDo entity)
        {
            bool result = false;

            using (var context = new CSPostOAEntities())
            {
                context.ERPNWorkToDo.Attach(entity);
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
                var bidSectionModel = context.ProjectBidSection.Where(u => u.ApplicationId == intId);

                if (bidSectionModel.Any())
                {
                    context.ProjectBidSection.RemoveRange(bidSectionModel);// 删除项目时，先删除标段信息
                }

                var deleteEntity = context.ERPNWorkToDo.FirstOrDefault(u => u.ID == intId);

                if (deleteEntity != null)
                {
                    context.ERPNWorkToDo.Remove(deleteEntity);
                    context.Entry(deleteEntity).State = EntityState.Deleted;

                    try
                    {
                        context.SaveChanges();
                        result = true;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else
                {
                    result = false;
                }

                return result;
            }
        }

        public ERPNWorkToDo GetEntityById(object id)
        {
            ERPNWorkToDo entity = new ERPNWorkToDo();

            using (var context = new CSPostOAEntities())
            {
                entity = context.ERPNWorkToDo.Find(id);

                return entity;
            }
        }

        public IList<ERPNWorkToDo> GetAllEntities()
        {
            using (var context = new CSPostOAEntities())
            {
                var result = context.ERPNWorkToDo;

                return result.ToList<ERPNWorkToDo>();
            }
        }

        public IList<ERPNWorkToDo> GetEntities(Func<ERPNWorkToDo, bool> whereLambda)
        {
            using (var context = new CSPostOAEntities())
            {
                var result = context.ERPNWorkToDo.Where(whereLambda);

                return result.ToList<ERPNWorkToDo>();
            }
        }

        public IList<ERPNWorkToDo> GetPageEntities(Func<ERPNWorkToDo, bool> whereLambda, int pageSize, int pageIndex, out int totalPage)
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
                var result = context.ERPNWorkToDo.Where(whereLambda);

                totalPage = (int)Math.Ceiling(result.Count() / (double)pageSize);
                result = result.Skip<ERPNWorkToDo>(pageSize * (pageIndex - 1))
                    .Take<ERPNWorkToDo>(pageSize);

                return result.ToList<ERPNWorkToDo>();
            }
        }

        public IList<ERPNWorkToDoTransferObject> GetPageEntities(Func<ERPNWorkToDo, bool> whereLambda, string sortField, int pageSize, int pageIndex, out int totalPage)
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
                var result = context.ERPNWorkToDo
                    .Where<ERPNWorkToDo>(whereLambda);

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

                result = result.Skip<ERPNWorkToDo>(pageSize * (pageIndex - 1))
                    .Take<ERPNWorkToDo>(pageSize);
                var collection = result.ToList<ERPNWorkToDo>();

                // 模块名称，用来区别是政府采购(科研成果，学术带头人)，有独立的项目确立，还是论文发表(学术会议)，没有独立的项目确立
                bool seperateProjectEstablish = false;

                if (collection.Count > 0)
                {
                    if (collection.First().FormID == (int)GoodSubjectTypeOfFormId.Application
                        || collection.First().FormID == (int)ResearchAwardTypeOfFormId.Application
                        || collection.First().FormID == (int)ScienceResearchTypeOfFormId.Application
                        || collection.First().FormID == (int)SubjectLeaderTypeOfFormId.Application)
                    {
                        seperateProjectEstablish = true;
                    }
                    else if (collection.First().FormID == (int)PaperPublishTypeOfFormId.Application
                        || collection.First().FormID == (int)ScienceConferenceTypeOfFormId.Application)
                    {
                        seperateProjectEstablish = false;
                    }
                    else
                    {

                    }
                }
                IList<ERPNWorkToDoTransferObject> resultList = new List<ERPNWorkToDoTransferObject>();

                foreach (var item in collection)
                {
                    var temp = item.ToDataTransferObjectModel();

                    if (item.ProjectStatus == ApplicationStatus.BigProjectProcessing.ToString())
                    {
                        if (seperateProjectEstablish)
                        {
                            // TODO: 把create time作为 确立 时间 。。StartTime
                            temp.ProjectEstablishTime = context.ProjectRecord.First(x => x.ApplicationId == item.ID).StartTime;
                        }
                        else
                        {
                            // 把 外键 applicationId 等于当前 item 的id的项目确立记录的创建时间作为项目确立时间
                            temp.ProjectEstablishTime = context.ERPNWorkToDo.First(x => x.ApplicationId == item.ID).TimeStr;
                        }
                    }
                    resultList.Add(temp);
                }
                return resultList;
            }
        }
        /// <summary>
        /// 全部数据
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <param name="sortField"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalPage"></param>
        /// <returns></returns>
        public IList<ERPNWorkToDoTransferObject> GetAllPageEntities(Func<ERPNWorkToDo, bool> whereLambda, string sortField)
        {
            using (var context = new CSPostOAEntities())
            {
                var result = context.ERPNWorkToDo
                    .Where<ERPNWorkToDo>(whereLambda);

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

                var collection = result.ToList<ERPNWorkToDo>();

                // 模块名称，用来区别是政府采购(科研成果，学术带头人)，有独立的项目确立，还是论文发表(学术会议)，没有独立的项目确立
                bool seperateProjectEstablish = false;

                if (collection.Count > 0)
                {
                    if (collection.First().FormID == (int)GoodSubjectTypeOfFormId.Application
                        || collection.First().FormID == (int)ResearchAwardTypeOfFormId.Application
                        || collection.First().FormID == (int)ScienceResearchTypeOfFormId.Application
                        || collection.First().FormID == (int)SubjectLeaderTypeOfFormId.Application)
                    {
                        seperateProjectEstablish = true;
                    }
                    else if (collection.First().FormID == (int)PaperPublishTypeOfFormId.Application
                        || collection.First().FormID == (int)ScienceConferenceTypeOfFormId.Application)
                    {
                        seperateProjectEstablish = false;
                    }
                    else
                    {

                    }
                }
                IList<ERPNWorkToDoTransferObject> resultList = new List<ERPNWorkToDoTransferObject>();

                foreach (var item in collection)
                {
                    var temp = item.ToDataTransferObjectModel();

                    if (item.ProjectStatus == ApplicationStatus.BigProjectProcessing.ToString())
                    {
                        if (seperateProjectEstablish)
                        {
                            // TODO: 把create time作为 确立 时间 。。StartTime
                            temp.ProjectEstablishTime = context.ProjectRecord.First(x => x.ApplicationId == item.ID).StartTime;
                        }
                        else
                        {
                            // 把 外键 applicationId 等于当前 item 的id的项目确立记录的创建时间作为项目确立时间
                            temp.ProjectEstablishTime = context.ERPNWorkToDo.First(x => x.ApplicationId == item.ID).TimeStr;
                        }
                    }
                    resultList.Add(temp);
                }
                return resultList;
            }
        }
    }
}
