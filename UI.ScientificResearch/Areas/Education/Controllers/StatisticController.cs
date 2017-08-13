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
using System.Data;
using ScientificResearch.Utility.Helper;
using ScientificResearch.DomainModel;
using System.Data.Entity;
using System.Text;

namespace UI.ScientificResearch.Areas.Education.Controllers
{
    [CheckLogin]
    [Authorize(Roles = "普通用户")]
    public class StatisticController : Controller
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

        private IFundsThresholdService FundsThresholdService;

        private IFundsManageProgramStatisticsService FundsManageProgramStatisticsService;

        private IScienceResearchAmountOfSectionStatisticService ScienceResearchAmountOfSectionStatisticService;
        private IScienceResearchAmountOfFundStatisticService ScienceResearchAmountOfFundStatisticService;
        private IScienceResearchOfCapitalStatisticService ScienceResearchOfCapitalStatisticService;
        private IScienceResearchAmountOfApplicationStatisticService ScienceResearchAmountOfApplicationStatisticService;
        private ISession MySession;

        #endregion

        #region Constructor

        public StatisticController()
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
            new FundsThresholdServiceImplement(),
                new FundsManageProgramStatisticsImplement(),
                new ScienceResearchAmountOfSectionStatisticServiceImplement(),
                new ScienceResearchAmountOfFundStatisticServiceImplement(),
            new ScienceResearchOfCapitalStatisticServiceImplement(),
            new ScienceResearchAmountOfApplicationStatisticServiceImplement(),
                new SessionManager()

            )
        {
        }

        public StatisticController(
            IERPNFormService eRPNFormService,
            IERPBuMenService eRPBuMenService,
            IERPRiZhiService eRPRiZhiService,
            IApplicationService applicationService,
            IERPNWorkFlowService eRPNWorkFlowService,
            IERPNWorkFlowNodeService eRPNWorkFlowNodeService,
            IFundsRecordService eFundsRecordService,
            IProjectRecordService eProjectRecordService,
            IStatisticService statisticService,
            IFundsThresholdService eFundsThresholdService,
            IFundsManageProgramStatisticsService fundsManageProgramStatisticsService,
            IScienceResearchAmountOfSectionStatisticService scienceResearchAmountOfSectionStatisticService,
            IScienceResearchAmountOfFundStatisticService scienceResearchAmountOfFundStatisticService,
            IScienceResearchOfCapitalStatisticService scienceResearchOfCapitalStatisticService,
            IScienceResearchAmountOfApplicationStatisticService scienceResearchAmountOfApplicationStatisticService,
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
            this.FundsThresholdService = eFundsThresholdService;
            this.FundsManageProgramStatisticsService = fundsManageProgramStatisticsService;
            this.ScienceResearchAmountOfSectionStatisticService = scienceResearchAmountOfSectionStatisticService;
            this.ScienceResearchAmountOfFundStatisticService = scienceResearchAmountOfFundStatisticService;
            this.ScienceResearchAmountOfApplicationStatisticService = scienceResearchAmountOfApplicationStatisticService;
            this.ScienceResearchOfCapitalStatisticService = scienceResearchOfCapitalStatisticService;
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
        /// 日常经费统计
        /// </summary>
        /// <returns></returns>
        public ActionResult FundsList(string moduleName)
        {
            ViewBag.Module = "工程项目";
            ViewBag.Title = "日常经费记录";

            ViewBag.ModuleName = moduleName;

            return View();
        }
        /// <summary>
        /// 科室科研数量统计
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ScienceResearchAmountOfSectionStatistic()
        {
            ViewBag.Module = "工程项目";
            ViewBag.Title = "科室科研数量统计";
            //科室	类别	
            //类型	数量
            //住院医	科研	
            //省中医药科研课题	1
            //论文	
            //2
            //A临床论著	1
            //科技成果奖	

            IList<ScienceResearchAmountOfSectionStatisticTransferObject> resultList = new List<ScienceResearchAmountOfSectionStatisticTransferObject>();
            DateTime stattime = DateTime.MinValue;
            DateTime endtime = DateTime.MaxValue;
            resultList = ScienceResearchAmountOfSectionStatisticService.GetScienceResearchAmountOfSectionStatistics(stattime, endtime);//

            var groupedResult = resultList.GroupBy(x => x.DepartMentName);
            return View(groupedResult);
        }
        /// <summary>
        /// 科室科研数量统计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ScienceResearchAmountOfSectionStatistic(FormCollection collection)
        {
            ViewBag.Module = "工程项目";
            ViewBag.Title = "科室科研数量统计";
            if (collection["search"].ToString() != null)
            {
                //科室	类别	
                //类型	数量
                //住院医	科研	
                //省中医药科研课题	1
                //论文	
                //2
                //A临床论著	1
                //科技成果奖	

                IList<ScienceResearchAmountOfSectionStatisticTransferObject> resultList = new List<ScienceResearchAmountOfSectionStatisticTransferObject>();


                DateTime stattime = DateTime.MinValue;
                DateTime endtime = DateTime.MaxValue;
                if (collection["startTime"].ToString().Trim() != "")
                {
                    stattime = Convert.ToDateTime(collection["startTime"]);
                }
                if (collection["endTime"].ToString().Trim() != "")
                {
                    endtime = Convert.ToDateTime(collection["endTime"]).AddDays(1);
                }
                resultList = ScienceResearchAmountOfSectionStatisticService.GetScienceResearchAmountOfSectionStatistics(stattime, endtime);//

                var groupedResult = resultList.GroupBy(x => x.DepartMentName);
                return View(groupedResult);
            }
            else
            {
                return View();
            }
        }
        ///<summary>
        /// 科研申请书统计
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ScienceResearchAmountOfApplicationStatistic()
        {
            ViewBag.Module = "科研统计";
            ViewBag.Title = "科研申请书统计";
            //项目	项目类型	申请书(个)	申报成功(个)
            //学术会议	学术会议	1082	1060
            //总计	1082	1060
            IList<ScienceResearchAmountOfApplicationStatisticTransferObject> resultList = new List<ScienceResearchAmountOfApplicationStatisticTransferObject>();
            DateTime stattime = DateTime.MinValue;
            DateTime endtime = DateTime.MaxValue;
            resultList = ScienceResearchAmountOfApplicationStatisticService.GetScienceResearchAmountOfApplicationStatistics(stattime, endtime);//

            var groupedResult = resultList.GroupBy(x => x.Modeule.ToString());
            return View(groupedResult);

        }
        ///<summary>
        /// 科研申请书统计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ScienceResearchAmountOfApplicationStatistic(FormCollection collection)
        {
            ViewBag.Module = "工程项目";
            ViewBag.Title = "科研申请书统计";
            if (collection["search"].ToString() != null)
            {

                //项目	项目类型	申请书(个)	申报成功(个)
                //学术会议	学术会议	1082	1060
                //总计	1082	1060
                IList<ScienceResearchAmountOfApplicationStatisticTransferObject> resultList = new List<ScienceResearchAmountOfApplicationStatisticTransferObject>();
                DateTime stattime = DateTime.MinValue;
                DateTime endtime = DateTime.MaxValue;
                if (collection["startTime"].ToString().Trim() != "")
                {
                    stattime = Convert.ToDateTime(collection["startTime"]);
                }
                if (collection["endTime"].ToString().Trim() != "")
                {
                    endtime = Convert.ToDateTime(collection["endTime"]).AddDays(1);
                }
                resultList = ScienceResearchAmountOfApplicationStatisticService.GetScienceResearchAmountOfApplicationStatistics(stattime, endtime);//

                var groupedResult = resultList.GroupBy(x => x.Modeule.ToString());
                return View(groupedResult);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// 工程项目经费统计
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ScienceResearchAmountOfFundStatistic()
        {
            ViewBag.Module = "科研统计";
            ViewBag.Title = "工程项目经费统计";
            return View();
        }
        [HttpPost]
        public ActionResult ScienceResearchAmountOfFundStatistic(FormCollection collection)
        {
            if (collection["OutScienceResearchAmountOfFundlist"] != null)
            {
                string starttime = "";
                string endtime = "";
                string projectName ="";
                string departmentName = "";
                string reiburseName ="";
                string moduleName = "全部";

                IEnumerable<ScienceResearchAmountOfFundStatisticTransferObject> mainData = new List<ScienceResearchAmountOfFundStatisticTransferObject>();
                mainData = ScienceResearchAmountOfFundStatisticService.GetAllScienceResearchAmountOfFundStatistic(starttime, endtime, projectName,
            departmentName, reiburseName, moduleName);

                DataTable dt = new DataTable();
                string excelName = "科研经费统计列表";
                string fileName = DateTime.Now.ToString("yyyyMMddhhmmss");
                string path = System.Web.HttpContext.Current.Server.MapPath("~/UploadFiles/科研经费统计列表-" + fileName + ".xls");
                //Excel表头
                dt.Columns.Add("模块");
                dt.Columns.Add("科室");
                dt.Columns.Add("项目名称");
                dt.Columns.Add("申请人");
                dt.Columns.Add("经费名称");
                dt.Columns.Add("类型");
                dt.Columns.Add("金额");
                dt.Columns.Add("创建时间");
                //往datatable中填入内容
                foreach (var item in mainData)
                {
                    DataRow row = dt.NewRow();
                    row["模块"] = item.Modeule;
                    row["科室"] = item.DepartmentName;
                    row["项目名称"] = item.ProjectName;
                    row["申请人"] = item.ApplicationMan;
                    row["经费名称"] = item.RemburseName;
                    row["类型"] = item.OutOrIn;
                    row["金额"] = item.Money;
                    row["创建时间"] = item.CreateTime;
                    dt.Rows.Add(row);
                }
                //写入Excel
                try
                {
                    using (ExcelHelper exceHelper = new ExcelHelper(path))
                    {
                        int returnCount = exceHelper.DataTableToExcel(dt, excelName, true);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return this.File(path, "application/octet-stream", "科研经费统计列表" + fileName + ".xls");
            }
            return View();
        }
        [HttpPost]
        public ActionResult PrintScienceResearchAmountOfFundStatistic()
        {
            string starttime = "";
            string endtime = "";
            string projectName = "";
            string departmentName = "";
            string reiburseName = "";
            string moduleName = "全部";

            IEnumerable<ScienceResearchAmountOfFundStatisticTransferObject> mainData = new List<ScienceResearchAmountOfFundStatisticTransferObject>();
            mainData = ScienceResearchAmountOfFundStatisticService.GetAllScienceResearchAmountOfFundStatistic(starttime, endtime, projectName,
        departmentName, reiburseName, moduleName);
            StringBuilder builder = new StringBuilder();
            builder.Append("<div id='printArea'><table>");
            builder.Append("<tr>");
            builder.Append("<th>模块</th>");
            builder.Append("<th>学科</th>");
            builder.Append("<th>项目名称</th>");
            builder.Append("<th>申请人</th>");
            builder.Append("<th>经费名称</th>");
            builder.Append("<th>类型</th>");
            builder.Append("<th>金额</th>");
            builder.Append("<th>创建时间</th>");
            builder.Append("</tr>");
            foreach (var item in mainData)
            {
                builder.Append("<tr>");
                builder.Append("<td>" + item.Modeule + "</td>");
                builder.Append("<td>" + item.DepartmentName + "</td>");
                builder.Append("<td>" + item.ProjectName + "</td>");
                builder.Append("<td>" + item.ApplicationMan + "</td>");
                builder.Append("<td>" + item.RemburseName + "</td>");
                builder.Append("<td>" + item.OutOrIn + "</td>");
                builder.Append("<td>" + item.Money + "</td>");
                builder.Append("<td>" + item.CreateTime + "</td>");
                builder.Append("</tr>");
            }
            builder.Append("</table></div>");
            string htmlList = builder.ToString();
            return Json(htmlList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        /// <summary>
        /// 工程项目经费统计查询后的列表数据
        /// </summary>
        /// <returns></returns>
        public ActionResult ScienceResearchAmountOfFundStatisticList(int pageIndex, int pageSize)
        {
            int totalcount = 0;
            string starttime = Request.QueryString["startTime"].ToString();
            string endtime = Request.QueryString["endTime"].ToString();
            string projectName = Request.QueryString["projectName"].ToString();
            string departmentName = Request.QueryString["departmentName"].ToString();
            string reiburseName = Request.QueryString["remburseName"].ToString();
            string moduleName = Request.QueryString["moduleName"].ToString();

            IEnumerable<ScienceResearchAmountOfFundStatisticTransferObject> mainData = new List<ScienceResearchAmountOfFundStatisticTransferObject>();
            mainData = ScienceResearchAmountOfFundStatisticService.GetScienceResearchAmountOfFundStatistic(starttime, endtime, projectName,
            departmentName, reiburseName, moduleName, pageIndex, pageSize, out totalcount);

            return Json(new { data = mainData, total = totalcount }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 科研资金统计
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ScienceResearchOfCapitalStatistic()
        {
            ViewBag.Module = "科研统计";
            ViewBag.Title = "科研资金统计";
            return View();
        }
        [HttpPost]
        public ActionResult ScienceResearchOfCapitalStatistic(FormCollection collection)
        {
            if (collection["OutScienceResearchOfCapitalStatisticList"] != null)
            {
                string starttime = "";
                string endtime = "";
                string projectName = "";
                string moduleName = "全部";

                IEnumerable<ScienceResearchOfCapitalStatisticTransferObject> mainData = new List<ScienceResearchOfCapitalStatisticTransferObject>();
                mainData = ScienceResearchOfCapitalStatisticService.GetAllScienceResearchOfCapitalStatistic(starttime, endtime,
                projectName, moduleName);

                DataTable dt = new DataTable();
                string excelName = "科研资金统计列表";
                string fileName = DateTime.Now.ToString("yyyyMMddhhmmss");
                string path = System.Web.HttpContext.Current.Server.MapPath("~/UploadFiles/科研资金统计列表-" + fileName + ".xls");
                //Excel表头
                dt.Columns.Add("模块");
                dt.Columns.Add("项目名称");
                dt.Columns.Add("负责人");
                dt.Columns.Add("拨款时间");
                dt.Columns.Add("拨款金额");
                dt.Columns.Add("报销金额");
                //往datatable中填入内容
                foreach (var item in mainData)
                {
                    DataRow row = dt.NewRow();
                    row["模块"] = item.Modeule;
                    row["项目名称"] = item.ProjectName;
                    row["负责人"] = item.PrincipalMan;
                    row["拨款时间"] = item.AllocationOfTime;
                    row["拨款金额"] = item.AppropriationMoney;
                    row["报销金额"] = item.ReimbursementAmount;
                    dt.Rows.Add(row);
                }
                //写入Excel
                try
                {
                    using (ExcelHelper exceHelper = new ExcelHelper(path))
                    {
                        int returnCount = exceHelper.DataTableToExcel(dt, excelName, true);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return this.File(path, "application/octet-stream", "科研资金统计列表" + fileName + ".xls");
            }
            return View();
        }
        [HttpPost]
        public ActionResult PrintScienceResearchOfCapitalStatisticList()
        {
            string starttime = "";
            string endtime = "";
            string projectName = "";
            string moduleName = "全部";

            IEnumerable<ScienceResearchOfCapitalStatisticTransferObject> mainData = new List<ScienceResearchOfCapitalStatisticTransferObject>();
            mainData = ScienceResearchOfCapitalStatisticService.GetAllScienceResearchOfCapitalStatistic(starttime, endtime,
            projectName, moduleName);
            StringBuilder builder = new StringBuilder();
            builder.Append("<div id='printArea'><table>");
            builder.Append("<tr>");
            builder.Append("<th>模块</th>");
            builder.Append("<th>项目名称</th>");
            builder.Append("<th>负责人</th>");
            builder.Append("<th>拨款时间</th>");
            builder.Append("<th>拨款金额</th>");
            builder.Append("<th>报销金额</th>");
            builder.Append("</tr>");
            foreach(var item in mainData)
            {
                builder.Append("<tr>");
                builder.Append("<td>"+item.Modeule+"</td>");
                builder.Append("<td>" + item.ProjectName + "</td>");
                builder.Append("<td>" + item.PrincipalMan + "</td>");
                builder.Append("<td>"+item.AllocationOfTime+"</td>");
                builder.Append("<td>"+item.AppropriationMoney+"</td>");
                builder.Append("<td>"+item.ReimbursementAmount+"</td>");
                builder.Append("</tr>");
            }
            builder.Append("</table></div>");
            string htmlList = builder.ToString();
            return Json(htmlList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 科研资金统计列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ScienceResearchOfCapitalStatisticList(int pageIndex, int pageSize)
        {
            int totalcount = 0;
            string starttime = Request.QueryString["startTime"].ToString();
            string endtime = Request.QueryString["endTime"].ToString();
            string projectName = Request.QueryString["projectName"].ToString();
            string moduleName = Request.QueryString["moduleName"].ToString();

            IEnumerable<ScienceResearchOfCapitalStatisticTransferObject> mainData = new List<ScienceResearchOfCapitalStatisticTransferObject>();
            mainData = ScienceResearchOfCapitalStatisticService.GetScienceResearchOfCapitalStatistic(starttime, endtime,
            projectName, moduleName, pageIndex, pageSize, out  totalcount);

            return Json(new { data = mainData, total = totalcount }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 资金管理项目统计
        /// </summary>
        /// <returns></returns>
        public ActionResult FundsManageProgramStatistics(int formId)
        {
            ViewBag.Module = "工程项目";
            ViewBag.Title = "项目统计";

            ViewBag.formId = formId;
            return View();
        }

        /// <summary>
        /// 资金管理项目统计
        /// </summary>
        /// <returns></returns>
        public ActionResult BidWinnerNoticeList(int formId)
        {
            ViewBag.Module = "工程项目";
            ViewBag.Title = "项目统计";

            ViewBag.formId = formId;
            return View();
        }
        [HttpPost]
        /// <summary>
        /// 资金管理项目统计查询时主表
        /// </summary>
        /// <returns></returns>
        public ActionResult FundsManageProgramMasterStatistics(int pageIndex, int pageSize)
        {
            string porgramename = Request.QueryString["porgramename"].ToString();
            string locationdepartment = Request.QueryString["locationdepartment"].ToString();
            string type = Request.QueryString["type"].ToString();
            string fundstype = Request.QueryString["fundstype"].ToString();
            string starttime = Request.QueryString["starttime"].ToString();
            string endtime = Request.QueryString["endtime"].ToString();
            string fundsoprator = Request.QueryString["fundsoprator"].ToString();
            int formid = Convert.ToInt32(Request.QueryString["formid"]);
            IEnumerable<FundsManageProgramStatisticsTransferObject> mainData = new List<FundsManageProgramStatisticsTransferObject>();

            mainData = FundsManageProgramStatisticsService.GetFundsManageProgramStatistics(null,formid);
            //页面第一次统计
            if (!string.IsNullOrEmpty(porgramename))
            {
                mainData = mainData.Where(x => x.ProjectName == porgramename);
            }
           
            if (!string.IsNullOrEmpty(locationdepartment))
            {
                mainData = mainData.Where(x => x.LocalDepartment == locationdepartment);
            }
            if (type != "全部")
            {
                if (type == "支出")
                {
                    mainData = mainData.Where(x => x.IsIncome == false);
                }
                else if (type == "收入")
                {
                    mainData = mainData.Where(x => x.IsIncome == true);
                }

            }
            if (fundstype != "全部")
            {
                if (fundstype == "日常经费")
                {
                    mainData = mainData.Where(x => x.FundsType == fundstype);
                }
            }


            if (!string.IsNullOrEmpty(starttime) || !string.IsNullOrEmpty(endtime))
            {
                if (!string.IsNullOrEmpty(starttime) && string.IsNullOrEmpty(endtime))
                {
                    DateTime temptime = Convert.ToDateTime(starttime);
                    mainData = mainData.Where(x => x.ProjecProcessTime >= temptime);

                }
                else if (!string.IsNullOrEmpty(endtime) && string.IsNullOrEmpty(starttime))
                {
                    DateTime temptime = Convert.ToDateTime(starttime);
                    mainData = mainData.Where(x => x.ProjecProcessTime <= temptime.AddDays(1));
                }
                else if (!string.IsNullOrEmpty(endtime) && !string.IsNullOrEmpty(starttime))
                {
                    DateTime starttemptime = Convert.ToDateTime(starttime);
                    DateTime endtemptime = Convert.ToDateTime(endtime);
                    mainData = mainData.Where(x => x.ProjecProcessTime >= starttemptime && x.ProjecProcessTime <= endtemptime);
                }
            }


            if (!string.IsNullOrEmpty(fundsoprator))
            {
                mainData = mainData.Where(x => x.ApplyMan.Contains(fundsoprator));
            }

            int totalcount = mainData.Count();
            mainData = mainData.Skip<FundsManageProgramStatisticsTransferObject>(pageSize * (pageIndex - 1)).Take<FundsManageProgramStatisticsTransferObject>(pageSize);

            return Json(new { data = mainData, total = totalcount }, JsonRequestBehavior.AllowGet);
        }

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
                Description = "3384296",// 投标报价(元) 
                Comment ="第一名",// 名次
                Name = "四川宁沣建筑工程有限公司",
                Type = "3013048 ", // 经评审的投标价（元）
                TotalPrice = 92.77,//  综合评标得分
                JieDianID =1,//序号
            });
            result.Add(new FundsRecordViewModel()
            {
                Description = "3368265",// 投标报价(元) 
                Comment = "第二名",// 名次
                Name = "日昌（福建）集团有限公司",
                Type = "3013048 ", // 经评审的投标价（元）
                TotalPrice = 92.74,//  综合评标得分
                JieDianID = 1,//序号
            }); result.Add(new FundsRecordViewModel()
            {
                Description = "3313048",// 投标报价(元) 
                Comment = "第三名",// 名次
                Name = "四川省晨翕建筑工程有限公司",
                Type = "3313048 ", // 经评审的投标价（元）
                TotalPrice = 92.03,//  综合评标得分
                JieDianID = 3,//序号
            });

            var temp = FundsRecordService.GetEntities(p => p.ApplicationId == id).ToList();

            result.AddRange(temp.Select(x => x.ToViewModel()));

            return Json(result, JsonRequestBehavior.AllowGet);
        }

       

        /// <summary>
        /// 工程项目统计--工程项目
        /// </summary>
        /// <returns></returns>
        public ActionResult GoodSubjectStatisticsAnalysis()
        {
            var result = StatisticService.GetScienceProjectStatistics(x => true, (int)GoodSubjectTypeOfFormId.Application);
            //var groupedResult = result.Select(x => x.ToViewModel()).GroupBy(x => x.EstablishType);
            var groupedResult = result.Select(x => x.ToViewModel());
            return PartialView(groupedResult);
        }
       

        /// <summary>
        /// 工程项目统计--学科技术带头人
        /// </summary>
        /// <returns></returns>
        public ActionResult SubjectLeaderStatisticsAnalysis()
        {
            var result = StatisticService.GetScienceProjectStatistics(x => true, (int)SubjectLeaderTypeOfFormId.Application);
            //var groupedResult = result.Select(x => x.ToViewModel()).GroupBy(x => x.EstablishType);
            var groupedResult = result.Select(x => x.ToViewModel());
            return View("ScienceProjectStatisticsAnalysis", groupedResult);
        }

        /// <summary>
        /// 工程项目统计--科技成果
        /// </summary>
        /// <returns></returns>
        public ActionResult ResearchRewardStatisticsAnalysis()
        {
            var result = StatisticService.GetScienceProjectStatistics(x => true, (int)ResearchAwardTypeOfFormId.Application);
            //var groupedResult = result.Select(x => x.ToViewModel()).GroupBy(x => x.EstablishType);
            var groupedResult = result.Select(x => x.ToViewModel());
            return View("ResearchRewardStatisticsAnalysis", groupedResult);
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
            string FundsCriteriaType = Request.QueryString["projectType"].ToString();
           // string FundsCriteriaType = ViewBag.moduleName;
            //FundsCriteriaStartTime = DateTime.MinValue;
            //FundsCriteriaEndTime = DateTime.MaxValue;

            int totalPage = 0;

            IEnumerable<FundsRecordTransferObject> result = SearchFundsList(FundsCriteriaFundsName, FundsCriteriaIsIncome, FundsCriteriaFundsType, FundsCriteriaStartTime,
                FundsCriteriaEndTime, FundsCriteriaUserName, FundsCriteriaState, FundsCriteriaType, pageSize, page, ref totalPage);

            return Json(new { data = result, total = totalPage * pageSize }, JsonRequestBehavior.AllowGet);

            //return View(result.Select(x => x.ToViewModel()));
        }
        /// <summary>
        /// 资金管理的日常经费记录二级表，报销金额限制
        /// </summary>
        /// <returns></returns>
        
        public ActionResult ReimburseQuota(int id)
        {
            FundsRecordViewModel tempFundsRecordModel = FundsRecordService.GetEntityById(id).ToViewModel();
            int applicationId = tempFundsRecordModel.ApplicationId;
            string type = tempFundsRecordModel.Type.ToString();
            var erpnworkToDoModel = ApplicationService.GetEntityById(applicationId);

            int workFlowId = Convert.ToInt16(erpnworkToDoModel.WorkFlowID);

            var tempProjectRecordModel = ProjectRecordService.GetEntities(p=>p.ApplicationId==applicationId&&p.WorkflowId==workFlowId).FirstOrDefault();
            string projectType = tempProjectRecordModel.ProjectLevel;
            string moudulName = "";
            switch (workFlowId)
            {
                case (int)TypeOfWorkFlowId.Application:
                    moudulName = ModuleNameOfScienceResearch.ScienceResearch.ToString();
                    break;
                case (int)GoodSubjectTypeOfWorkflowId.Application:
                    moudulName = ModuleNameOfScienceResearch.GoodSubject.ToString();
                    break;
                case (int)SubjectLeaderTypeOfWorkflowId.Application:
                    moudulName = ModuleNameOfScienceResearch.SubjectLeader.ToString();
                    break;
                case (int)PaperPublishTypeOfWorkflowId.Application:
                    moudulName = ModuleNameOfScienceResearch.PaperPublish.ToString();
                    break;
                case (int)ScienceConferenceTypeOfWorkflowId.Application:
                    moudulName = ModuleNameOfScienceResearch.ScienceConference.ToString();
                    break;
                case (int)ResearchAwardTypeOfWorkflowId.Application:
                    moudulName = ModuleNameOfScienceResearch.ResearchAward.ToString();
                    break;
            }
            var result = new List<FundsThresholdViewModel>();
            result = FundsThresholdService.GetEntities(p => p.ModuleName == moudulName && p.ProjectType == projectType &&p.FundsType == type).Select(x => x.ToViewModel()).ToList();
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        /// <summary>
        /// 工程项目统计--工程项目统计分析列表导出
        /// </summary>
        /// <returns></returns>
        public ActionResult GoodSubjectStatisticsAnalysis(FormCollection collection)
        {
            if (collection["OutGoodSubjectStatistics"] != null)
            {
                var result = StatisticService.GetScienceProjectStatistics(x => true, (int)GoodSubjectTypeOfFormId.Application);
                //var groupedResult = result.Select(x => x.ToViewModel()).GroupBy(x => x.EstablishType);
                IEnumerable<ScienceProjectStatisticsViewModel> groupedResult = result.Select(x => x.ToViewModel());

                DataTable dt = new DataTable();
                string excelName = "工程项目统计分析列表";
                string fileName = DateTime.Now.ToString("yyyyMMddhhmmss");
                string path = System.Web.HttpContext.Current.Server.MapPath("~/UploadFiles/工程项目-统计分析-" + fileName + ".xls");
                //Excel表头
                dt.Columns.Add("序号");
                dt.Columns.Add("项目年份");
                dt.Columns.Add("项目类型");
                dt.Columns.Add("项目名称");
                dt.Columns.Add("立案类型");
                dt.Columns.Add("科室");
                dt.Columns.Add("项目负责人");
                dt.Columns.Add("下达经费");
                dt.Columns.Add("配套经费");
                dt.Columns.Add("费用总计");
                dt.Columns.Add("支出");
                dt.Columns.Add("余额");
                //序号
                int number = 1;
                //往datatable中填入内容
                foreach (var item in groupedResult)
                {

                    DataRow row = dt.NewRow();
                    row["序号"] = number++;
                    row["项目年份"] = item.Time;
                    row["项目类型"] = item.Type;
                    row["项目名称"] = item.Name;
                    row["立案类型"] = item.EstablishType;
                    row["科室"] = item.Department;
                    row["项目负责人"] = item.ProjectManager;
                    row["下达经费"] = item.ReleaseFunds;
                    row["配套经费"] = item.CounterpartFunds;
                    row["费用总计"] = item.TotalFunds;
                    row["支出"] = item.Payment;
                    row["余额"] = item.Balance;

                    dt.Rows.Add(row);
                }
                
                //写入Excel
                try
                {
                    using (ExcelHelper exceHelper = new ExcelHelper(path))
                    {
                        int returnCount = exceHelper.DataTableToExcel(dt, excelName, true);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return this.File(path, "application/octet-stream", "工程项目-统计分析-" + fileName + ".xls");
               
            }
            else
            {
                return PartialView();
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
            DateTime startTime, DateTime endTime, string userName, string state, string projectType, int pageSize, int pageIndex, ref int totalPage)
        {
            bool hasRolesFlag = HasRolesFlag();
            IEnumerable<FundsRecordTransferObject> result = null;

            //非普通用户
            if (hasRolesFlag)
            {
                result = FundsRecordService.GetPageEntities(p =>
                    (string.IsNullOrEmpty(fundsName) ? true : (p.Name.Contains(fundsName)))
                    && (p.IsDeleted == true ? false : true)
                    && ((isIncome == Constant.All) ? true : p.IsIncome == (isIncome == "收入"))
                    && ((fundsType == Constant.All) ? true : (p.Type == fundsType))
                    && p.TimeStr.Value > startTime
                    && p.TimeStr.Value < endTime
                    && ((state == Constant.All) ? true : p.JieDianName == state)
                    && p.ModuleName == projectType,
                    ApplicationSortField.TimeStr_Desc.ToString(), pageSize, pageIndex, out totalPage);
            }
            //普通用户
           else
           {
               result = FundsRecordService.GetPageEntities(p => (
                   string.IsNullOrEmpty(fundsName) ? true : (p.Name.Contains(fundsName)))
                    && (p.IsDeleted == true ? false : true)
                   && ((isIncome == Constant.All) ? true : p.IsIncome == (isIncome == "收入"))
                   && ((fundsType == Constant.All) ? true : (p.Type == fundsType))
                   && p.TimeStr.Value > startTime
                   && p.TimeStr.Value < endTime
                   && ((state == Constant.All) ? true : p.JieDianName == state)
                   && p.ModuleName == projectType
                   && p.UserName == User.Identity.Name,
                   ApplicationSortField.TimeStr_Desc.ToString(), Constant.PageSize, pageIndex, out totalPage);
           }
            return result;
        }

        #endregion

        #region Statistic Module


        /// <summary>
        /// 科研统计-科研申请书统计
        /// </summary>
        /// <returns></returns>
        public ActionResult ScienceResearchApplicationStatistic()
        {
            ViewBag.Module = "科研统计";
            ViewBag.Title = "日常经费记录";

            //项目	项目类型	申请书(个)	申报成功(个)
            //学术会议	学术会议	1082	1060
            //总计	1082	1060
            return View();
        }

        /// <summary>
        /// 科研经费统计
        /// </summary>
        /// <returns></returns>
        public ActionResult ScienceResearchFundsStatistic()
        {
            ViewBag.Module = "科研统计";
            ViewBag.Title = "日常经费记录";
            //            序号 
            //模块 
            //学科 
            //项目名称 
            //申请人 
            //经费名称 
            //类型 
            //金额 
            //创建时间 
            //1	
            //工程项目
            //泌尿外科
            //马鞍山地区女性尿失禁流行病学调查
            //包娟
            //马鞍山地区女性尿失禁流行病学调查--科研立项奖奖励
            //支出	0	
            //2012-11-09 10:12
            return View();
        }

        /// <summary>
        /// 科研申请书统计
        /// </summary>
        /// <returns></returns>
        public ActionResult ScienceResearchProjectFundsStatistic()
        {
            ViewBag.Module = "科研统计";
            ViewBag.Title = "日常经费记录";
            //序号 
            //模块 
            //项目名称 
            //负责人 
            //拨款时间 
            //拨款金额 
            //报销金额 
            //1	
            //科技成果奖
            //复合组织瓣移植修复四肢组织缺损的临床应用
            //王金华
            //2009-01-01
            //1000	
            return View();
        }
        [HttpGet]
        /// <summary>
        /// 科教项目统计
        /// </summary>
        /// <returns></returns>
        public ActionResult ScienceResearchProjectStatistic()
        {
            ViewBag.Module = "科研统计";
            ViewBag.Title = "日常经费记录";

            //        编号 
            // 申请时间 
            //项目名称 
            //模块 
            //申请人 
            //申请人所在科室 
            //1	2012-11-09	
            //马鞍山地区女性尿失禁流行病学调查
            //工程项目	包娟	泌尿外科
            return View();
        }
        [HttpPost]
        public ActionResult ScienceResearchProjectStatistic(FormCollection collection)
        {
            if (collection["OutScienceResearchProjectStatistic"] != null)
            {
                DateTime startTime = DateTime.MinValue;
                DateTime endTime = DateTime.MaxValue;
            
                bool hasRolesFlag = HasRolesFlag();
                
                IEnumerable<ERPNWorkToDoTransferObject> result;
                //非普通用户
                if (hasRolesFlag)
                {
                    result = ApplicationService.GetAllPageEntities(p =>
                        (p.IsDeleted == true ? false : true)
                        && p.TimeStr.Value > startTime
                        && p.TimeStr.Value < endTime
                        && (p.FormID == 1037 || p.FormID == 1046 || p.FormID == 1052 || p.FormID == 1055 || p.FormID == 1057 || p.FormID == 1076)
                         ,
                        ApplicationSortField.TimeStr_Desc.ToString());
                }
                //普通用户
                else
                {
                    result = ApplicationService.GetAllPageEntities(p =>
                       (p.IsDeleted == true ? false : true)
                        && p.TimeStr.Value > startTime
                        && p.TimeStr.Value < endTime
                        && (p.FormID == 1037 || p.FormID == 1046 || p.FormID == 1052 || p.FormID == 1055 || p.FormID == 1057 || p.FormID == 1076)
                        && p.UserName == User.Identity.Name,
                        ApplicationSortField.TimeStr_Desc.ToString());
                }
               var result1= result.Select(x=>x.ToProjectStatiticViewModel());
                DataTable dt = new DataTable();
                string excelName = "项目信息统计列表";
                string fileName = DateTime.Now.ToString("yyyyMMddhhmmss");
                string path = System.Web.HttpContext.Current.Server.MapPath("~/UploadFiles/项目信息统计列表-" + fileName + ".xls");
                //Excel表头
                dt.Columns.Add("申请时间");
                dt.Columns.Add("项目名称");
                dt.Columns.Add("模块");
                dt.Columns.Add("申请人");
                dt.Columns.Add("科室");
                //往datatable中填入内容
                foreach (var item in result1)
                {
                    DataRow row = dt.NewRow();
                    row["申请时间"] = item.ApplicationTime;
                    row["项目名称"] = item.ProjectName;
                    row["模块"] = item.ModuleName;
                    row["申请人"] = item.UserName;
                    row["科室"] = item.SectionName;
                    dt.Rows.Add(row);
                }
                //写入Excel
                try
                {
                    using (ExcelHelper exceHelper = new ExcelHelper(path))
                    {
                        int returnCount = exceHelper.DataTableToExcel(dt, excelName, true);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return this.File(path, "application/octet-stream", "项目信息统计列表" + fileName + ".xls");
            }
            return View();

        }
        [HttpPost]
        public ActionResult Printsearchrightdataoffundslist()
        {
            DateTime startTime = DateTime.MinValue;
            DateTime endTime = DateTime.MaxValue;

            bool hasRolesFlag = HasRolesFlag();

            IEnumerable<ERPNWorkToDoTransferObject> result;
            //非普通用户
            if (hasRolesFlag)
            {
                result = ApplicationService.GetAllPageEntities(p =>
                    (p.IsDeleted == true ? false : true)
                    && p.TimeStr.Value > startTime
                    && p.TimeStr.Value < endTime
                    && (p.FormID == 1037 || p.FormID == 1046 || p.FormID == 1052 || p.FormID == 1055 || p.FormID == 1057 || p.FormID == 1076)
                     ,
                    ApplicationSortField.TimeStr_Desc.ToString());
            }
            //普通用户
            else
            {
                result = ApplicationService.GetAllPageEntities(p =>
                   (p.IsDeleted == true ? false : true)
                    && p.TimeStr.Value > startTime
                    && p.TimeStr.Value < endTime
                    && (p.FormID == 1037 || p.FormID == 1046 || p.FormID == 1052 || p.FormID == 1055 || p.FormID == 1057 || p.FormID == 1076)
                    && p.UserName == User.Identity.Name,
                    ApplicationSortField.TimeStr_Desc.ToString());
            }
            var result1 = result.Select(x => x.ToProjectStatiticViewModel());
           
            StringBuilder builder = new StringBuilder();
            builder.Append("<div id='printArea'><table>");
            builder.Append("<tr>");
            builder.Append("<th>申请时间</th>");
            builder.Append("<th>项目名称</th>");
            builder.Append("<th>模块</th>");
            builder.Append("<th>申请人</th>");
            builder.Append("<th>科室</th>");
            builder.Append("</tr>");
            foreach (var item in result1)
            {
                builder.Append("<tr>");
                builder.Append("<td>" + item.ApplicationTime + "</td>");
                builder.Append("<td>" + item.ProjectName + "</td>");
                builder.Append("<td>" + item.ProjectName + "</td>");
                builder.Append("<td>" + item.ModuleName + "</td>");
                builder.Append("<td>" + item.UserName + "</td>");
                builder.Append("<td>" + item.SectionName + "</td>");
                builder.Append("</tr>");
            }
            builder.Append("</table></div>");
            string htmlList = builder.ToString();
            return Json(htmlList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ScirnceResearchProjectlistStatistic(int page, int pageSize)
        {
            InitialTheSearchProjectCriteria();
            //  DateTime FundsCriteriaStartTime = DateTime.MinValue;
            //  DateTime FundsCriteriaEndTime = DateTime.MaxValue;

            int totalPage = 0;
            IEnumerable<ERPNWorkToDoTransferObject> result = SearchProjectList(FundsCriteriaStartTime,
                FundsCriteriaEndTime, pageSize, page, ref totalPage);

            return Json(new { data = result.Select(x => x.ToProjectStatiticViewModel()), total = totalPage * pageSize }, JsonRequestBehavior.AllowGet);

            //return View(result.Select(x => x.ToViewModel()));
        }

        private void InitialTheSearchProjectCriteria()
        {


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
        private void InitialProjectSession()
        {
            MySession[SessionKeyEnum.FundsCriteriaEndTime] = FundsCriteriaEndTime;
            MySession[SessionKeyEnum.FundsCriteriaStartTime] = FundsCriteriaStartTime;
        }

        /// <summary>
        /// 查找经费记录
        /// </summary>
        /// <param name="startTime">经费查找开始时间</param>
        /// <param name="endTime">经费查找结束时间</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="totalPage">总页数</param>
        /// <returns></returns>
        private IEnumerable<ERPNWorkToDoTransferObject> SearchProjectList(
            DateTime startTime, DateTime endTime, int pageSize, int pageIndex, ref int totalPage)
        {
            
            bool hasRolesFlag = HasRolesFlag();
            IEnumerable<ERPNWorkToDoTransferObject> result;
            //非普通用户
            if (hasRolesFlag)
            {
                result = ApplicationService.GetPageEntities(p =>
                    (p.IsDeleted == true ? false : true)
                    && p.TimeStr.Value > startTime
                    && p.TimeStr.Value < endTime
                    && (p.FormID == 1037 || p.FormID == 1046 || p.FormID == 1052 || p.FormID == 1055 || p.FormID == 1057 || p.FormID == 1076)
                     ,
                    ApplicationSortField.TimeStr_Desc.ToString(), pageSize, pageIndex, out totalPage);
            }
            //普通用户
           else
           {
               result = ApplicationService.GetPageEntities(p =>
                  (p.IsDeleted == true ? false : true)
                   && p.TimeStr.Value > startTime
                   && p.TimeStr.Value < endTime
                   && (p.FormID == 1037 || p.FormID == 1046 || p.FormID == 1052 || p.FormID == 1055 || p.FormID == 1057 || p.FormID == 1076)
                   && p.UserName == User.Identity.Name,
                   ApplicationSortField.TimeStr_Desc.ToString(), Constant.PageSize, pageIndex, out totalPage);
           }
            return result;
        }

        #endregion


        public ActionResult AddFundsThreshold()
        {
            return View();
        }
        public ActionResult EidtFundsThreshold(int id)
        {
            using (var context = new CSPostOAEntities())
            {
                var entity = context.FundsThreshold.Find(id);
            
            return View(entity);
            }
        }

        public ActionResult DeleteFundsThreshole(int id)
        {
            bool result = false;

            using (var context = new CSPostOAEntities())
            {
                var model = context.FundsThreshold.Find(id);
                model.IsDeleted = true;
                context.FundsThreshold.Attach(model);
                context.Entry(model).State = EntityState.Modified;

                if (1 == context.SaveChanges())
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

            return RedirectToAction("FundsThresholdList");
        }
        public ActionResult FundsThresholdList()
        {
            using (var context = new CSPostOAEntities())
            {
                var result = context.FundsThreshold.Where(x => x.IsDeleted == false);

                ViewBag.Module = "经费管理";
                ViewBag.Title = "报销限额设置";

                return View(result.ToList<FundsThreshold>());
            }
        }
        [HttpPost]
        public ActionResult AddFundsThreshold(FundsThresholdViewModel model) 
        {
            model.IsDeleted = false;
            model.CreatedTime = DateTime.Now;
            model.CreatedBy = User.Identity.Name;
            int Add = FundsThresholdService.AddFundsThreshold(model.ToDataTransferObjectModel());
            //return RedirectToAction("FundsThresholdList");
            return View(model);
        }
        [HttpPost]
        public ActionResult EidtFundsThreshold(FundsThreshold model)
        {
            bool result = false;

            model.UpdatedBy = Constant.USER_NAME;
            model.UpdatedTime = DateTime.Now;

            using (var context = new CSPostOAEntities())
            {
                context.FundsThreshold.Attach(model);
                context.Entry(model).State = EntityState.Modified;

                if (1 == context.SaveChanges())
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

            return RedirectToAction("FundsThresholdList");
        }
        /// <summary>
        /// 判断登录者角色是否有权限查看申请书列表和过程记录列表内容
        /// </summary>
        /// <param name="ourDepartment">本科室主任</param>
        /// <param name="typeOfWorkFlowId">WorkFlowId</param>
        /// <returns></returns>
        public bool HasRolesFlag()
        {
            //判断登录者是否有除普通用户的权限，还有其他更高级的权限
            bool hasRolesFlag = false;
            if (User.IsInRole(UserRoles.普通用户.ToString()))
            {
                using (ApplicationDbContext userManager = new ApplicationDbContext())
                {
                    var allRolesList = userManager.Roles.ToList();

                    foreach (var item in allRolesList)
                    {
                        if (item.Name == UserRoles.普通用户.ToString())
                        {
                            continue;
                        }
                        else
                        {
                            hasRolesFlag = User.IsInRole(item.Name);
                            if (hasRolesFlag == true)
                            {
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }
            }
            else
            {

            }
            return hasRolesFlag;
        }
    }
}