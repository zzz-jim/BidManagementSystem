using ScientificResearch.DataTransferModel;
using ScientificResearch.DomainModel;
using ScientificResearch.IDataAccess;
using ScientificResearch.Utility.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScientificResearch.DataAccessImplement
{
    public class FundsManageProgramStatisticsRepository : IFundsManageProgramStatisticsRepository
    {
        public IList<FundsManageProgramStatisticsTransferObject> GetFundsManageProgramStatistics(Func<FundsManageProgramStatisticsTransferObject, bool> whereLambda, int formid)
        {
            IList<FundsManageProgramStatisticsTransferObject> result = new List<FundsManageProgramStatisticsTransferObject>();
            using (var context = new CSPostOAEntities())
            {
                var tempResult = context.ERPNWorkToDo.Where(p => p.ID >= 75 && p.FormID == formid && !p.ApplicationStatus.Contains("Ap")).OrderByDescending(p => p.ID).ToList();

                foreach (var item in tempResult)
                {
                    var valueArray = item.FormValues.Split(Constant.SharpChar);
                    var temp = new FundsManageProgramStatisticsTransferObject();
                    //序号todo：重排序
                    temp.Number = item.ID;
                    //项目名称
                    temp.ProjectName = item.WenHao;
                    //经费类型
                    temp.FundsType = item.FundsRecord.Count == 0 ? string.Empty : item.FundsRecord.FirstOrDefault().Type;

                    if (item.FormID == 1055) { 
                       //项目类型
                        temp.Programtype = "";
                        //申请人
                        temp.ApplyMan = valueArray[0];
                        //科室
                        temp.LocalDepartment = valueArray[1];
                    }
                    else if (item.FormID == 1052)
                    {
                        //项目类型
                        temp.Programtype = "";
                        //申请人
                        temp.ApplyMan = valueArray[5];
                        //科室
                        temp.LocalDepartment = valueArray[6];
                    }
                    else { 
                    //项目类型
                    temp.Programtype = valueArray[0];
                    //立项类型
                    //申请时间
                    //申请人
                    temp.ApplyMan = valueArray[3];
                    //科室
                    temp.LocalDepartment = valueArray[4];
                    }
                    //项目负责人
                    //项目参与人员
                    //概述
                    //上级拨款
                    temp.SuperiorFunds = item.ProjectRecord.Count == 0 ? 0 : item.ProjectRecord.FirstOrDefault().SuperiorFunds;
                    //院内拨款
                    temp.HospitalFunds = item.ProjectRecord.Count == 0 ? 0 : item.ProjectRecord.FirstOrDefault().HospitalFunds;
                    //项目总金额
                    temp.ProjectTotalFunds = (Convert.ToDouble(item.ProjectRecord.Count == 0 ? 0 : item.ProjectRecord.FirstOrDefault().SuperiorFunds) + Convert.ToDouble(item.ProjectRecord.Count == 0 ? 0 : item.ProjectRecord.FirstOrDefault().HospitalFunds));
                    //判断收支
                    temp.IsIncome = item.FundsRecord.Count == 0 ? null : item.FundsRecord.FirstOrDefault().IsIncome;
                    //收入
                    temp.Income = item.FundsRecord.Where(p => p.IsIncome == true).Sum(p => p.TotalPrice);
                    //支出
                    temp.Expend = item.FundsRecord.Where(p => p.IsIncome == false).Sum(p => p.TotalPrice);
                    //余额
                    temp.Balance = item.FundsRecord.Where(p => p.IsIncome == true).Sum(p => p.TotalPrice) - item.FundsRecord.Where(p => p.IsIncome == false).Sum(p => p.TotalPrice);
                    //项目确立时间
                    temp.EstablishTiem = item.ProjectRecord.Count == 0 ? System.DateTime.MinValue : item.ProjectRecord.FirstOrDefault().FundsTime;
                    //项目进行时间
                    temp.ProjecProcessTime = Convert.ToDateTime(item.TimeStr);
                    result.Add(temp);
                }
            }
            return result;
        }
    }
}
