using System.Collections.Generic;

using ScientificResearch.DataTransferModel;
using ScientificResearch.DomainModel;
using ScientificResearch.IDataAccess;
using ScientificResearch.Utility.Constants;
using ScientificResearch.Utility.Enums;
using ScientificResearch.Utility.Helper;
using System;
using System.Linq;

namespace ScientificResearch.DataAccessImplement
{
    public class ScienceResearchOfCapitalStatisticRepository : IScienceResearchOfCapitalStatisticRepository
    {
        /// <summary>
        /// 分页后的部分数据
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="projectName"></param>
        /// <param name="moduleName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IList<ScienceResearchOfCapitalStatisticTransferObject> GetScienceResearchOfCapitalStatistic(string startTime, string endTime, 
            string projectName, string moduleName, int pageIndex, int pageSize, out int totalCount)
        {
            IList<ScienceResearchOfCapitalStatisticTransferObject> resultlist = new List<ScienceResearchOfCapitalStatisticTransferObject>();
            resultlist = GetAllScienceResearchOfCapitalStatistic( startTime, endTime, projectName, moduleName);
            //总行数
            totalCount = resultlist.Count();
            //当前页数的数据
            resultlist = resultlist.Skip<ScienceResearchOfCapitalStatisticTransferObject>((pageIndex - 1) * pageSize).Take<ScienceResearchOfCapitalStatisticTransferObject>(pageSize).ToList();
            return resultlist;
        }
        /// <summary>
        /// 全部数据，不分页
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="projectName"></param>
        /// <param name="moduleName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IList<ScienceResearchOfCapitalStatisticTransferObject> GetAllScienceResearchOfCapitalStatistic(string startTime, string endTime,
           string projectName, string moduleName)
        {
            IList<ScienceResearchOfCapitalStatisticTransferObject> resultlist = new List<ScienceResearchOfCapitalStatisticTransferObject>();
            DateTime starttime = DateTime.MinValue;
            DateTime endtime = DateTime.MaxValue;
            if (startTime.Trim() != "")
            {
                starttime = Convert.ToDateTime(startTime);
            }
            if (endTime.Trim() != "")
            {
                endtime = Convert.ToDateTime(endTime).AddDays(1);
            }
            int formId = 0;
            if (moduleName != Constant.All)
            {
                switch (EnumToStringHelper.ConvertStringToEnumOfModuleName(moduleName))
                {
                    case ModuleNameOfScienceResearch.ScienceResearch:
                        formId = (int)ScienceResearchTypeOfFormId.Application;
                        break;
                    case ModuleNameOfScienceResearch.GoodSubject:
                        formId = (int)GoodSubjectTypeOfFormId.Application;
                        break;
                    case ModuleNameOfScienceResearch.ResearchAward:
                        formId = (int)ResearchAwardTypeOfFormId.Application;
                        break;
                    case ModuleNameOfScienceResearch.SubjectLeader:
                        formId = (int)SubjectLeaderTypeOfFormId.Application;
                        break;
                }
            }
            else
            {
                formId = 0;
            }

            using (var context = new CSPostOAEntities())
            {
                //已经项目确立且在政府采购，政府采购，科研成果，学科技术带头人范围内
                var ErpnworkToDoResultList = context.ERPNWorkToDo.Where(p =>
                    p.ApplicationStatus != null && !(p.ApplicationStatus == ApplicationStatus.AplicationWriting.ToString()
                    || p.ApplicationStatus == ApplicationStatus.ApplicationApproving.ToString()
                    || p.ApplicationStatus == ApplicationStatus.ApplicationRejected.ToString()
                    || p.ApplicationStatus == ApplicationStatus.ApplicationApproved.ToString()
                    || p.ApplicationStatus == ApplicationStatus.ProjectEstablishing.ToString()
                    || p.ApplicationStatus == ApplicationStatus.ProjectRejected.ToString())
                    && (projectName == "" ? true : (p.WenHao.Contains(projectName)))
                    && (formId == 0 ? (p.FormID == (int)ScienceResearchTypeOfFormId.Application || p.FormID == (int)GoodSubjectTypeOfFormId.Application ||
                    p.FormID == (int)ResearchAwardTypeOfFormId.Application || p.FormID == (int)SubjectLeaderTypeOfFormId.Application) : (p.FormID == formId)));

                //获取模块名
                string blockName = "";
                foreach (var item in ErpnworkToDoResultList)
                {
                    ScienceResearchOfCapitalStatisticTransferObject tempresult = new ScienceResearchOfCapitalStatisticTransferObject();
                    //获取拨款时间，及项目确立时间
                    int applicationId = item.ID;
                    var ProjectRecordResult = context.ProjectRecord.Where(p => p.ApplicationId == applicationId).FirstOrDefault();
                    tempresult.AllocationOfTime = Convert.ToDateTime(ProjectRecordResult.CreatedTime);

                    if (ProjectRecordResult.CreatedTime <= endtime && ProjectRecordResult.CreatedTime >= starttime)
                    {

                    }
                    else
                    {
                        continue;//跳出此次循环，进行下一次循环
                    }

                    switch (item.FormID)
                    {
                        case (int)ScienceResearchTypeOfFormId.Application:
                            blockName = "政府采购";
                            break;
                        case (int)GoodSubjectTypeOfFormId.Application:
                            blockName = "政府采购";
                            break;
                        case (int)ResearchAwardTypeOfFormId.Application:
                            blockName = "科研成果";
                            break;
                        case (int)SubjectLeaderTypeOfFormId.Application:
                            blockName = "学科技术带头人";
                            break;
                    }

                    tempresult.Modeule = blockName;
                    //获取项目名称
                    tempresult.ProjectName = item.WenHao;
                    //获取负责人,可能还会有问题，数组位置可能不对
                    tempresult.PrincipalMan = item.FormValues.Split(Constant.SharpChar)[5];

                    //获取拨款金额
                    tempresult.AppropriationMoney = ProjectRecordResult.Total;
                    //获取报销金额
                    var FundsRecordResult = context.FundsRecord.Where(p => p.ApplicationId == applicationId).ToList();
                    double reimbursementAmount = 0;

                    foreach (var item1 in FundsRecordResult)
                    {
                        reimbursementAmount = reimbursementAmount + item1.TotalPrice;
                    }

                    tempresult.ReimbursementAmount = reimbursementAmount;
                    resultlist.Add(tempresult);
                }
            }
            return resultlist;
        }
    }
}
