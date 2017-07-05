using System;
using System.Collections.Generic;
using System.Linq;

using ScientificResearch.DataTransferModel;
using ScientificResearch.DomainModel;
using ScientificResearch.IDataAccess;
using ScientificResearch.Utility.Enums;
using ScientificResearch.Utility.Constants;

namespace ScientificResearch.DataAccessImplement
{
    public class StatisticRepository : IStatisticRepository
    {
        public IList<ScienceProjectStatisticsTransferObject> GetScienceProjectStatistics(Func<ScienceProjectStatisticsTransferObject, bool> whereLambda, int formId)
        {
            Dictionary<string, string> keyValuePair = new Dictionary<string, string>();
            Func<ScienceProjectStatisticsTransferObject, bool> exp = whereLambda;
            IList<ScienceProjectStatisticsTransferObject> result = new List<ScienceProjectStatisticsTransferObject>();

            using (var context = new CSPostOAEntities())
            {
                var AplicationWritingStatus = ApplicationStatus.AplicationWriting.ToString();
                var ApplicationSubmittedStatus = ApplicationStatus.ApplicationApproving.ToString();
                var ApplicationApprovingStatus = ApplicationStatus.ApplicationApproving.ToString();
                var ApplicationApprovedStatus = ApplicationStatus.ApplicationApproved.ToString();

                var tempResult = context.ERPNWorkToDo.Where(x => x.FormID == formId
                    && x.ApplicationStatus != AplicationWritingStatus
                    && x.ApplicationStatus != ApplicationSubmittedStatus
                    && x.ApplicationStatus != ApplicationApprovingStatus
                    && x.ApplicationStatus != ApplicationApprovedStatus
                    );

                //result = tempResult.Select(p => new ScienceProjectStatisticsTransferObject() { 

                //}).Where(x=>true).ToList();

                foreach (var item in tempResult)
                {
                    AddKeyValueToDictionary(keyValuePair, item);

                    result.Add(new ScienceProjectStatisticsTransferObject()
                    {
                        // 年份
                        Time = item.TimeStr.HasValue ? item.TimeStr.Value : DateTime.MinValue,
                        // 项目名称
                        Name = item.WenHao,

                        Type = keyValuePair[ApplicationFormKeys.项目类型.ToString()],
                        EstablishType = keyValuePair[ApplicationFormKeys.立项类型.ToString()],
                        Department = keyValuePair[ApplicationFormKeys.科室.ToString()],
                        ProjectManager = keyValuePair[ApplicationFormKeys.项目负责人.ToString()],
                        TeamMemberList = keyValuePair[ApplicationFormKeys.项目参与人员.ToString()],

                        // 下达经费, 上级拨款 
                        ReleaseFunds = item.ProjectRecord.Count == 0 ? 0 : item.ProjectRecord.FirstOrDefault().SuperiorFunds,//item.FundsRecord. ,

                        // 配套经费, 院内拨款
                        CounterpartFunds = item.ProjectRecord.Count == 0 ? 0 : item.ProjectRecord.FirstOrDefault().HospitalFunds,

                        // 支出经费
                        Payment = item.FundsRecord.Where(x => x.IsIncome == false).Sum(x => x.TotalPrice),// < 0

                        // 余额经费
                        // ReleaseFunds + CounterpartFunds + Payment = Balance
                        Balance = 0,// To assign the value in the extension.
                    });
                }

                return result;
            }
        }

        /// <summary>
        /// 将ERPNWorkToDo对象的formKeys和formValues放到keyValuePair字典中
        /// </summary>
        /// <param name="keyValuePair"></param>
        /// <param name="source"></param>
        private static void AddKeyValueToDictionary(Dictionary<string, string> keyValuePair, ERPNWorkToDo source)
        {
            var keysArray = source.FormKeys.Split(Constant.SharpChar);
            var valuesArray = source.FormValues.Split(Constant.SharpChar);

            for (int length = keysArray.Length, i = 0; i < length; i++)
            {
                if (keyValuePair.ContainsKey(keysArray[i]))
                {
                    keyValuePair[keysArray[i]] = valuesArray[i];
                }
                else
                {
                    keyValuePair.Add(keysArray[i], valuesArray[i]);
                }
            }
        }

        /// <summary>
        /// 科研立项时间按年份统计
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public IList<ScienceProjectEstablishTimeStatisticsTransferObject> GetScienceProjectEstablishTimeStatistics(Func<ScienceProjectEstablishTimeStatisticsTransferObject, bool> whereLambda)
        {
            IList<ScienceProjectEstablishTimeStatisticsTransferObject> result = new List<ScienceProjectEstablishTimeStatisticsTransferObject>();

            using (var context = new CSPostOAEntities())
            {
                var AplicationWritingStatus = ApplicationStatus.AplicationWriting.ToString();
                var ApplicationSubmittedStatus = ApplicationStatus.ApplicationApproving.ToString();
                var ApplicationApprovingStatus = ApplicationStatus.ApplicationApproving.ToString();
                var ApplicationApprovedStatus = ApplicationStatus.ApplicationApproved.ToString();

                var tempResult = context.ProjectRecord;

                var groupedYear = tempResult.GroupBy(x => x.CreatedTime.Value.Year);

                var CityLevel = TypeOfProjectLevel.市级.ToString();
                var CountyLevel = TypeOfProjectLevel.局级.ToString();
                var CountryLevel = TypeOfProjectLevel.国家级.ToString();
                var HospitalLevel = TypeOfProjectLevel.院级.ToString();
                var ProvinceLevel = TypeOfProjectLevel.省级.ToString();

                foreach (var item in groupedYear)
                {
                    result.Add(new ScienceProjectEstablishTimeStatisticsTransferObject()
                    {
                        Year = item.Key,
                        CityLevel = item.Count(x => x.ProjectLevel == CityLevel),
                        CountyLevel = item.Count(x => x.ProjectLevel == CountyLevel),
                        CountryLevel = item.Count(x => x.ProjectLevel == CountryLevel),
                        HospitalLevel = item.Count(x => x.ProjectLevel == HospitalLevel),
                        ProvinceLevel = item.Count(x => x.ProjectLevel == ProvinceLevel),
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// 论文统计分析
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public IList<PaperPublishStatisticsTransferObject> GetPaperPublishStatistics(Func<PaperPublishStatisticsTransferObject, bool> whereLambda)
        {
            IList<PaperPublishStatisticsTransferObject> result = new List<PaperPublishStatisticsTransferObject>();

            using (var context = new CSPostOAEntities())
            {
                var AplicationWritingStatus = ApplicationStatus.AplicationWriting.ToString();
                var ApplicationSubmittedStatus = ApplicationStatus.ApplicationApproving.ToString();
                var ApplicationApprovingStatus = ApplicationStatus.ApplicationApproving.ToString();
                var ApplicationApprovedStatus = ApplicationStatus.ApplicationApproved.ToString();

                var tempResult = context.PaperMagazine;

                var groupedYear = tempResult.GroupBy(x => x.CreateTime.Value.Year);

                var Superjournal = TypeOfPaperLevel.特I类期刊.ToString();
                var Onejournal = TypeOfPaperLevel.I类期刊.ToString();
                var Twojournal = TypeOfPaperLevel.II类期刊.ToString();
                var Threejournal = TypeOfPaperLevel.III类期刊.ToString();

                foreach (var item in groupedYear)
                {
                    result.Add(new PaperPublishStatisticsTransferObject()
                    {
                        Year = item.Key,
                        Superjournal = item.Count(x => x.Level == Superjournal),
                        Onejournal = item.Count(x => x.Level == Onejournal),
                        Twojournal = item.Count(x => x.Level == Twojournal),
                        Threejournal = item.Count(x => x.Level == Threejournal),
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// 学术会议统计分析
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <param name="formId"></param>
        /// <returns></returns>
        public IList<ScienceConferenceStatisticsTransferObject> GetScienceConferenceStatistics(Func<ScienceConferenceStatisticsTransferObject, bool> whereLambda, int formId)
        {
           
            IList<ScienceConferenceStatisticsTransferObject> resultList= new List<ScienceConferenceStatisticsTransferObject>();
            using (var context = new CSPostOAEntities())
            {
                var AplicationWritingStatus = ScienceConferenceApplicationStatus.AplicationWriting.ToString();
                var ApplicationSubmittedStatus = ScienceConferenceApplicationStatus.ApplicationApproving.ToString();
                var ApplicationApprovingStatus = ScienceConferenceApplicationStatus.ApplicationRejected.ToString();

                var tempResult = context.ERPNWorkToDo.Where(x => x.FormID == formId
                   && x.ApplicationStatus != AplicationWritingStatus
                    && x.ApplicationStatus != ApplicationSubmittedStatus
                    && x.ApplicationStatus != ApplicationApprovingStatus
                   );
                
                foreach (var item in tempResult)
                {
                    double allMoney = 0;
                    ScienceConferenceStatisticsTransferObject ScienceConferenceStatisticsModel = new ScienceConferenceStatisticsTransferObject();
                    int applicatinId = item.ID;
                     var formValuesArry=item.FormValues.Split(Constant.SharpChar);
                     allMoney += Convert.ToDouble(formValuesArry[7])+Convert.ToDouble(formValuesArry[8]);
                    var fundsrecordsModel = context.FundsRecord.Where(p => p.ApplicationId == applicatinId).ToList();
                    foreach (var item1 in fundsrecordsModel)
                    {
                        int fundsRecordId = item.ID;
                        allMoney += item1.TotalPrice;
                        var travelModel = context.TravelFundsDetail.Where(p => p.FundsRecordId == fundsRecordId).ToList();
                        foreach (var itme2 in travelModel)
                        {
                            allMoney += (double)itme2.OtherFee + (double)itme2.HotelFee+(double)itme2.TransportationFee;
                        }
                    }
                    //科室
                    ScienceConferenceStatisticsModel.Department = formValuesArry[1];
                    //总的经费
                    ScienceConferenceStatisticsModel.Funds = allMoney;

                    //参会身份
                      string Students = ScienceConferenceFormKeys.学员.ToString();
                      string Member = ScienceConferenceFormKeys.委员.ToString();
                      string Others = ScienceConferenceFormKeys.其他.ToString();
                      string No = ScienceConferenceFormKeys.无.ToString();
                      if (formValuesArry[9] == ScienceConferenceFormKeys.学员.ToString())
                      {
                          ScienceConferenceStatisticsModel.Students = 1;
                      }
                     else{
                        ScienceConferenceStatisticsModel.Students = 0;
                      }
                      if (formValuesArry[9] == ScienceConferenceFormKeys.委员.ToString())
                      {
                          ScienceConferenceStatisticsModel.Member = 1;
                      }
                      else
                      {
                          ScienceConferenceStatisticsModel.Member = 0;
                      }
                      if (formValuesArry[9] == ScienceConferenceFormKeys.其他.ToString())
                      {
                          ScienceConferenceStatisticsModel.Others = 1;
                      }
                      else
                      {
                          ScienceConferenceStatisticsModel.Others = 0;
                      }
                      if (formValuesArry[9] == ScienceConferenceFormKeys.无.ToString())
                      {
                          ScienceConferenceStatisticsModel.No = 1;
                      }
                      else
                      {
                          ScienceConferenceStatisticsModel.No = 0;
                      }
                      
                    resultList.Add(ScienceConferenceStatisticsModel);
                }
                
            }
            return resultList ;
        }
    }
}
