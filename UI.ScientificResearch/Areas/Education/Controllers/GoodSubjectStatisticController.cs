using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

using ScientificResearch.Utility.Constants;
using ScientificResearch.BusinessLogicImplement;
using ScientificResearch.IBusinessLogic;
using ScientificResearch.ViewModel;
using ScientificResearch.DataTransferModel;
using UI.ScientificResearch.Extensions;
using ScientificResearch.Utility.Enums;
using PF.DomainModel.Identity;

namespace UI.ScientificResearch.Areas.Education.Controllers
{
    [CheckLogin]
    [Authorize(Roles = "普通用户")]
    public class GoodSubjectStatisticController : Controller
    {
        #region Private Service

        private IERPNFormService ERPNFormService;
        private IERPBuMenService ERPBuMenService;
        private IERPRiZhiService ERPRiZhiService;
        private IApplicationService ApplicationService;
        private IERPNWorkFlowService ERPNWorkFlowService;
        private IERPNWorkFlowNodeService ERPNWorkFlowNodeService;
        private IFundsRecordService FundsRecordService;
        private IProjectRecordService ProjectRecordService;

        private IStatisticService StatisticService;

        private IFundsManageProgramStatisticsService FundsManageProgramStatisticsService;

        private ISession MySession;

        #endregion

        #region Constructor

        public GoodSubjectStatisticController()
            : this(
                new ERPNFormServiceImplement(),
                new ERPBuMenServiceImplement(),
                new ERPRiZhiServiceImplement(),
                new ApplicationServiceImplement(),
                new ERPNWorkFlowServiceImplement(),
                new ERPNWorkFlowNodeServiceImplement(),
                new FundsRecordServiceImplement(),
                new ProjectRecordServiceImplement(),
                new StatisticServiceImplement(),
                new FundsManageProgramStatisticsImplement(),
                new SessionManager()
            )
        {
        }

        public GoodSubjectStatisticController(
            IERPNFormService eRPNFormService,
            IERPBuMenService eRPBuMenService,
            IERPRiZhiService eRPRiZhiService,
            IApplicationService applicationService,
            IERPNWorkFlowService eRPNWorkFlowService,
            IERPNWorkFlowNodeService eRPNWorkFlowNodeService,
            IFundsRecordService eFundsRecordService,
            IProjectRecordService eProjectRecordService,
            IStatisticService statisticService,
            IFundsManageProgramStatisticsService fundsManageProgramStatisticsService,
            ISession session)
        {
            this.ERPNFormService = eRPNFormService;
            this.ERPBuMenService = eRPBuMenService;
            this.ERPRiZhiService = eRPRiZhiService;
            this.ApplicationService = applicationService;
            this.ERPNWorkFlowService = eRPNWorkFlowService;
            this.ERPNWorkFlowNodeService = eRPNWorkFlowNodeService;
            this.FundsRecordService = eFundsRecordService;
            this.ProjectRecordService = eProjectRecordService;
            this.MySession = session;
            this.StatisticService = statisticService;
            this.FundsManageProgramStatisticsService = fundsManageProgramStatisticsService;
        }

        #endregion

        #region Private Field

        private string FundsCriteriaFundsName;
        private string FundsCriteriaIsIncome;
        private string FundsCriteriaFundsType;
        private DateTime FundsCriteriaStartTime;
        private DateTime FundsCriteriaEndTime;
        private string FundsCriteriaUserName;
        private string FundsCriteriaState;

        #endregion

        #region Get Action

        /// <summary>
        /// 工程项目导航首页统计分析容器页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ScienceStatisticContainer()
        {
            ViewBag.Module = "工程项目";
            ViewBag.Title = "统计分析";
            return View();
        }

        /// <summary>
        /// 日常经费统计
        /// </summary>
        /// <returns></returns>
        public ActionResult FundsList()
        {
            ViewBag.Module = "工程项目";
            ViewBag.Title = "日常经费记录";
            return View();
        }


        /// <summary>
        /// 资金管理项目统计
        /// </summary>
        /// <returns></returns>
        public ActionResult FundsManageProgramStatistics()
        {
            ViewBag.Module = "工程项目";
            ViewBag.Title = "项目统计";

            return View();
        }
        [HttpPost]
        /// <summary>
        /// 资金管理项目统计查询时主表
        /// </summary>
        /// <returns></returns>
        /// 
        //public ActionResult FundsManageProgramMasterStatistics(int pageIndex, int pageSize)
        //{
        //    string porgramename = Request.QueryString["porgramename"].ToString();
        //    string locationdepartment = Request.QueryString["locationdepartment"].ToString();
        //    string type = Request.QueryString["type"].ToString();
        //    string fundstype = Request.QueryString["fundstype"].ToString();
        //    string starttime = Request.QueryString["starttime"].ToString();
        //    string endtime = Request.QueryString["endtime"].ToString();
        //    string fundsoprator = Request.QueryString["fundsoprator"].ToString();
        //    //IList<ERPNWorkToDoTransferObject> mainData = new List<ERPNWorkToDoTransferObject>();
        //    IEnumerable<FundsManageProgramStatisticsTransferObject> mainData = new List<FundsManageProgramStatisticsTransferObject>();

        //    mainData = FundsManageProgramStatisticsService.GetFundsManageProgramStatistics(null);
        //    //页面第一次统计
        //    if (!string.IsNullOrEmpty(porgramename))
        //    {
        //        mainData = mainData.Where(x => x.ProjectName == porgramename);
        //    }
        //    if (!string.IsNullOrEmpty(locationdepartment))
        //    {
        //        mainData = mainData.Where(x => x.LocalDepartment == locationdepartment);
        //    }
        //    if (type != "全部")
        //    {
        //        if (type == "支出")
        //        {
        //            mainData = mainData.Where(x => x.IsIncome == false);
        //        }
        //        else if (type == "收入")
        //        {
        //            mainData = mainData.Where(x => x.IsIncome == true);
        //        }

        //    }
        //    if (fundstype != "全部")
        //    {
        //        if (fundstype == "日常经费")
        //        {
        //            mainData = mainData.Where(x => x.FundsType == fundstype);
        //        }
        //    }


        //    if (!string.IsNullOrEmpty(starttime) || !string.IsNullOrEmpty(endtime))
        //    {
        //        if (!string.IsNullOrEmpty(starttime) && string.IsNullOrEmpty(endtime))
        //        {
        //            DateTime temptime = Convert.ToDateTime(starttime);
        //            mainData = mainData.Where(x => x.ProjecProcessTime >= temptime);

        //        }
        //        else if (!string.IsNullOrEmpty(endtime) && string.IsNullOrEmpty(starttime))
        //        {
        //            DateTime temptime = Convert.ToDateTime(starttime);
        //            mainData = mainData.Where(x => x.ProjecProcessTime <= temptime.AddDays(1));
        //        }
        //        else if (!string.IsNullOrEmpty(endtime) && !string.IsNullOrEmpty(starttime))
        //        {
        //            DateTime starttemptime = Convert.ToDateTime(starttime);
        //            DateTime endtemptime = Convert.ToDateTime(endtime);
        //            mainData = mainData.Where(x => x.ProjecProcessTime >= starttemptime && x.ProjecProcessTime <= endtemptime);
        //        }
        //    }


        //    if (!string.IsNullOrEmpty(fundsoprator))
        //    {
        //        mainData = mainData.Where(x => x.ApplyMan.Contains(fundsoprator));
        //    }

        //    int totalcount = mainData.Count();
        //    mainData = mainData.Skip<FundsManageProgramStatisticsTransferObject>(pageSize * (pageIndex - 1)).Take<FundsManageProgramStatisticsTransferObject>(pageSize);

        //    return Json(new { data = mainData, total = totalcount }, JsonRequestBehavior.AllowGet);
        //    //return Json(mainData, JsonRequestBehavior.AllowGet);
        //}

        /// <summary>
        /// 资金管理项目统计查询时经费详情
        /// </summary>
        /// <returns></returns>
        public ActionResult FundsManageProgramStatisticsDetailStatistics(int id)
        {
            //var result = FundsRecordService.GetEntities(p => p.StateNow == "正在办理" && p.ShenPiUserList.Contains(Constant.USER_NAME) && p.ApplicationId == id).OrderByDescending(p => p.TimeStr).ToList();

            //IList<FundsRecordViewModel> resultList = new List<FundsRecordViewModel>();

            //foreach (var item in result)
            //{
            //    resultList.Add(item.ToViewModel());
            //}
            var projectRecordResult = ProjectRecordService.GetEntities(p => p.ApplicationId == id).FirstOrDefault();

            var result = new List<FundsRecordViewModel>();
            result.Add(new FundsRecordViewModel()
            {
                Name = "项目确定经费记录",
                Type = "上级拨款",
                TotalPrice = projectRecordResult.SuperiorFunds,
                IsIncome = true,
            });
            result.Add(new FundsRecordViewModel()
            {
                Name = "项目确定经费记录",
                Type = "院内拨款",
                TotalPrice = projectRecordResult.HospitalFunds,
                IsIncome = true,
            });

            var temp = FundsRecordService.GetEntities(p => p.ApplicationId == id).ToList();

            result.AddRange(temp.Select(x => x.ToViewModel()));

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 工程项目统计
        /// </summary>
        /// <returns></returns>
        public ActionResult ScienceProjectStatisticsAnalysis()
        {
            var result = StatisticService.GetScienceProjectStatistics(x => true, (int)GoodSubjectTypeOfFormId.Application);

            return View(result.Select(x => x.ToViewModel()));
        }

        /// <summary>
        /// 工程项目立项统计
        /// </summary>
        /// <returns></returns>
        public ActionResult ScienceProjectEstablishStatisticsAnalysis()
        {
            IList<ScienceProjectEstablishTimeStatisticsViewModel> resultList;
            var result = StatisticService.GetScienceProjectEstablishTimeStatistics(x => true);

            if (result.Count != 0)
            {
                resultList = result.Select(x => x.ToViewModel()).ToList();
            }
            else
            {
                resultList = new List<ScienceProjectEstablishTimeStatisticsViewModel>();
            }

            return View(resultList);
        }

        #endregion

        #region Post Action

        [HttpPost]
        public ActionResult DeleteFundsListByModelId(string modelId)
        {
            int id = Convert.ToInt16(modelId);//fundsrecordId
            FundsRecordTransferObject result = new FundsRecordTransferObject();
            result = FundsRecordService.GetEntityById(id);
            result.IsDeleted = true;
            bool update = FundsRecordService.UpdateFundsRecord(result);
            return View();
        }
        [HttpPost]
        public ActionResult FundsListStatistics(int page, int pageSize)
        {
            InitialTheSearchCriteria();
            string FundsCriteriaFundsName = Request.QueryString["fundsName"].ToString();
            string FundsCriteriaIsIncome = Request.QueryString["inAndExType"].ToString();
            string FundsCriteriaFundsType = Request.QueryString["fundsType"].ToString();
            string FundsCriteriaUserName = Request.QueryString["userName"].ToString();
            string FundsCriteriaState = Request.QueryString["fundsState"].ToString();
            DateTime FundsCriteriaStartTime = DateTime.MinValue;
            DateTime FundsCriteriaEndTime = DateTime.MaxValue;

            int totalPage = 0;

            IEnumerable<FundsRecordTransferObject> result = SearchFundsList(FundsCriteriaFundsName, FundsCriteriaIsIncome, FundsCriteriaFundsType, FundsCriteriaStartTime,
                FundsCriteriaEndTime, FundsCriteriaUserName, FundsCriteriaState, pageSize, page, ref totalPage);

            return Json(new { data = result, total = totalPage * pageSize }, JsonRequestBehavior.AllowGet);

            //return View(result.Select(x => x.ToViewModel()));
        }
        /// <summary>
        /// 工程项目统计
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public ActionResult ScienceProjectStatisticsAnalysis(FormCollection collection)
        {
            if (string.IsNullOrEmpty(collection["name"]))
            {
                var name = collection["name"].Trim();
                var result = StatisticService.GetScienceProjectStatistics(x => true, (int)GoodSubjectTypeOfFormId.Application);

                return View(result.Select(x => x.ToViewModel()));
            }
            else
            {
                var result = StatisticService.GetScienceProjectStatistics(x => true, (int)GoodSubjectTypeOfFormId.Application);

                return View(result.Select(x => x.ToViewModel()));
            }
        }

        #endregion

        #region Private Method

        /// <summary>
        /// Initail the searchCriteria from session of this controller
        /// </summary>
        private void InitialTheSearchCriteria()
        {
            if (MySession[SessionKeyEnum.FundsCriteriaUserName] != null)
            {
                FundsCriteriaUserName = MySession[SessionKeyEnum.FundsCriteriaUserName].ToString();
            }

            if (MySession[SessionKeyEnum.FundsCriteriaFundsName] != null)
            {
                FundsCriteriaFundsName = MySession[SessionKeyEnum.FundsCriteriaFundsName].ToString();
            }

            if (MySession[SessionKeyEnum.FundsCriteriaState] != null)
            {
                FundsCriteriaState = MySession[SessionKeyEnum.FundsCriteriaState].ToString();
            }
            else
            {
                FundsCriteriaState = Constant.All;
            }

            if (MySession[SessionKeyEnum.FundsCriteriaIsIncome] != null)
            {
                FundsCriteriaIsIncome = MySession[SessionKeyEnum.FundsCriteriaIsIncome].ToString();
            }
            else
            {
                FundsCriteriaIsIncome = Constant.All;
            }

            if (MySession[SessionKeyEnum.FundsCriteriaFundsType] != null)
            {
                FundsCriteriaFundsType = MySession[SessionKeyEnum.FundsCriteriaFundsType].ToString();
            }
            else
            {
                FundsCriteriaFundsType = Constant.All;
            }

            if (MySession[SessionKeyEnum.FundsCriteriaStartTime] != null)
            {
                FundsCriteriaStartTime = (DateTime)MySession[SessionKeyEnum.FundsCriteriaStartTime];
            }
            else
            {
                FundsCriteriaStartTime = Constant.MinTime;
            }

            if (MySession[SessionKeyEnum.FundsCriteriaEndTime] != null)
            {
                FundsCriteriaEndTime = (DateTime)MySession[SessionKeyEnum.FundsCriteriaEndTime];
            }
            else
            {
                FundsCriteriaEndTime = Constant.MaxTime;
            }
        }

        /// <summary>
        /// Initail the session of this controller
        /// </summary>
        private void InitialSession()
        {
            MySession[SessionKeyEnum.FundsCriteriaEndTime] = FundsCriteriaEndTime;
            MySession[SessionKeyEnum.FundsCriteriaFundsName] = FundsCriteriaFundsName;
            MySession[SessionKeyEnum.FundsCriteriaFundsType] = FundsCriteriaFundsType;
            MySession[SessionKeyEnum.FundsCriteriaIsIncome] = FundsCriteriaIsIncome;
            MySession[SessionKeyEnum.FundsCriteriaStartTime] = FundsCriteriaStartTime;
            MySession[SessionKeyEnum.FundsCriteriaState] = FundsCriteriaState;
            MySession[SessionKeyEnum.FundsCriteriaUserName] = FundsCriteriaUserName;
        }

        /// <summary>
        /// 查找经费记录
        /// </summary>
        /// <param name="fundsName">经费名称</param>
        /// <param name="isIncome">收入支出类型</param>
        /// <param name="fundsType">经费类型</param>
        /// <param name="startTime">经费查找开始时间</param>
        /// <param name="endTime">经费查找结束时间</param>
        /// <param name="userName">经费操作人</param>
        /// <param name="state">经费当前状态</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="totalPage">总页数</param>
        /// <returns></returns>
        private IEnumerable<FundsRecordTransferObject> SearchFundsList(string fundsName, string isIncome, string fundsType,
            DateTime startTime, DateTime endTime, string userName, string state, int pageSize, int pageIndex, ref int totalPage)
        {
            //    //var result = FundsRecordService.GetPageEntities(p => p.StateNow == "正在办理" && p.ShenPiUserList.Contains(Constant.USER_NAME)).OrderByDescending(p => p.TimeStr).ToList();

            IEnumerable<FundsRecordTransferObject> result = null;

            if (User.IsInRole(UserRoles.超级管理员.ToString())
                || User.IsInRole(UserRoles.账务科科长.ToString())
                || User.IsInRole(UserRoles.账务科科员.ToString()))
            {
                result = FundsRecordService.GetPageEntities(p =>
                    (string.IsNullOrEmpty(fundsName) ? true : (p.Name.Contains(fundsName)))
                    && (p.IsDeleted == true ? false : true)
                    && ((isIncome == Constant.All) ? true : p.IsIncome == (isIncome == "收入"))
                    && ((fundsType == Constant.All) ? true : (p.Type == fundsType))
                    && p.TimeStr.Value > startTime
                    && p.TimeStr.Value < endTime
                    && ((state == Constant.All) ? true : p.JieDianName == state)

                    && p.UserName == "admin",// User.Identity.Name,

                    ApplicationSortField.TimeStr_Desc.ToString(), pageSize, pageIndex, out totalPage);
            }
            else
            {
                // 普通科室
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    // Get sections
                    var userSeciton = MySession[SessionKeyEnum.SectionId];

                    if (userSeciton != null)
                    {
                        // Gets roles
                        var tempChiefRole = userSeciton + Constant.Chief;

                        if (User.IsInRole(tempChiefRole))
                        {
                            // 主任
                            // 找出本科室所有员工 
                            var usersOfCurrentSection = context.Sections.FirstOrDefault(x => x.Id == userSeciton).ApplicationUsers.Select(x => x.ApplicationUserId);
                            var userNamesOfCurrentSection = context.Users.Where(x => usersOfCurrentSection.Contains(x.Id)).Select(x => x.UserName);

                            result = FundsRecordService.GetPageEntities(p =>
                                (string.IsNullOrEmpty(fundsName) ? true : (p.Name.Contains(fundsName)))
                                && (p.IsDeleted == true ? false : true)
                                && ((isIncome == Constant.All) ? true : p.IsIncome == (isIncome == "收入"))
                                && ((fundsType == Constant.All) ? true : (p.Type == fundsType))
                                && p.TimeStr.Value > startTime
                                && p.TimeStr.Value < endTime
                                && ((state == Constant.All) ? true : p.JieDianName == state)
                                && userNamesOfCurrentSection.Contains(p.UserName),

                                ApplicationSortField.TimeStr_Desc.ToString(), Constant.PageSize, pageIndex, out totalPage);
                        }
                        else
                        {
                            // 普通员工
                            result = FundsRecordService.GetPageEntities(p => (
                                string.IsNullOrEmpty(fundsName) ? true : (p.Name.Contains(fundsName)))
                                 && (p.IsDeleted == true ? false : true)
                                && ((isIncome == Constant.All) ? true : p.IsIncome == (isIncome == "收入"))
                                && ((fundsType == Constant.All) ? true : (p.Type == fundsType))
                                && p.TimeStr.Value > startTime
                                && p.TimeStr.Value < endTime
                                && ((state == Constant.All) ? true : p.JieDianName == state)
                                && p.UserName == User.Identity.Name

                                ,
                                ApplicationSortField.TimeStr_Desc.ToString(), Constant.PageSize, pageIndex, out totalPage);
                        }
                    }
                    else
                    {
                        result = new List<FundsRecordTransferObject>();
                    }
                }
            }
            return result;
        }

        #endregion
    }
}