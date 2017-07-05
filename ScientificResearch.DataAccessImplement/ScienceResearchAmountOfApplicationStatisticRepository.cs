using ScientificResearch.DataTransferModel;
using ScientificResearch.DomainModel;
using ScientificResearch.IDataAccess;
using ScientificResearch.Utility.Constants;
using ScientificResearch.Utility.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScientificResearch.DataAccessImplement
{
    public class ScienceResearchAmountOfApplicationStatisticRepository : IScienceResearchAmountOfApplicationStatisticRepository
    {
        public IList<ScienceResearchAmountOfApplicationStatisticTransferObject> GetScienceResearchAmountOfApplicationStatistics(DateTime startTime, DateTime endTime)
        {
            IList<ScienceResearchAmountOfApplicationStatisticTransferObject> resultlist = new List<ScienceResearchAmountOfApplicationStatisticTransferObject>();
            using (var context = new CSPostOAEntities())
            {
                var result = context.ERPNWorkToDo.Where(p => (p.FormID ==
                    (int)ScienceResearchTypeOfFormId.Application || p.FormID ==
                    (int)GoodSubjectTypeOfFormId.Application || p.FormID ==
                    (int)SubjectLeaderTypeOfFormId.Application || p.FormID ==
                    (int)PaperPublishTypeOfFormId.Application || p.FormID ==
                    (int)ScienceConferenceTypeOfFormId.Application || p.FormID ==
                    (int)ResearchAwardTypeOfFormId.Application)
                    && p.TimeStr <= endTime && p.TimeStr >= startTime).ToList();
                string blockName = "";
                string projectType = "";
                foreach (var item in result)
                {
                    ScienceResearchAmountOfApplicationStatisticTransferObject tempresult = new ScienceResearchAmountOfApplicationStatisticTransferObject();

                    switch (item.FormID)
                    {
                        case (int)ScienceResearchTypeOfFormId.Application:
                            blockName = "政府采购";
                            projectType = item.FormValues.Split(Constant.SharpChar)[0].ToString();
                            break;
                        case (int)GoodSubjectTypeOfFormId.Application:
                            blockName = "政府采购";
                            projectType = item.FormValues.Split(Constant.SharpChar)[0].ToString();
                            break;
                        case (int)ResearchAwardTypeOfFormId.Application:
                            blockName = "科研成果";
                            projectType = item.FormValues.Split(Constant.SharpChar)[0].ToString();
                            break;
                        case (int)PaperPublishTypeOfFormId.Application:
                            blockName = "论文管理";
                            projectType = "论文管理";
                            break;
                        case (int)SubjectLeaderTypeOfFormId.Application:
                            blockName = "学科技术带头人";
                            projectType = item.FormValues.Split(Constant.SharpChar)[0].ToString();
                            break;
                        case (int)ScienceConferenceTypeOfFormId.Application:
                            blockName = "学术会议";
                            projectType = "学术会议";
                            break;
                    }
                    //获取项目模块
                    tempresult.Modeule = blockName;
                    //获取项目类型
                    tempresult.ProjectApprovalType = projectType;
                    //申请书个数和申请成功个数
                    if (item.ProjectStatus.ToString() == "BigProjectProcessing")
                    {
                        tempresult.ApplicationSuccessCount = 1;
                        tempresult.ApplicationCount = 0;
                    }
                    else
                    {
                        tempresult.ApplicationCount = 1;
                        tempresult.ApplicationSuccessCount = 0;
                    }

                    resultlist.Add(tempresult);
                }


            }
            return resultlist;
        }
    }
}
