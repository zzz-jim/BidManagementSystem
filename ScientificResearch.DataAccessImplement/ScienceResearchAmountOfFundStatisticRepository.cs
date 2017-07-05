using PF.DomainModel.Identity;
using ScientificResearch.DataTransferModel;
using ScientificResearch.DomainModel;
using ScientificResearch.IDataAccess;
using ScientificResearch.Utility.Constants;
using ScientificResearch.Utility.Enums;
using ScientificResearch.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScientificResearch.DataAccessImplement
{
    public class ScienceResearchAmountOfFundStatisticRepository : IScienceResearchAmountOfFundStatisticRepository
    {
        /// <summary>
        /// 分页后的部分数据
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="projectName"></param>
        /// <param name="departmentName"></param>
        /// <param name="reiburseName"></param>
        /// <param name="moduleName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IList<ScienceResearchAmountOfFundStatisticTransferObject> GetScienceResearchAmountOfFundStatistic(string startTime, string endTime, string projectName,
            string departmentName, string reiburseName, string moduleName, int pageIndex, int pageSize, out int totalCount)
        {
            IList<ScienceResearchAmountOfFundStatisticTransferObject> resultlist = new List<ScienceResearchAmountOfFundStatisticTransferObject>();
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
            using (var context = new CSPostOAEntities())
            {
                totalCount = context.FundsRecord.Count();

                var FundsRecordResult = context.FundsRecord.Where(p =>
                p.CreatedTime <= endtime && p.CreatedTime >= starttime
                && (projectName == "" ? true : (p.ProjectName.Contains(projectName)))
                && (reiburseName == "" ? true : (p.Name.Contains(reiburseName)))
                && (moduleName == Constant.All ? true : (p.ModuleName.Contains(moduleName)))
                ).ToList();

                // TODO: if add departmentName, we need to add this parameter.
                //   .Where(p =>
                //(string.IsNullOrEmpty(departmentName) ? true : (p.DepartmentName.Contains(departmentName))) )

                FundsRecordResult = FundsRecordResult.Skip<FundsRecord>(pageSize * (pageIndex - 1)).Take<FundsRecord>(pageSize).ToList();

                foreach (var item in FundsRecordResult)
                {
                    ScienceResearchAmountOfFundStatisticTransferObject tempresult = new ScienceResearchAmountOfFundStatisticTransferObject();
                    //获取模块名
                    switch (EnumToStringHelper.ConvertStringToEnumOfModuleName(item.ModuleName))
                    {
                        case ModuleNameOfScienceResearch.ScienceResearch:
                            tempresult.Modeule = "政府采购";
                            break;
                        case ModuleNameOfScienceResearch.GoodSubject:
                            tempresult.Modeule = "政府采购";
                            break;
                        case ModuleNameOfScienceResearch.PaperPublish:
                            tempresult.Modeule = "论文发表";
                            break;
                        case ModuleNameOfScienceResearch.ResearchAward:
                            tempresult.Modeule = "科研成果";
                            break;
                        case ModuleNameOfScienceResearch.ScienceConference:
                            tempresult.Modeule = "学术会议";
                            break;
                        case ModuleNameOfScienceResearch.SubjectLeader:
                            tempresult.Modeule = "学科技术带头人";
                            break;
                    }

                    int applicationId = item.ApplicationId;
                    var ErpnWorkToDoResult = context.ERPNWorkToDo.Where(p => p.ID == applicationId).FirstOrDefault();

                    string userName = ErpnWorkToDoResult.UserName.ToString();
                    ApplicationDbContext usercontext = new ApplicationDbContext();
                    string UserId = usercontext.Users.Where(x => x.UserName == userName).FirstOrDefault().Id;
                    //根据用户名找科室
                    string departMentName = usercontext.Sections.Where(x => x.ApplicationUsers.Any(u => u.ApplicationUserId == UserId)).FirstOrDefault().Name;
                    //获取科室
                    tempresult.DepartmentName = departMentName;
                    //获取项目名称
                    tempresult.ProjectName = item.ProjectName;
                    //获取申请人名字
                    tempresult.ApplicationMan = userName;
                    //获取经费名称
                    tempresult.RemburseName = item.Name;
                    //获取经费类型
                    tempresult.OutOrIn = item.IsIncome == true ? "收入" : "支出";
                    //获取经费金额
                    tempresult.Money = item.TotalPrice;
                    //获取创建时间
                    tempresult.CreateTime = Convert.ToDateTime(item.CreatedTime);

                    resultlist.Add(tempresult);
                }
            }
            return resultlist;
        }
        /// <summary>
        /// 全部数据
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="projectName"></param>
        /// <param name="departmentName"></param>
        /// <param name="reiburseName"></param>
        /// <param name="moduleName"></param>
        public IList<ScienceResearchAmountOfFundStatisticTransferObject> GetAllScienceResearchAmountOfFundStatistic(string startTime, string endTime, string projectName,
           string departmentName, string reiburseName, string moduleName)
        {
            IList<ScienceResearchAmountOfFundStatisticTransferObject> resultlist = new List<ScienceResearchAmountOfFundStatisticTransferObject>();
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
            using (var context = new CSPostOAEntities())
            {
                var FundsRecordResult = context.FundsRecord.Where(p =>
                p.CreatedTime <= endtime && p.CreatedTime >= starttime
                && (projectName == "" ? true : (p.ProjectName.Contains(projectName)))
                && (reiburseName == "" ? true : (p.Name.Contains(reiburseName)))
                && (moduleName == Constant.All ? true : (p.ModuleName.Contains(moduleName)))
                ).ToList();

                foreach (var item in FundsRecordResult)
                {
                    ScienceResearchAmountOfFundStatisticTransferObject tempresult = new ScienceResearchAmountOfFundStatisticTransferObject();
                    //获取模块名
                    switch (EnumToStringHelper.ConvertStringToEnumOfModuleName(item.ModuleName))
                    {
                        case ModuleNameOfScienceResearch.ScienceResearch:
                            tempresult.Modeule = "政府采购";
                            break;
                        case ModuleNameOfScienceResearch.GoodSubject:
                            tempresult.Modeule = "政府采购";
                            break;
                        case ModuleNameOfScienceResearch.PaperPublish:
                            tempresult.Modeule = "论文发表";
                            break;
                        case ModuleNameOfScienceResearch.ResearchAward:
                            tempresult.Modeule = "科研成果";
                            break;
                        case ModuleNameOfScienceResearch.ScienceConference:
                            tempresult.Modeule = "学术会议";
                            break;
                        case ModuleNameOfScienceResearch.SubjectLeader:
                            tempresult.Modeule = "学科技术带头人";
                            break;
                    }

                    int applicationId = item.ApplicationId;
                    var ErpnWorkToDoResult = context.ERPNWorkToDo.Where(p => p.ID == applicationId).FirstOrDefault();

                    string userName = ErpnWorkToDoResult.UserName.ToString();
                    ApplicationDbContext usercontext = new ApplicationDbContext();
                    string UserId = usercontext.Users.Where(x => x.UserName == userName).FirstOrDefault().Id;
                    //根据用户名找科室
                    string departMentName = usercontext.Sections.Where(x => x.ApplicationUsers.Any(u => u.ApplicationUserId == UserId)).FirstOrDefault().Name;
                    //获取科室
                    tempresult.DepartmentName = departMentName;
                    //获取项目名称
                    tempresult.ProjectName = item.ProjectName;
                    //获取申请人名字
                    tempresult.ApplicationMan = userName;
                    //获取经费名称
                    tempresult.RemburseName = item.Name;
                    //获取经费类型
                    tempresult.OutOrIn = item.IsIncome == true ? "收入" : "支出";
                    //获取经费金额
                    tempresult.Money = item.TotalPrice;
                    //获取创建时间
                    tempresult.CreateTime = Convert.ToDateTime(item.CreatedTime);

                    resultlist.Add(tempresult);
                }
            }
            return resultlist;
        }
    }
}
