using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using ScientificResearch.BusinessLogicImplement;
using ScientificResearch.DataTransferModel;
using ScientificResearch.IBusinessLogic;
using ScientificResearch.Utility.Constants;
using ScientificResearch.Utility.Enums;
using ScientificResearch.Utility.Helper;
using ScientificResearch.ViewModel;
using PF.DomainModel.Identity;
using PF.DomainModel;
using UI.ScientificResearch.Extensions;
using System.Data;
using ScientificResearch.DomainModel;
using UI.ScientificResearch.Models;
using System.Text;
using ScientificResearch.IDataAccess;
using ScientificResearch.DataAccessImplement;

namespace UI.ScientificResearch.Areas.Education.Controllers
{
    [CheckLogin]
    [Authorize(Roles = "普通用户")]
    public class ApprovalController : Controller
    {
        #region Private Service

        private IERPNFormService ERPNFormService;
        private IERPBuMenService ERPBuMenService;
        private IERPRiZhiService ERPRiZhiService;
        private IApplicationService ApplicationService;
        private IERPNWorkFlowService ERPNWorkFlowService;
        private IERPNWorkFlowNodeService ERPNWorkFlowNodeService;
        private IFundsRecordService FundsRecordService;
        private ITravelFundsDetailService TravelFundsDetailService;
        private IProjectRecordService ProjectRecordService;
        private IProjectBonusCreditService ProjectBonusCreditService;
        private IStatisticService StatisticService;
        private IFundsThresholdService FundsThresholdService;
        private IProjectBidSectionRepository ProjectBidSectionService;

        private ISession MySession;

        #endregion

        #region Private Field

        private string SearchCriteriaProjectName;
        private string SearchCriteriaProjectStatus;
        private string SearchCriteriaIsLocked;
        private DateTime SearchCriteriaStartTime;
        private DateTime SearchCriteriaEndTime;

        #endregion

        #region Constructor

        public ApprovalController()
            : this(
                new ERPNFormServiceImplement(),
                new ERPBuMenServiceImplement(),
                new ERPRiZhiServiceImplement(),
                new ApplicationServiceImplement(),
                new ERPNWorkFlowServiceImplement(),
                new ERPNWorkFlowNodeServiceImplement(),
                new FundsRecordServiceImplement(),
                new TravelFundsDetailServiceImplement(),
                new ProjectRecordServiceImplement(),
                new ProjectBonusCreditServiceImplement(),
                new StatisticServiceImplement(),
                new FundsThresholdServiceImplement(),
                new ProjectBidSectionRepository(),
                new SessionManager()
            )
        {
        }

        public ApprovalController(
            IERPNFormService eRPNFormService,
            IERPBuMenService eRPBuMenService,
            IERPRiZhiService eRPRiZhiService,
            IApplicationService applicationService,
            IERPNWorkFlowService eRPNWorkFlowService,
            IERPNWorkFlowNodeService eRPNWorkFlowNodeService,
            IFundsRecordService eFundsRecordService,
            ITravelFundsDetailService eTravelFundsDetailService,
            IProjectRecordService eProjectRecordService,
            IProjectBonusCreditService eProjectBonusCreditService,
            IStatisticService statisticService,
            IFundsThresholdService eFundsThresholdService,
            IProjectBidSectionRepository projectBidSectionService,
            ISession session
            )
        {

            this.ERPNFormService = eRPNFormService;
            this.ERPBuMenService = eRPBuMenService;
            this.ERPRiZhiService = eRPRiZhiService;
            this.ApplicationService = applicationService;
            this.ERPNWorkFlowService = eRPNWorkFlowService;
            this.ERPNWorkFlowNodeService = eRPNWorkFlowNodeService;
            this.FundsRecordService = eFundsRecordService;
            this.TravelFundsDetailService = eTravelFundsDetailService;
            this.ProjectRecordService = eProjectRecordService;
            this.ProjectBonusCreditService = eProjectBonusCreditService;
            this.StatisticService = statisticService;
            this.FundsThresholdService = eFundsThresholdService;
            this.ProjectBidSectionService = projectBidSectionService;
            this.MySession = session;

        }

        #endregion

        #region Get Action

        /// <summary>
        /// 工程项目导航首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Module = "工程项目";
            ViewBag.Title = "简介";

            return RedirectToAction("ApplicationList");

            //return View();
        }

        /// <summary>
        /// 工程项目导航首页待办事宜容器页面
        /// </summary>
        /// <returns></returns>
        public ActionResult WorkContainer()
        {
            ViewBag.Module = "工程项目";
            ViewBag.Title = "待办事宜";

            return View();
        }

        /// <summary>
        /// 工程项目导航首页资金管理容器页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ScienceFundsManageContainer()
        {
            ViewBag.Module = "工程项目";
            ViewBag.Title = "中标通知书";

            return View();
        }
        /// <summary>
        /// 工程项目导航首页统计分析
        /// </summary>
        /// <returns></returns>
        public ActionResult StatisticManageContainer()
        {
            ViewBag.Module = "工程项目";
            ViewBag.Title = "统计分析";

            return View();
        }
        /// <summary>
        /// 创建申请书
        /// </summary>
        /// <returns></returns>
        public ActionResult SubmitApplication(string id)
        {
            int formId = (int)EngineeringProjectTypeOfFormId.Application;
            int workflowId = (int)TypeOfWorkFlowId.Application;
            ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();
            ERPNWorkFlowNodeTransferObject currentNode;

            //加载表单内容
            var formModel = ERPNFormService.GetEntityById(formId).ToViewModel();

            //获取当前表单对应的工作数据列
            string[] formItemArray = formModel.ItemsList.Split(Constant.SplitChar);

            if (id != null)//保存或上报后的view
            {
                int nworkid = Convert.ToInt32(id);
                model = ApplicationService.GetEntityById(nworkid).ToViewModel();
                currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.ID == model.JieDianID).First();
            }
            else//新建申请书的view
            {
                model.WorkFlowID = workflowId;
                model.FormID = formId;

                //设置上传的附件为空
                // ZWL.Common.PublicMethod.CheckSession();
                // 自动生成 编号
                //model.BeiYong1 = CommonHelper.GenerateProjectNumber(ApplicationType.ScienceResearch);
                string Number = CommonHelper.GenerateProjectNumber(ApplicationType.ScienceResearch);

                string content = formModel.ContentStr;
                if (content.Contains(""))
                {
                    string oldvalue = "Text309803372";
                    string newvalue = @"Text309803372"" value=""" + Number + @"""";
                    content = content.Replace(oldvalue, newvalue);
                }

                //加载表单内容
                model.FormContent = content;
                model.FormContent = model.FormContent.Replace(Constant.MacroSectionString, MySession[SessionKeyEnum.SectionName].ToString());
                model.FormContent = model.FormContent.Replace(Constant.MacroUserNameString, User.Identity.Name);

                ////绑定工作名称
                var workFlowModel = ERPNWorkFlowService.GetEntityById(workflowId);
                model.WorkName = User.Identity.Name + Constant.DoubleHyphenString + workFlowModel.WorkFlowName;

                //绑定下一节点
                currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == workflowId && p.NodeAddr == Constant.MacroStartString).First();
                model.JieDianID = currentNode.ID;
                model.JieDianName = currentNode.NodeName;
            }

            //批量设置字段的可写、保密属性
            ViewBag.PiLiangSet = CommonHelper.SetTheWriteAndHiddenField(currentNode.CanWriteSet, currentNode.SecretSet, formItemArray);

            return View(model);
        }

        /// <summary>
        /// 新建招标公告
        /// </summary>
        /// <returns></returns>
        public ActionResult AddTenderNotice(string id)
        {
            int formId = (int)EngineeringProjectTypeOfFormId.TenderNotice;
            int workflowId = (int)TypeOfWorkFlowId.Application;
            ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();
            ERPNWorkFlowNodeTransferObject currentNode;

            //加载表单内容
            var formModel = ERPNFormService.GetEntityById(formId).ToViewModel();

            //获取当前表单对应的工作数据列
            string[] formItemArray = formModel.ItemsList.Split(Constant.SplitChar);

            if (id != null)//保存或上报后的view
            {
                int nworkid = Convert.ToInt32(id);
                model = ApplicationService.GetEntityById(nworkid).ToViewModel();
                currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.ID == model.JieDianID).First();
            }
            else//新建申请书的view
            {
                model.WorkFlowID = workflowId;
                model.FormID = formId;

                //设置上传的附件为空
                // ZWL.Common.PublicMethod.CheckSession();
                // 自动生成 编号
                //model.BeiYong1 = CommonHelper.GenerateProjectNumber(ApplicationType.ScienceResearch);
                string Number = CommonHelper.GenerateProjectNumber(ApplicationType.ScienceResearch);

                string content = formModel.ContentStr;
                if (content.Contains(""))
                {
                    string oldvalue = "Text309803372";
                    string newvalue = @"Text309803372"" value=""" + Number + @"""";
                    content = content.Replace(oldvalue, newvalue);
                }

                //加载表单内容
                model.FormContent = content;
                model.FormContent = model.FormContent.Replace(Constant.MacroSectionString, MySession[SessionKeyEnum.SectionName].ToString());
                model.FormContent = model.FormContent.Replace(Constant.MacroUserNameString, User.Identity.Name);



                ////绑定工作名称
                var workFlowModel = ERPNWorkFlowService.GetEntityById(workflowId);
                model.WorkName = User.Identity.Name + Constant.DoubleHyphenString + workFlowModel.WorkFlowName;

                //绑定下一节点
                currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == workflowId && p.NodeAddr == Constant.MacroStartString).First();
                model.JieDianID = currentNode.ID;
                model.JieDianName = currentNode.NodeName;
            }

            //批量设置字段的可写、保密属性
            ViewBag.PiLiangSet = CommonHelper.SetTheWriteAndHiddenField(currentNode.CanWriteSet, currentNode.SecretSet, formItemArray);

            return View(model);
        }

        /// <summary>
        /// 新建中标公示
        /// </summary>
        /// <returns></returns>
        public ActionResult AddBidWinnerNotice(string id)
        {
            int formId = (int)EngineeringProjectTypeOfFormId.BidWinnerNotice;
            int workflowId = (int)TypeOfWorkFlowId.Application;
            ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();
            ERPNWorkFlowNodeTransferObject currentNode;

            //加载表单内容
            var formModel = ERPNFormService.GetEntityById(formId).ToViewModel();

            //获取当前表单对应的工作数据列
            string[] formItemArray = formModel.ItemsList.Split(Constant.SplitChar);

            if (id != null)//保存或上报后的view
            {
                int nworkid = Convert.ToInt32(id);
                model = ApplicationService.GetEntityById(nworkid).ToViewModel();
                currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.ID == model.JieDianID).First();
            }
            else//新建申请书的view
            {
                model.WorkFlowID = workflowId;
                model.FormID = formId;

                //设置上传的附件为空
                // ZWL.Common.PublicMethod.CheckSession();
                // 自动生成 编号
                //model.BeiYong1 = CommonHelper.GenerateProjectNumber(ApplicationType.ScienceResearch);
                string Number = CommonHelper.GenerateProjectNumber(ApplicationType.ScienceResearch);

                string content = formModel.ContentStr;
                if (content.Contains(""))
                {
                    string oldvalue = "Text309803372";
                    string newvalue = @"Text309803372"" value=""" + Number + @"""";
                    content = content.Replace(oldvalue, newvalue);
                }

                //加载表单内容
                model.FormContent = content;
                model.FormContent = model.FormContent.Replace(Constant.MacroSectionString, MySession[SessionKeyEnum.SectionName].ToString());
                model.FormContent = model.FormContent.Replace(Constant.MacroUserNameString, User.Identity.Name);



                ////绑定工作名称
                var workFlowModel = ERPNWorkFlowService.GetEntityById(workflowId);
                model.WorkName = User.Identity.Name + Constant.DoubleHyphenString + workFlowModel.WorkFlowName;

                //绑定下一节点
                currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == workflowId && p.NodeAddr == Constant.MacroStartString).First();
                model.JieDianID = currentNode.ID;
                model.JieDianName = currentNode.NodeName;
            }

            //批量设置字段的可写、保密属性
            ViewBag.PiLiangSet = CommonHelper.SetTheWriteAndHiddenField(currentNode.CanWriteSet, currentNode.SecretSet, formItemArray);

            return View(model);
        }

        /// <summary>
        /// 中标通知列表查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult BidWinnerNoticeList()
        {
            ViewBag.Module = "工程项目";
            ViewBag.Title = "中标通知";
            return View();
        }

        /// <summary>
        /// Get://显示审批申请书
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ApplicationAgree(string id, string nextaction)
        {
            ERPNWorkToDoViewModel model = ApplicationService.GetEntityById(Convert.ToInt32(id)).ToViewModel();
            //判断驳回或继续审批
            bool isRejected = Convert.ToBoolean(model.IsRejected);

            ERPNWorkFlowNodeTransferObject currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.ID == model.JieDianID).First();

            //加载表单内容
            var formModel = ERPNFormService.GetEntityById(model.FormID.Value);

            //获取当前表单对应的工作数据列
            string[] formItemArray = formModel.ItemsList.Split(Constant.SplitChar);

            ViewBag.ApprovalStep = model.JieDianName;
            ViewBag.Id = id;
            //批量设置字段的可写、保密属性
            ViewBag.PiLiangSet = CommonHelper.SetTheWriteAndHiddenField(currentNode.CanWriteSet, currentNode.SecretSet, formItemArray);

            // 设置审批流程的所有结点和当前结点
            var list = ERPNWorkFlowNodeService.GetEntities(x => x.WorkFlowID == model.WorkFlowID).OrderBy(p => p.NodeSerils).ToList();
            ViewBag.ApprovalWorkflowNode = list;
            ViewBag.CurrentNodeSerils = currentNode.NodeSerils;
            ViewBag.allCount = list.Count;
            bool isRightRoles = IsRightRoles(id);
            ViewBag.isRightPersonApproval = isRightRoles;
            return View(model);
        }
        /// <summary>
        /// 申请书被驳回
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ApplicationRejected(string id)
        {
            var model = ApplicationService.GetEntityById(Convert.ToInt32(id));

            ERPNWorkFlowNodeTransferObject currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.ID == model.JieDianID).First();

            //加载表单内容
            var formModel = ERPNFormService.GetEntityById(model.FormID.Value);

            //获取当前表单对应的工作数据列
            string[] formItemArray = formModel.ItemsList.Split(Constant.SplitChar);
            ViewBag.ApprovalStep = model.JieDianName;
            ViewBag.Id = id;
            //批量设置字段的可写、保密属性
            ViewBag.PiLiangSet = CommonHelper.SetTheWriteAndHiddenField(currentNode.CanWriteSet, currentNode.SecretSet, formItemArray);

            return View(model.ToViewModel());
        }

        /// <summary>
        /// 项目确立显示
        /// </summary>
        /// <returns></returns>
        public ActionResult ProjectEstablish(string id, string nextaction)
        {
            int applicaionId = Convert.ToInt32(id);

            var resultOfEstablish = ProjectRecordService.GetEntities(p => p.ApplicationId == applicaionId && p.IsTemporary == false && p.IsDeleted == false);

            if (resultOfEstablish.Count > 0)
            {
                ViewBag.Id = id;
                ProjectRecordViewModel projectModel = new ProjectRecordViewModel();
                projectModel = ProjectRecordService.GetEntities(p => p.ApplicationId == applicaionId).FirstOrDefault().ToViewModel();

                ERPNWorkToDoViewModel model = ApplicationService.GetEntityById(applicaionId).ToViewModel();
                projectModel.Application = model;

                return View("ProjectEstablishDetail", projectModel);
            }

            //点击保存后的view
            if (nextaction == "save")
            {
                ProjectRecordViewModel projectModel = new ProjectRecordViewModel();
                projectModel = ProjectRecordService.GetEntities(p => p.ApplicationId == applicaionId).FirstOrDefault().ToViewModel();
                projectModel.CreatedTime = DateTime.Now;

                ERPNWorkToDoViewModel model = ApplicationService.GetEntityById(applicaionId).ToViewModel();
                projectModel.Application = model;
                ViewBag.Id = id;//ERPNWorkToDo的ID
                ViewBag.act = "save";

                return View(projectModel);
            }
            //点击上报时的view
            else
            {
                ViewBag.Id = id;//ERPNWorkToDo的ID
                ViewBag.act = "reported";
                var result = ProjectRecordService.GetEntities(p => p.ApplicationId == applicaionId).ToList();            //todo：项目确立是否已经填写的状态判断
                if (result.Count > 0)//显示已建项目确立,详情或保存
                {
                    ProjectRecordViewModel projectModel = new ProjectRecordViewModel();
                    ERPNWorkToDoViewModel model = ApplicationService.GetEntityById(applicaionId).ToViewModel();

                    projectModel = result.FirstOrDefault().ToViewModel();
                    projectModel.Application = model;
                    return View(projectModel);

                    //return View("ProjectEstablishDetail", result.FirstOrDefault().ToViewModel());

                }//新建项目确立
                else
                {
                    ProjectRecordViewModel projectModel = new ProjectRecordViewModel();
                    ERPNWorkToDoViewModel model = ApplicationService.GetEntityById(applicaionId).ToViewModel();
                    projectModel.Application = model;
                    projectModel.ApplicationId = model.NWorkToDoID;
                    projectModel.WorkflowId = model.WorkFlowID.Value;
                    projectModel.CreatedTime = DateTime.Now;

                    return View(projectModel);
                }
            }
        }

        /// <summary>
        /// 合同记录显示
        /// </summary>
        /// <returns></returns>
        public ActionResult SignContract(string id, string nextaction)
        {
            ERPNWorkFlowNodeTransferObject currentNode;
            int applicaionId = Convert.ToInt32(id);
            //点击保存后的view
            if (nextaction == "save")
            {
                ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();
                model = ApplicationService.GetEntities(p => p.ID == applicaionId).FirstOrDefault().ToViewModel();
                ViewBag.Id = model.ApplicationId;
                ViewBag.act = "save";
                return View(model);
            }
            ////点击修改数据后的view
            //else if (nextaction == "updatedata")
            //{
            //    ViewBag.Id = id;
            //    ViewBag.act = "updatedata";

            //    ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();
            //    model = ApplicationService.GetEntities(p => p.ID == applicaionId).FirstOrDefault().ToViewModel();
            //    return View(model);
            //}
            //点击上报时的view
            else
            {
                int formId = (int)EngineeringProjectTypeOfFormId.Contract;
                int workflowId = (int)TypeOfWorkFlowId.Contract;
                //加载表单内容
                var temerpnformmodel = ERPNFormService.GetEntityById(formId).ToViewModel();

                //获取当前表单对应的工作数据列
                string[] formItemArray = temerpnformmodel.ItemsList.Split(Constant.SplitChar);

                ViewBag.Id = id;
                ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();

                var result = ApplicationService.GetEntities(p => p.ApplicationId == applicaionId && p.FormID == (int)EngineeringProjectTypeOfFormId.Contract).ToList();

                if (result.Count > 0)//已经填写了合同记录,显示改条合同记录
                {
                    model = result.FirstOrDefault().ToViewModel();
                    currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.ID == model.JieDianID).First();
                    FundsThresholdViewModel fundmodel = new FundsThresholdViewModel();
                    var fundresult = FundsThresholdService.GetEntities(p => p.NWorkToDoID == applicaionId && p.ModuleName == "ScienceResearch").ToList();
                    ViewBag.fundresultlist = fundresult;
                }
                else //新建合同记录
                {
                    ViewBag.act = "reported";
                    var appresult = ApplicationService.GetEntities(p => p.ID == applicaionId).ToList();
                    string projectname = appresult[0].WenHao;//项目名称
                    var keyvaluearry = appresult[0].FormValues.Split(Constant.SharpChar);
                    string projecttype = keyvaluearry[0].ToString();//项目类型
                    var projectresult = ProjectRecordService.GetEntities(p => p.ApplicationId == applicaionId).ToList();
                    string projectestablishtime = projectresult[0].CreatedTime.ToString();

                    model.WorkFlowID = workflowId;
                    model.FormID = formId;
                    model.ApplicationId = applicaionId;

                    string content = temerpnformmodel.ContentStr;
                    if (content.Contains("Text1009653833") && content.Contains("Text307349525") && content.Contains("Date609234174"))//项目名称
                    {
                        string oldvalue1 = "Text1009653833";
                        string newvalue1 = @"Text1009653833"" value=""" + projectname + @"""";
                        content = content.Replace(oldvalue1, newvalue1);

                        string oldvalue2 = "Text307349525";
                        string newvalue2 = @"Text307349525"" value=""" + projecttype + @"""";
                        content = content.Replace(oldvalue2, newvalue2);

                        string oldvalue3 = "Date609234174";
                        string newvalue3 = @"Date609234174"" value=""" + projectestablishtime + @"""";
                        content = content.Replace(oldvalue3, newvalue3);
                    }

                    model.FormContent = content;

                    ////绑定工作名称
                    var temperpnworkflowmodel = ERPNWorkFlowService.GetEntityById(workflowId);
                    model.WorkName = User.Identity.Name + Constant.DoubleHyphenString + temperpnworkflowmodel.WorkFlowName;
                    //绑定下一节点
                    currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == workflowId && p.NodeAddr == Constant.MacroStartString).First();
                    model.JieDianID = currentNode.ID;
                    model.JieDianName = currentNode.NodeName;
                }
                //批量设置字段的可写、保密属性
                ViewBag.PiLiangSet = CommonHelper.SetTheWriteAndHiddenField(currentNode.CanWriteSet, currentNode.SecretSet, formItemArray);
                return View(model);
            }
        }

        /// <summary>
        /// 过程记录显示
        /// </summary>
        /// <returns></returns>
        public ActionResult ProcessRecords(string id, string nextaction)
        {
            int workToDoId = Convert.ToInt32(id);
            var workToDoModel = ApplicationService.GetEntityById(workToDoId).ToViewModel();

            if (workToDoModel == null)
            {
                return RedirectToAction("Error");
            }

            if (workToDoModel.ApplicationId == 0)
            {
                // 说明这个Id是application id, 新建过程记录
                ViewBag.Id = workToDoId;
                int applicationid = workToDoId;

                int formId = (int)EngineeringProjectTypeOfFormId.ProcessRecord;
                int workflowId = (int)TypeOfWorkFlowId.ProcessRecord;

                #region 数据准备

                ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();
                ERPNWorkFlowNodeTransferObject currentNode;
                var appresult = ApplicationService.GetEntities(p => p.ID == applicationid).ToList();
                string projectname = workToDoModel.BeiYong1;//课题编号
                string name = workToDoModel.WenHao;//项目名称
                var keyvaluearry = workToDoModel.FormValues.Split(Constant.SharpChar);
                string projectestablishtime = keyvaluearry[2].ToString();//申请时间

                model.WorkFlowID = workflowId;
                model.FormID = formId;
                model.ApplicationId = applicationid;
                //加载表单内容
                var temerpnformmodel = ERPNFormService.GetEntityById(formId).ToViewModel();

                //获取当前表单对应的工作数据列
                string[] formItemArray = temerpnformmodel.ItemsList.Split(Constant.SplitChar);
                string content = temerpnformmodel.ContentStr;
                if (content.Contains("Text397670573") && content.Contains("Text1900333495"))//项目名称
                {
                    string oldvalue1 = "Text397670573";
                    string newvalue1 = @"Text397670573"" value=""" + name + @"""";
                    content = content.Replace(oldvalue1, newvalue1);

                    string oldvalue2 = "Text1900333495";
                    string newvalue2 = @"Text1900333495"" value=""" + projectname + @"""";
                    content = content.Replace(oldvalue2, newvalue2);
                }
                model.FormContent = content;

                //绑定工作名称
                var temperpnworkflowmodel = ERPNWorkFlowService.GetEntityById(workflowId);
                model.WorkName = User.Identity.Name + "--" + temperpnworkflowmodel.WorkFlowName;

                ////绑定工作名称
                var workFlowModel = ERPNWorkFlowService.GetEntityById(workflowId);
                model.WorkName = User.Identity.Name + Constant.DoubleHyphenString + workFlowModel.WorkFlowName;
                //绑定下一节点
                currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == workflowId && p.NodeAddr == Constant.MacroStartString).First();
                model.JieDianID = currentNode.ID;
                model.JieDianName = currentNode.NodeName;

                //批量设置字段的可写、保密属性
                ViewBag.PiLiangSet = CommonHelper.SetTheWriteAndHiddenField(currentNode.CanWriteSet, currentNode.SecretSet, formItemArray);
                #endregion
                return View(model);
            }
            else
            {
                // 说明这个Id是过程记录的 id，根据id得到当前过程记录的详情
                ViewBag.Id = workToDoModel.ApplicationId;

                return View(workToDoModel);
            }
        }

        /// <summary>
        /// 过程记录审批
        /// </summary>
        /// <returns></returns>
        public ActionResult processAgree(string id, string nextaction)
        {
            ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();

            int id1 = Convert.ToInt32(id);//过程记录的ID
            model = ApplicationService.GetEntityById(id1).ToViewModel();
            //判断驳回或继续审批

            bool isRejected = Convert.ToBoolean(model.IsRejected);
            bool isTemporary = Convert.ToBoolean(model.IsTemporary);

            //驳回
            if (isRejected || isTemporary)
            {
                if (isTemporary && model.IsRejected == null)
                {
                    return RedirectToAction("ProcessRecords", new { id = id });
                }
                else
                {
                    return RedirectToAction("ProcessRecordsRejected", new { id = id });
                }
            }
            //审批
            else
            {
                model.NWorkToDoID = id1;

                ERPNWorkFlowNodeTransferObject currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.ID == model.JieDianID).First();

                //  model.ApplicationId = id1;
                //加载表单内容
                var formModel = ERPNFormService.GetEntityById(model.FormID.Value);

                //获取当前表单对应的工作数据列
                string[] formItemArray = formModel.ItemsList.Split(Constant.SplitChar);

                ViewBag.ApprovalStep = model.JieDianName;
                ViewBag.Id = id;

                //批量设置字段的可写、保密属性
                ViewBag.PiLiangSet = CommonHelper.SetTheWriteAndHiddenField(currentNode.CanWriteSet, currentNode.SecretSet, formItemArray);

                // 设置审批流程的所有结点和当前结点
                var list = ERPNWorkFlowNodeService.GetEntities(x => x.WorkFlowID == model.WorkFlowID).OrderBy(p => p.NodeSerils).ToList();
                ViewBag.ApprovalWorkflowNode = list;
                ViewBag.CurrentNodeSerils = currentNode.NodeSerils;
                ViewBag.allCount = list.Count;
                bool isRightRoles = IsRightRoles(id);
                ViewBag.isRightPersonApproval = isRightRoles;
                return View(model);
            }
        }
        /// <summary>
        /// 过程记录审批被驳回
        /// </summary>
        /// <returns></returns>
        public ActionResult ProcessRecordsRejected(string id)
        {
            ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();

            int id1 = Convert.ToInt32(id);//过程记录的ID
            model = ApplicationService.GetEntityById(id1).ToViewModel();
            //判断驳回或继续审批

            bool isRejected = Convert.ToBoolean(model.IsRejected);
            bool isTemporary = Convert.ToBoolean(model.IsTemporary);

            model.NWorkToDoID = id1;

            ERPNWorkFlowNodeTransferObject currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.ID == model.JieDianID).First();

            //加载表单内容
            var formModel = ERPNFormService.GetEntityById(model.FormID.Value);

            //获取当前表单对应的工作数据列
            string[] formItemArray = formModel.ItemsList.Split(Constant.SplitChar);

            ViewBag.ApprovalStep = model.JieDianName;
            ViewBag.Id = id;
            //批量设置字段的可写、保密属性
            ViewBag.PiLiangSet = CommonHelper.SetTheWriteAndHiddenField(currentNode.CanWriteSet, currentNode.SecretSet, formItemArray);

            // 设置审批流程的所有结点和当前结点
            ViewBag.ApprovalWorkflowNode = ERPNWorkFlowNodeService.GetEntities(x => x.WorkFlowID == model.WorkFlowID).ToList();
            ViewBag.CurrentNodeSerils = currentNode.NodeSerils;

            return View(model);
        }
        /// <summary>
        /// 费用报销单显示
        /// </summary>
        /// <returns></returns>
        public ActionResult ReimburseProcess(string id, string nextaction)
        {
            int applicaionId = Convert.ToInt32(id);
            ViewBag.Id = id;

            FundsRecordViewModel model = new FundsRecordViewModel();

            //点击保存后的view
            if (nextaction == "save")
            {
                var fundsModel = FundsRecordService.GetAllById(applicaionId);

                var todoModel = ApplicationService.GetEntityById(fundsModel.ApplicationId);

                //string projectType = todoModel.FormValues.Split('#')[1];
                ViewBag.act = "save";
                ///获取经费记录类型
                using (var context = new CSPostOAEntities())
                {
                    ViewBag.FundsType = (from h in context.FundsThreshold
                                         where (h.IsDeleted == false && h.ModuleName == ModuleNameOfScienceResearch.ScienceResearch.ToString() && h.NWorkToDoID == applicaionId)

                                         select new SelectListItem()
                                         {
                                             Text = h.FundsType,
                                             Value = h.FundsType,
                                         }).ToList();
                }

                model.CreatedTime = DateTime.Now;
                model = FundsRecordService.GetEntityById(applicaionId).ToViewModel();
                return View(model);
            }
            //点击上报时的view
            else
            {
                var todoModel = ApplicationService.GetEntityById(applicaionId);

                string projectType = todoModel.FormValues.Split('#')[1];

                ///获取经费记录类型
                using (var context = new CSPostOAEntities())
                {
                    ViewBag.FundsType = (from h in context.FundsThreshold
                                         where (h.IsDeleted == false && h.ModuleName == ModuleNameOfScienceResearch.ScienceResearch.ToString() && h.NWorkToDoID == applicaionId)

                                         select new SelectListItem()
                                         {
                                             Text = h.FundsType,
                                             Value = h.FundsType,
                                         }).ToList();
                }
                ERPNWorkFlowNodeTransferObject currentNode;

                int workflowId = (int)TypeOfWorkFlowId.FeeReimbursement;
                int formId = (int)EngineeringProjectTypeOfFormId.FeeReimbursement;

                //加载表单内容
                var formModel = ERPNFormService.GetEntityById(formId).ToViewModel();

                //获取当前表单对应的工作数据列
                string[] formItemArray = formModel.ItemsList.Split(Constant.SplitChar);

                model.CreatedTime = DateTime.Now;

                ERPNWorkToDoViewModel todomodel = ApplicationService.GetEntityById(applicaionId).ToViewModel();
                model.ApplicationId = todomodel.NWorkToDoID;
                model.WorkflowId = workflowId;
                model.ProjectName = todomodel.WenHao;
                model.UserName = User.Identity.Name;

                //批量设置字段的可写、保密属性
                string PiLiangSet = "";
                //设置上传的附件为空
                //绑定下一节点
                string jiedianid = string.Empty;
                var temperpnworkflownodemodel = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == workflowId && p.NodeAddr == Constant.MacroStartString).ToList();
                string[] NextStrList = temperpnworkflownodemodel[0].NextNode.Split(',');
                for (int i = 0; i < NextStrList.Length; i++)
                {
                    //根据节点序号和WorkFlowID获取节点
                    var temperpnworkflownodemodel1 = ERPNWorkFlowNodeService.GetEntities(p => p.NodeSerils == NextStrList[i].ToString() && p.WorkFlowID == workflowId).ToList();
                    jiedianid = temperpnworkflownodemodel1[0].ID.ToString();
                    //根据序号和workflowID获得节点ID
                    model.JieDianID = int.Parse(jiedianid);
                    model.JieDianName = temperpnworkflownodemodel1[0].NodeName;
                }
                //根据选择的节点，绑定人员等信息。
                //当前开始节点是否有查看、编辑、删除按钮？当前按钮属性
                string NowNodeID = temperpnworkflownodemodel[0].ID.ToString();

                int nodeid = int.Parse(NowNodeID);
                var MyJieDianNow = ERPNWorkFlowNodeService.GetEntityById(nodeid);
                //获取当前表单对应的工作数据列
                //获取当前节点的可写权限、保密权限
                string CanWriteStr = MyJieDianNow.CanWriteSet;
                string SecretStr = MyJieDianNow.CanWriteSet;

                //批量设置字段的可写、保密属性
                ViewBag.PiLiangSet = CommonHelper.SetTheWriteAndHiddenField(MyJieDianNow.CanWriteSet, MyJieDianNow.CanWriteSet, formItemArray);

                return View(model);
            }
        }

        /// <summary>
        /// 费用审批单
        ///// </summary>
        /// <returns></returns>
        public ActionResult ReimburseAgree(string id, string nextaction)
        {
            FundsRecordViewModel model = new FundsRecordViewModel();
            //批量设置字段的可写、保密属性

            int id1 = Convert.ToInt32(id);

            model = FundsRecordService.GetAllById(id1).ToViewModel();

            if (model.TravelFundsList.Count > 0)
            {
                ViewBag.reimburse = "travel";
            }
            else
            {
                ViewBag.reimburse = "funds";
            }

            //判断驳回或继续审批
            bool isRejected = Convert.ToBoolean(model.IsRejected);
            bool isTemporary = Convert.ToBoolean(model.IsTemporary);
            //驳回
            if (isRejected || isTemporary)
            {
                if (isTemporary && model.IsRejected == null)
                {
                    return RedirectToAction("ReimburseProcess", new { id = id });
                }
                else
                {
                    return RedirectToAction("ReimburseRejected", new { id = id });
                }
            }
            //审批
            else
            {

                int NowNodeID = Convert.ToInt32(model.JieDianID);
                var MyJieDianNow = ERPNWorkFlowNodeService.GetEntityById(NowNodeID);

                ViewBag.ApprovalStep = model.JieDianName;

                //获取当前表单对应的工作数据列
                //获取当前节点的可写权限、保密权限
                string CanWriteStr = MyJieDianNow.CanWriteSet;
                string SecretStr = MyJieDianNow.SecretSet;

                //批量设置字段的可写、保密属性

                // 设置审批流程的所有结点和当前结点
                var list = ERPNWorkFlowNodeService.GetEntities(x => x.WorkFlowID == model.WorkflowId).OrderBy(p => p.NodeSerils).ToList();
                ViewBag.ApprovalWorkflowNode = list;
                ViewBag.CurrentNodeSerils = MyJieDianNow.NodeSerils;
                ViewBag.allCount = list.Count;
                bool isRightRoles = IsRightRolesAboutFunds(id);
                ViewBag.isRightPersonApproval = isRightRoles;
                return View(model);
            }
        }
        /// <summary>
        /// 费用审批单的驳回
        /// </summary>
        /// <returns></returns>
        public ActionResult ReimburseRejected(string id)
        {

            var fundsModel = FundsRecordService.GetEntityById(Convert.ToInt32(id));

            var todoModel = ApplicationService.GetEntityById(fundsModel.ApplicationId);

            //string projectType = todoModel.FormValues.Split('#')[1];
            ///获取经费记录类型
            using (var context = new CSPostOAEntities())
            {
                ViewBag.FundsType = (from h in context.FundsThreshold
                                     where (h.IsDeleted == false && h.ModuleName == ModuleNameOfScienceResearch.ScienceResearch.ToString() && h.NWorkToDoID == fundsModel.ApplicationId)

                                     select new SelectListItem()
                                     {
                                         Text = h.FundsType,
                                         Value = h.FundsType,
                                     }).ToList();
            }

            FundsRecordViewModel model = new FundsRecordViewModel();
            //批量设置字段的可写、保密属性

            int id1 = Convert.ToInt32(id);

            model = FundsRecordService.GetAllById(id1).ToViewModel();

            if (model.TravelFundsList.Count > 0)
            {
                ViewBag.reimburse = "travel";
            }
            else
            {
                ViewBag.reimburse = "funds";
            }

            //判断驳回或继续审批
            bool isRejected = Convert.ToBoolean(model.IsRejected);
            bool isTemporary = Convert.ToBoolean(model.IsTemporary);

            int NowNodeID = Convert.ToInt32(model.JieDianID);
            var MyJieDianNow = ERPNWorkFlowNodeService.GetEntityById(NowNodeID);

            //获取当前表单对应的工作数据列
            //获取当前节点的可写权限、保密权限
            string CanWriteStr = MyJieDianNow.CanWriteSet;
            string SecretStr = MyJieDianNow.SecretSet;

            return View(model);

        }
        /// <summary>
        /// 差旅报销单的驳回
        /// </summary>
        /// <returns></returns>
        public ActionResult TravelExpensesRejected(string id)
        {
            FundsRecordViewModel model = new FundsRecordViewModel();
            //批量设置字段的可写、保密属性

            int id1 = Convert.ToInt32(id);

            model = FundsRecordService.GetAllById(id1).ToViewModel();

            if (model.TravelFundsList.Count > 0)
            {
                ViewBag.reimburse = "travel";
            }
            else
            {
                ViewBag.reimburse = "funds";
            }


            //判断驳回或继续审批
            bool isRejected = Convert.ToBoolean(model.IsRejected);
            bool isTemporary = Convert.ToBoolean(model.IsTemporary);

            int NowNodeID = Convert.ToInt32(model.JieDianID);
            var MyJieDianNow = ERPNWorkFlowNodeService.GetEntityById(NowNodeID);

            //获取当前表单对应的工作数据列
            //获取当前节点的可写权限、保密权限
            string CanWriteStr = MyJieDianNow.CanWriteSet;
            string SecretStr = MyJieDianNow.SecretSet;

            return View(model);

        }
        /// <summary>
        /// 差旅费用报销单显示
        /// </summary>
        /// <returns></returns>
        public ActionResult TravelExpenses(string id, string nextaction)
        {
            int applicaionId = Convert.ToInt32(id);
            ViewBag.Id = id;
            //点击保存后的view
            if (nextaction == "save")
            {
                ViewBag.act = "save";

                TravelFundsRecordViewModel model = new TravelFundsRecordViewModel();
                model = FundsRecordService.GetEntityWithTravelDetailListById(applicaionId).ToViewModel();

                return View(model);
            }
            //点击上报时的view
            else
            {
                TravelFundsRecordViewModel model = new TravelFundsRecordViewModel();
                int workflowId = (int)TypeOfWorkFlowId.TravelReimbursement;

                ERPNWorkToDoViewModel todomodel = ApplicationService.GetEntityById(applicaionId).ToViewModel();
                model.ApplicationId = todomodel.NWorkToDoID;

                model.WorkflowId = workflowId;
                model.ProjectName = todomodel.WenHao;
                model.UserName = User.Identity.Name;
                model.CreatedTime = DateTime.Now.Date;

                // Add a new model to the list for the diplay name lable.
                model.TravelFundsList.Add(new TravelFundsDetailViewModel());

                //批量设置字段的可写、保密属性
                string PiLiangSet = "";
                //设置上传的附件为空
                //绑定下一节点
                string jiedianid = string.Empty;
                var temperpnworkflownodemodel = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == workflowId && p.NodeAddr == Constant.MacroStartString).ToList();
                string[] NextStrList = temperpnworkflownodemodel[0].NextNode.Split(',');
                for (int i = 0; i < NextStrList.Length; i++)
                {
                    //根据节点序号和WorkFlowID获取节点
                    var temperpnworkflownodemodel1 = ERPNWorkFlowNodeService.GetEntities(p => p.NodeSerils == NextStrList[i].ToString() && p.WorkFlowID == workflowId).ToList();
                    jiedianid = temperpnworkflownodemodel1[0].ID.ToString(); //根据序号和workflowID获得节点ID
                    model.JieDianID = int.Parse(jiedianid);
                    model.JieDianName = temperpnworkflownodemodel1[0].NodeName;
                }
                //根据选择的节点，绑定人员等信息。
                //当前开始节点是否有查看、编辑、删除按钮？当前按钮属性
                string NowNodeID = temperpnworkflownodemodel[0].ID.ToString();

                int nodeid = int.Parse(NowNodeID);
                var MyJieDianNow = ERPNWorkFlowNodeService.GetEntityById(nodeid);
                //获取当前表单对应的工作数据列
                //获取当前节点的可写权限、保密权限
                string CanWriteStr = MyJieDianNow.CanWriteSet;
                string SecretStr = MyJieDianNow.SecretSet;
                return View(model);
            }
        }

        /// <summary>
        /// 差旅审批单
        /// </summary>
        /// <returns></returns>
        public ActionResult TravelAgree(string id)
        {
            TravelFundsRecordViewModel model = new TravelFundsRecordViewModel();
            //string PiLiangSet = "";//批量设置字段的可写、保密属性
            int id1 = Convert.ToInt32(id);

            var shenpimodel = FundsRecordService.GetEntityWithTravelDetailListById(id1);
            model = shenpimodel.ToViewModel();

            int NowNodeID = Convert.ToInt32(shenpimodel.JieDianID);
            var MyJieDianNow = ERPNWorkFlowNodeService.GetEntityById(NowNodeID);
            string CanWriteStr = MyJieDianNow.CanWriteSet;
            string SecretStr = MyJieDianNow.SecretSet;

            return View(model);
        }

        /// <summary>
        /// 课题结案审批单
        /// </summary>
        /// <returns></returns>
        public ActionResult ConclusionsAgree(string id, string nextaction)
        {
            int id1 = Convert.ToInt32(id);
            //根据applicationid获取所有经费记录的总金额
            var tempfundslist = FundsRecordService.GetEntities(p => p.ApplicationId == Convert.ToInt32(id)).ToList();
            double totaloutmoney = 0;
            foreach (var item in tempfundslist)
            {
                totaloutmoney = totaloutmoney + item.TotalPrice;
            }
            ViewBag.TotalOutMoney = totaloutmoney;

            ProjectRecordViewModel tempprojectestablishlist = ProjectRecordService.GetEntities(p => p.ApplicationId == id1).FirstOrDefault().ToViewModel();

            double totalinmoney = tempprojectestablishlist.Total;
            ViewBag.TotalInMoney = totalinmoney;

            double balancemoney = totalinmoney - totaloutmoney;
            ViewBag.BalanceMoney = balancemoney;

            var model = ApplicationService.GetEntities(p => p.ApplicationId == id1 && p.FormID == Convert.ToInt32(EngineeringProjectTypeOfFormId.Conclusion)).FirstOrDefault().ToViewModel();
            //继续审批

            bool isRejected = Convert.ToBoolean(model.IsRejected);
            bool isTemporary = Convert.ToBoolean(model.IsTemporary);

            ERPNWorkFlowNodeTransferObject currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.ID == model.JieDianID).First();

            model.ApplicationId = Convert.ToInt32(id);
            //加载表单内容
            var formModel = ERPNFormService.GetEntityById(model.FormID.Value);

            //获取当前表单对应的工作数据列
            string[] formItemArray = formModel.ItemsList.Split(Constant.SplitChar);

            ViewBag.ApprovalStep = model.JieDianName;
            ViewBag.Id = id;
            //批量设置字段的可写、保密属性
            ViewBag.PiLiangSet = CommonHelper.SetTheWriteAndHiddenField(currentNode.CanWriteSet, currentNode.SecretSet, formItemArray);

            // 设置审批流程的所有结点和当前结点
            var list = ERPNWorkFlowNodeService.GetEntities(x => x.WorkFlowID == model.WorkFlowID).OrderBy(p => p.NodeSerils).ToList();
            ViewBag.ApprovalWorkflowNode = list;
            ViewBag.CurrentNodeSerils = currentNode.NodeSerils;
            ViewBag.allCount = list.Count;
            bool isRightRoles = IsRightRoles(id);
            ViewBag.isRightPersonApproval = isRightRoles;
            return View(model);
        }
        /// <summary>
        /// 课题结案驳回
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nextaction"></param>
        /// <returns></returns>ApplicationListStatistics
        public ActionResult ConclusionsRejected(string id)
        {
            int id1 = Convert.ToInt32(id);
            //根据applicationid获取所有经费记录的总金额
            var tempfundslist = FundsRecordService.GetEntities(p => p.ApplicationId == Convert.ToInt32(id)).ToList();
            double totaloutmoney = 0;
            foreach (var item in tempfundslist)
            {
                totaloutmoney = totaloutmoney + item.TotalPrice;
            }
            ViewBag.TotalOutMoney = totaloutmoney;

            ProjectRecordViewModel tempprojectestablishlist = ProjectRecordService.GetEntities(p => p.ApplicationId == id1).FirstOrDefault().ToViewModel();

            double totalinmoney = tempprojectestablishlist.Total;
            ViewBag.TotalInMoney = totalinmoney;

            double balancemoney = totalinmoney - totaloutmoney;
            ViewBag.BalanceMoney = balancemoney;

            var model = ApplicationService.GetEntities(p => p.ApplicationId == id1 && p.FormID == Convert.ToInt32(EngineeringProjectTypeOfFormId.Conclusion)).FirstOrDefault().ToViewModel();
            //继续审批

            bool isRejected = Convert.ToBoolean(model.IsRejected);
            bool isTemporary = Convert.ToBoolean(model.IsTemporary);

            ERPNWorkFlowNodeTransferObject currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.ID == model.JieDianID).First();

            model.ApplicationId = Convert.ToInt32(id);
            //加载表单内容
            var formModel = ERPNFormService.GetEntityById(model.FormID.Value);

            //获取当前表单对应的工作数据列
            string[] formItemArray = formModel.ItemsList.Split(Constant.SplitChar);

            ViewBag.ApprovalStep = model.JieDianName;
            ViewBag.Id = id;
            //批量设置字段的可写、保密属性
            ViewBag.PiLiangSet = CommonHelper.SetTheWriteAndHiddenField(currentNode.CanWriteSet, currentNode.SecretSet, formItemArray);

            return View(model);
        }
        /// <summary>
        /// 课题结案显示
        /// </summary>
        /// <returns></returns>
        public ActionResult Conclusions(string id, string nextaction)
        {
            int applicationid = Convert.ToInt32(id);//applicationid
            ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();
            ///合同记录中的成果形式和数量
            model = ApplicationService.GetEntities(p => p.FormID == (int)EngineeringProjectTypeOfFormId.Contract && p.ApplicationId == applicationid).FirstOrDefault().ToViewModel();
            var resultQuantity = model.FormValues.Split(Constant.SharpChar);
            string ResultCount = resultQuantity[2].ToString();
            ViewBag.Achieve = ResultCount;

            ///论文关联的显示
            var mainResult = ApplicationService.GetEntityById(applicationid);
            string Number = mainResult.BeiYong1;
            var lunwenResult = ApplicationService.GetEntities(p => p.BeiYong1 == Number && p.FormID == (int)PaperPublishTypeOfFormId.Application).ToList();
            if (lunwenResult.Count > 0)
            {
                string lunWen = null;
                foreach (var item in lunwenResult)
                {
                    lunWen += item.WenHao + ",";
                }
                ViewBag.LunWen = lunWen;
            }
            else
            {
                ViewBag.LunWen = "暂无";
            }
            //ToDo

            //判断是否已经进入到项目延期审批
            int countOfExtensionRequest = ApplicationService.GetEntities(p => p.ID == applicationid && p.ApplicationStatus == ApplicationStatus.ExtensionRequestApproving.ToString()).Count();
            if (countOfExtensionRequest > 0)
            {
                return RedirectToAction("ExtensionAgree", new { id = applicationid });
            }

            //判断是否已经进入到项目结题审批中
            var applicaitonodelcount = ApplicationService.GetEntities(p => p.ApplicationId == applicationid && p.FormID == (int)EngineeringProjectTypeOfFormId.Conclusion).Count();

            if (applicaitonodelcount > 0)
            {
                model = ApplicationService.GetEntities(p => p.ApplicationId == applicationid && p.FormID == Convert.ToInt32(EngineeringProjectTypeOfFormId.Conclusion)).FirstOrDefault().ToViewModel();
                if (model.IsTemporary == true)
                {
                    ViewBag.Id = model.ApplicationId;
                    ViewBag.act = "save";
                    return View(model);
                }
                else
                {
                    return RedirectToAction("ConclusionsAgree", new { id = id });
                }
            }

            //根据applicationid获取所有经费记录的总金额
            var tempfundslist = FundsRecordService.GetEntities(p => p.ApplicationId == applicationid).ToList();
            double totaloutmoney = 0;
            foreach (var item in tempfundslist)
            {
                totaloutmoney = totaloutmoney + item.TotalPrice;
            }
            ViewBag.TotalOutMoney = totaloutmoney;

            ProjectRecordViewModel tempprojectestablishlist = ProjectRecordService.GetEntities(p => p.ApplicationId == applicationid).FirstOrDefault().ToViewModel();

            double totalinmoney = tempprojectestablishlist.Total;
            ViewBag.TotalInMoney = totalinmoney;

            double balancemoney = totalinmoney - totaloutmoney;
            ViewBag.BalanceMoney = balancemoney;

            //点击保存后的view
            if (nextaction == "save")
            {
                model = ApplicationService.GetEntities(p => p.FormID == (int)EngineeringProjectTypeOfFormId.Conclusion && p.ApplicationId == applicationid).FirstOrDefault().ToViewModel();
                ViewBag.Id = model.ApplicationId;
                ViewBag.act = "save";
                return View(model);

            }
            ////点击修改数据后的view
            //else if (nextaction == "updatedata")
            //{
            //    ViewBag.Id = id;
            //    ViewBag.act = "updatedata";
            //    ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();
            //    model = ApplicationService.GetEntities(p => p.FormID == (int)EngineeringProjectTypeOfFormId.Conclusion && p.ApplicationId == applicationid).FirstOrDefault().ToViewModel();
            //    return View(model);
            //}
            //点击上报时的view
            else
            {
                ERPNWorkFlowNodeTransferObject currentNode;
                ViewBag.Id = id;
                var appresult = ApplicationService.GetEntities(p => p.ID == applicationid).ToList();
                string projectname = appresult[0].WenHao;//项目名称
                var keyvaluearry = appresult[0].FormValues.Split(Constant.SharpChar);
                string projectestablishtime = keyvaluearry[2].ToString();//申请时间
                int formId = (int)EngineeringProjectTypeOfFormId.Conclusion;
                int workflowId = (int)TypeOfWorkFlowId.Conclusion;
                model.WorkFlowID = workflowId;
                model.FormID = formId;

                //设置上传的附件为空
                //加载表单内容
                var temerpnformmodel = ERPNFormService.GetEntityById(formId).ToViewModel();

                //获取当前表单对应的工作数据列
                string[] formItemArray = temerpnformmodel.ItemsList.Split(Constant.SplitChar);

                string content = temerpnformmodel.ContentStr;
                if (content.Contains("Text851498255") && content.Contains("Date1636751336"))//项目名称
                {
                    string oldvalue1 = "Text851498255";
                    string newvalue1 = @"Text851498255"" value=""" + projectname + @"""";
                    content = content.Replace(oldvalue1, newvalue1);

                    string oldvalue3 = "Date1636751336";
                    string newvalue3 = @"Date1636751336"" value=""" + projectestablishtime + @"""";
                    content = content.Replace(oldvalue3, newvalue3);
                }
                content = content.Replace(Constant.MacroSectionString, MySession[SessionKeyEnum.SectionName].ToString());
                content = content.Replace(Constant.MacroUserNameString, User.Identity.Name);
                model.FormContent = content;
                model.ApplicationId = applicationid;

                ////绑定工作名称
                var workFlowModel = ERPNWorkFlowService.GetEntityById(workflowId);
                model.WorkName = User.Identity.Name + Constant.DoubleHyphenString + workFlowModel.WorkFlowName;
                //绑定下一节点
                currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == workflowId && p.NodeAddr == Constant.MacroStartString).First();
                model.JieDianID = currentNode.ID;
                model.JieDianName = currentNode.NodeName;

                //批量设置字段的可写、保密属性
                ViewBag.PiLiangSet = CommonHelper.SetTheWriteAndHiddenField(currentNode.CanWriteSet, currentNode.SecretSet, formItemArray);


                var result = ApplicationService.GetEntityById(applicationid);
                string wheatherHiddenBtn = "";
                if (result.UserName == User.Identity.Name || User.IsInRole(UserRoles.超级管理员.ToString()))
                {
                    wheatherHiddenBtn = "No";
                }
                else
                {
                    wheatherHiddenBtn = "Yes";
                }
                ViewBag.wheatherHiddenBtn = wheatherHiddenBtn;
                return View(model);
            }
        }

        /// <summary>
        /// 项目延期申请显示
        /// </summary>
        /// <returns></returns>
        public ActionResult ExtensionRequest(string id, string nextaction)
        {
            int applicationid = Convert.ToInt32(id);//applicationid


            //填写项目延期时判断是否已经在课题结案审批
            int countOfConclusion = ApplicationService.GetEntities(p => p.ApplicationId == applicationid && p.FormID == (int)EngineeringProjectTypeOfFormId.Conclusion && p.IsTemporary != true).Count();

            if (countOfConclusion > 0)
            {
                return RedirectToAction("ConclusionsAgree", new { id = applicationid });
            }

            //填写项目延期申请时判断是否已经进入项目延期审批
            int countOfExtensionRequest = ApplicationService.GetEntities(p => p.ApplicationId == applicationid && p.FormID == (int)EngineeringProjectTypeOfFormId.ExtensionRequest).Count();
            if (countOfExtensionRequest > 0)
            {
                ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();
                model = ApplicationService.GetEntities(p => p.ApplicationId == applicationid && p.FormID == Convert.ToInt32(EngineeringProjectTypeOfFormId.ExtensionRequest)).FirstOrDefault().ToViewModel();
                if (model.IsTemporary == true)
                {
                    ViewBag.Id = model.ApplicationId;
                    ViewBag.act = "save";
                    return View(model);
                }
                else
                {
                    return RedirectToAction("ExtensionAgree", new { id = applicationid });
                }
            }

            //点击保存后的view
            if (nextaction == "save")
            {
                ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();
                model = ApplicationService.GetEntityById(applicationid).ToViewModel();
                ViewBag.Id = model.ApplicationId;
                ViewBag.act = "save";
                return View(model);
            }
            ////点击修改数据后的view
            //else if (nextaction == "updatedata")
            //{
            //    ViewBag.Id = id;
            //    ViewBag.act = "updatedata";

            //    ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();
            //    model = ApplicationService.GetEntityById(applicationid).ToViewModel();
            //    return View(model);
            //}
            //点击上报时的view
            else
            {
                int formId = (int)EngineeringProjectTypeOfFormId.ExtensionRequest;
                int workflowId = (int)TypeOfWorkFlowId.ExtensionRequest;

                ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();
                ERPNWorkFlowNodeTransferObject currentNode;

                ViewBag.Id = id;
                var appresult = ApplicationService.GetEntities(p => p.ID == applicationid).ToList();
                string projectname = appresult[0].WenHao;//项目名称

                model.WorkFlowID = workflowId;
                model.FormID = formId;
                //设置上传的附件为空
                //加载表单内容
                var temerpnformmodel = ERPNFormService.GetEntityById(formId).ToViewModel();

                //获取当前表单对应的工作数据列
                string[] formItemArray = temerpnformmodel.ItemsList.Split(Constant.SplitChar);

                model.ApplicationId = applicationid;
                string content = temerpnformmodel.ContentStr;
                if (content.Contains("Text1465642221"))//项目名称
                {
                    string oldvalue1 = "Text1465642221";
                    //string newvalue1 = "\"" + "Text1465642221" + "\"value=\"" + projectname + "\"";
                    string newvalue1 = @"Text1465642221"" value=""" + projectname + @"""";
                    content = content.Replace(oldvalue1, newvalue1);
                }
                model.FormContent = content;

                ////绑定工作名称
                var workFlowModel = ERPNWorkFlowService.GetEntityById(workflowId);
                model.WorkName = User.Identity.Name + Constant.DoubleHyphenString + workFlowModel.WorkFlowName;
                //绑定下一节点
                currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == workflowId && p.NodeAddr == Constant.MacroStartString).First();
                model.JieDianID = currentNode.ID;
                model.JieDianName = currentNode.NodeName;

                //批量设置字段的可写、保密属性
                ViewBag.PiLiangSet = CommonHelper.SetTheWriteAndHiddenField(currentNode.CanWriteSet, currentNode.SecretSet, formItemArray);

                var result = ApplicationService.GetEntityById(applicationid);
                string wheatherHiddenBtn = "";
                if (result.UserName == User.Identity.Name || User.IsInRole(UserRoles.超级管理员.ToString()))
                {
                    wheatherHiddenBtn = "No";
                }
                else
                {
                    wheatherHiddenBtn = "Yes";
                }
                ViewBag.wheatherHiddenBtn = wheatherHiddenBtn;

                return View(model);
            }
        }

        /// <summary>
        /// 项目延伸审批单
        /// </summary>
        /// <returns></returns>
        public ActionResult ExtensionAgree(string id, string nextaction)
        {
            ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();

            int id1 = Convert.ToInt32(id);//applicationid

            model = ApplicationService.GetEntities(p => p.ApplicationId == id1 && p.FormID == Convert.ToInt32(EngineeringProjectTypeOfFormId.ExtensionRequest)).FirstOrDefault().ToViewModel();
            //继续审批

            bool isRejected = Convert.ToBoolean(model.IsRejected);
            bool isTemporary = Convert.ToBoolean(model.IsTemporary);

            ERPNWorkFlowNodeTransferObject currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.ID == model.JieDianID).First();

            model.ApplicationId = Convert.ToInt32(id);
            //加载表单内容
            var formModel = ERPNFormService.GetEntityById(model.FormID.Value);

            //获取当前表单对应的工作数据列
            string[] formItemArray = formModel.ItemsList.Split(Constant.SplitChar);

            ViewBag.ApprovalStep = model.JieDianName;
            ViewBag.Id = id;
            //批量设置字段的可写、保密属性
            ViewBag.PiLiangSet = CommonHelper.SetTheWriteAndHiddenField(currentNode.CanWriteSet, currentNode.SecretSet, formItemArray);

            // 设置审批流程的所有结点和当前结点
            var list = ERPNWorkFlowNodeService.GetEntities(x => x.WorkFlowID == model.WorkFlowID).OrderBy(p => p.NodeSerils).ToList();
            ViewBag.ApprovalWorkflowNode = list;
            ViewBag.CurrentNodeSerils = currentNode.NodeSerils;
            ViewBag.allCount = list.Count;
            bool isRightRoles = IsRightRoles(model.NWorkToDoID.ToString());
            ViewBag.isRightPersonApproval = isRightRoles;
            return View(model);
        }
        /// <summary>
        /// 延期申请的驳回
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nextaction"></param>
        /// <returns></returns>
        public ActionResult ExtensionRejected(string id)
        {
            ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();

            int id1 = Convert.ToInt32(id);//applicationid

            model = ApplicationService.GetEntities(p => p.ApplicationId == id1 && p.FormID == Convert.ToInt32(EngineeringProjectTypeOfFormId.ExtensionRequest)).FirstOrDefault().ToViewModel();
            //继续审批

            bool isRejected = Convert.ToBoolean(model.IsRejected);
            bool isTemporary = Convert.ToBoolean(model.IsTemporary);

            ERPNWorkFlowNodeTransferObject currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.ID == model.JieDianID).First();

            model.ApplicationId = Convert.ToInt32(id);
            //加载表单内容
            var formModel = ERPNFormService.GetEntityById(model.FormID.Value);

            //获取当前表单对应的工作数据列
            string[] formItemArray = formModel.ItemsList.Split(Constant.SplitChar);

            ViewBag.ApprovalStep = model.JieDianName;
            ViewBag.Id = id;
            //批量设置字段的可写、保密属性
            ViewBag.PiLiangSet = CommonHelper.SetTheWriteAndHiddenField(currentNode.CanWriteSet, currentNode.SecretSet, formItemArray);

            return View(model);
        }

        /// <summary>
        /// 判断申请书状态，返回状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ReturnStateValue(string id)
        {
            var result = ApplicationService.GetEntityById(Convert.ToInt32(id));
            string ReturnState = result.ApplicationStatus;//数据库中该条申请书所处状态
            return Json(ReturnState, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 判断首页待办前10条申请书状态，返回状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ReturnTop10StateValue(string id)
        {
            int id1 = Convert.ToInt32(id);//ERPNWorkToDo的ID
            string ReturnState;
            FundsRecordTransferObject fundsCount = null;
            try
            {
                fundsCount = FundsRecordService.GetEntityById(id1);
                var fundsresult = FundsRecordService.GetEntityById(id1);
                var result = ApplicationService.GetEntityById(fundsresult.ApplicationId);
                ReturnState = result.ApplicationStatus;//数据库中该条申请书所处状态
                int ReturnId;
                if (result.ApplicationId == 0)
                {
                    ReturnId = result.ID;
                }
                else
                {
                    ReturnId = result.ApplicationId;
                }
                int ReturnFundsId = fundsresult.WorkflowId;
                //return Json(ReturnState, JsonRequestBehavior.AllowGet);
                return Json(new { ReturnState, fundsId = ReturnId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                var result = ApplicationService.GetEntityById(id1);
                ReturnState = result.ApplicationStatus;//数据库中该条申请书所处状态
                int ReturnId;
                if (result.ApplicationId == 0)
                {
                    ReturnId = result.ID;
                }
                else
                {
                    ReturnId = result.ApplicationId;
                }
                return Json(new { ReturnState, fundsId = ReturnId }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 判断所有ALL已办申请书状态，返回状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ReturnDoneStateValue(string id)
        {
            int applicationId;
            var result = ApplicationService.GetEntityById(Convert.ToInt32(id));
            string ReturnState = result.ApplicationStatus;//数据库中该条申请书所处状态
            if (result.ApplicationId == 0)
            {
                applicationId = result.ID;
            }
            else
            {
                applicationId = result.ApplicationId;
            }

            return Json(new { ReturnState, applicationId }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 登录者是否是申请书填写者
        /// </summary>
        /// <param name="id">申请书ID</param>
        /// <returns></returns>
        public ActionResult LoginPersonIsEqualApprovalWritePerson(string id)
        {
            int id1 = Convert.ToInt32(id);//ERPNWorkToDo的ID

            var result = ApplicationService.GetEntityById(id1);
            string ReturnState = "";
            if (result.UserName == User.Identity.Name || User.IsInRole(UserRoles.超级管理员.ToString()))
            {
                ReturnState = "Equal";
            }
            else
            {
                ReturnState = "NotEqual";
            }
            return Json(ReturnState, JsonRequestBehavior.AllowGet);
        }
        ///// <summary>
        ///// 判断填写过程记录的人和审批过程记录的人有权限
        ///// </summary>
        ///// <param name="id">申请书ID</param>
        ///// <returns></returns>
        //public ActionResult IsInContactWithProcessRecord(string id)
        //{

        //    ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(Convert.ToInt32(id)).ToViewModel();
        //    int typeOfWorkFlowId = Convert.ToInt32(TypeOfWorkFlowId.ProcessRecord);

        //        IEnumerable<ERPNWorkFlowNodeViewModel> workFlowNodeModelList = new List<ERPNWorkFlowNodeViewModel>();
        //        //除填写申请书的审批节点
        //        workFlowNodeModelList = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == typeOfWorkFlowId).OrderBy(p => p.NodeSerils).Skip(2).Select(x => x.ToViewModel());
        //        //将从第二节点（包括第二节点）的NodeSerials全部装在workFlowNodeModelList
        //        bool isInContactWithProcessRecord = false;
        //        string loginPersonDepartment = MySession[SessionKeyEnum.SectionName].ToString();
        //        ApplicationDbContext usercontext = new ApplicationDbContext();
        //        string UserId = usercontext.Users.Where(x => x.UserName == tempmodel.UserName).FirstOrDefault().Id;
        //        //根据用户名找科室
        //        string applicantdepartMentName = usercontext.Sections.Where(x => x.ApplicationUsers.Any(u => u.ApplicationUserId == UserId)).FirstOrDefault().Name;
        //               string writePerson = tempmodel.UserName;
        //               if (User.Identity.Name == writePerson)
        //               {
        //                   isInContactWithProcessRecord = true;
        //               }
        //               else
        //               {
        //                   isInContactWithProcessRecord = false;
        //               }

        //        if (isInContactWithProcessRecord)
        //        {

        //        }
        //        else
        //        {
        //            foreach (var item in workFlowNodeModelList)
        //            {
        //                if (item.SPDefaultList == "本科室主任")
        //                {
        //                    if (loginPersonDepartment == "系统账户")
        //                    {
        //                        isInContactWithProcessRecord = true;
        //                    }
        //                    else
        //                    {
        //                        isInContactWithProcessRecord = User.IsInRole(applicantdepartMentName + "主任");
        //                    }
        //                }
        //                else
        //                {
        //                    if (loginPersonDepartment == "系统账户")
        //                    {
        //                        isInContactWithProcessRecord = true;
        //                    }
        //                    else
        //                    {
        //                        isInContactWithProcessRecord = User.IsInRole(item.SPDefaultList);
        //                    }
        //                }
        //                if (isInContactWithProcessRecord)
        //                {
        //                    break;
        //                }
        //            }
        //        }
        //        return Json(isInContactWithProcessRecord,JsonRequestBehavior.AllowGet);
        //}
        /// <summary>
        /// RiZhi判断申请书状态，返回状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult RiZhiReturnStateValue(string id)
        {
            int id1 = Convert.ToInt32(id);//ERPNWorkToDo的ID或者是日志中FkApplicationId

            int IsProcessIdCount = ERPRiZhiService.GetEntities(p => p.FKApplicationID == id && p.DoSomething.Contains("过程记录")).Count;
            if (IsProcessIdCount > 0)
            {
                var applicationmodel = ApplicationService.GetEntities(p => p.ID == id1).FirstOrDefault();
                id1 = applicationmodel.ApplicationId;
            }
            else
            {

            }
            var result = ApplicationService.GetEntityById(id1);
            string ReturnState = result.ApplicationStatus;//数据库中该条申请书所处状态
            return Json(new { ReturnState, applicationId = id1 }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 判断经费列表弹出框的弹出页面
        /// </summary>
        /// <param name="id">//FundsRecord的ID</param>
        /// <returns>
        /// ReturnReimburseState=RejectAndTemporary是经费审批页面，
        /// ReturnReimburseState=OnlyTemporary是经费填写页面，
        /// ReturnReimburseState=NotSuccess不成功，</returns>
        public ActionResult ReturnReimburseStateValue(string id)
        {
            int id1 = Convert.ToInt32(id);

            var result = FundsRecordService.GetEntityById(id1);
            string ReturnReimburseState;

            if (result.WorkflowId == 1033)
            {
                if (result.IsRejected == false && result.IsTemporary == false)
                {
                    ReturnReimburseState = "RejectAndTemporary";
                }
                else
                {
                    ReturnReimburseState = "TravelTemporary";
                }
            }
            else
            {
                if (result.IsTemporary == true && result.IsDeleted == false && result.IsRejected == null && result.IsLocked == false)
                {
                    //经费填写是点击了保存
                    ReturnReimburseState = "OnlyTemporary";
                }
                else if (result.IsTemporary == true && result.IsDeleted == false && result.IsRejected == false && result.IsLocked == false)
                {
                    //经费填写是点击了保存
                    ReturnReimburseState = "OnlyTemporary";
                }
                else
                {
                    //经费审批中驳回后点击了保存
                    ReturnReimburseState = "RejectAndTemporary";
                }
            }
            return Json(ReturnReimburseState, JsonRequestBehavior.AllowGet);
        }


        public ActionResult RetrunFreezeFlag(string id, string pagestate, string random)
        {
            int id1 = Convert.ToInt32(id);//ERPNWorkToDo的ID
            string successFlag = string.Empty;
            ERPNWorkToDoViewModel resultList = new ERPNWorkToDoViewModel();
            resultList = ApplicationService.GetEntityById(id1).ToViewModel();
            if (pagestate == "FirstLoad")
            {
                if (resultList.IsLocked != null)
                {
                    bool freezeFlag = Convert.ToBoolean(resultList.IsLocked);
                    if (freezeFlag)
                    {
                        successFlag = "areadyFreezed";
                    }
                    else
                    {
                        successFlag = "notFreezed";
                    }
                }
                else
                {
                    successFlag = "noValue";
                }
            }
            else if (pagestate == "Freezed")//点击已冻结
            {
                resultList.IsLocked = false;
                bool updateSuccess = ApplicationService.UpdateApplication(resultList.ToDataTransferObjectModel());
                if (updateSuccess)
                {
                    successFlag = "notFreezed";
                }
            }
            else//点击冻结
            {
                resultList.IsLocked = true;
                bool updateSuccess = ApplicationService.UpdateApplication(resultList.ToDataTransferObjectModel());
                if (updateSuccess)
                {
                    successFlag = "areadyFreezed";
                }
            }
            return Json(successFlag, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 正在审批的过程记录列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ProcessRecordList(string id)
        {
            //为什么会进入两次
            int applicationid = Convert.ToInt32(id);
            ViewBag.Id = applicationid;
            var result = ApplicationService.GetEntities(p => p.StateNow == "正在办理" && p.FormID == (int)EngineeringProjectTypeOfFormId.ProcessRecord && p.ApplicationId == applicationid).OrderByDescending(p => p.TimeStr).ToList();
            IList<ERPNWorkToDoViewModel> resultList = new List<ERPNWorkToDoViewModel>();
            foreach (var item in result)
            {
                resultList.Add(item.ToViewModel());
            }
            var tempresult = ApplicationService.GetEntityById(applicationid);
            string WeatherHidenCreadBtn = "";
            if (tempresult.UserName == User.Identity.Name || User.IsInRole(UserRoles.超级管理员.ToString()))
            {
                WeatherHidenCreadBtn = "No";
            }
            else
            {
                WeatherHidenCreadBtn = "Yes";
            }
            ViewBag.WeatherHidenCreadBtn = WeatherHidenCreadBtn;
            return View(resultList);
        }

        /// <summary>
        /// 正在审批的经费记录列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ReimburseWorkList(string id)
        {
            int applicationid = Convert.ToInt32(id);
            ViewBag.Id = applicationid;
            var result = FundsRecordService.GetEntities(p => p.StateNow == "正在办理" && p.ApplicationId == applicationid).OrderByDescending(p => p.TimeStr).ToList();

            IList<FundsRecordViewModel> resultList = new List<FundsRecordViewModel>();

            foreach (var item in result)
            {
                resultList.Add(item.ToViewModel());

            }
            var tempresult = ApplicationService.GetEntityById(applicationid);
            string WeatherHidenCreadBtn = "";
            if (tempresult.UserName == User.Identity.Name || User.IsInRole(UserRoles.超级管理员.ToString()))
            {
                WeatherHidenCreadBtn = "No";
            }
            else
            {
                WeatherHidenCreadBtn = "Yes";
            }
            ViewBag.WeatherHidenCreadBtn = WeatherHidenCreadBtn;
            //todo:差旅报销单的数据显示
            return View(resultList);
        }

        /// <summary>
        /// 经费管理列表
        /// </summary>
        /// <returns></returns>
        public ActionResult FundsToDoList()
        {
            IList<FundsRecordViewModel> fundsList = new List<FundsRecordViewModel>();
            fundsList.Add(new FundsRecordViewModel()
            {
                UserName = "admin",
                Name = "经费报销",
                Description = "报销单",

            });
            fundsList.Add(new FundsRecordViewModel()
            {
                StateNow = "正在待办",
            });

            IList<FundsToDoViewModel> listResult = new List<FundsToDoViewModel>();

            listResult.Add(new FundsToDoViewModel
            {
                FundsList = fundsList,
                ApplicationStatus = "",
                UserName = "admin",
                WorkName = "工程项目申请书",


            });
            return View(listResult);
        }
        public ActionResult AllWorkContainer()
        {
            ViewBag.Module = "全部";
            ViewBag.Title = "待办事宜";

            return View();
        }
        /// <summary>
        /// 全部待办事宜消息
        /// </summary>
        /// <returns></returns>
        public ActionResult AllToDoThings()
        {
            var result = ERPRiZhiService.GetEntities(p => p.FKAction == "待办").OrderByDescending(p => p.ID).Take(20);

            IList<ERPRiZhiViewModel> resultlist = new List<ERPRiZhiViewModel>();
            foreach (var item in result)
            {
                resultlist.Add(item.ToViewModel());
            }
            return View(resultlist);
        }

        /// <summary>
        /// 全部已办事宜
        /// </summary>
        /// <returns></returns>
        public ActionResult AllWorkDoneList()
        {
            var result = ERPRiZhiService.GetEntities(p => p.FKAction == "已办").OrderByDescending(p => p.ID).Take(20);
            IList<ERPRiZhiViewModel> resultlist = new List<ERPRiZhiViewModel>();
            foreach (var item in result)
            {
                resultlist.Add(item.ToViewModel());
            }
            return View(resultlist);
        }
        /// <summary>
        /// 顶部的待办事宜消息
        /// </summary>
        /// <returns></returns>
        public ActionResult UpToDoThings()
        {
            var result = ERPRiZhiService.GetEntities(p => p.FKAction == "待办").OrderByDescending(p => p.ID);

            IList<ERPRiZhiViewModel> resultlist = new List<ERPRiZhiViewModel>();
            foreach (var item in result)
            {
                resultlist.Add(item.ToViewModel());
            }
            return Json(resultlist, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 首页近期经费
        /// </summary>
        /// <returns></returns>
        public ActionResult LatedReimburse()
        {

            var result = FundsRecordService.GetEntities(p => p.StateNow == "正在办理").OrderByDescending(p => p.TimeStr).Take(3).ToList();

            IList<FundsRecordViewModel> resultList = new List<FundsRecordViewModel>();

            foreach (var item in result)
            {
                resultList.Add(item.ToViewModel());

            }
            return Json(resultList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 首页近期审批
        /// </summary>
        /// <returns></returns>
        public ActionResult LatedAppraval()
        {
            var result = ERPRiZhiService.GetEntities(p => p.FKAction == "已办").Take(5).OrderByDescending(p => p.ID);
            IList<LatedApprovalTransferObject> resultList = new List<LatedApprovalTransferObject>();

            foreach (var item in result)
            {

                LatedApprovalTransferObject tempModel = new LatedApprovalTransferObject();

                var hasCountInNworktToDo = ApplicationService.GetEntities(p => p.ID == (int.Parse(item.FKApplicationID))).Count;
                //判断该条是经费的日志还是其它日志
                int applicationId = 0;
                if (hasCountInNworktToDo > 0)
                {
                    applicationId = int.Parse(item.FKApplicationID);

                }
                else
                {
                    var fundsRecordModel = FundsRecordService.GetEntityById(int.Parse(item.FKApplicationID));
                    applicationId = fundsRecordModel.ApplicationId;
                }
                var nworkToDoModel = ApplicationService.GetEntityById(applicationId);
                tempModel.ApplicationManName = nworkToDoModel.UserName;

                ApplicationDbContext usercontext = new ApplicationDbContext();
                string UserId = usercontext.Users.Where(x => x.UserName == nworkToDoModel.UserName).FirstOrDefault().Id;
                //根据用户名找科室
                string departMentName = usercontext.Sections.Where(x => x.ApplicationUsers.Any(u => u.ApplicationUserId == UserId)).FirstOrDefault().Name;
                tempModel.Department = departMentName;
                tempModel.ApplicationId = applicationId;
                TimeSpan time1 = new TimeSpan(System.DateTime.Now.Ticks);
                TimeSpan time2 = new TimeSpan(Convert.ToDateTime(item.TimeStr).Ticks);
                TimeSpan time3 = time1.Subtract(time2).Duration();
                tempModel.TimeDifference = time3.TotalHours.ToString();
                tempModel.ApprovalManName = item.UserName;
                resultList.Add(tempModel);
            }
            return Json(resultList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 待办事宜
        /// </summary>
        /// <returns></returns>
        public ActionResult WorkToDoList()
        {
            var result = ERPRiZhiService.GetEntities(p => p.FKAction == "待办" && p.ModuleName == ModuleNameOfScienceResearch.ScienceResearch.ToString()).OrderByDescending(p => p.ID).Take(20);

            IList<ERPRiZhiViewModel> resultlist = new List<ERPRiZhiViewModel>();
            foreach (var item in result)
            {
                resultlist.Add(item.ToViewModel());
            }
            return View(resultlist);
        }

        /// <summary>
        /// 已办事宜
        /// </summary>
        /// <returns></returns>
        public ActionResult WorkDoneList()
        {
            var result = ERPRiZhiService.GetEntities(p => p.FKAction == "已办" && p.ModuleName == ModuleNameOfScienceResearch.ScienceResearch.ToString()).OrderByDescending(p => p.ID).Take(20);
            IList<ERPRiZhiViewModel> resultlist = new List<ERPRiZhiViewModel>();
            foreach (var item in result)
            {
                resultlist.Add(item.ToViewModel());
            }
            return View(resultlist);
        }

        /// <summary>
        /// 待办事宜(Top 10)
        /// </summary>
        /// <returns></returns>
        public ActionResult Top10WorkToDoList()
        {
            var result = ERPRiZhiService.GetEntities(p => p.FKAction == "待办" && p.ModuleName == ModuleNameOfScienceResearch.ScienceResearch.ToString()).OrderByDescending(p => p.ID).Take(10);
            IList<ERPRiZhiViewModel> resultlist = new List<ERPRiZhiViewModel>();

            foreach (var item in result)
            {
                resultlist.Add(item.ToViewModel());
            }

            return View(resultlist);
        }

        /// <summary>
        /// 正在进行中的事宜(Top 10，所有处于过程记录中的项目)
        /// </summary>
        /// <returns></returns>
        public ActionResult Top10WorkDoingList()
        {
            IEnumerable<ERPNWorkToDoTransferObject> result;

            int totalPage;

            if (User.IsInRole(UserRoles.超级管理员.ToString()))
            {
                result = ApplicationService.GetPageEntities(p => p.StateNow == "正在办理"
                    && p.FormID == (int)EngineeringProjectTypeOfFormId.Application
                    && p.ProjectStatus == ApplicationStatus.BigProjectProcessing.ToString(), ApplicationSortField.TimeStr_Desc.ToString(), Constant.PageSize, 1, out totalPage);
            }
            else
            {
                result = ApplicationService.GetPageEntities(p => p.StateNow == "正在办理"
                    && p.UserName == User.Identity.Name
                    && p.FormID == (int)EngineeringProjectTypeOfFormId.Application
                    && p.ProjectStatus == ApplicationStatus.BigProjectProcessing.ToString(), ApplicationSortField.TimeStr_Desc.ToString(), Constant.PageSize, 1, out totalPage);
            }

            return View(result.Select(x => x.ToViewModel()));
        }
        /// <summary>
        /// 奖励设置
        /// </summary>
        /// <returns></returns>
        public ActionResult ProjectBonusCredit()
        {
            var result = ProjectBonusCreditService.GetEntities(p => p.ModuleName == "工程项目").OrderByDescending(p => p.Id);
            IList<ProjectBonusCreditViewModel> resultlist = new List<ProjectBonusCreditViewModel>();
            foreach (var item in result)
            {
                resultlist.Add(item.ToViewModel());
            }

            ViewBag.Module = "工程项目";
            ViewBag.Title = "资金管理";

            return View(resultlist);
        }

        /// <summary>
        /// 判断奖励设置类型，返回类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ReturnProjectBonusValue(string id)
        {
            int id1 = Convert.ToInt32(id); //ProjectBonusCredit的ID

            var result = ProjectBonusCreditService.GetEntityById(id1);
            string ReturnState = result.ModuleName;//数据库中该条类型所处状态
            return Json(ReturnState, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 正在审批中的申请书列表，还未项目确立的申请书
        /// </summary>
        /// <returns></returns>
        public ActionResult ApplicationList()
        {
            ViewBag.Module = "工程项目";
            ViewBag.Title = "项目注册";
            return View();
        }

        /// <summary>
        /// 过程记录列表查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ProcessingProjectList()
        {
            ViewBag.Module = "工程项目";
            ViewBag.Title = "过程记录";
            return View();
        }
        /// <summary>
        /// 工程项目导航首页统计分析容器页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ScienceStatisticContainer(string moduleName)
        {
            ViewBag.Module = "工程项目";
            ViewBag.Title = "统计分析";

            ViewBag.ModuleName = moduleName;

            return View();
        }
        /// <summary>
        /// 工程项目统计
        /// </summary>
        /// <returns></returns>
        public ActionResult ScienceProjectStatisticsAnalysis()
        {
            var result = StatisticService.GetScienceProjectStatistics(x => true, (int)EngineeringProjectTypeOfFormId.Application);
            var groupedResult = result.Select(x => x.ToViewModel());
            return PartialView(groupedResult);
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

            return PartialView(resultList);
        }
        #endregion

        #region Post Action

        /// <summary>
        /// 提交申请书
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubmitApplication(string flag, ERPNWorkToDoViewModel model, FormCollection collection)
        {

            // TODO:
            ///验证
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.UserName = User.Identity.Name;
            if (collection["Date222222222"] != null)
            {
                model.TimeStr = DateTime.Parse(collection["Date222222222"].ToString());
            }
            //model.TimeStr = DateTime.Now;

            var sectionName = MySession[SessionKeyEnum.SectionName].ToString();

            //加载表单内容
            var temerpnformmodel = ERPNFormService.GetEntityById((int)EngineeringProjectTypeOfFormId.Application).ToViewModel();

            var itemList = temerpnformmodel.ItemsList;
            string formKeys, formvalues;
            Hepler.GetFormKeyValueAndRemark(collection, itemList, out formKeys, out formvalues);
            model.FormContent = temerpnformmodel.ContentStr;
            model.FormContent = model.FormContent.Replace(Constant.MacroSectionString, sectionName);
            model.FormContent = model.FormContent.Replace(Constant.MacroUserNameString, User.Identity.Name);
            model.FormContent = FormContentReplaceHelper.ReplaceFormContentValue(model.FormContent, collection);

            int countOfTravelItems;

            if (!string.IsNullOrEmpty(Request["count"]))
            {
                countOfTravelItems = Convert.ToInt32(Request["count"]);
            }
            else
            {
                countOfTravelItems = 0;
            }

            string[] itemNameArray = { "SectionName", "SectionNumber", "CreatedTime" };

            for (int i = 1; i < countOfTravelItems + 1; i++)
            {
                var item = new ProjectBidSectionViewModel();
                item.SectionName = Request["item" + i.ToString() + itemNameArray[0]];
                item.SectionNumber = Request["item" + i.ToString() + itemNameArray[1]];
                item.CreatedTime = Convert.ToDateTime(Request["item" + i.ToString() + itemNameArray[2]]);
                model.BidSectionList.Add(item);
            }

            try
            {
                model.StateNow = "正在办理";
            }
            catch
            {
                model.JieDianName = "结束";
                model.StateNow = "强制结束";
            }

            model.OKUserList = "默认";

            int jiedianid = Convert.ToInt32(model.JieDianID);
            var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
            model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));
            model.FormKeys = formKeys;
            model.FormValues = formvalues;

            if (flag == "Save")//保存
            {
                model.IsTemporary = true;
                model.IsLocked = false;
                model.IsDeleted = false;
                model.IsRejected = false;

                //申请书状态
                model.ApplicationStatus = BiddingProjectStatus.ProjectRegitering.ToString();
                //整个项目进行的状态
                model.ProjectStatus = BiddingProjectStatus.ProjectRegitering.ToString();
                int nworktodoid = this.ApplicationService.AddApplication(model.ToDataTransferObjectModel(), model.BidSectionList.Select(x => x.ConvertTo<ProjectBidSection>()).ToList());
                //todo:日志

                //写系统日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                //已办
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.DoSomething = "保存(工程项目申请书)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.Application.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = nworktodoid.ToString();
                int AddRiZhiSuccess = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                model.NWorkToDoID = nworktodoid;
                /// return RedirectToAction("SubmitApplication", new { id = model.NWorkToDoID });
                return Json(AddRiZhiSuccess, JsonRequestBehavior.AllowGet);
            }
            else//上报
            {
                // 更改当前结点id和name
                var currentNode = ERPNWorkFlowNodeService.GetEntityById(model.JieDianID.Value);
                string nextNodeSerial = currentNode.NextNode;
                var nextNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.NodeSerils == nextNodeSerial && p.WorkFlowID == model.WorkFlowID).ToList();
                model.JieDianID = nextNodeModel.First().ID;
                model.JieDianName = nextNodeModel.First().NodeName;

                //页面加载后就上报,就添加新上报的数据行
                if (model.NWorkToDoID == 0)
                {
                    model.ApplicationStatus = BiddingProjectStatus.ProjectRegitered.ToString();
                    model.ProjectStatus = BiddingProjectStatus.ProjectRegitered.ToString();
                    //上报时设置保存为false、驳回为false、删除为false、冻结为false
                    model.IsTemporary = true;
                    model.IsLocked = false;
                    model.IsDeleted = false;
                    model.IsRejected = false;

                    int nworktodoid = this.ApplicationService.AddApplication(model.ToDataTransferObjectModel(), model.BidSectionList.Select(x => x.ConvertTo<ProjectBidSection>()).ToList());

                    //写系统日志
                    ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                    ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();
                    //已办
                    MyRiZhi.UserName = User.Identity.Name;
                    MyRiZhi.DoSomething = User.Identity.Name + "上报(工程项目申请书)";
                    MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    // MyRiZhi.NotificationContent = "添加的" + model.WenHao + "已经提交";
                    MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.Application.ToString();
                    MyRiZhi.FKAction = "已办";
                    MyRiZhi.FKApplicationID = nworktodoid.ToString();
                    MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                    int AddRiZhiSuccess = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                    //待办
                    MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkFlowID), sectionName);

                    MyRiZhi1.DoSomething = "需要审批(" + User.Identity.Name + "上报的工程项目申请书)";
                    MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    // MyRiZhi1.NotificationContent = "上报的" + model.WenHao + "需要审批";
                    MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.Application.ToString();
                    MyRiZhi1.FKAction = "待办";
                    MyRiZhi1.FKApplicationID = nworktodoid.ToString();
                    MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                    int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());
                    return Json(AddRiZhiSuccess, JsonRequestBehavior.AllowGet);
                }
                //先保存在上报的,就更新保存数据行的IsIsTemporary
                else
                {
                    model.ApplicationStatus = BiddingProjectStatus.ProjectRegitered.ToString();
                    model.ProjectStatus = BiddingProjectStatus.ProjectRegitered.ToString();
                    //上报时设置保存为false、驳回为false、删除为false、冻结为false
                    model.IsTemporary = true;
                    model.IsLocked = false;
                    model.IsDeleted = false;
                    model.IsRejected = false;

                    bool UpdateIstemporySuccess = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());
                    //todo:add rizhi

                    //更新已办，新增上报，写系统日志
                    ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                    ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();
                    //更新已办
                    string nworktodoid = model.NWorkToDoID.ToString();
                    string formname = EngineeringProjectTypeOfFormId.Application.ToString();
                    MyRiZhi = ERPRiZhiService.GetEntities(p => p.FKApplicationID == nworktodoid && p.FkFormName == formname).FirstOrDefault().ToViewModel();
                    MyRiZhi.UserName = User.Identity.Name;
                    MyRiZhi.DoSomething = User.Identity.Name + "上报的(工程项目申请书)";
                    MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    // MyRiZhi.NotificationContent = "添加的" + model.WenHao + "已经提交";
                    MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.Application.ToString();
                    MyRiZhi.FKAction = "已办";
                    MyRiZhi.FKApplicationID = model.NWorkToDoID.ToString();
                    MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                    bool updateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                    //新增待办
                    MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkFlowID), sectionName);
                    MyRiZhi1.DoSomething = "需要审批(" + User.Identity.Name + "上报的工程项目申请书)";
                    MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    // MyRiZhi1.NotificationContent = "上报的" + model.WenHao + "需要审批";
                    MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.Application.ToString();
                    MyRiZhi1.FKAction = "待办";
                    MyRiZhi1.FKApplicationID = model.NWorkToDoID.ToString();
                    MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();
                    int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                    return Json(AddRiZhi1Success, JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        /// 提交审批申请书
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ApplicationAgree(string flag, ERPNWorkToDoViewModel model, FormCollection collection)
        {
            string nodeSerils;
            string act = string.Empty;
            //审批
            if (flag == "Approval")
            {
                var attachment = model.FuJianList;
                string PiShiStr = model.ShenPiYiJian;

                // TODO: 审批上传附件
                model.ShenPiYiJian = attachment;
                model.OKUserList = model.OKUserList + "," + User.Identity.Name;

                if (collection["SingleShenPiYiJian"] != "")
                {
                    //审批意见
                    model.ShenPiYiJian = PiShiStr + model.JieDianName + ":" + collection["SingleShenPiYiJian"].ToString() + Constant.SplitChar;
                }
                else
                {
                    model.ShenPiYiJian = PiShiStr;
                }

                // 更改当前结点id和name
                var erpnrowkflownoderesult1 = ERPNWorkFlowNodeService.GetEntityById(model.JieDianID.Value).ToViewModel();
                string nextNodeSerial = erpnrowkflownoderesult1.NextNode;
                var nextNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.NodeSerils == nextNodeSerial && p.WorkFlowID == model.WorkFlowID).ToList();

                if (nextNodeModel.Count == Constant.ZERO_INT)
                {
                    // 说明审批流程已经完成，审批通过
                    model.JieDianName = "结束";
                    model.ApplicationStatus = ApplicationStatus.ApplicationApproved.ToString();//申请书审批完成
                    model.ProjectStatus = ApplicationStatus.ApplicationApproved.ToString();
                    bool isUpdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());
                    ViewBag.Title = model.JieDianName;

                    nodeSerils = "结束";
                }
                else
                {
                    // 说明审批流程还未完成，正在等待下一级审批
                    model.ApplicationStatus = ApplicationStatus.ApplicationApproving.ToString();//申请书待审批中 
                    model.ProjectStatus = ApplicationStatus.ApplicationApproving.ToString();
                    //根据序号和workflowID获得下一级节点ID
                    model.JieDianID = nextNodeModel.FirstOrDefault().ID;
                    model.JieDianName = nextNodeModel.FirstOrDefault().NodeName;
                    bool isUpdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());
                    ViewBag.Title = model.JieDianName;

                    //下一个节点的NodeSerils
                    nodeSerils = nextNodeModel.FirstOrDefault().NodeSerils;
                }
            }

            //驳回collection["Reject"]
            else
            {
                act = "rejected";
                model.IsRejected = true;
                model.ApplicationStatus = ApplicationStatus.ApplicationRejected.ToString();//申请书审批完成
                model.ProjectStatus = ApplicationStatus.ApplicationRejected.ToString();
                var workFlowNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == model.WorkFlowID).OrderBy(p => p.NodeSerils).Skip(1).Take(1).FirstOrDefault();

                model.JieDianID = workFlowNodeModel.ID;
                model.JieDianName = workFlowNodeModel.NodeName.ToString();

                bool isUpdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());

                nodeSerils = "驳回";

            }

            string formname = EngineeringProjectTypeOfFormId.Application.ToString();
            var rizhiresult = ERPRiZhiService.GetEntities(p => p.FKApplicationID == model.NWorkToDoID.ToString() && p.FkFormName == formname && p.FKAction == "待办").FirstOrDefault();

            IEnumerable<ERPNWorkFlowNodeViewModel> workFlowNodeModelList = new List<ERPNWorkFlowNodeViewModel>();
            //除填写申请书和第二级审批
            workFlowNodeModelList = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == model.WorkFlowID).OrderBy(p => p.NodeSerils).Skip(2).Select(x => x.ToViewModel());
            //将从第三节点（包括第三节点）的NodeSerials全部装在workFlowNodeModelList
            string nodeSerilsList = "";
            foreach (var item in workFlowNodeModelList)
            {
                nodeSerilsList = nodeSerilsList + item.NodeSerils;
            }
            //判断下一节点是否存在
            if (nodeSerilsList.Contains(nodeSerils))
            {
                //日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();

                //待办变成已办
                MyRiZhi.FKAction = "已办";
                MyRiZhi.ID = rizhiresult.ID;
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.DoSomething = "已审批" + model.UserName + "上报的工程项目申请书";
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.Application.ToString();
                MyRiZhi.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //新生成待办，下一审批人的待办
                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.ID = rizhiresult.ID;
                MyRiZhi1.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.DoSomething = "需要审批(" + model.UserName + "上报的工程项目申请书)";
                MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.Application.ToString();
                MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                ERPNWorkFlowNodeViewModel erpnWorkFlowNodeModel = new ERPNWorkFlowNodeViewModel();
                erpnWorkFlowNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == model.WorkFlowID && p.NodeSerils == nodeSerils).FirstOrDefault().ToViewModel();

                MyRiZhi1.UserName = erpnWorkFlowNodeModel.SPDefaultList;
                int returnid = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());
                //继续审批
                return Json(returnid, JsonRequestBehavior.AllowGet);
            }
            else if (nodeSerils == "结束")
            {
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();

                MyRiZhi.FKAction = "已办";
                MyRiZhi.ID = rizhiresult.ID;
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.DoSomething = "已审批" + model.UserName + "上报的工程项目申请书";
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.Application.ToString();
                MyRiZhi.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //新生成新建项目确立的待办，申请人待办
                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.ID = rizhiresult.ID;
                MyRiZhi1.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.UserName = model.UserName;
                MyRiZhi1.DoSomething = model.UserName + "需要添加项目确立";
                MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.Establishment.ToString();
                MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                int returnid = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());
                // 更新成功并且更新后，申请书状态变为 已经审批通过，跳转到项目确立页面
                return Json(returnid, JsonRequestBehavior.AllowGet);
            }
            //toDoAction == "驳回"
            else
            {
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();

                //审批人已办
                MyRiZhi.FKAction = "已办";
                MyRiZhi.ID = rizhiresult.ID;
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.DoSomething = "已驳回" + model.UserName + "上报的工程项目申请书";
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.Application.ToString();
                MyRiZhi.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                bool UpdateRiZhiSuccess1 = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());
                //申请人待办

                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.ID = rizhiresult.ID;
                MyRiZhi1.UserName = model.UserName;
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.DoSomething = "工程项目申请书被驳回，需修改数据";
                MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.Application.ToString();
                MyRiZhi1.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                int returnid = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                return Json(returnid, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 申请书被驳回
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ApplicationRejected(string flag, ERPNWorkToDoViewModel model, FormCollection collection)
        {
            ///验证
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.UserName = User.Identity.Name;
            model.TimeStr = DateTime.Now;
            var sectionName = MySession[SessionKeyEnum.SectionName].ToString();

            //加载表单内容
            var temerpnformmodel = ERPNFormService.GetEntityById((int)EngineeringProjectTypeOfFormId.Application).ToViewModel();
            model.FormContent = temerpnformmodel.ContentStr;
            model.FormContent = model.FormContent.Replace(Constant.MacroSectionString, sectionName);
            model.FormContent = model.FormContent.Replace(Constant.MacroUserNameString, User.Identity.Name);

            string formKeys = "项目类型#立项类型#立项时间#登记人#科室#项目负责人#项目参与人员#概述#外部编号";

            string programeType = collection["Drop1364262284"].ToString();
            string prjectapprovalType = collection["Drop968600384"].ToString();
            string applyDate = collection["Date442495555"].ToString();
            string applyMan = collection["Text435761615"].ToString();
            string department = collection["Text289827346"].ToString();
            string programLeader = collection["Text1783445882"].ToString();
            string temMenber = collection["Text309804476"].ToString();
            string summarize = collection["TextArea683159807"].ToString();
            string externalNumber = collection["Text309803372"].ToString();
            string formvalues = programeType + "#" + prjectapprovalType + "#" + applyDate + "#" + applyMan + "#" + department + "#" + programLeader + "#" + temMenber + "#" + summarize + "#" + externalNumber;

            model.FormContent = FormContentReplaceHelper.ReplaceFormContentValue(model.FormContent, collection);

            try
            {
                model.StateNow = "正在办理";
            }
            catch
            {
                model.JieDianName = "结束";
                model.StateNow = "强制结束";
            }

            model.OKUserList = "默认";

            int jiedianid = Convert.ToInt32(model.JieDianID);
            var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
            model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));
            model.FormKeys = formKeys;
            model.FormValues = formvalues;

            if (flag == "Save")//根据已有ID保存更新
            {
                model.IsTemporary = true;
                model.IsLocked = false;
                model.IsDeleted = false;
                model.IsRejected = false;

                //申请书状态
                model.ApplicationStatus = ApplicationStatus.ApplicationRejected.ToString();
                //整个项目进行的状态
                model.ProjectStatus = ApplicationStatus.ApplicationRejected.ToString();
                bool saveUpdateSuccess = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());
                //todo:日志

                //写系统日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                //已办
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.DoSomething = "保存(工程项目申请书)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.Application.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.TimeStr = DateTime.Now;
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();
                MyRiZhi.FKApplicationID = model.NWorkToDoID.ToString();
                int UpdateRiZhiSuccess = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                return Json(UpdateRiZhiSuccess, JsonRequestBehavior.AllowGet);
            }
            else//根据已有ID上报更新
            {
                model.ApplicationStatus = ApplicationStatus.ApplicationApproving.ToString();
                model.ProjectStatus = ApplicationStatus.ApplicationApproving.ToString();
                //上报时设置保存为false、驳回为false、删除为false、冻结为false
                model.IsTemporary = false;
                model.IsRejected = false;
                model.IsDeleted = false;
                model.IsLocked = false;

                bool UpdateIstemporySuccess = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());
                //todo:add rizhi

                //更新已办，新增上报，写系统日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();
                //更新已办
                string nworktodoid = model.NWorkToDoID.ToString();
                string formname = EngineeringProjectTypeOfFormId.Application.ToString();
                MyRiZhi = ERPRiZhiService.GetEntities(p => p.FKApplicationID == nworktodoid && p.FKAction == "待办").FirstOrDefault().ToViewModel();
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.DoSomething = User.Identity.Name + "上报的(工程项目申请书)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.Application.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi.TimeStr = DateTime.Now;
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                bool updateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //新增待办
                MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkFlowID), sectionName); ;
                MyRiZhi1.DoSomething = "需要审批(" + User.Identity.Name + "上报的工程项目申请书)";
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.Application.ToString();
                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();
                int UpdateRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                return Json(UpdateRiZhi1Success, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// 项目确立提交
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ProjectEstablish(string flag, ProjectRecordViewModel projectModel, FormCollection collection)
        {
            ///验证
            if (!ModelState.IsValid)
            {
                return View(projectModel);
            }

            ERPNWorkToDoViewModel erpnworktodomodel = new ERPNWorkToDoViewModel();

            string fundstime = collection["FundsTime"].ToString();
            string endtime = collection["EndTime"].ToString();
            string superiorfundtime = collection["SuperiorFundTime"].ToString();
            DateTime fundtime = Convert.ToDateTime(fundstime);
            DateTime etime = Convert.ToDateTime(endtime);
            DateTime supertime = Convert.ToDateTime(superiorfundtime);
            projectModel.FundsTime = fundtime;
            projectModel.EndTime = etime;
            projectModel.SuperiorFundTime = supertime;

            projectModel.UpdatedTime = DateTime.Now;

            //保存
            projectModel.CreatedBy = User.Identity.Name;
            projectModel.UpdatedBy = User.Identity.Name;
            projectModel.Total = projectModel.HospitalFunds + projectModel.SuperiorFunds;
            erpnworktodomodel = ApplicationService.GetEntityById(projectModel.ApplicationId).ToViewModel();
            //保存
            if (flag == "Save")
            {
                //添加申请人保存
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                //已办
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.DoSomething = "保存(项目确立)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.Establishment.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = projectModel.ApplicationId.ToString();
                MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                int returnRiZhiId = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //第一次保存
                if (projectModel.PojectEstablishID == 0)
                {
                    string act = "save";
                    projectModel.IsTemporary = true;
                    projectModel.ProjectLevel = erpnworktodomodel.FormValues.Split(Constant.SharpChar)[1];
                    int returnProjectId = ProjectRecordService.AddProjectRecord(projectModel.ToDataTransferObjectModel());

                    //更新申请书中的applicationstatus
                    erpnworktodomodel.ApplicationStatus = ApplicationStatus.ProjectEstablishing.ToString();
                    erpnworktodomodel.ProjectStatus = ApplicationStatus.ProjectEstablishing.ToString();
                    bool isUpdateSuccess = ApplicationService.UpdateApplication(erpnworktodomodel.ToDataTransferObjectModel());

                    //return RedirectToAction("ProjectEstablish", new { id = projectModel.ApplicationId, nextaction = act });
                    return Json(returnProjectId, JsonRequestBehavior.AllowGet);
                }
                //第二次或第N次保存
                else
                {
                    string act = "save";
                    projectModel.IsTemporary = true;
                    projectModel.ProjectLevel = erpnworktodomodel.FormValues.Split(Constant.SharpChar)[1];
                    bool updateSuccess = ProjectRecordService.UpdateProjectRecord(projectModel.ToDataTransferObjectModel());

                    //更新申请书中的applicationstatus
                    erpnworktodomodel.ApplicationStatus = ApplicationStatus.ProjectEstablishing.ToString();
                    erpnworktodomodel.ProjectStatus = ApplicationStatus.ProjectEstablishing.ToString();
                    bool isUpdateSuccess = ApplicationService.UpdateApplication(erpnworktodomodel.ToDataTransferObjectModel());

                    return RedirectToAction("ProjectEstablish", new { id = projectModel.ApplicationId, nextaction = act });
                }
            }
            //上报
            else
            {
                //写系统日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();

                //待办变成已办
                string formname = EngineeringProjectTypeOfFormId.Establishment.ToString();
                var rizhiresult = ERPRiZhiService.GetEntities(p => p.FKApplicationID == projectModel.ApplicationId.ToString() && p.FkFormName == formname && p.FKAction == "待办").FirstOrDefault();
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.ID = rizhiresult.ID;
                MyRiZhi.DoSomething = "已填写项目确立";
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.Establishment.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = projectModel.ApplicationId.ToString();
                MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //新生成待办
                MyRiZhi1.UserName = erpnworktodomodel.UserName;
                MyRiZhi1.DoSomething = erpnworktodomodel.UserName + "需要添加合同记录";
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.Contract.ToString();
                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.FKApplicationID = projectModel.ApplicationId.ToString();
                MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                int AddRiZhiSuccess = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                // 项目确立时间（只有上报时才有）
                projectModel.CreatedTime = DateTime.Now;
                projectModel.StartTime = DateTime.Now;

                //页面加载后就上报，就添加新的数据
                if (projectModel.PojectEstablishID == 0)
                {
                    projectModel.ProjectLevel = erpnworktodomodel.FormValues.Split(Constant.SharpChar)[1];
                    //上报时设置保存为false、驳回为false、删除为false、冻结为false
                    projectModel.IsTemporary = false;
                    projectModel.IsRejected = false;
                    projectModel.IsDeleted = false;
                    projectModel.IsLocked = false;
                    int Addsuccess = this.ProjectRecordService.AddProjectRecord(projectModel.ToDataTransferObjectModel());

                    erpnworktodomodel.ApplicationStatus = ApplicationStatus.ProjectEstablished.ToString();
                    erpnworktodomodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    //更新申请书中ApplicationStatus的状态为ProjectEstablished
                    bool isupdate = ApplicationService.UpdateApplication(erpnworktodomodel.ToDataTransferObjectModel());

                    //return RedirectToAction("SignContract", new { id = projectModel.ApplicationId });
                    return Json(Addsuccess, JsonRequestBehavior.AllowGet);
                }
                //先点保存再上报，就更新保存数据行的IsTemprory
                else
                {
                    projectModel.ProjectLevel = erpnworktodomodel.FormValues.Split(Constant.SharpChar)[1];
                    //上报时设置保存为false、驳回为false、删除为false、冻结为false
                    projectModel.IsTemporary = false;
                    projectModel.IsRejected = false;
                    projectModel.IsDeleted = false;
                    projectModel.IsLocked = false;
                    bool isUpdateSuccess = this.ProjectRecordService.UpdateProjectRecord(projectModel.ToDataTransferObjectModel());

                    erpnworktodomodel.ApplicationStatus = ApplicationStatus.ProjectEstablished.ToString();
                    erpnworktodomodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    //更新申请书中ApplicationStatus的状态为ProjectEstablished
                    bool isupdate = ApplicationService.UpdateApplication(erpnworktodomodel.ToDataTransferObjectModel());

                    return Json(AddRiZhiSuccess, JsonRequestBehavior.AllowGet);
                    //return RedirectToAction("SignContract", new { id = projectModel.ApplicationId });
                }
            }
        }

        /// <summary>
        /// 合同记录,签订合同
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SignContract(string flag, ERPNWorkToDoViewModel model, FormCollection collection)
        {
            model.UserName = User.Identity.Name;
            model.TimeStr = DateTime.Now;
            var sectionName = MySession[SessionKeyEnum.SectionName].ToString();

            //加载表单内容
            var temerpnformmodel = ERPNFormService.GetEntityById((int)EngineeringProjectTypeOfFormId.Contract).ToViewModel();
            model.FormContent = temerpnformmodel.ContentStr;
            model.FormContent = model.FormContent.Replace(Constant.MacroSectionString, sectionName);
            model.FormContent = model.FormContent.Replace(Constant.MacroUserNameString, User.Identity.Name);
            model.FormContent = FormContentReplaceHelper.ReplaceFormContentValue(model.FormContent, collection);

            string formKeys = "合同签订时间#项目实施计划#成功形式和数量";

            string contractTime = collection["Date652495532"].ToString();
            string projectPlan = collection["TextArea1266765043"].ToString();
            string resultQuantity = collection["TextArea3266765036"].ToString();

            ///资金限额表
            string projectType = collection["ProjectType"].ToString();
            string threshold = collection["Threshold"].ToString();

            string projectType1 = collection["ProjectType1"].ToString();
            string threshold1 = collection["Threshold1"].ToString();

            string projectType2 = collection["ProjectType2"].ToString();
            string threshold2 = collection["Threshold2"].ToString();

            string projectType3 = collection["ProjectType3"].ToString();
            string threshold3 = collection["Threshold3"].ToString();

            string projectType4 = collection["ProjectType4"].ToString();
            string threshold4 = collection["Threshold4"].ToString();

            string projectType5 = collection["ProjectType5"].ToString();
            string threshold5 = collection["Threshold5"].ToString();

            string projectType6 = collection["ProjectType6"].ToString();
            string threshold6 = collection["Threshold6"].ToString();

            string projectType7 = collection["ProjectType7"].ToString();
            string threshold7 = collection["Threshold7"].ToString();

            string projectType8 = collection["ProjectType8"].ToString();
            string threshold8 = collection["Threshold8"].ToString();

            string projectType9 = collection["ProjectType9"].ToString();
            string threshold9 = collection["Threshold9"].ToString();

            string projectType0 = collection["ProjectType0"].ToString();
            string threshold0 = collection["Threshold0"].ToString();

            string formvalues = contractTime + "#" + projectPlan + "#" + resultQuantity;

            string[] projectTypeList = { "ProjectType", "ProjectType1", "ProjectType2", "ProjectType3", "ProjectType4", "ProjectType5", "ProjectType6", "ProjectType7", "ProjectType8", "ProjectType9", "ProjectType0" };
            string[] thresholdList = { "threshold", "threshold1", "threshold2", "threshold3", "threshold4", "threshold5", "threshold6", "threshold7", "threshold8", "threshold9", "threshold0" };




            model.FormKeys = formKeys;
            model.FormValues = formvalues;
            try
            {
                model.StateNow = "正在办理";
            }
            catch
            {
                model.JieDianName = "结束";
                model.StateNow = "强制结束";
            }

            model.OKUserList = "默认";

            if (flag == "Save")//保存
            {
                //日志
                //添加申请人保存
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                //已办
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.DoSomething = "保存(合同记录)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.Contract.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                int returnRiZhiId = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //第一次保存
                if (model.NWorkToDoID == 0)
                {

                    string act = "save";
                    model.IsTemporary = true;
                    model.IsDeleted = false;
                    model.NWorkToDoID = model.ApplicationId;

                    int jiedianid = Convert.ToInt32(model.JieDianID);
                    var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                    model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));

                    int returnid = this.ApplicationService.AddApplication(model.ToDataTransferObjectModel());

                    //更新申请书中的ApplicationStatus为ContractSigning
                    ERPNWorkToDoViewModel erpnworktodomodel = new ERPNWorkToDoViewModel();
                    erpnworktodomodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                    erpnworktodomodel.ApplicationStatus = ApplicationStatus.ContractSigning.ToString();
                    erpnworktodomodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    bool isUpdateSuccess = ApplicationService.UpdateApplication(erpnworktodomodel.ToDataTransferObjectModel());

                    return Json(returnid, JsonRequestBehavior.AllowGet);
                    // return RedirectToAction("SignContract", new { id = returnid, nextaction = act });
                }
                //第二次或第N次保存
                else
                {
                    string act = "save";
                    model.IsTemporary = true;

                    int jiedianid = Convert.ToInt32(model.JieDianID);
                    var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                    model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));

                    bool isupdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());

                    //更新申请书中的ApplicationStatus为ContractSigning
                    ERPNWorkToDoViewModel erpnworktodomodel = new ERPNWorkToDoViewModel();
                    erpnworktodomodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                    erpnworktodomodel.ApplicationStatus = ApplicationStatus.ContractSigning.ToString();
                    erpnworktodomodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    bool isUpdateSuccess = ApplicationService.UpdateApplication(erpnworktodomodel.ToDataTransferObjectModel());

                    return RedirectToAction("SignContract", new { id = model.NWorkToDoID, nextaction = act });
                }
            }
            //上报
            else
            {
                ERPNWorkToDoViewModel erpnworktodomodel = new ERPNWorkToDoViewModel();
                erpnworktodomodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                //写系统日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();

                //待办变成已办
                string formname = EngineeringProjectTypeOfFormId.Contract.ToString();
                var rizhiresult = ERPRiZhiService.GetEntities(p => p.FKApplicationID == model.ApplicationId.ToString() && p.FkFormName == formname && p.FKAction == "待办").FirstOrDefault();
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.ID = rizhiresult.ID;
                MyRiZhi.DoSomething = "已填写合同记录";
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.Contract.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                // 更改当前结点id和name
                var currentNode = ERPNWorkFlowNodeService.GetEntityById(model.JieDianID.Value);
                string nextNodeSerial = currentNode.NextNode;
                var nextNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.NodeSerils == nextNodeSerial && p.WorkFlowID == model.WorkFlowID).ToList();
                model.JieDianID = nextNodeModel.First().ID;
                model.JieDianName = nextNodeModel.First().NodeName;

                //页面加载后就上报，就添加新的数据
                if (model.NWorkToDoID == 0)
                {

                    for (int i = 0; i < 11; i++)
                    {
                        var item = new FundsThresholdViewModel();

                        item.FundsType = Request[projectTypeList[i]];
                        item.Threshold = string.IsNullOrEmpty(Request[thresholdList[i]]) ? 0 : Convert.ToDouble(Request[thresholdList[i]]);
                        item.NWorkToDoID = model.ApplicationId;
                        item.IsDeleted = false;
                        item.ProjectType = Constant.ScienceResearch.ToString();
                        item.CreatedBy = model.UserName;
                        item.CreatedTime = DateTime.Now;
                        item.ModuleName = "ScienceResearch";
                        item.UpdatedBy = null;
                        item.UpdatedTime = null;
                        int Add = FundsThresholdService.AddFundsThreshold(item.ToDataTransferObjectModel());
                        //model.FundsLimitsList.Add(item);
                    }
                    //上报时设置保存为false、驳回为false、删除为false、冻结为false
                    model.IsTemporary = false;
                    model.IsRejected = false;
                    model.IsDeleted = false;
                    model.IsLocked = false;

                    int jiedianid = Convert.ToInt32(model.JieDianID);
                    var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                    model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));

                    int isAddSuccess = this.ApplicationService.AddApplication(model.ToDataTransferObjectModel());

                    //更新申请书中的ApplicationStatus为ProjectProcessing
                    erpnworktodomodel.ApplicationStatus = ApplicationStatus.ContractSigned.ToString();
                    erpnworktodomodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    bool isUpdateSuccess = ApplicationService.UpdateApplication(erpnworktodomodel.ToDataTransferObjectModel());

                    return Json(isAddSuccess, JsonRequestBehavior.AllowGet);
                    //return RedirectToAction("ProcessRecordList", new { id = model.ApplicationId });
                }
                //先点保存再上报，就更新保存数据行的IsTemprory
                else
                {
                    //上报时设置保存为false、驳回为false、删除为false、冻结为false
                    model.IsTemporary = false;
                    model.IsRejected = false;
                    model.IsDeleted = false;
                    model.IsLocked = false;
                    int jiedianid = Convert.ToInt32(model.JieDianID);
                    var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                    model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));

                    bool isUpdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());

                    //更新申请书中的ApplicationStatus为ProjectProcessing
                    erpnworktodomodel.ApplicationStatus = ApplicationStatus.ContractSigned.ToString();
                    erpnworktodomodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    bool isUpdateSuccess = ApplicationService.UpdateApplication(erpnworktodomodel.ToDataTransferObjectModel());

                    ///成功标志
                    int isAddsuccess = 1;
                    return Json(isAddsuccess, JsonRequestBehavior.AllowGet);
                    //return RedirectToAction("ProcessRecordList", new { id = model.ApplicationId });
                }
            }
            ////修改数
            //else
            //{
            //    string act = "updatedata";
            //    return RedirectToAction("SignContract", new { id = model.NWorkToDoID, nextaction = act });
            //}
        }

        /// <summary>
        /// 过程记录提交
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ProcessRecords(string flag, ERPNWorkToDoViewModel model, FormCollection collection)
        {
            model.UserName = User.Identity.Name;
            model.TimeStr = DateTime.Now;
            var sectionName = MySession[SessionKeyEnum.SectionName].ToString();

            #region 数据准备

            //加载表单内容
            var temerpnformmodel = ERPNFormService.GetEntityById((int)EngineeringProjectTypeOfFormId.ProcessRecord).ToViewModel();
            model.FormContent = temerpnformmodel.ContentStr;
            model.FormContent = model.FormContent.Replace(Constant.MacroSectionString, sectionName);
            model.FormContent = model.FormContent.Replace(Constant.MacroUserNameString, User.Identity.Name);

            string formKeys = "课题名称#记录类型#记录时间#记录人";
            string name = collection["Text397670573"].ToString();
            string type = collection["Drop46201924"].ToString();
            string time = collection["Date1476233541"].ToString();
            string people = collection["Text309804476"].ToString();
            string formvalues = name + "#" + type + "#" + time + "#" + people;

            model.FormContent = FormContentReplaceHelper.ReplaceFormContentValue(model.FormContent, collection);

            try
            {
                model.StateNow = "正在办理";
            }
            catch
            {
                model.JieDianName = "结束";
                model.StateNow = "强制结束";
            }

            model.OKUserList = "默认";

            int jiedianid = Convert.ToInt32(model.JieDianID);
            var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
            model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));
            model.FormKeys = formKeys;
            model.FormValues = formvalues;

            #endregion

            //保存
            if (flag == "Save")
            {
                //日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                //已办
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.DoSomething = "保存(过程记录)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.ProcessRecord.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi.TimeStr = DateTime.Now;
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                #region 过程记录

                string act = "save";
                int returnProcessId = 0;
                model.IsTemporary = true;
                model.IsDeleted = false;

                //第一次保存
                if (model.NWorkToDoID == 0)
                {
                    model.IsLocked = false;
                    returnProcessId = this.ApplicationService.AddApplication(model.ToDataTransferObjectModel());
                    model.NWorkToDoID = returnProcessId;
                    #region 日志


                    //FKApplicationID是过程记录的ID
                    MyRiZhi.FKApplicationID = returnProcessId.ToString();

                    int returnRiZhiId = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                    #endregion
                }
                //第二次或第N次保存
                else
                {
                    bool isupdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());

                    #region 日志


                    //FKApplicationID是过程记录的ID
                    MyRiZhi.FKApplicationID = model.NWorkToDoID.ToString();

                    int returnRiZhiId = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                    #endregion
                }

                #endregion

                #region 更新过程记录相关申请书的applicationstatus

                ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                tempmodel.ApplicationStatus = ApplicationStatus.ProjectProcessing.ToString();
                tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                bool isMainProjectUpdate = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());

                #endregion

                ViewBag.Id = model.ApplicationId;

                ///成功标志
                int isAddsuccess = 1;
                return Json(isAddsuccess, JsonRequestBehavior.AllowGet);
            }
            //上报
            else
            {
                #region 已办日志

                //写系统日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();
                //已办
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.DoSomething = "添加(过程记录)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.ProcessRecord.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi.TimeStr = DateTime.Now;
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                #endregion

                #region 待办日志

                //待办
                MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkFlowID), sectionName);
                MyRiZhi1.DoSomething = "需要审批(过程记录)";
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.ProcessRecord.ToString();
                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                // 更改当前结点id和name
                var currentNode = ERPNWorkFlowNodeService.GetEntityById(model.JieDianID.Value);
                string nextNodeSerial = currentNode.NextNode;
                var nextNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.NodeSerils == nextNodeSerial && p.WorkFlowID == model.WorkFlowID).ToList();
                model.JieDianID = nextNodeModel.First().ID;
                model.JieDianName = nextNodeModel.First().NodeName;

                #endregion

                #region 过程记录

                //上报时设置保存为false、驳回为false、删除为false、冻结为false
                model.IsTemporary = false;
                model.IsRejected = false;
                model.IsDeleted = false;
                model.IsLocked = false;

                int nworktodoId = 0;
                //页面加载后就上报，就添加新的数据
                if (model.NWorkToDoID == 0)
                {
                    int returnprocessid = this.ApplicationService.AddApplication(model.ToDataTransferObjectModel());
                    nworktodoId = returnprocessid;
                    //过程记录的ID
                    MyRiZhi.FKApplicationID = returnprocessid.ToString();
                    int AddRiZhiSuccess = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());
                    //过程记录的ID
                    MyRiZhi1.FKApplicationID = returnprocessid.ToString();
                    int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());
                }
                //先点保存再上报，就更新保存数据行的IsTemprory
                else
                {
                    bool isUpdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());
                    //过程记录的ID
                    MyRiZhi.FKApplicationID = model.NWorkToDoID.ToString();
                    nworktodoId = model.NWorkToDoID;
                    int AddRiZhiSuccess = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());
                    //过程记录的ID
                    MyRiZhi1.FKApplicationID = model.NWorkToDoID.ToString();
                    int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());
                }

                #endregion

                #region 更新过程记录相关申请书的applicationstatus

                ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                tempmodel.ApplicationStatus = ApplicationStatus.ProjectProcessing.ToString();
                tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                bool isMainProjectUpdate = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());

                #endregion
                ERPNWorkToDoViewModel returnmodel = ApplicationService.GetEntityById(nworktodoId).ToViewModel();
                //上报成功的标志
                ViewBag.SendUpSuccess = true;

                ///成功标志
                int isAddsuccess = 1;
                return Json(isAddsuccess, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 过程记录审批提交
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult processAgree(string flag, ERPNWorkToDoViewModel model, FormCollection collection)
        {
            //action flag
            //string toDoAction;
            string nodeSerils;
            string act = string.Empty;
            //审批
            if (flag == "Approval")
            {
                var attachment = model.FuJianList;
                string PiShiStr = model.ShenPiYiJian;
                // TODO: 审批上传附件
                model.ShenPiYiJian = attachment;
                model.OKUserList = model.OKUserList + "," + User.Identity.Name;
                if (collection["SingleShenPiYiJian"] != "")
                {
                    //审批意见
                    model.ShenPiYiJian = PiShiStr + model.JieDianName + ":" + collection["SingleShenPiYiJian"].ToString() + Constant.SplitChar;
                }
                else
                {
                    model.ShenPiYiJian = PiShiStr;
                }
                // 更改当前结点id和name
                // 说明过程记录审批流程还未完成，正在等待下一级审批
                var erpnrowkflownoderesult1 = ERPNWorkFlowNodeService.GetEntityById(model.JieDianID.Value).ToViewModel();
                string nextNodeSerial = erpnrowkflownoderesult1.NextNode;
                var nextNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.NodeSerils == nextNodeSerial && p.WorkFlowID == model.WorkFlowID).ToList();
                // 说明过程记录审批流程已经完成，审批通过
                if (nextNodeModel.Count == Constant.ZERO_INT)
                {
                    model.JieDianName = "结束";
                    bool isUpdateSuccess = ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());
                    ViewBag.Title = model.JieDianName;

                    nodeSerils = "结束";

                }
                // 说明过程记录审批流程还未完成，正在等待下一级审批
                else
                {
                    ///根据序号和workflowID获得下一级节点ID
                    model.JieDianID = nextNodeModel.FirstOrDefault().ID;
                    model.JieDianName = nextNodeModel.FirstOrDefault().NodeName;

                    bool isUpdateSuccess = ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());
                    ViewBag.Title = model.JieDianName;

                    //下一个节点的NodeSerils
                    nodeSerils = nextNodeModel.FirstOrDefault().NodeSerils;
                }
            }
            //驳回collection["Reject"]
            else
            {
                act = "rejected";
                model.IsRejected = true;
                var workFlowNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == model.WorkFlowID).OrderBy(p => p.NodeSerils).Skip(1).Take(1).FirstOrDefault();

                model.JieDianID = workFlowNodeModel.ID;
                model.JieDianName = workFlowNodeModel.NodeName.ToString();

                bool isUpdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());

                nodeSerils = "驳回";
            }

            string formname = EngineeringProjectTypeOfFormId.ProcessRecord.ToString();
            var rizhiresult = ERPRiZhiService.GetEntities(p => p.FKApplicationID == model.NWorkToDoID.ToString() && p.FkFormName == formname && p.FKAction == "待办").FirstOrDefault();
            ERPNWorkToDoViewModel erpnworktodoModel = new ERPNWorkToDoViewModel();
            erpnworktodoModel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
            IEnumerable<ERPNWorkFlowNodeViewModel> workFlowNodeModelList = new List<ERPNWorkFlowNodeViewModel>();
            //除填写申请书和第二级审批
            workFlowNodeModelList = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == model.WorkFlowID).OrderBy(p => p.NodeSerils).Skip(2).Select(x => x.ToViewModel());
            //将从第三节点（包括第三节点）的NodeSerials全部装在workFlowNodeModelList
            string nodeSerilsList = "";
            foreach (var item in workFlowNodeModelList)
            {
                nodeSerilsList = nodeSerilsList + item.NodeSerils;
            }
            //判断下一节点是否存在
            if (nodeSerilsList.Contains(nodeSerils))
            {
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();

                //待办变成已办
                MyRiZhi.FKAction = "已办";
                MyRiZhi.ID = rizhiresult.ID;
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.DoSomething = "已审批" + erpnworktodoModel.UserName + "上报的过程记录";
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.ProcessRecord.ToString();
                //过程记录的ID
                MyRiZhi.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //新生成待办，下一审批人的待办
                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.ID = rizhiresult.ID;
                //过程记录的ID
                MyRiZhi1.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.DoSomething = "需要审批(" + erpnworktodoModel.UserName + "上报的过程记录)";
                MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.ProcessRecord.ToString();
                MyRiZhi1.TimeStr = DateTime.Now;
                MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                ERPNWorkFlowNodeViewModel erpnWorkFlowNodeModel = new ERPNWorkFlowNodeViewModel();
                erpnWorkFlowNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == model.WorkFlowID && p.NodeSerils == nodeSerils).FirstOrDefault().ToViewModel();

                MyRiZhi1.UserName = erpnWorkFlowNodeModel.SPDefaultList;

                int returnid = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                return Json(returnid, JsonRequestBehavior.AllowGet);

                //继续审批
                //return RedirectToAction("processAgree", new { id = model.NWorkToDoID });
            }
            else if (nodeSerils == "结束")
            {
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();

                MyRiZhi.FKAction = "已办";
                MyRiZhi.ID = rizhiresult.ID;
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.DoSomething = "已审批" + erpnworktodoModel.UserName + "上报的过程记录";
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.ProcessRecord.ToString();
                //过程记录的ID
                MyRiZhi.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                ///成功标志
                int isAddsuccess = 1;
                return Json(isAddsuccess, JsonRequestBehavior.AllowGet);
                //// 更新成功并且更新后，申请书状态变为 已经审批通过，跳转到项目确立页面
                //return RedirectToAction("processAgree", new { id = model.NWorkToDoID });
            }
            //toDoAction == "驳回"
            else
            {
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();

                //审批人已办
                MyRiZhi.FKAction = "已办";
                MyRiZhi.ID = rizhiresult.ID;
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.DoSomething = "已驳回" + erpnworktodoModel.UserName + "上报的过程记录";
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.ProcessRecord.ToString();
                //过程记录的ID
                MyRiZhi.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi.TimeStr = DateTime.Now;
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                bool UpdateRiZhiSuccess1 = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());
                //申请人待办

                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.ID = rizhiresult.ID;
                MyRiZhi1.UserName = erpnworktodoModel.UserName;
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.DoSomething = "过程记录被驳回，需修改数据";
                MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.ProcessRecord.ToString();
                //过程记录的ID 
                MyRiZhi1.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi1.TimeStr = DateTime.Now;
                MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                int returnid = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                return Json(returnid, JsonRequestBehavior.AllowGet);
                //return RedirectToAction("ProcessRecordsRejected", new { id = model.NWorkToDoID });
            }

        }
        /// <summary>
        /// 过程记录审批驳回
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ProcessRecordsRejected(ERPNWorkToDoViewModel model, FormCollection collection)
        {
            model.UserName = User.Identity.Name;
            model.TimeStr = DateTime.Now;
            var sectionName = MySession[SessionKeyEnum.SectionName].ToString();

            //加载表单内容
            var temerpnformmodel = ERPNFormService.GetEntityById((int)EngineeringProjectTypeOfFormId.ProcessRecord).ToViewModel();
            model.FormContent = temerpnformmodel.ContentStr;
            model.FormContent = model.FormContent.Replace(Constant.MacroSectionString, sectionName);
            model.FormContent = model.FormContent.Replace(Constant.MacroUserNameString, User.Identity.Name);

            string formKeys = "课题名称#记录类型#记录时间#记录人";
            string name = collection["Text397670573"].ToString();
            string type = collection["Drop46201924"].ToString();
            string time = collection["Date1476233541"].ToString();
            string people = collection["Text309804476"].ToString();
            string formvalues = name + "#" + type + "#" + time + "#" + people;

            model.FormContent = FormContentReplaceHelper.ReplaceFormContentValue(model.FormContent, collection);

            try
            {
                model.StateNow = "正在办理";
            }
            catch
            {
                model.JieDianName = "结束";
                model.StateNow = "强制结束";
            }

            model.OKUserList = "默认";

            int jiedianid = Convert.ToInt32(model.JieDianID);
            var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
            model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));
            model.FormKeys = formKeys;
            model.FormValues = formvalues;

            //保存
            if (collection["Save"] != null)
            {
                //日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                //已办
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.DoSomething = "保存(过程记录)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.ProcessRecord.ToString();
                MyRiZhi.FKAction = "已办";
                //过程记录的ID
                MyRiZhi.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi.TimeStr = DateTime.Now;
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                int returnRiZhiId = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                model.IsTemporary = true;
                model.IsLocked = false;
                model.IsDeleted = false;
                model.IsRejected = false;

                bool isupdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());

                ViewBag.Id = model.ApplicationId;


                ///上报成功标志
                ViewBag.SendUpSuccess = true;

                return View(model);
                // return RedirectToAction("ProcessRecordsRejected", new { id = model.NWorkToDoID.ToString() });
            }
            //上报collection["Reported"]
            else
            {
                //写系统日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();
                //已办
                MyRiZhi = ERPRiZhiService.GetEntities(p => p.FKApplicationID == model.NWorkToDoID.ToString() && p.FKAction == "待办").FirstOrDefault().ToViewModel();
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.DoSomething = "添加(过程记录)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.ProcessRecord.ToString();
                MyRiZhi.FKAction = "已办";
                //过程记录的ID
                MyRiZhi.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi.TimeStr = DateTime.Now;
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                #region 科室

                ////登陆人所在科室
                //string departments;
                //string userId;
                //string userName = User.Identity.Name;
                //using (ApplicationDbContext userManager = new ApplicationDbContext())
                //{
                //    var currentUser = userManager.Users.Where(p => p.UserName == userName).FirstOrDefault();

                //    userId = currentUser.Id;
                //    var currentSection = userManager.Sections.Where(x => x.ApplicationUsers.Where(p => p.ApplicationUserId == userId).Count() > 0).FirstOrDefault();
                //    departments = currentSection.Name.ToString();
                //}

                #endregion

                //待办
                MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkFlowID), sectionName);
                MyRiZhi1.DoSomething = "需要审批(过程记录)";
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.ProcessRecord.ToString();
                MyRiZhi1.FKAction = "待办";
                //过程记录的ID
                MyRiZhi1.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi1.TimeStr = DateTime.Now;
                MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                //上报时设置保存为false、驳回为false、删除为false、冻结为false
                model.IsTemporary = false;
                model.IsRejected = false;
                model.IsDeleted = false;
                model.IsLocked = false;

                bool isUpdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());

                ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                tempmodel.ApplicationStatus = ApplicationStatus.ProjectProcessing.ToString();
                tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                bool isMainProjectUpdate = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());

                ///上报成功标志
                ViewBag.SendUpSuccess = true;

                return View(model);
                //return RedirectToAction("ProcessRecordsRejected", new { id = model.NWorkToDoID });
            }
        }
        /// <summary>
        /// 费用报销申请书
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ReimburseProcess(FundsRecordViewModel model, FormCollection collection)
        {
            var sectionName = MySession[SessionKeyEnum.SectionName].ToString();
            ///验证
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string type = collection["TypeId"].ToString();
            double priceTotal = Convert.ToDouble(collection["TotalPrice"].ToString());

            var todoModel = ApplicationService.GetEntityById(Convert.ToInt32(model.ApplicationId));

            //string projectType = todoModel.FormValues.Split('#')[1];

            FundsThresholdViewModel fundModel = new FundsThresholdViewModel();
            fundModel = FundsThresholdService.GetEntities(p => p.FundsType == type && p.ModuleName == ModuleNameOfScienceResearch.ScienceResearch.ToString() && p.NWorkToDoID == model.ApplicationId).FirstOrDefault().ToViewModel();
            //int IsLimited;
            if (priceTotal > fundModel.Threshold)
            {
                ViewBag.limit = "true";
                ViewBag.limitFund = fundModel.Threshold;
                using (var context = new CSPostOAEntities())
                {
                    ViewBag.FundsType = (from h in context.FundsThreshold
                                         where (h.IsDeleted == false && h.ModuleName == ModuleNameOfScienceResearch.ScienceResearch.ToString() && h.NWorkToDoID == model.ApplicationId)

                                         select new SelectListItem()
                                         {
                                             Text = h.FundsType,
                                             Value = h.FundsType,
                                         }).ToList();
                }

                return View(model);
            }
            else
            {
                #region
                model.CreatedTime = DateTime.Now;
                model.Type = type;
                model.TimeStr = DateTime.Now;
                model.CreatedBy = User.Identity.Name;
                model.UpdatedBy = User.Identity.Name;
                model.Quantity = '1';
                model.Unit = "元";

                try
                {
                    model.StateNow = "正在办理";
                }
                catch
                {
                    model.JieDianName = "结束";
                    model.StateNow = "强制结束";
                }

                model.OKUserList = "默认";

                if (collection["Save"] != null)//保存
                {
                    //日志
                    ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                    //已办
                    MyRiZhi.UserName = User.Identity.Name;
                    MyRiZhi.DoSomething = "保存(经费报销单)";
                    MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.FeeReimbursement.ToString();
                    MyRiZhi.FKAction = "已办";
                    MyRiZhi.TimeStr = DateTime.Now;
                    MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                    string act = "save";
                    model.IsTemporary = true;
                    model.IsDeleted = false;
                    model.IsLocked = false;

                    //第一次保存
                    if (model.FundsRecordID == 0)
                    {

                        //支出，以后要根据情况修改
                        model.IsIncome = false;

                        model.ModuleName = ApplicationType.ScienceResearch.ToString();

                        int jiedianid = Convert.ToInt32(model.JieDianID);
                        var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                        model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));

                        int returnid = this.FundsRecordService.AddFundsRecord(model.ToDataTransferObjectModel());

                        //FKApplicationID是过程记录的ID
                        MyRiZhi.FKApplicationID = returnid.ToString();
                        int returnRiZhiId = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());
                        ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                        tempmodel.ApplicationStatus = ApplicationStatus.ProjectProcessing.ToString();
                        tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                        bool isupdate = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());

                        return RedirectToAction("ReimburseProcess", new { id = returnid, nextaction = act });
                    }
                    //第二次或第N次保存
                    else
                    {

                        int jiedianid = Convert.ToInt32(model.JieDianID);
                        var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                        model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));

                        bool isAddSuccess = this.FundsRecordService.UpdateFundsRecord(model.ToDataTransferObjectModel());

                        //FKApplicationID是过程记录的ID
                        MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                        int returnid = this.FundsRecordService.AddFundsRecord(model.ToDataTransferObjectModel());
                        ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                        tempmodel.ApplicationStatus = ApplicationStatus.ProjectProcessing.ToString();
                        tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                        bool isupdate = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());
                        return RedirectToAction("ReimburseProcess", new { id = returnid, nextaction = act });
                    }

                }
                //上报collection["Reported"]
                else
                {
                    //写系统日志
                    ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                    ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();
                    //已办
                    MyRiZhi.UserName = User.Identity.Name;
                    MyRiZhi.DoSomething = "添加(经费记录)";
                    MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.FeeReimbursement.ToString();
                    MyRiZhi.FKAction = "已办";
                    MyRiZhi.TimeStr = DateTime.Now;
                    MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                    //待办
                    MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkflowId), sectionName);
                    MyRiZhi1.DoSomething = "需要审批(经费记录)";
                    MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.FeeReimbursement.ToString();
                    MyRiZhi1.FKAction = "待办";
                    MyRiZhi1.TimeStr = DateTime.Now;
                    MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                    //上报时设置保存为false、驳回为false、删除为false、冻结为false
                    model.IsTemporary = false;
                    model.IsRejected = false;
                    model.IsDeleted = false;
                    model.IsLocked = false;
                    //页面加载后就上报，就添加新的数据
                    int fundsrecordId = 0;
                    if (model.FundsRecordID == 0)
                    {

                        //支出，以后要根据情况修改
                        model.IsIncome = false;
                        model.ModuleName = ApplicationType.ScienceResearch.ToString();

                        int jiedianid = Convert.ToInt32(model.JieDianID);
                        var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                        model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));

                        int isAddSuccess = this.FundsRecordService.AddFundsRecord(model.ToDataTransferObjectModel());
                        fundsrecordId = isAddSuccess;
                        //FundsRecord的ID
                        MyRiZhi.FKApplicationID = isAddSuccess.ToString();
                        int AddRiZhiSuccess = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());
                        //FundsRecord的ID
                        MyRiZhi1.FKApplicationID = isAddSuccess.ToString();
                        int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());
                    }
                    //先点保存再上报，就更新保存数据行的IsTemprory
                    else
                    {
                        //上报时设置保存为false、驳回为false、删除为false、冻结为false
                        model.IsTemporary = false;
                        model.IsRejected = false;
                        model.IsDeleted = false;
                        model.IsLocked = false;

                        int jiedianid = Convert.ToInt32(model.JieDianID);
                        var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                        model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));

                        bool isAddSuccess = this.FundsRecordService.UpdateFundsRecord(model.ToDataTransferObjectModel());

                        MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                        fundsrecordId = model.FundsRecordID;
                        int AddRiZhiSuccess = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());
                        //过程记录的ID
                        MyRiZhi1.FKApplicationID = model.FundsRecordID.ToString();
                        int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                    }
                    //更新过程记录相关申请书的applicationstatus
                    ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                    tempmodel.ApplicationStatus = ApplicationStatus.ProjectProcessing.ToString();
                    tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    bool isupdate = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());

                    FundsRecordViewModel returnmodel = FundsRecordService.GetEntityById(fundsrecordId).ToViewModel();
                    //上报成功的标志
                    ViewBag.SendUpSuccess = true;
                    ///获取经费记录类型
                    using (var context = new CSPostOAEntities())
                    {
                        ViewBag.FundsType = (from h in context.FundsThreshold
                                             where (h.IsDeleted == false && h.ModuleName == ModuleNameOfScienceResearch.ScienceResearch.ToString() && h.NWorkToDoID == model.ApplicationId)

                                             select new SelectListItem()
                                             {
                                                 Text = h.FundsType,
                                                 Value = h.FundsType,
                                             }).ToList();
                    }
                    return View(returnmodel);
                }
                #endregion
            }
        }
        /// <summary>
        /// 费用审批提交
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ReimburseAgree(string flag, FundsRecordViewModel model, FormCollection collection)
        {
            ///验证
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.CreatedTime = DateTime.Now;

            string nodeSerils;
            string act = string.Empty;
            //审批
            if (flag == "Approval")
            {
                var attachment = model.FuJianList;
                FundsRecordService.GetEntityById(model.FundsRecordID);
                string PiShiStr = model.ShenPiYiJian;
                // TODO: 审批上传附件
                model.OKUserList = model.OKUserList + "," + User.Identity.Name;
                if (collection["SingleShenPiYiJian"] != "")
                {
                    //审批意见
                    model.ShenPiYiJian = PiShiStr + model.JieDianName + ":" + collection["SingleShenPiYiJian"].ToString() + Constant.SplitChar;
                }
                else
                {
                    model.ShenPiYiJian = PiShiStr;
                }
                // 更改当前结点id和name
                // 说明过程记录审批流程还未完成，正在等待下一级审批
                var erpnrowkflownoderesult1 = ERPNWorkFlowNodeService.GetEntityById(model.JieDianID.Value).ToViewModel();
                string nextNodeSerial = erpnrowkflownoderesult1.NextNode;
                var nextNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.NodeSerils == nextNodeSerial && p.WorkFlowID == model.WorkflowId).ToList();
                // 说明过程记录审批流程已经完成，审批通过
                if (nextNodeModel.Count == Constant.ZERO_INT)
                {
                    model.JieDianName = "结束";
                    bool isUpdateSuccess = FundsRecordService.UpdateFundsRecord(model.ToDataTransferObjectModel());
                    ViewBag.Title = model.JieDianName;

                    ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                    tempmodel.ApplicationStatus = ApplicationStatus.ProjectProcessing.ToString();
                    tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    bool isupdate1 = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());

                    nodeSerils = "结束";
                }
                // 说明过程记录审批流程还未完成，正在等待下一级审批
                else
                {
                    ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                    tempmodel.ApplicationStatus = ApplicationStatus.ProjectProcessing.ToString();
                    tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    bool isupdate1 = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());

                    ///根据序号和workflowID获得下一级节点ID
                    model.JieDianID = nextNodeModel.FirstOrDefault().ID;
                    model.JieDianName = nextNodeModel.FirstOrDefault().NodeName;

                    bool isUpdateSuccess = FundsRecordService.UpdateFundsRecord(model.ToDataTransferObjectModel());
                    ViewBag.Title = model.JieDianName;

                    //下一个节点的NodeSerils
                    nodeSerils = nextNodeModel.FirstOrDefault().NodeSerils;

                    ViewBag.Title = model.JieDianName;
                }
            }
            //驳回collection["Reject"]
            else
            {
                act = "rejected";
                model.IsRejected = true;
                var workFlowNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == model.WorkflowId).OrderBy(p => p.NodeSerils).Skip(1).Take(1).FirstOrDefault();

                model.JieDianID = workFlowNodeModel.ID;
                model.JieDianName = workFlowNodeModel.NodeName.ToString();

                nodeSerils = "驳回";
                bool isUpdate = this.FundsRecordService.UpdateFundsRecord(model.ToDataTransferObjectModel());
            }

            //经费报销单的日志
            if (model.WorkflowId == Convert.ToInt16(TypeOfWorkFlowId.FeeReimbursement))
            {
                string formname = EngineeringProjectTypeOfFormId.FeeReimbursement.ToString();
                var rizhiresult = ERPRiZhiService.GetEntities(p => p.FKApplicationID == model.FundsRecordID.ToString() && p.FkFormName == formname && p.FKAction == "待办").FirstOrDefault();
                ERPNWorkToDoViewModel erpnworktodoModel = new ERPNWorkToDoViewModel();
                erpnworktodoModel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                IEnumerable<ERPNWorkFlowNodeViewModel> workFlowNodeModelList = new List<ERPNWorkFlowNodeViewModel>();
                //除填写申请书和第二级审批
                workFlowNodeModelList = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == model.WorkflowId).OrderBy(p => p.NodeSerils).Skip(2).Select(x => x.ToViewModel());
                //将从第三节点（包括第三节点）的NodeSerials全部装在workFlowNodeModelList
                string nodeSerilsList = "";
                foreach (var item in workFlowNodeModelList)
                {
                    nodeSerilsList = nodeSerilsList + item.NodeSerils;
                }
                //判断下一节点是否存在
                if (nodeSerilsList.Contains(nodeSerils))
                {
                    //日志
                    ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                    ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();

                    //待办变成已办
                    MyRiZhi.FKAction = "已办";
                    MyRiZhi.ID = rizhiresult.ID;
                    MyRiZhi.UserName = User.Identity.Name;
                    MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    MyRiZhi.DoSomething = "已审批" + erpnworktodoModel.UserName + "上报的经费报销单";
                    MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.FeeReimbursement.ToString();
                    //FundsRecord的ID
                    MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi.TimeStr = DateTime.Now;
                    MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                    bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                    //新生成待办，下一审批人的待办

                    MyRiZhi1.FKAction = "待办";
                    MyRiZhi1.ID = rizhiresult.ID;
                    //FundsRecord的ID
                    MyRiZhi1.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    MyRiZhi1.DoSomething = "需要审批(" + erpnworktodoModel.UserName + "上报的经费报销单)";
                    MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.FeeReimbursement.ToString();
                    MyRiZhi1.TimeStr = DateTime.Now;
                    MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                    ERPNWorkFlowNodeViewModel erpnWorkFlowNodeModel = new ERPNWorkFlowNodeViewModel();
                    erpnWorkFlowNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == model.WorkflowId && p.NodeSerils == nodeSerils).FirstOrDefault().ToViewModel();

                    MyRiZhi1.UserName = erpnWorkFlowNodeModel.SPDefaultList;

                    int returnid = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                    return Json(returnid, JsonRequestBehavior.AllowGet);
                    //继续审批
                    //return RedirectToAction("ReimburseAgree", new { id = model.FundsRecordID });
                }
                else if (nodeSerils == "结束")
                {
                    ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                    ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();

                    MyRiZhi.FKAction = "已办";
                    MyRiZhi.ID = rizhiresult.ID;
                    MyRiZhi.UserName = User.Identity.Name;
                    MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    MyRiZhi.DoSomething = "已审批" + erpnworktodoModel.UserName + "上报的经费报销单";
                    MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.FeeReimbursement.ToString();
                    //FundsRecord的ID
                    MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                    bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                    int UpdateSuccess = 1;
                    return Json(UpdateSuccess, JsonRequestBehavior.AllowGet);
                    // 更新成功并且更新后，申请书状态变为 已经审批通过，跳转到项目确立页面
                    // return RedirectToAction("ReimburseAgree", new { id = model.FundsRecordID });
                }
                //toDoAction == "驳回"
                else
                {
                    ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                    ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();

                    //审批人已办
                    MyRiZhi.FKAction = "已办";
                    MyRiZhi.ID = rizhiresult.ID;
                    MyRiZhi.UserName = User.Identity.Name;
                    MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    MyRiZhi.DoSomething = "已驳回" + erpnworktodoModel.UserName + "上报的经费报销单";
                    MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.FeeReimbursement.ToString();
                    //FundsRecord的ID
                    MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi.TimeStr = DateTime.Now;
                    MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                    bool UpdateRiZhiSuccess1 = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());
                    //申请人待办

                    MyRiZhi1.FKAction = "待办";
                    MyRiZhi1.ID = rizhiresult.ID;
                    MyRiZhi1.UserName = erpnworktodoModel.UserName;
                    MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    MyRiZhi1.DoSomething = "经费报销单被驳回，需修改数据";
                    MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.FeeReimbursement.ToString();
                    MyRiZhi1.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi1.TimeStr = DateTime.Now;
                    MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                    int returnid = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                    return Json(returnid, JsonRequestBehavior.AllowGet);
                    // return RedirectToAction("ReimburseRejected", new { id = model.FundsRecordID });
                }
            }
            //差旅报销单的日志
            else
            {
                string formname = EngineeringProjectTypeOfFormId.TravelReimbursement.ToString();
                var rizhiresult = ERPRiZhiService.GetEntities(p => p.FKApplicationID == model.FundsRecordID.ToString() && p.FkFormName == formname && p.FKAction == "待办").FirstOrDefault();
                ERPNWorkToDoViewModel erpnworktodoModel = new ERPNWorkToDoViewModel();
                erpnworktodoModel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                IEnumerable<ERPNWorkFlowNodeViewModel> workFlowNodeModelList = new List<ERPNWorkFlowNodeViewModel>();
                //除填写申请书和第二级审批
                workFlowNodeModelList = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == model.WorkflowId).OrderBy(p => p.NodeSerils).Skip(2).Select(x => x.ToViewModel());
                //将从第三节点（包括第三节点）的NodeSerials全部装在workFlowNodeModelList
                string nodeSerilsList = "";
                foreach (var item in workFlowNodeModelList)
                {
                    nodeSerilsList = nodeSerilsList + item.NodeSerils;
                }
                //判断下一节点是否存在
                if (nodeSerilsList.Contains(nodeSerils))
                {
                    //日志
                    ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                    ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();

                    //待办变成已办
                    MyRiZhi.FKAction = "已办";
                    MyRiZhi.ID = rizhiresult.ID;
                    MyRiZhi.UserName = User.Identity.Name;
                    MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    MyRiZhi.DoSomething = "已审批" + erpnworktodoModel.UserName + "上报的差旅报销单";
                    MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.TravelReimbursement.ToString();
                    //FundsRecord的ID
                    MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi.TimeStr = DateTime.Now;
                    MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                    bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                    //新生成待办，下一审批人的待办

                    MyRiZhi1.FKAction = "待办";
                    MyRiZhi1.ID = rizhiresult.ID;
                    //FundsRecord的ID
                    MyRiZhi1.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    MyRiZhi1.DoSomething = "需要审批(" + erpnworktodoModel.UserName + "上报的差旅报销单)";
                    MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.TravelReimbursement.ToString();
                    MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                    ERPNWorkFlowNodeViewModel erpnWorkFlowNodeModel = new ERPNWorkFlowNodeViewModel();
                    erpnWorkFlowNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == model.WorkflowId && p.NodeSerils == nodeSerils).FirstOrDefault().ToViewModel();

                    MyRiZhi1.UserName = erpnWorkFlowNodeModel.SPDefaultList;

                    int returnid = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                    return Json(returnid, JsonRequestBehavior.AllowGet);
                    //继续审批
                    // return RedirectToAction("ReimburseAgree", new { id = model.FundsRecordID });
                }
                else if (nodeSerils == "结束")
                {
                    ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                    ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();

                    MyRiZhi.FKAction = "已办";
                    MyRiZhi.ID = rizhiresult.ID;
                    MyRiZhi.UserName = User.Identity.Name;
                    MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    MyRiZhi.DoSomething = "已审批" + erpnworktodoModel.UserName + "上报的差旅报销单";
                    MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.TravelReimbursement.ToString();
                    //FundsRecord的ID
                    MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                    bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                    int UpdateSuccess = 1;
                    return Json(UpdateSuccess, JsonRequestBehavior.AllowGet);
                    // 更新成功并且更新后，申请书状态变为 已经审批通过，跳转到项目确立页面
                    //return RedirectToAction("ReimburseAgree", new { id = model.FundsRecordID });
                }
                //toDoAction == "驳回"
                else
                {
                    ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                    ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();

                    //审批人已办
                    MyRiZhi.FKAction = "已办";
                    MyRiZhi.ID = rizhiresult.ID;
                    MyRiZhi.UserName = User.Identity.Name;
                    MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    MyRiZhi.DoSomething = "已驳回" + erpnworktodoModel.UserName + "上报的差旅报销单";
                    MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.TravelReimbursement.ToString();
                    //FundsRecord的ID
                    MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi.TimeStr = DateTime.Now;
                    MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                    bool UpdateRiZhiSuccess1 = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());
                    //申请人待办

                    MyRiZhi1.FKAction = "待办";
                    MyRiZhi1.ID = rizhiresult.ID;
                    MyRiZhi1.UserName = erpnworktodoModel.UserName;
                    MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    MyRiZhi1.DoSomething = "差旅报销单被驳回，需修改数据";
                    MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.TravelReimbursement.ToString();
                    //FundsRecord的ID
                    MyRiZhi1.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi1.TimeStr = DateTime.Now;
                    MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                    int returnid = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                    return Json(returnid, JsonRequestBehavior.AllowGet);
                    // return RedirectToAction("TravelExpensesRejected", new { id = model.FundsRecordID });
                }
            }

        }
        /// <summary>
        /// 费用报销申请书驳回
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ReimburseRejected(FundsRecordViewModel model, FormCollection collection)
        {
            var sectionName = MySession[SessionKeyEnum.SectionName].ToString();
            ///验证
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string type = collection["TypeId"].ToString();
            double priceTotal = Convert.ToDouble(collection["TotalPrice"].ToString());

            var todoModel = ApplicationService.GetEntityById(Convert.ToInt32(model.ApplicationId));

            //string projectType = todoModel.FormValues.Split('#')[1];

            FundsThresholdViewModel fundModel = new FundsThresholdViewModel();
            fundModel = FundsThresholdService.GetEntities(p => p.FundsType == type && p.ModuleName == ModuleNameOfScienceResearch.ScienceResearch.ToString() && p.NWorkToDoID == model.ApplicationId).FirstOrDefault().ToViewModel();
            //int IsLimited;
            if (priceTotal > fundModel.Threshold)
            {

                /// IsLimited =1;
                ViewBag.limit = "true";
                ViewBag.limitFund = fundModel.Threshold;
                using (var context = new CSPostOAEntities())
                {
                    ViewBag.FundsType = (from h in context.FundsThreshold
                                         where (h.IsDeleted == false && h.ModuleName == ModuleNameOfScienceResearch.ScienceResearch.ToString() && h.NWorkToDoID == model.ApplicationId)

                                         select new SelectListItem()
                                         {
                                             Text = h.FundsType,
                                             Value = h.FundsType,
                                         }).ToList();
                }
                model.IsRejected = true;
                return View(model);
            }
            else
            {

                model.CreatedTime = DateTime.Now;
                model.Type = type;
                model.TimeStr = DateTime.Now;
                model.CreatedBy = User.Identity.Name;
                model.UpdatedBy = User.Identity.Name;
                model.Quantity = '1';
                model.Unit = "元";

                try
                {
                    model.StateNow = "正在办理";
                }
                catch
                {
                    model.JieDianName = "结束";
                    model.StateNow = "强制结束";
                }

                model.OKUserList = "默认";

                if (collection["Save"] != null)//保存
                {
                    //日志
                    ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                    //已办
                    MyRiZhi.UserName = User.Identity.Name;
                    MyRiZhi.DoSomething = "保存(经费报销单)";
                    MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.FeeReimbursement.ToString();
                    MyRiZhi.FKAction = "已办";
                    //FKApplicationID是过程记录的ID
                    MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();
                    int returnRiZhiId = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                    string act = "save";
                    model.IsTemporary = true;
                    model.IsLocked = false;
                    model.IsDeleted = false;
                    model.IsRejected = false;

                    int jiedianid = Convert.ToInt32(model.JieDianID);
                    var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                    model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));

                    ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                    tempmodel.ApplicationStatus = ApplicationStatus.ProjectProcessing.ToString();
                    tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    bool isupdate = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());

                    bool isAddSuccess = this.FundsRecordService.UpdateFundsRecord(model.ToDataTransferObjectModel());

                    return RedirectToAction("ReimburseRejected", new { id = model.FundsRecordID });
                }
                //上报collection["Reported"]
                else
                {
                    //写系统日志
                    ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                    ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();
                    //已办
                    //已办
                    MyRiZhi = ERPRiZhiService.GetEntities(p => p.FKApplicationID == model.FundsRecordID.ToString() && p.FKAction == "待办").FirstOrDefault().ToViewModel();
                    MyRiZhi.UserName = User.Identity.Name;
                    MyRiZhi.DoSomething = "添加(经费记录)";
                    MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.FeeReimbursement.ToString();
                    MyRiZhi.FKAction = "已办";
                    MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi.TimeStr = DateTime.Now;
                    MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();
                    bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                    //待办
                    MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkflowId), sectionName);
                    MyRiZhi1.DoSomething = "需要审批(经费记录)";
                    MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.FeeReimbursement.ToString();
                    MyRiZhi1.FKAction = "待办";
                    MyRiZhi1.TimeStr = DateTime.Now;
                    MyRiZhi1.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                    int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                    //上报时设置保存为false、驳回为false、删除为false、冻结为false
                    model.IsTemporary = false;
                    model.IsRejected = false;
                    model.IsDeleted = false;
                    model.IsLocked = false;

                    int jiedianid = Convert.ToInt32(model.JieDianID);
                    var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                    model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));

                    bool isUpdate = this.FundsRecordService.UpdateFundsRecord(model.ToDataTransferObjectModel());

                    //更新过程记录相关申请书的applicationstatus
                    ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                    tempmodel.ApplicationStatus = ApplicationStatus.ProjectProcessing.ToString();
                    tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    bool isupdate = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());

                    return RedirectToAction("ReimburseRejected", new { id = model.FundsRecordID });
                }
            }
        }
        /// <summary>
        /// 差旅费用报销驳回
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult TravelExpensesRejected(TravelFundsRecordViewModel model, FormCollection collection)
        {
            var sectionName = MySession[SessionKeyEnum.SectionName].ToString();
            ///验证
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int countOfTravelItems;

            if (!string.IsNullOrEmpty(Request["count"]))
            {
                countOfTravelItems = Convert.ToInt32(Request["count"]);
            }
            else
            {
                countOfTravelItems = 0;
            }

            string[] itemNameArray = { "StartDate", "EndDate", "FromAddress", "ToAddress", "Transportation", "TransportationFee", "HotelFee", "OtherFee", "OtherFeeDescription" };
            string[,] travelItemValues = new string[countOfTravelItems, 9];

            for (int i = 1; i < countOfTravelItems + 1; i++)
            {
                var item = new TravelFundsDetailViewModel();

                item.StartDate = Convert.ToDateTime(Request["item" + i.ToString() + itemNameArray[0]]);
                item.EndDate = Convert.ToDateTime(Request["item" + i.ToString() + itemNameArray[1]]);
                item.FromAddress = Request["item" + i.ToString() + itemNameArray[2]];
                item.ToAddress = Request["item" + i.ToString() + itemNameArray[3]];
                item.Transportation = Request["item" + i.ToString() + itemNameArray[4]];
                item.TransportationFee = string.IsNullOrEmpty(Request["item" + i.ToString() + itemNameArray[5]]) ? 0 : Convert.ToDouble(Request["item" + i.ToString() + itemNameArray[5]]);
                item.HotelFee = string.IsNullOrEmpty(Request["item" + i.ToString() + itemNameArray[6]]) ? 0 : Convert.ToDouble(Request["item" + i.ToString() + itemNameArray[6]]);
                item.OtherFee = string.IsNullOrEmpty(Request["item" + i.ToString() + itemNameArray[7]]) ? 0 : Convert.ToDouble(Request["item" + i.ToString() + itemNameArray[7]]);
                item.OtherFeeComment = Request["item" + i.ToString() + itemNameArray[8]];

                model.TravelFundsList.Add(item);
            }

            model.TotalPrice = model.TravelFundsList.Count == 0 ? 0 : model.TravelFundsList.Sum(x => ((x.TransportationFee.HasValue ? x.TransportationFee.Value : 0) + (x.HotelFee.HasValue ? x.HotelFee.Value : 0) + (x.OtherFee.HasValue ? x.OtherFee.Value : 0)));
            model.TimeStr = DateTime.Now;
            model.CreatedBy = User.Identity.Name;
            model.UpdatedBy = User.Identity.Name;
            model.Quantity = '1';
            model.Unit = "元";
            model.WorkflowId = (int)TypeOfWorkFlowId.TravelReimbursement;
            model.Type = "差旅报销单";

            model.CreatedTime = DateTime.Now;

            try
            {
                model.StateNow = "正在办理";
            }
            catch
            {
                model.JieDianName = "结束";
                model.StateNow = "强制结束";
            }

            model.OKUserList = "默认";

            if (collection["Save"] != null)//保存
            {
                //日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                //已办
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.DoSomething = "保存(差旅报销单)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.TravelReimbursement.ToString();
                MyRiZhi.FKAction = "已办";
                //FKApplicationID是过程记录的ID
                MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();
                int returnRiZhiId = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                string act = "save";
                model.IsTemporary = true;
                model.IsLocked = false;
                model.IsDeleted = false;
                model.IsRejected = false;


                int jiedianid = Convert.ToInt32(model.JieDianID);
                var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));

                ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                tempmodel.ApplicationStatus = ApplicationStatus.ProjectProcessing.ToString();
                tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                bool isupdate = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());

                bool isAddSuccess = this.FundsRecordService.UpdateFundsRecord(model.ToDataTransferObjectModel());

                return RedirectToAction("TravelExpensesRejected", new { id = model.FundsRecordID });
            }
            //上报collection["Reported"]
            else
            {
                //写系统日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();
                //已办
                MyRiZhi = ERPRiZhiService.GetEntities(p => p.FKApplicationID == model.FundsRecordID.ToString() && p.FKAction == "待办").FirstOrDefault().ToViewModel();
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.DoSomething = "添加(差旅报销单)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.TravelReimbursement.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                MyRiZhi.TimeStr = DateTime.Now;
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();
                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //待办
                MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkflowId), sectionName);
                MyRiZhi1.DoSomething = "需要审批(差旅报销单)";
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.TravelReimbursement.ToString();
                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.TimeStr = DateTime.Now;
                MyRiZhi1.FKApplicationID = model.FundsRecordID.ToString();
                MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                //上报时设置保存为false、驳回为false、删除为false、冻结为false
                model.IsTemporary = false;
                model.IsRejected = false;
                model.IsDeleted = false;
                model.IsLocked = false;

                int jiedianid = Convert.ToInt32(model.JieDianID);
                var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));

                bool isUpdate = this.FundsRecordService.UpdateFundsRecord(model.ToDataTransferObjectModel());

                //更新过程记录相关申请书的applicationstatus
                ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                tempmodel.ApplicationStatus = ApplicationStatus.ProjectProcessing.ToString();
                tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                bool isupdate1 = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());
                return RedirectToAction("TravelExpensesRejected", new { id = model.FundsRecordID });
            }
        }

        /// <summary>
        /// 差旅费用报销
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult TravelExpenses(TravelFundsRecordViewModel model, FormCollection collection)
        {
            var sectionName = MySession[SessionKeyEnum.SectionName].ToString();
            ///验证
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int countOfTravelItems;

            if (!string.IsNullOrEmpty(Request["count"]))
            {
                countOfTravelItems = Convert.ToInt32(Request["count"]);
            }
            else
            {
                countOfTravelItems = 0;
            }

            string[] itemNameArray = { "StartDate", "EndDate", "FromAddress", "ToAddress", "Transportation", "TransportationFee", "HotelFee", "OtherFee", "OtherFeeDescription" };
            string[,] travelItemValues = new string[countOfTravelItems, 9];

            for (int i = 1; i < countOfTravelItems + 1; i++)
            {
                var item = new TravelFundsDetailViewModel();

                item.StartDate = Convert.ToDateTime(Request["item" + i.ToString() + itemNameArray[0]]);
                item.EndDate = Convert.ToDateTime(Request["item" + i.ToString() + itemNameArray[1]]);
                item.FromAddress = Request["item" + i.ToString() + itemNameArray[2]];
                item.ToAddress = Request["item" + i.ToString() + itemNameArray[3]];
                item.Transportation = Request["item" + i.ToString() + itemNameArray[4]];
                item.TransportationFee = string.IsNullOrEmpty(Request["item" + i.ToString() + itemNameArray[5]]) ? 0 : Convert.ToDouble(Request["item" + i.ToString() + itemNameArray[5]]);
                item.HotelFee = string.IsNullOrEmpty(Request["item" + i.ToString() + itemNameArray[6]]) ? 0 : Convert.ToDouble(Request["item" + i.ToString() + itemNameArray[6]]);
                item.OtherFee = string.IsNullOrEmpty(Request["item" + i.ToString() + itemNameArray[7]]) ? 0 : Convert.ToDouble(Request["item" + i.ToString() + itemNameArray[7]]);
                item.OtherFeeComment = Request["item" + i.ToString() + itemNameArray[8]];

                model.TravelFundsList.Add(item);
            }

            model.TotalPrice = model.TravelFundsList.Count == 0 ? 0 : model.TravelFundsList.Sum(x => ((x.TransportationFee.HasValue ? x.TransportationFee.Value : 0) + (x.HotelFee.HasValue ? x.HotelFee.Value : 0) + (x.OtherFee.HasValue ? x.OtherFee.Value : 0)));
            model.TimeStr = DateTime.Now;
            model.CreatedBy = User.Identity.Name;
            model.UpdatedBy = User.Identity.Name;
            model.Quantity = '1';
            model.Unit = "元";
            model.WorkflowId = (int)TypeOfWorkFlowId.TravelReimbursement;
            model.Type = "差旅报销单";
            model.CreatedTime = DateTime.Now;

            try
            {
                model.StateNow = "正在办理";
            }
            catch
            {
                model.JieDianName = "结束";
                model.StateNow = "强制结束";
            }

            model.OKUserList = "默认";

            if (collection["Save"] != null)//保存
            {
                //日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                //已办
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.DoSomething = "保存(差旅报销单)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.TravelReimbursement.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi.TimeStr = DateTime.Now;
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                string act = "save";
                model.IsTemporary = true;
                model.IsDeleted = false;
                //第一次保存
                //注意直接用ID可能出错，页面也未改
                if (model.FundsRecordID == 0)
                {

                    //支出，以后要根据情况修改
                    model.IsIncome = false;

                    model.ModuleName = ApplicationType.ScienceResearch.ToString();

                    int jiedianid = Convert.ToInt32(model.JieDianID);
                    var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                    model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));

                    int returnid = this.FundsRecordService.AddFundsRecord(model.ToDataTransferObjectModel());

                    //FKApplicationID是过程记录的ID
                    MyRiZhi.FKApplicationID = returnid.ToString();
                    int returnRiZhiId = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                }
                //第二次或第N次保存
                else
                {

                    int jiedianid = Convert.ToInt32(model.JieDianID);
                    var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                    model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));

                    bool isupdate = this.FundsRecordService.UpdateFundsRecord(model.ToDataTransferObjectModel());
                    //FKApplicationID是过程记录的ID
                    MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                    int returnid = this.FundsRecordService.AddFundsRecord(model.ToDataTransferObjectModel());

                }
                ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                tempmodel.ApplicationStatus = ApplicationStatus.ProjectProcessing.ToString();
                tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                bool isupdate1 = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());
                return RedirectToAction("TravelExpenses", new { id = model.FundsRecordID, nextaction = act });
            }
            //上报collection["Reported"]
            else
            {
                //写系统日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();
                //已办
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.DoSomething = "添加(差旅报销单)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.TravelReimbursement.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.TimeStr = DateTime.Now;
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                //待办
                MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkflowId), sectionName);
                MyRiZhi1.DoSomething = "需要审批(差旅报销单)";
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.TravelReimbursement.ToString();
                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.TimeStr = DateTime.Now;
                MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                //上报时设置保存为false、驳回为false、删除为false、冻结为false
                model.IsTemporary = false;
                model.IsRejected = false;
                model.IsDeleted = false;
                model.IsLocked = false;
                int fundsrecordId = 0;
                //页面加载后就上报，就添加新的数据
                if (model.FundsRecordID == 0)
                {

                    //支出，以后要根据情况修改
                    model.IsIncome = false;
                    model.ModuleName = ApplicationType.ScienceResearch.ToString();

                    int jiedianid = Convert.ToInt32(model.JieDianID);
                    var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                    model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));

                    int isAddSuccess = this.FundsRecordService.AddFundsRecord(model.ToDataTransferObjectModel());
                    fundsrecordId = isAddSuccess;
                    //FundsRecord的ID
                    MyRiZhi.FKApplicationID = isAddSuccess.ToString();
                    int AddRiZhiSuccess = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());
                    //FundsRecord的ID
                    MyRiZhi1.FKApplicationID = isAddSuccess.ToString();
                    int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());
                }
                //先点保存再上报，就更新保存数据行的IsTemprory
                else
                {
                    int jiedianid = Convert.ToInt32(model.JieDianID);
                    var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                    model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));

                    bool isupdate = this.FundsRecordService.UpdateFundsRecord(model.ToDataTransferObjectModel());

                    MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                    fundsrecordId = model.FundsRecordID;
                    int AddRiZhiSuccess = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());
                    //过程记录的ID
                    MyRiZhi1.FKApplicationID = model.FundsRecordID.ToString();
                    int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());
                }
                //更新过程记录相关申请书的applicationstatus
                ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                tempmodel.ApplicationStatus = ApplicationStatus.ProjectProcessing.ToString();
                tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                bool isupdate1 = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());

                TravelFundsRecordViewModel returnmodel = new TravelFundsRecordViewModel();
                model = FundsRecordService.GetEntityWithTravelDetailListById(fundsrecordId).ToViewModel();
                //上报成功的标志
                ViewBag.SendUpSuccess = true;
                return View(returnmodel);
            }
        }

        /// <summary>
        /// 课题结案提交
        /// </summary>
        /// <param name="model"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Conclusions(string flag, ERPNWorkToDoViewModel model, FormCollection collection)
        {
            model.UserName = User.Identity.Name;
            model.TimeStr = DateTime.Now;
            var sectionName = MySession[SessionKeyEnum.SectionName].ToString();

            //加载表单内容
            var temerpnformmodel = ERPNFormService.GetEntityById((int)EngineeringProjectTypeOfFormId.Conclusion).ToViewModel();
            model.FormContent = temerpnformmodel.ContentStr;
            model.FormContent = model.FormContent.Replace(Constant.MacroSectionString, sectionName);
            model.FormContent = model.FormContent.Replace(Constant.MacroUserNameString, User.Identity.Name);
            model.FormContent = FormContentReplaceHelper.ReplaceFormContentValue(model.FormContent, collection);

            try
            {
                model.StateNow = "正在办理";
            }
            catch
            {
                model.JieDianName = "结束";
                model.StateNow = "强制结束";
            }

            model.OKUserList = "默认";
            //保存
            if (flag == "Save")
            {
                //添加申请人保存
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                //已办
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.DoSomething = "保存(课题结案)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.Conclusion.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                int returnRiZhiId = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //第一次保存
                if (model.NWorkToDoID == 0)
                {
                    string act = "save";

                    ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                    tempmodel.IsTemporary = true;
                    tempmodel.IsDeleted = false;
                    tempmodel.ApplicationStatus = ApplicationStatus.ConcludeUnSubmit.ToString();
                    tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    bool isupdate = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());

                    int jiedianid = Convert.ToInt32(model.JieDianID);
                    model.IsTemporary = true;
                    model.IsDeleted = false;
                    var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                    model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));
                    int returncolusionid = this.ApplicationService.AddApplication(model.ToDataTransferObjectModel());

                    return Json(returncolusionid, JsonRequestBehavior.AllowGet);
                    // return RedirectToAction("Conclusions", new { id = model.ApplicationId, nextaction = act });
                }
                //第二次或第N次保存
                else
                {
                    string act = "save";
                    model.IsTemporary = true;
                    model.IsDeleted = false;

                    ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                    tempmodel.ApplicationStatus = ApplicationStatus.ConcludeUnSubmit.ToString();
                    tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    bool isupdate = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());

                    int jiedianid = Convert.ToInt32(model.JieDianID);
                    var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                    model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));
                    bool isUpdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());

                    return RedirectToAction("Conclusions", new { id = model.ApplicationId, nextaction = act });
                }
            }
            //上报
            else
            {
                //写系统日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();
                //已办
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.DoSomething = "添加(课题结案)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.Conclusion.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                int AddRiZhiSuccess = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //待办
                MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkFlowID), sectionName);
                MyRiZhi1.DoSomething = "需要审批(课题结案)";
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.Conclusion.ToString();
                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());


                // 更改当前结点id和name
                var currentNode = ERPNWorkFlowNodeService.GetEntityById(model.JieDianID.Value);
                string nextNodeSerial = currentNode.NextNode;
                var nextNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.NodeSerils == nextNodeSerial && p.WorkFlowID == model.WorkFlowID).ToList();
                model.JieDianID = nextNodeModel.First().ID;
                model.JieDianName = nextNodeModel.First().NodeName;

                //页面加载后就上报，就添加新的数据
                if (model.NWorkToDoID == 0)
                {
                    //上报时设置保存为false、驳回为false、删除为false、冻结为false
                    model.IsTemporary = false;
                    model.IsRejected = false;
                    model.IsDeleted = false;
                    model.IsLocked = false;

                    ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                    tempmodel.ApplicationStatus = ApplicationStatus.ProjectConcluding.ToString();
                    tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    bool isupdate = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());

                    int jiedianid = Convert.ToInt32(model.JieDianID);
                    var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                    model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));
                    int returncolusionid = this.ApplicationService.AddApplication(model.ToDataTransferObjectModel());

                    return Json(returncolusionid, JsonRequestBehavior.AllowGet);
                    //  return RedirectToAction("ConclusionsAgree", new { id = model.ApplicationId });
                }
                //先点保存再上报，就更新保存数据行的IsTemprory
                else
                {
                    //上报时设置保存为false、驳回为false、删除为false、冻结为false
                    model.IsTemporary = false;
                    model.IsRejected = false;
                    model.IsDeleted = false;
                    model.IsLocked = false;

                    ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                    tempmodel.ApplicationStatus = ApplicationStatus.ProjectConcluding.ToString();
                    tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    bool isupdate = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());

                    int jiedianid = Convert.ToInt32(model.JieDianID);
                    var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                    model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));
                    bool isUpdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());

                    ///成功标准
                    int AddSuccess = 1;
                    return Json(AddSuccess, JsonRequestBehavior.AllowGet);
                    // return RedirectToAction("ConclusionsAgree", new { id = model.ApplicationId });
                }
            }
            ////修改数据
            //else
            //{
            //    string act = "updatedata";
            //    return RedirectToAction("Conclusions", new { id = model.ApplicationId, nextaction = act });
            //}
        }

        /// <summary>
        /// 课题结案审批提交
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ConclusionsAgree(string flag, ERPNWorkToDoViewModel model, FormCollection collection)
        {
            //action flag
            //string toDoAction;
            string nodeSerils;
            if (flag == "Approval")
            {
                var attachment = model.FuJianList;
                int id = model.ApplicationId;
                string PiShiStr = model.ShenPiYiJian;

                // TODO: 审批上传附件
                model.ShenPiYiJian = attachment;
                model.OKUserList = model.OKUserList + "," + User.Identity.Name;
                if (collection["SingleShenPiYiJian"] != "")
                {
                    //审批意见
                    model.ShenPiYiJian = PiShiStr + model.JieDianName + ":" + collection["SingleShenPiYiJian"].ToString() + Constant.SplitChar;
                }
                else
                {
                    model.ShenPiYiJian = PiShiStr;
                }

                // 更改当前结点id和name
                var erpnrowkflownoderesult1 = ERPNWorkFlowNodeService.GetEntityById(model.JieDianID.Value).ToViewModel();
                string nextNodeSerial = erpnrowkflownoderesult1.NextNode;
                var nextNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.NodeSerils == nextNodeSerial && p.WorkFlowID == model.WorkFlowID).ToList();
                // 说明审批流程已经完成，审批通过
                if (nextNodeModel.Count == Constant.ZERO_INT)
                {
                    model.JieDianName = "结束";

                    ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                    tempmodel.ApplicationStatus = ApplicationStatus.ProjectConcluded.ToString();
                    tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    bool isupdate = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());

                    bool isUpdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());
                    ViewBag.Title = model.JieDianName;
                    nodeSerils = "结束";
                }
                //继续审批
                else
                {
                    model.JieDianID = nextNodeModel.FirstOrDefault().ID;
                    model.JieDianName = nextNodeModel.FirstOrDefault().NodeName;

                    ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                    tempmodel.ApplicationStatus = ApplicationStatus.ProjectConcluding.ToString();
                    tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    bool isupdate = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());

                    bool isUpdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());
                    ViewBag.Title = model.JieDianName;

                    //下一个节点的NodeSerils
                    nodeSerils = nextNodeModel.FirstOrDefault().NodeSerils;

                }
            }
            //驳回collection["Reject"]
            else
            {
                model.IsRejected = true;

                //更新申请书中
                ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                tempmodel.ApplicationStatus = ApplicationStatus.ConcludeRejected.ToString();
                tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                bool isupdate = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());


                var workFlowNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == model.WorkFlowID).OrderBy(p => p.NodeSerils).Skip(1).Take(1).FirstOrDefault();

                model.JieDianID = workFlowNodeModel.ID;
                model.JieDianName = workFlowNodeModel.NodeName.ToString();
                bool isUpdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());
                nodeSerils = "驳回";

            }

            string formname = EngineeringProjectTypeOfFormId.Conclusion.ToString();
            var rizhiresult = ERPRiZhiService.GetEntities(p => p.FKApplicationID == model.ApplicationId.ToString() && p.FkFormName == formname && p.FKAction == "待办").FirstOrDefault();
            ERPNWorkToDoViewModel erpnworktodoModel = new ERPNWorkToDoViewModel();
            erpnworktodoModel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
            IEnumerable<ERPNWorkFlowNodeViewModel> workFlowNodeModelList = new List<ERPNWorkFlowNodeViewModel>();
            //除填写申请书和第二级审批
            workFlowNodeModelList = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == model.WorkFlowID).OrderBy(p => p.NodeSerils).Skip(2).Select(x => x.ToViewModel());
            //将从第三节点（包括第三节点）的NodeSerials全部装在workFlowNodeModelList
            string nodeSerilsList = "";
            foreach (var item in workFlowNodeModelList)
            {
                nodeSerilsList = nodeSerilsList + item.NodeSerils;
            }
            //判断下一节点是否存在
            if (nodeSerilsList.Contains(nodeSerils))
            {
                //日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();

                //待办变成已办
                MyRiZhi.FKAction = "已办";
                MyRiZhi.ID = rizhiresult.ID;
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.DoSomething = "已审批" + erpnworktodoModel.UserName + "上报的课题结案";
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.Conclusion.ToString();
                MyRiZhi.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //新生成待办，下一审批人的待办

                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.ID = rizhiresult.ID;
                MyRiZhi1.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.DoSomething = "需要审批" + erpnworktodoModel.UserName + "上报的课题结案)";
                MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.Conclusion.ToString();
                MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                ERPNWorkFlowNodeViewModel erpnWorkFlowNodeModel = new ERPNWorkFlowNodeViewModel();
                erpnWorkFlowNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == model.WorkFlowID && p.NodeSerils == nodeSerils).FirstOrDefault().ToViewModel();

                MyRiZhi1.UserName = erpnWorkFlowNodeModel.SPDefaultList;

                int returnid = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                return Json(returnid, JsonRequestBehavior.AllowGet);
                //继续审批
                // return RedirectToAction("ConclusionsAgree", new { id = model.ApplicationId });
            }
            else if (nodeSerils == "结束")
            {
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();

                MyRiZhi.FKAction = "已办";
                MyRiZhi.ID = rizhiresult.ID;
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.DoSomething = "已审批" + erpnworktodoModel.UserName + "上报的课题结案";
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.Conclusion.ToString();
                MyRiZhi.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                ///成功标准
                int UpdateSuccess = 1;
                return Json(UpdateSuccess, JsonRequestBehavior.AllowGet);
                // 更新成功并且更新后，申请书状态变为 已经审批通过，跳转到项目确立页面
                //return RedirectToAction("ConclusionsAgree", new { id = model.ApplicationId });
            }
            //驳回toDoAction == "驳回"
            else
            {
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();

                //审批人已办
                MyRiZhi.FKAction = "已办";
                MyRiZhi.ID = rizhiresult.ID;
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.DoSomething = "已驳回" + erpnworktodoModel.UserName + "上报的课题结案";
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.Conclusion.ToString();
                MyRiZhi.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                bool UpdateRiZhiSuccess1 = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());
                //申请人待办

                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.ID = rizhiresult.ID;
                MyRiZhi1.UserName = erpnworktodoModel.UserName;
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.DoSomething = "课题结案被驳回，需修改数据";
                MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.Conclusion.ToString();
                MyRiZhi1.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                int returnid = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                return Json(returnid, JsonRequestBehavior.AllowGet);
                //return RedirectToAction("ConclusionsRejected", new { id = model.ApplicationId });
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        /// <summary>
        /// 课题结案驳回
        /// </summary>
        /// <param name="model"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        public ActionResult ConclusionsRejected(ERPNWorkToDoViewModel model, FormCollection collection)
        {
            model.UserName = User.Identity.Name;
            model.TimeStr = DateTime.Now;
            var sectionName = MySession[SessionKeyEnum.SectionName].ToString();

            //加载表单内容
            var temerpnformmodel = ERPNFormService.GetEntityById((int)EngineeringProjectTypeOfFormId.Conclusion).ToViewModel();
            model.FormContent = temerpnformmodel.ContentStr;
            model.FormContent = model.FormContent.Replace(Constant.MacroSectionString, sectionName);
            model.FormContent = model.FormContent.Replace(Constant.MacroUserNameString, User.Identity.Name);
            model.FormContent = FormContentReplaceHelper.ReplaceFormContentValue(model.FormContent, collection);

            try
            {
                model.StateNow = "正在办理";
            }
            catch
            {
                model.JieDianName = "结束";
                model.StateNow = "强制结束";
            }

            model.OKUserList = "默认";
            //保存
            if (collection["Save"] != null)
            {
                //添加申请人保存
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                //已办
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.DoSomething = "保存(课题结案)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.Conclusion.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi.TimeStr = DateTime.Now;
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                int returnRiZhiId = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                string act = "save";
                model.IsTemporary = true;
                model.IsLocked = false;
                model.IsDeleted = false;
                model.IsRejected = false;

                ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                tempmodel.ApplicationStatus = ApplicationStatus.ConcludeRejected.ToString();
                tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                bool isupdate = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());

                int jiedianid = Convert.ToInt32(model.JieDianID);
                var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));
                bool isUpdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());

                return RedirectToAction("ConclusionsRejected", new { id = model.ApplicationId });

            }
            //上报collection["Reported"]
            else
            {
                //写系统日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();
                //已办
                MyRiZhi = ERPRiZhiService.GetEntities(p => p.FKApplicationID == model.ApplicationId.ToString() && p.FKAction == "待办").FirstOrDefault().ToViewModel();
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.DoSomething = "添加(课题结案)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.Conclusion.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi.TimeStr = DateTime.Now;
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //待办
                MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkFlowID), sectionName);
                MyRiZhi1.DoSomething = "需要审批(Conclusion)";
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.Conclusion.ToString();
                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());


                //先点保存再上报，就更新保存数据行的IsTemprory

                //上报时设置保存为false、驳回为false、删除为false、冻结为false
                model.IsTemporary = false;
                model.IsRejected = false;
                model.IsDeleted = false;
                model.IsLocked = false;

                ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                tempmodel.ApplicationStatus = ApplicationStatus.ProjectConcluding.ToString();
                tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                bool isupdate = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());

                int jiedianid = Convert.ToInt32(model.JieDianID);
                var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));
                bool isUpdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());
                return RedirectToAction("ConclusionsRejected", new { id = model.ApplicationId });

            }

        }

        /// <summary>
        /// 项目延期申请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ExtensionRequest(string flag, ERPNWorkToDoViewModel model, FormCollection collection)
        {
            model.UserName = User.Identity.Name;
            model.TimeStr = DateTime.Now;
            var sectionName = MySession[SessionKeyEnum.SectionName].ToString();

            //加载表单内容
            var temerpnformmodel = ERPNFormService.GetEntityById((int)EngineeringProjectTypeOfFormId.ExtensionRequest).ToViewModel();
            model.FormContent = temerpnformmodel.ContentStr;
            model.FormContent = model.FormContent.Replace(Constant.MacroSectionString, sectionName);
            model.FormContent = model.FormContent.Replace(Constant.MacroUserNameString, User.Identity.Name);
            model.FormContent = FormContentReplaceHelper.ReplaceFormContentValue(model.FormContent, collection);

            try
            {
                model.StateNow = "正在办理";
            }
            catch
            {
                model.JieDianName = "结束";
                model.StateNow = "强制结束";
            }

            model.OKUserList = "默认";

            //保存
            if (flag == "Save")
            {
                //添加申请人保存
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                //已办
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.DoSomething = "保存(项目延期)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.ExtensionRequest.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = model.ApplicationId.ToString();
                int returnRiZhiId = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());
                //第一次保存
                if (model.NWorkToDoID == 0)
                {
                    string act = "save";

                    int jiedianid = Convert.ToInt32(model.JieDianID);
                    var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                    model.IsTemporary = true;
                    model.IsDeleted = false;
                    model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));
                    int returnextensionrequestid = this.ApplicationService.AddApplication(model.ToDataTransferObjectModel());

                    ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                    tempmodel.IsTemporary = true;
                    tempmodel.IsDeleted = false;
                    tempmodel.ApplicationStatus = ApplicationStatus.ExtensionUnWrite.ToString();
                    tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    bool isupdate = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());

                    ///成功标志
                    int UpdateSuccess = 1;
                    return Json(UpdateSuccess, JsonRequestBehavior.AllowGet);
                    // return RedirectToAction("ExtensionRequest", new { id = returnextensionrequestid, nextaction = act });
                }
                //第二次或第N次保存
                else
                {
                    string act = "save";

                    int jiedianid = Convert.ToInt32(model.JieDianID);
                    var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                    model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));
                    bool isupdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());

                    ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                    tempmodel.IsTemporary = true;
                    tempmodel.IsDeleted = false;
                    tempmodel.ApplicationStatus = ApplicationStatus.ExtensionUnWrite.ToString();
                    tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    bool isupdate1 = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());

                    return RedirectToAction("ExtensionRequest", new { id = model.NWorkToDoID, nextaction = act });
                }
            }
            //上报
            else
            {
                //写系统日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();
                //已办
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.DoSomething = "添加(项目延期)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.ExtensionRequest.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                int AddRiZhiSuccess = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //待办
                MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkFlowID), sectionName);
                MyRiZhi1.DoSomething = "需要审批(项目延期)";
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.ExtensionRequest.ToString();
                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                // 更改当前结点id和name
                var currentNode = ERPNWorkFlowNodeService.GetEntityById(model.JieDianID.Value);
                string nextNodeSerial = currentNode.NextNode;
                var nextNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.NodeSerils == nextNodeSerial && p.WorkFlowID == model.WorkFlowID).ToList();
                model.JieDianID = nextNodeModel.First().ID;
                model.JieDianName = nextNodeModel.First().NodeName;

                //页面加载后就上报，就添加新的数据
                if (model.NWorkToDoID == 0)
                {
                    //上报时设置保存为false、驳回为false、删除为false、冻结为false
                    model.IsTemporary = false;
                    model.IsRejected = false;
                    model.IsDeleted = false;
                    model.IsLocked = false;


                    ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                    tempmodel.ApplicationStatus = ApplicationStatus.ExtensionRequestApproving.ToString();
                    tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    bool isupdate1 = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());

                    int jiedianid = Convert.ToInt32(model.JieDianID);
                    var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                    model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));
                    int returnextensionrequestid = this.ApplicationService.AddApplication(model.ToDataTransferObjectModel());

                    return Json(returnextensionrequestid, JsonRequestBehavior.AllowGet);
                    //return RedirectToAction("ExtensionAgree", new { id = model.ApplicationId });
                }
                //先点保存再上报，就更新保存数据行的IsTemprory
                else
                {
                    //上报时设置保存为false、驳回为false、删除为false、冻结为false
                    model.IsTemporary = false;
                    model.IsRejected = false;
                    model.IsDeleted = false;
                    model.IsLocked = false;

                    ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                    tempmodel.ApplicationStatus = ApplicationStatus.ExtensionRequestApproving.ToString();
                    tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    bool isupdate1 = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());

                    int jiedianid = Convert.ToInt32(model.JieDianID);
                    var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                    model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));
                    bool isupdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());

                    int UpdateSuccess = 1;
                    return Json(UpdateSuccess, JsonRequestBehavior.AllowGet);
                    //return RedirectToAction("ExtensionAgree", new { id = model.ApplicationId });
                }
            }
            ////修改数据
            //else
            //{
            //    string act = "updatedata";
            //    return RedirectToAction("ExtensionRequest", new { id = model.NWorkToDoID, nextaction = act });
            //}
        }

        /// <summary>
        /// 项目延伸审批提交
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ExtensionAgree(string flag, ERPNWorkToDoViewModel model, FormCollection collection)
        {
            //action flag
            //string toDoAction;
            string nodeSerils;
            //审批
            if (flag == "Approval")
            {
                var attachment = model.FuJianList;
                int id = model.ApplicationId;
                string PiShiStr = model.ShenPiYiJian;
                // TODO: 审批上传附件
                model.OKUserList = model.OKUserList + "," + User.Identity.Name;
                if (collection["SingleShenPiYiJian"] != "")
                {
                    //审批意见
                    model.ShenPiYiJian = PiShiStr + model.JieDianName + ":" + collection["SingleShenPiYiJian"].ToString() + Constant.SplitChar;
                }
                else
                {
                    model.ShenPiYiJian = PiShiStr;
                }

                // 更改当前结点id和name
                var erpnrowkflownoderesult1 = ERPNWorkFlowNodeService.GetEntityById(model.JieDianID.Value).ToViewModel();
                string nextNodeSerial = erpnrowkflownoderesult1.NextNode;
                var nextNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.NodeSerils == nextNodeSerial && p.WorkFlowID == model.WorkFlowID).ToList();
                if (nextNodeModel.Count == Constant.ZERO_INT)
                {
                    // 说明审批流程已经完成，审批通过
                    model.JieDianName = "结束";
                    ViewBag.Title = "结束";

                    ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                    tempmodel.ApplicationStatus = ApplicationStatus.ExtensionAgreed.ToString();
                    tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    bool isupdate1 = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());

                    bool isUpdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());
                    nodeSerils = "结束";
                }
                // 说明审批流程还未完成，正在等待下一级审批
                else
                {
                    ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                    tempmodel.ApplicationStatus = ApplicationStatus.ExtensionRequestApproving.ToString();
                    tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    bool isupdate1 = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());
                    //根据序号和workflowID获得下一级节点ID
                    model.JieDianID = nextNodeModel.FirstOrDefault().ID;
                    model.JieDianName = nextNodeModel.FirstOrDefault().NodeName;
                    bool isUpdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());
                    ViewBag.Title = model.JieDianName;
                    //下一个节点的NodeSerils
                    nodeSerils = nextNodeModel.FirstOrDefault().NodeSerils;
                }

            }
            //驳回collection["Reject"]
            else
            {
                model.IsRejected = true;

                ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                tempmodel.ApplicationStatus = ApplicationStatus.ExtensionRejected.ToString();
                tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                bool isupdate = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());

                var workFlowNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == model.WorkFlowID).OrderBy(p => p.NodeSerils).Skip(1).Take(1).FirstOrDefault();

                model.JieDianID = workFlowNodeModel.ID;
                model.JieDianName = workFlowNodeModel.NodeName.ToString();
                bool isUpdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());

                nodeSerils = "驳回";

            }

            string formname = EngineeringProjectTypeOfFormId.ExtensionRequest.ToString();
            var rizhiresult = ERPRiZhiService.GetEntities(p => p.FKApplicationID == model.ApplicationId.ToString() && p.FkFormName == formname && p.FKAction == "待办").FirstOrDefault();
            ERPNWorkToDoViewModel erpnworktodoModel = new ERPNWorkToDoViewModel();
            erpnworktodoModel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
            IEnumerable<ERPNWorkFlowNodeViewModel> workFlowNodeModelList = new List<ERPNWorkFlowNodeViewModel>();
            //除填写申请书和第二级审批
            workFlowNodeModelList = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == model.WorkFlowID).OrderBy(p => p.NodeSerils).Skip(2).Select(x => x.ToViewModel());
            //将从第三节点（包括第三节点）的NodeSerials全部装在workFlowNodeModelList
            string nodeSerilsList = "";
            foreach (var item in workFlowNodeModelList)
            {
                nodeSerilsList = nodeSerilsList + item.NodeSerils;
            }
            //判断下一节点是否存在
            if (nodeSerilsList.Contains(nodeSerils))
            {
                //日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();

                //待办变成已办
                MyRiZhi.FKAction = "已办";
                MyRiZhi.ID = rizhiresult.ID;
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.DoSomething = "已审批" + erpnworktodoModel.UserName + "上报的项目延期";
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.ExtensionRequest.ToString();
                MyRiZhi.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //新生成待办，下一审批人的待办

                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.ID = rizhiresult.ID;
                MyRiZhi1.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.DoSomething = "需要审批(" + erpnworktodoModel.UserName + "上报的项目延期)";
                MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.ExtensionRequest.ToString();
                MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                ERPNWorkFlowNodeViewModel erpnWorkFlowNodeModel = new ERPNWorkFlowNodeViewModel();
                erpnWorkFlowNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == model.WorkFlowID && p.NodeSerils == nodeSerils).FirstOrDefault().ToViewModel();

                MyRiZhi1.UserName = erpnWorkFlowNodeModel.SPDefaultList;
                int returnid = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                return Json(returnid, JsonRequestBehavior.AllowGet);
                //继续审批
                // return RedirectToAction("ExtensionAgree", new { id = model.ApplicationId });
            }
            else if (nodeSerils == "结束")
            {
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();

                MyRiZhi.FKAction = "已办";
                MyRiZhi.ID = rizhiresult.ID;
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.DoSomething = "已审批" + erpnworktodoModel.UserName + "上报的项目延期";
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.ExtensionRequest.ToString();
                MyRiZhi.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                ///成功标志
                int UpdateSuccess = 1;
                return Json(UpdateSuccess, JsonRequestBehavior.AllowGet);
                // 更新成功并且更新后，申请书状态变为 已经审批通过，跳转到项目确立页面
                // return RedirectToAction("ExtensionAgree", new { id = model.ApplicationId });
            }
            //toDoAction == "驳回"
            else
            {
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();

                //审批人已办
                MyRiZhi.FKAction = "已办";
                MyRiZhi.ID = rizhiresult.ID;
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.DoSomething = "已驳回" + erpnworktodoModel.UserName + "上报的项目延期";
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.ExtensionRequest.ToString();
                MyRiZhi.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                bool UpdateRiZhiSuccess1 = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());
                //申请人待办

                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.ID = rizhiresult.ID;
                MyRiZhi1.UserName = erpnworktodoModel.UserName;
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.DoSomething = "项目延期被驳回，需修改数据";
                MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.ExtensionRequest.ToString();
                MyRiZhi1.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                int returnid = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                return Json(returnid, JsonRequestBehavior.AllowGet);
                //return RedirectToAction("ExtensionRejected", new { id = model.ApplicationId });
            }
        }
        /// <summary>
        /// 项目延期申请驳回
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ExtensionRejected(ERPNWorkToDoViewModel model, FormCollection collection)
        {
            model.UserName = User.Identity.Name;
            model.TimeStr = DateTime.Now;
            var sectionName = MySession[SessionKeyEnum.SectionName].ToString();

            //加载表单内容
            var temerpnformmodel = ERPNFormService.GetEntityById((int)EngineeringProjectTypeOfFormId.ExtensionRequest).ToViewModel();
            model.FormContent = temerpnformmodel.ContentStr;
            model.FormContent = model.FormContent.Replace(Constant.MacroSectionString, sectionName);
            model.FormContent = model.FormContent.Replace(Constant.MacroUserNameString, User.Identity.Name);
            model.FormContent = FormContentReplaceHelper.ReplaceFormContentValue(model.FormContent, collection);

            try
            {
                model.StateNow = "正在办理";
            }
            catch
            {
                model.JieDianName = "结束";
                model.StateNow = "强制结束";
            }

            model.OKUserList = "默认";

            //保存
            if (collection["Save"] != null)
            {
                //添加申请人保存
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                //已办
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.DoSomething = "保存(项目延期)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.ExtensionRequest.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();
                MyRiZhi.TimeStr = DateTime.Now;
                int returnRiZhiId = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                string act = "save";
                model.IsTemporary = true;
                model.IsLocked = false;
                model.IsDeleted = false;
                model.IsRejected = false;

                ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                tempmodel.IsTemporary = true;
                tempmodel.IsDeleted = false;
                tempmodel.ApplicationStatus = ApplicationStatus.ExtensionRejected.ToString();
                tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                bool isupdate = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());

                int jiedianid = Convert.ToInt32(model.JieDianID);
                var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));
                bool isUpdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());

                return RedirectToAction("ExtensionRejected", new { id = model.ApplicationId });

            }
            //上报
            else
            {

                //写系统日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();
                //已办
                MyRiZhi = ERPRiZhiService.GetEntities(p => p.FKApplicationID == model.ApplicationId.ToString() && p.FKAction == "待办").FirstOrDefault().ToViewModel();
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.DoSomething = "添加(项目延期)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.ExtensionRequest.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //待办
                MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkFlowID), sectionName);
                MyRiZhi1.DoSomething = "需要审批(项目延期)";
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.ExtensionRequest.ToString();
                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();
                MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());


                //上报时设置保存为false、驳回为false、删除为false、冻结为false
                model.IsTemporary = false;
                model.IsRejected = false;
                model.IsDeleted = false;
                model.IsLocked = false;

                ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                tempmodel.ApplicationStatus = ApplicationStatus.ExtensionRequestApproving.ToString();
                tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                bool isupdate1 = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());

                int jiedianid = Convert.ToInt32(model.JieDianID);
                var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));
                bool isupdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());

                return RedirectToAction("ExtensionRejected", new { id = model.ApplicationId });
            }
        }

        /// <summary>
        /// 奖励设置  /// TODO
        /// </summary>
        /// <param name="model"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ProjectBonusCredit(ProjectBonusCreditViewModel model, FormCollection collection)
        {
            bool isUpdate = this.ProjectBonusCreditService.UpdateProjectBonusCredit(model.ToDataTransferObjectModel());
            return View();
        }

        /// <summary>
        /// 根据id删除申请书
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public ActionResult DeleteApplicationListByModelId(string modelId)
        {
            ///删除所有申请书的日志
            IList<ERPRiZhiTransferObject> riZhiModelList = new List<ERPRiZhiTransferObject>();
            riZhiModelList = ERPRiZhiService.GetEntities(p => p.FKApplicationID == modelId);
            foreach (var item in riZhiModelList)
            {
                int riZhiId = item.ID;
                bool deleteSuccess = ERPRiZhiService.DeleteEntityById(riZhiId);

            }

            ERPNWorkToDoTransferObject toDoresult = new ERPNWorkToDoTransferObject();
            toDoresult = ApplicationService.GetEntityById(Convert.ToInt32(modelId));

            ///如果有项目确立，则删除项目确立projectrecord的数据
            IList<ProjectRecordTransferObject> projectresultlist = new List<ProjectRecordTransferObject>();
            projectresultlist = ProjectRecordService.GetEntities(p => p.ApplicationId == Convert.ToInt32(modelId));

            foreach (var item in projectresultlist)
            {
                int projectId = item.ID;
                bool projectdelete = ProjectRecordService.DeleteEntityById(projectId);
            }

            ///如果有经费记录，则删除fundsrecord的数据
            IList<FundsRecordTransferObject> fundsModelList = new List<FundsRecordTransferObject>();
            fundsModelList = FundsRecordService.GetEntities(p => p.ApplicationId == Convert.ToInt32(modelId));


            foreach (var item in fundsModelList)
            {
                int fundsId = item.ID;

                IList<ERPRiZhiTransferObject> riZhiModelList1 = new List<ERPRiZhiTransferObject>();
                riZhiModelList1 = ERPRiZhiService.GetEntities(p => p.FKApplicationID == fundsId.ToString());

                foreach (var item1 in riZhiModelList1)
                {
                    int riZhiId = item1.ID;
                    bool fundsRizhidelete = ERPRiZhiService.DeleteEntityById(riZhiId);

                }

                bool fundsdelete = FundsRecordService.DeleteEntityById(fundsId);

            }

            ///删除申请书后面的流程。如：过程记录、课题结案等
            IList<ERPNWorkToDoTransferObject> toDoModelList = new List<ERPNWorkToDoTransferObject>();
            toDoModelList = ApplicationService.GetEntities(p => p.ApplicationId == Convert.ToInt32(modelId));
            foreach (var item in toDoModelList)
            {
                int toDoId = item.ID;
                bool ToDodelete = ApplicationService.DeleteEntityById(toDoId);
            }

            ///删除项目申请书
            bool delete = ApplicationService.DeleteEntityById(Convert.ToInt32(modelId));

            return View();
        }
        [HttpPost]
        /// <summary>
        /// 申请书列表查询
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="projectName">项目名称</param>
        /// <param name="state">状态</param>
        /// <param name="freeze">是否冻结</param>
        /// <returns></returns>
        public ActionResult ApplicationListStatistics(string projectName, string State, string projectNumber, string startTime, string endTime, int pageIndex, int pageSize)
        {
            DateTime start, end;
            int totalPage = 0;
            if (!string.IsNullOrEmpty(endTime))
            {
                end = Convert.ToDateTime(endTime);
                end = end.AddDays(1);
            }
            else
            {
                end = Constant.MaxTime;
            }

            if (!string.IsNullOrEmpty(startTime))
            {
                start = Convert.ToDateTime(startTime);
            }
            else
            {
                start = Constant.MinTime;
            }

            IEnumerable<ERPNWorkToDoTransferObject> result = SearchProcessingApplicationList(projectName, State, projectNumber, start,
                end, pageSize, pageIndex, ref totalPage);

            bool hasRolesFlag = HasRolesFlag();
            IEnumerable<ERPNWorkToDoTransferObject> resultpage;
            int totalcount = 0;
            //非普通用户
            if (hasRolesFlag)
            {
                resultpage = ApplicationService.GetEntities(p => p.FormID == (int)EngineeringProjectTypeOfFormId.Application
                    && p.TimeStr.Value > start
                    && p.TimeStr.Value < end
                    && ((State == Constant.All) ? true : p.ApplicationStatus == State)
                    && (string.IsNullOrEmpty(projectName) ? true : (p.WenHao.Contains(projectName)))
                    && p.StateNow == "正在办理"
                    && (string.IsNullOrEmpty(projectNumber) ? true : (p.BeiYong1.Contains(projectNumber)))
                    && p.ProjectStatus != ApplicationStatus.BigProjectProcessing.ToString());
                totalcount = resultpage.Count();
            }
            //普通用户
            else
            {
                resultpage = ApplicationService.GetEntities(p => p.FormID == (int)EngineeringProjectTypeOfFormId.Application
                    && p.TimeStr.Value > start
                    && p.TimeStr.Value < end
                    && ((State == Constant.All) ? true : p.ApplicationStatus == State)
                    && (string.IsNullOrEmpty(projectName) ? true : (p.WenHao.Contains(projectName)))
                    && p.StateNow == "正在办理"
                    && p.UserName == User.Identity.Name
                    && (string.IsNullOrEmpty(projectNumber) ? true : (p.BeiYong1.Contains(projectNumber)))
                    && p.ProjectStatus != ApplicationStatus.BigProjectProcessing.ToString());
                totalcount = resultpage.Count();
            }
            return Json(new { data = result, total = totalcount }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 工程项目统计
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public ActionResult ScienceProjectStatisticsAnalysis(FormCollection collection)
        {
            if (collection["OutScienceProjectStatistics"] != null)
            {
                IEnumerable<ScienceProjectStatisticsTransferObject> result = StatisticService.GetScienceProjectStatistics(x => true, (int)EngineeringProjectTypeOfFormId.Application);

                DataTable dt = new DataTable();
                string excelName = "工程项目-工程项目统计列表";
                string fileName = DateTime.Now.ToString("yyyyMMddhhmmss");
                string path = System.Web.HttpContext.Current.Server.MapPath("~/UploadFiles/工程项目-工程项目统计-" + fileName + ".xls");
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
                foreach (var item in result)
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
                return this.File(path, "application/octet-stream", "工程项目-工程项目统计-" + fileName + ".xls");
            }

            else
            {
                var result = StatisticService.GetScienceProjectStatistics(x => true, (int)EngineeringProjectTypeOfFormId.Application);

                return PartialView(result.Select(x => x.ToViewModel()));
            }
        }
        [HttpPost]
        /// <summary>
        /// 工程项目立项统计
        /// </summary>
        /// <returns></returns>
        public ActionResult ScienceProjectEstablishStatisticsAnalysis(FormCollection collection)
        {
            if (collection["OutScienceProjectEstablishStatistics"] != null)
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

                DataTable dt = new DataTable();
                string excelName = "工程项目统计列表";
                string fileName = DateTime.Now.ToString("yyyyMMddhhmmss");
                string path = System.Web.HttpContext.Current.Server.MapPath("~/UploadFiles/工程项目-科研立项统计-" + fileName + ".xls");
                //Excel表头
                dt.Columns.Add("年份");
                dt.Columns.Add("国家级");
                dt.Columns.Add("省级");
                dt.Columns.Add("市级");
                dt.Columns.Add("局级");
                dt.Columns.Add("院级");
                dt.Columns.Add("合计");

                //往datatable中填入内容
                foreach (var item in resultList)
                {

                    DataRow row = dt.NewRow();
                    row["年份"] = item.Year;
                    row["国家级"] = item.CountryLevel;
                    row["省级"] = item.ProvinceLevel;
                    row["市级"] = item.CityLevel;
                    row["局级"] = item.CountyLevel;
                    row["院级"] = item.HospitalLevel;
                    row["合计"] = item.TotalCount;

                    dt.Rows.Add(row);
                }
                DataRow totalrow = dt.NewRow();
                totalrow["年份"] = "合计";
                totalrow["国家级"] = resultList.Sum(x => x.CountryLevel);
                totalrow["省级"] = resultList.Sum(x => x.ProvinceLevel);
                totalrow["市级"] = resultList.Sum(x => x.CityLevel);
                totalrow["局级"] = resultList.Sum(x => x.CountyLevel);
                totalrow["院级"] = resultList.Sum(x => x.HospitalLevel);
                totalrow["合计"] = resultList.Sum(x => x.TotalCount);
                dt.Rows.Add(totalrow);
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
                return this.File(path, "application/octet-stream", "工程项目-科研立项统计-" + fileName + ".xls");

            }
            else
            {
                return PartialView();
            }
        }
        /// <summary>
        /// 根据id删除申请书
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public ActionResult DeleteProcessingListByModelId(string modelId)
        {
            ///删除所有worktodo申请书的日志
            IList<ERPRiZhiTransferObject> riZhiModelList = new List<ERPRiZhiTransferObject>();
            riZhiModelList = ERPRiZhiService.GetEntities(p => p.FKApplicationID == modelId);
            foreach (var item in riZhiModelList)
            {
                int riZhiId = item.ID;
                bool deleteSuccess = ERPRiZhiService.DeleteEntityById(riZhiId);

            }

            ERPNWorkToDoTransferObject toDoresult = new ERPNWorkToDoTransferObject();
            toDoresult = ApplicationService.GetEntityById(Convert.ToInt32(modelId));

            ///删除项目确立的数据
            IList<ProjectRecordTransferObject> projectresultlist = new List<ProjectRecordTransferObject>();
            projectresultlist = ProjectRecordService.GetEntities(p => p.ApplicationId == Convert.ToInt32(modelId));

            foreach (var item in projectresultlist)
            {
                int projectId = item.ID;
                bool projectdelete = ProjectRecordService.DeleteEntityById(projectId);
            }

            ///删除经费的数据
            IList<FundsRecordTransferObject> fundsModelList = new List<FundsRecordTransferObject>();
            fundsModelList = FundsRecordService.GetEntities(p => p.ApplicationId == Convert.ToInt32(modelId));


            foreach (var item in fundsModelList)
            {
                int fundsId = item.ID;
                ///删除经费的日志
                IList<ERPRiZhiTransferObject> riZhiModelList1 = new List<ERPRiZhiTransferObject>();
                riZhiModelList1 = ERPRiZhiService.GetEntities(p => p.FKApplicationID == fundsId.ToString());

                foreach (var item1 in riZhiModelList1)
                {
                    int riZhiId = item1.ID;
                    bool fundsRizhidelete = ERPRiZhiService.DeleteEntityById(riZhiId);

                }
                ///删除差旅的数据
                IList<TravelFundsDetailTransferObject> TravelModelList = new List<TravelFundsDetailTransferObject>();
                TravelModelList = TravelFundsDetailService.GetEntities(p => p.FundsRecordId == fundsId);


                foreach (var item2 in TravelModelList)
                {
                    int TravelId = item2.ID;
                    bool Traveldelete = TravelFundsDetailService.DeleteEntityById(TravelId);

                }

                bool fundsdelete = FundsRecordService.DeleteEntityById(fundsId);


            }
            ///删除申请项目后面申请单数据
            IList<ERPNWorkToDoTransferObject> toDoModelList = new List<ERPNWorkToDoTransferObject>();
            toDoModelList = ApplicationService.GetEntities(p => p.ApplicationId == Convert.ToInt32(modelId));
            foreach (var item in toDoModelList)
            {
                int toDoId = item.ID;
                bool ToDodelete = ApplicationService.DeleteEntityById(toDoId);
            }
            ///删除申请书
            bool delete = ApplicationService.DeleteEntityById(Convert.ToInt32(modelId));
            return View();
        }
        [HttpPost]
        /// <summary>
        /// 过程记录列表查询
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="projectName">项目名称</param>
        /// <param name="state">状态</param>
        /// <param name="freeze">是否冻结</param>
        /// <returns></returns>
        public ActionResult ProcessingListStatistics(string startTime, string endTime, string projectName, string State, string Freeze, int page, int pageSize)
        {
            DateTime start, end;
            int totalPage = 1;

            if (!string.IsNullOrEmpty(endTime))
            {
                end = Convert.ToDateTime(endTime);
                end = end.AddDays(1);
            }
            else
            {
                end = Constant.MaxTime;
            }

            if (!string.IsNullOrEmpty(startTime))
            {
                start = Convert.ToDateTime(startTime);
            }
            else
            {
                start = Constant.MinTime;
            }

            //SearchCriteriaProjectName = projectName;
            //SearchCriteriaProjectStatus = State;
            //SearchCriteriaIsLocked = Freeze;
            //SearchCriteriaStartTime = start;
            //SearchCriteriaEndTime = end;
            //InitialTheSearchCriteria();

            IEnumerable<ERPNWorkToDoTransferObject> result = SearchProcessingProjectList(projectName, State, Freeze, start,
                end, pageSize, page, ref totalPage);


            bool hasRolesFlag = HasRolesFlag();
            IEnumerable<ERPNWorkToDoTransferObject> resultpage;
            int pageCount;
            //非普通用户
            if (hasRolesFlag)
            {
                resultpage = ApplicationService.GetEntities(p => p.FormID == (int)EngineeringProjectTypeOfFormId.Application
                    && p.TimeStr.Value > start
                    && p.TimeStr.Value < end
                    && ((State == Constant.All) ? true : p.ApplicationStatus == State)
                    && (string.IsNullOrEmpty(projectName) ? true : (p.WenHao.Contains(projectName)))
                    && p.StateNow == "正在办理"
                    && ((Freeze == Constant.All) ? true : p.IsLocked == (Freeze == "冻结"))
                    && p.ProjectStatus == ApplicationStatus.BigProjectProcessing.ToString());
                pageCount = resultpage.Count();
            }
            //普通用户
            else
            {
                resultpage = ApplicationService.GetEntities(p => p.FormID == (int)EngineeringProjectTypeOfFormId.Application
                    && p.TimeStr.Value > start
                    && p.TimeStr.Value < end
                    && ((State == Constant.All) ? true : p.ApplicationStatus == State)
                    && (string.IsNullOrEmpty(projectName) ? true : (p.WenHao.Contains(projectName)))
                    && p.StateNow == "正在办理"
                    && p.UserName == User.Identity.Name
                    && ((Freeze == Constant.All) ? true : p.IsLocked == (Freeze == "冻结"))
                    && p.ProjectStatus == ApplicationStatus.BigProjectProcessing.ToString());
                pageCount = resultpage.Count();
            }


            return Json(new { data = result, total = pageCount }, JsonRequestBehavior.AllowGet);

        }

        #endregion

        public ActionResult SelectUser()
        {
            return View();
        }

        public ActionResult SelectUserData(string Id)
        {
            // Demo
            //IList<TravelFundsDetailViewModel> result = new List<TravelFundsDetailViewModel>();

            //result.Add(new TravelFundsDetailViewModel { FromAddress = "Chengdu", ToAddress = "Shanghai", });
            //result.Add(new TravelFundsDetailViewModel { FromAddress = "Sichuan", ToAddress = "Beijing", });

            //IList<TravelFundsRecordViewModel> master = new List<TravelFundsRecordViewModel>();

            //master.Add(new TravelFundsRecordViewModel { Name = "Jim test master", TravelFundsList = result });
            //master.Add(new TravelFundsRecordViewModel { Name = "Joan Liu", TravelFundsList = result });

            //master.Add(new TravelFundsRecordViewModel { Name = "Zhang WenBo", TravelFundsList = result });

            //return Json(master, JsonRequestBehavior.AllowGet);

            //ApplicationDbContext userManager = new ApplicationDbContext();
            using (ApplicationDbContext userManager = new ApplicationDbContext())
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    var arrayList = Id.Split(Constant.Comma);
                    if (arrayList[0] == "Hospital")
                    {
                        var key = arrayList[1];
                        var departments = userManager.Departments.Where(x => x.HospitalId == key).ToList();

                        return Json(departments.Select(x => x.ToDataTransferObjectModel()).ToList(), JsonRequestBehavior.AllowGet);
                    }
                    else if (arrayList[0] == "Department")
                    {
                        var key = arrayList[1];
                        //var sections = userManager.Sections.Where(x => x.DepartmentId == key).ToList();

                        //return Json(sections.Select(x => x.ToDataTransferObjectModel()).ToList(), JsonRequestBehavior.AllowGet);
                        return null;
                    }
                    else if (arrayList[0] == "Section")
                    {
                        var key = arrayList[1];
                        var userIdList = userManager.Sections.FirstOrDefault(x => x.Id == key).ApplicationUsers.Select(x => x.ApplicationUserId);

                        var userList = userManager.Users.Where(p => userIdList.Contains(p.Id)).ToList();
                        return Json(userList.Select(x => x.ToDataTransferObjectModel()).ToList(), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var hospitals = userManager.Hospitals.ToList();
                    var result = hospitals.Select(x => x.ToDataTransferObjectModel()).ToList();

                    return Json(result.ToList(), JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpGet]
        public ActionResult AllUserList()
        {
            IEnumerable<ApplicationUser> allUserList = new List<ApplicationUser>();
            using (ApplicationDbContext userManager = new ApplicationDbContext())
            {
                allUserList = userManager.Users.ToList();
            }
            return Json(allUserList, JsonRequestBehavior.AllowGet);
        }

        #region Private Method

        /// <summary>
        /// Initail the searchCriteria from session of this controller
        /// </summary>
        private void InitialTheSearchCriteria()
        {
            if (MySession[SessionKeyEnum.ProjectName] != null)
            {
                SearchCriteriaProjectName = MySession[SessionKeyEnum.ProjectName].ToString();
            }
            if (MySession[SessionKeyEnum.ProjectStatus] != null)
            {
                SearchCriteriaProjectStatus = MySession[SessionKeyEnum.ProjectStatus].ToString();
            }
            else
            {
                SearchCriteriaProjectStatus = Constant.All;
            }
            if (MySession[SessionKeyEnum.IsLocked] != null)
            {
                SearchCriteriaIsLocked = MySession[SessionKeyEnum.IsLocked].ToString();
            }
            else
            {
                SearchCriteriaIsLocked = Constant.All;
            }

            if (MySession[SessionKeyEnum.StartTime] != null)
            {
                SearchCriteriaStartTime = (DateTime)MySession[SessionKeyEnum.StartTime];
            }
            else
            {
                SearchCriteriaStartTime = Constant.MinTime;
            }

            if (MySession[SessionKeyEnum.EndTime] != null)
            {
                SearchCriteriaEndTime = (DateTime)MySession[SessionKeyEnum.EndTime];
            }
            else
            {
                SearchCriteriaEndTime = Constant.MaxTime;
            }
        }

        /// <summary>
        /// Initail the session of this controller
        /// </summary>
        private void InitialSession()
        {
            MySession[SessionKeyEnum.ProjectName] = SearchCriteriaProjectName;
            MySession[SessionKeyEnum.ProjectStatus] = SearchCriteriaProjectStatus;
            MySession[SessionKeyEnum.IsLocked] = SearchCriteriaIsLocked;
            MySession[SessionKeyEnum.StartTime] = SearchCriteriaStartTime;
            MySession[SessionKeyEnum.EndTime] = SearchCriteriaEndTime;
        }

        /// <summary>
        /// Search the application list that haven't been established.(查找还未 立项的项目申请书)
        /// </summary>
        /// <param name="projectName">项目名称</param>
        /// <param name="state">申请书状态</param>
        /// <param name="freeze">项目是否冻结</param>
        /// <param name="start">项目查找开始时间</param>
        /// <param name="end">项目查找结束时间</param>
        /// <param name="totalPage">总页数</param>
        /// <returns></returns>
        private IEnumerable<ERPNWorkToDoTransferObject> SearchProcessingApplicationList(string projectName, string state, string projectNumber, DateTime start, DateTime end, int pageSize, int pageIndex, ref int totalPage)
        {
            bool hasRolesFlag = HasRolesFlag();
            IEnumerable<ERPNWorkToDoTransferObject> result;
            //非普通用户
            if (hasRolesFlag)
            {
                result = ApplicationService.GetPageEntities(p => p.FormID == (int)EngineeringProjectTypeOfFormId.Application
                    && p.TimeStr.Value > start
                    && p.TimeStr.Value < end
                    && ((state == Constant.All) ? true : p.ApplicationStatus == state)
                    && (string.IsNullOrEmpty(projectName) ? true : (p.WenHao.Contains(projectName)))
                    && p.StateNow == "正在办理"
                    && (string.IsNullOrEmpty(projectNumber) ? true : (p.BeiYong1.Contains(projectNumber)))
                    && p.ProjectStatus != ApplicationStatus.BigProjectProcessing.ToString(), ApplicationSortField.TimeStr_Desc.ToString(), pageSize, pageIndex, out totalPage);
            }
            //普通用户
            else
            {
                result = ApplicationService.GetPageEntities(p => p.FormID == (int)EngineeringProjectTypeOfFormId.Application
                    && p.TimeStr.Value > start
                    && p.TimeStr.Value < end
                    && ((state == Constant.All) ? true : p.ApplicationStatus == state)
                    && (string.IsNullOrEmpty(projectName) ? true : (p.WenHao.Contains(projectName)))
                    && p.StateNow == "正在办理"
                    && p.UserName == User.Identity.Name
                    && (string.IsNullOrEmpty(projectNumber) ? true : (p.BeiYong1.Contains(projectNumber)))
                    && p.ProjectStatus != ApplicationStatus.BigProjectProcessing.ToString(), ApplicationSortField.TimeStr_Desc.ToString(), pageSize, pageIndex, out totalPage);
            }
            return result;
        }
        private IEnumerable<ERPNWorkToDoTransferObject> SearchArchiveDocumentList(string projectName, string state, string projectNumber, DateTime start, DateTime end, int pageSize, int pageIndex, ref int totalPage)
        {
            bool hasRolesFlag = HasRolesFlag();
            IEnumerable<ERPNWorkToDoTransferObject> result;
            //非普通用户
            if (hasRolesFlag)
            {
                result = ApplicationService.GetPageEntities(p => p.FormID == (int)EngineeringProjectTypeOfFormId.ArchiveDocument
                    && p.TimeStr.Value > start
                    && p.TimeStr.Value < end
                    && ((state == Constant.All) ? true : p.ApplicationStatus == state)
                    && (string.IsNullOrEmpty(projectName) ? true : (p.WenHao.Contains(projectName)))
                    && p.StateNow == "正在办理"
                    && (string.IsNullOrEmpty(projectNumber) ? true : (p.BeiYong1.Contains(projectNumber)))
                    && p.ProjectStatus != ApplicationStatus.BigProjectProcessing.ToString(), ApplicationSortField.TimeStr_Desc.ToString(), pageSize, pageIndex, out totalPage);
            }
            //普通用户
            else
            {
                result = ApplicationService.GetPageEntities(p => p.FormID == (int)EngineeringProjectTypeOfFormId.ArchiveDocument
                    && p.TimeStr.Value > start
                    && p.TimeStr.Value < end
                    && ((state == Constant.All) ? true : p.ApplicationStatus == state)
                    && (string.IsNullOrEmpty(projectName) ? true : (p.WenHao.Contains(projectName)))
                    && p.StateNow == "正在办理"
                    && p.UserName == User.Identity.Name
                    && (string.IsNullOrEmpty(projectNumber) ? true : (p.BeiYong1.Contains(projectNumber)))
                    && p.ProjectStatus != ApplicationStatus.BigProjectProcessing.ToString(), ApplicationSortField.TimeStr_Desc.ToString(), pageSize, pageIndex, out totalPage);
            }
            return result;
        }

        /// <summary>
        /// Search the processing list that haven't been established.(查找已经 立项的项目申请书，也就是在处在项目过程中的项目)
        /// </summary>
        /// <param name="projectName">项目名称</param>
        /// <param name="state">项目状态</param>
        /// <param name="freeze">项目是否冻结</param>
        /// <param name="start">项目查找开始时间</param>
        /// <param name="end">项目查找结束时间</param>
        /// <param name="totalPage">总页数</param>
        /// <returns></returns>
        private IEnumerable<ERPNWorkToDoTransferObject> SearchProcessingProjectList(string projectName, string state, string freeze, DateTime start, DateTime end, int pageSize, int pageIndex, ref int totalPage)
        {
            bool hasRolesFlag = HasRolesFlag();
            IEnumerable<ERPNWorkToDoTransferObject> result;
            //非普通用户
            if (hasRolesFlag)
            {
                result = ApplicationService.GetPageEntities(p => p.FormID == (int)EngineeringProjectTypeOfFormId.Application
                    && p.TimeStr.Value > start
                    && p.TimeStr.Value < end
                    && ((state == Constant.All) ? true : p.ApplicationStatus == state)
                    && (string.IsNullOrEmpty(projectName) ? true : (p.WenHao.Contains(projectName)))
                    && p.StateNow == "正在办理"
                    && ((freeze == Constant.All) ? true : p.IsLocked == (freeze == "冻结"))
                    && p.ProjectStatus == ApplicationStatus.BigProjectProcessing.ToString(), ApplicationSortField.TimeStr_Desc.ToString(), pageSize, pageIndex, out totalPage);
            }
            //普通用户
            else
            {
                result = ApplicationService.GetPageEntities(p => p.FormID == (int)EngineeringProjectTypeOfFormId.Application
                    && p.TimeStr.Value > start
                    && p.TimeStr.Value < end
                    && ((state == Constant.All) ? true : p.ApplicationStatus == state)
                    && (string.IsNullOrEmpty(projectName) ? true : (p.WenHao.Contains(projectName)))
                    && p.StateNow == "正在办理"
                    && p.UserName == User.Identity.Name
                    && ((freeze == Constant.All) ? true : p.IsLocked == (freeze == "冻结"))
                    && p.ProjectStatus == ApplicationStatus.BigProjectProcessing.ToString(), ApplicationSortField.TimeStr_Desc.ToString(), pageSize, pageIndex, out totalPage);
            }
            return result;
        }
        /// <summary>
        /// 判断此申请书节点的角色和登录人角色是否对应
        /// </summary>
        /// <param name="id">ERPNWorkToDoId</param>
        /// <returns></returns>
        public bool IsRightRoles(string id)
        {
            ERPNWorkToDoViewModel model = ApplicationService.GetEntityById(Convert.ToInt32(id)).ToViewModel();

            ERPNWorkFlowNodeTransferObject currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.ID == model.JieDianID).First();

            bool isRightRoles = false;

            string loginPersonDepartment = MySession[SessionKeyEnum.SectionName].ToString();
            ApplicationDbContext usercontext = new ApplicationDbContext();
            string UserId = usercontext.Users.Where(x => x.UserName == model.UserName).FirstOrDefault().Id;
            //根据用户名找科室
            string applicantdepartMentName = usercontext.Sections.Where(x => x.ApplicationUsers.Any(u => u.ApplicationUserId == UserId)).FirstOrDefault().Name;

            if (currentNode.SPDefaultList == UserRoles.本科室主任.ToString())
            {

                if (loginPersonDepartment == "系统账户")
                {
                    isRightRoles = true;
                }
                else
                {
                    isRightRoles = User.IsInRole(applicantdepartMentName + "主任");
                }
            }
            else
            {
                if (loginPersonDepartment == "系统账户")
                {
                    isRightRoles = true;
                }
                else
                {
                    isRightRoles = User.IsInRole(currentNode.SPDefaultList);
                }
            }
            return isRightRoles;
        }
        /// <summary>
        /// 判断此申请书节点的角色和登录人角色是否对应
        /// </summary>
        /// <param name="id">FundsRecord的ID</param>
        /// <returns></returns>
        public bool IsRightRolesAboutFunds(string id)
        {
            FundsRecordViewModel model = FundsRecordService.GetEntityById(Convert.ToInt32(id)).ToViewModel();
            //判断驳回或继续审批
            bool isRejected = Convert.ToBoolean(model.IsRejected);

            ERPNWorkFlowNodeTransferObject currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.ID == model.JieDianID).First();

            bool isRightRoles = false;

            string loginPersonDepartment = MySession[SessionKeyEnum.SectionName].ToString();
            ApplicationDbContext usercontext = new ApplicationDbContext();
            string UserId = usercontext.Users.Where(x => x.UserName == model.UserName).FirstOrDefault().Id;
            //根据用户名找科室
            string applicantdepartMentName = usercontext.Sections.Where(x => x.ApplicationUsers.Any(u => u.ApplicationUserId == UserId)).FirstOrDefault().Name;

            if (currentNode.SPDefaultList == UserRoles.本科室主任.ToString())
            {

                if (loginPersonDepartment == "系统账户")
                {
                    isRightRoles = true;
                }
                else
                {
                    isRightRoles = User.IsInRole(applicantdepartMentName + "主任");
                }
            }
            else
            {
                if (loginPersonDepartment == "系统账户")
                {
                    isRightRoles = true;
                }
                else
                {
                    isRightRoles = User.IsInRole(currentNode.SPDefaultList);
                }
            }
            return isRightRoles;
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
        /// <summary>
        ///  //当前申请书第一个审批人角色
        /// </summary>
        /// <param name="workflowId">workFlowId</param>
        /// <param name="sectionName">科室名称</param>
        /// <returns></returns>
        public string FillInRiZhi(int workflowId, string sectionName)
        {
            //当前申请书第一个审批人角色
            ERPNWorkFlowNodeViewModel firstDataModel = new ERPNWorkFlowNodeViewModel();
            string fillInRiZhiUserName = "";
            firstDataModel = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == workflowId && int.Parse(p.NodeSerils) == 2).FirstOrDefault().ToViewModel();
            if (firstDataModel.SPDefaultList == UserRoles.本科室主任.ToString())
            {
                if (sectionName == "系统账户")
                {
                    fillInRiZhiUserName = UserRoles.科教科主任.ToString();
                }
                else
                {
                    fillInRiZhiUserName = sectionName + "主任";
                }
            }
            else
            {
                fillInRiZhiUserName = firstDataModel.SPDefaultList;
            }
            return fillInRiZhiUserName;
        }
        #endregion

        /// <summary>
        /// Error action
        /// </summary>
        /// <returns></returns>
        public ActionResult Error()
        {
            ViewBag.Message = "Error.";

            return View();
        }

        public ActionResult UpoadFilePageCreate(UploadFilePageType type)
        {

            return View();
        }

        public ActionResult UpoadFileListPage(UploadFilePageType type)
        {
            var results = new List<FileUploadViewModels>();

            results.Add(new FileUploadViewModels
            {
                CreatedTime = DateTime.Today.AddDays(-10),
                FileAddress = "",
                FileName = "川国土资【2016】89号2016年度第一批省投资高标准基本农田建设项目立项的合同-新安、太平.pdf",

                FileSize = "5735 KB ",
                FileType = UploadFilePageType.合同资料,
                Number = 1,
                OperatorId = "123",
                OperatorName = "肖燕",
                Remark = "招标代理合同"
            });
            results.Add(new FileUploadViewModels
            {
                CreatedTime = DateTime.Now.AddDays(-8),
                FileAddress = "",
                FileName = "川国土资【2016】90号2016年度第一批省投资土地开发整理项目立项的合同-新安、太平.pdf",

                FileSize = "6422 KB ",
                FileType = UploadFilePageType.合同资料,
                Number = 2,
                OperatorId = "123",
                OperatorName = "肖燕",
                Remark = "招标代理合同"
            });
            return View(results);
        }

        public ActionResult UpoadFilePageDetail(UploadFilePageType type)
        {
            var result = new FileUploadViewModels
            {
                CreatedTime = DateTime.Today.AddDays(-10),
                FileAddress = "",
                FileName = "川国土资【2016】89号2016年度第一批省投资高标准基本农田建设项目立项的合同-新安、太平.pdf",
                FileSize = "5735 KB ",
                FileType = UploadFilePageType.合同资料,
                Number = 1,
                OperatorId = "123",
                OperatorName = "肖燕",
                Remark = "招标代理合同"
            };

            return View(result);
        }

        public ActionResult Test()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Test(string content)
        {
            return View();
        }

        /// <summary>
        /// 创建招标公告
        /// </summary>
        /// <returns></returns>
        public ActionResult TenderNotice(int id)
        {
            int applicationid = Convert.ToInt32(id);
            var application = ApplicationService.GetEntityById(applicationid);

            if (application == null)
            {
                return RedirectToAction("Error");
            }

            ViewBag.Id = applicationid;
            var bidSections = ProjectBidSectionService.GetEntities(x => x.ApplicationId == applicationid);
            var selectItemList = new List<SelectListItem>()
            {
                //new SelectListItem(){Value="0",Text="全部",Selected=true}
            };
            if (bidSections.Any())
            {
                var selectList = new SelectList(bidSections, "ID", "SectionName");
                selectItemList.AddRange(selectList);
            }

            ViewBag.bidSectionsList = selectItemList;

            var tenderNoticeModel = ApplicationService.GetEntities(p => p.FormID == (int)EngineeringProjectTypeOfFormId.TenderNotice && p.ApplicationId == applicationid).FirstOrDefault();

            // 判断招标公告是否已经存在
            if (tenderNoticeModel == null)
            {
                int formId = (int)EngineeringProjectTypeOfFormId.TenderNotice;
                int workflowId = (int)TypeOfWorkFlowId.Application;

                #region 数据准备

                ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();
                ERPNWorkFlowNodeTransferObject currentNode;
                var keyvaluearry = application.FormValues.Split(Constant.SharpChar);
                string projectestablishtime = keyvaluearry[2].ToString();//申请时间
                model.WenHao = application.WenHao;
                model.BeiYong1 = application.BeiYong1;
                model.WorkFlowID = workflowId;
                model.FormID = formId;
                model.ApplicationId = applicationid;

                //加载表单内容
                var temerpnformmodel = ERPNFormService.GetEntityById(formId).ToViewModel();

                //获取当前表单对应的工作数据列
                string[] formItemArray = temerpnformmodel.ItemsList.Split(Constant.SplitChar);
                string content = temerpnformmodel.ContentStr;
                //if (content.Contains("Text397670573") && content.Contains("Text1900333495"))//项目名称
                //{
                //    string oldvalue1 = "Text397670573";
                //    string newvalue1 = @"Text397670573"" value=""" + name + @"""";
                //    content = content.Replace(oldvalue1, newvalue1);

                //    string oldvalue2 = "Text1900333495";
                //    string newvalue2 = @"Text1900333495"" value=""" + projectname + @"""";
                //    content = content.Replace(oldvalue2, newvalue2);
                //}
                model.FormContent = content;

                //绑定工作名称
                var temperpnworkflowmodel = ERPNWorkFlowService.GetEntityById(workflowId);
                model.WorkName = User.Identity.Name + "--" + temperpnworkflowmodel.WorkFlowName;

                ////绑定工作名称
                var workFlowModel = ERPNWorkFlowService.GetEntityById(workflowId);
                model.WorkName = User.Identity.Name + Constant.DoubleHyphenString + workFlowModel.WorkFlowName;
                //绑定下一节点
                currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == workflowId && p.NodeAddr == Constant.MacroStartString).First();
                model.JieDianID = currentNode.ID;
                model.JieDianName = currentNode.NodeName;

                //批量设置字段的可写、保密属性
                //ViewBag.PiLiangSet = CommonHelper.SetTheWriteAndHiddenField(currentNode.CanWriteSet, currentNode.SecretSet, formItemArray);

                #endregion

                return View(model);
            }
            else
            {
                return View(tenderNoticeModel.ToViewModel());
            }
        }

        /// <summary>
        /// 创建招标公告
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateTenderNotice(int sectionId, int applicationId)
        {
            var application = ApplicationService.GetEntityById(applicationId);

            if (application == null)
            {
                return RedirectToAction("Error");
            }

            var section = ProjectBidSectionService.GetEntityById(sectionId);
            if (section == null)
            {
                return RedirectToAction("Error");
            }

            //ViewBag.Id = applicationId;
            //var bidSections = ProjectBidSectionService.GetEntities(x => x.ApplicationId == applicationId);
            //var selectItemList = new List<SelectListItem>()
            //{
            //    //new SelectListItem(){Value="0",Text="全部",Selected=true}
            //};
            //if (bidSections.Any())
            //{
            //    var selectList = new SelectList(bidSections, "ID", "SectionName");
            //    selectItemList.AddRange(selectList);
            //}

            //ViewBag.bidSectionsList = selectItemList;
            ViewBag.SectionNumber = section.SectionNumber;
            ViewBag.SectionName = section.SectionName;
            ViewBag.Id = section.ID; 
             var tenderNoticeModel = ApplicationService.GetEntities(p => p.FormID == (int)EngineeringProjectTypeOfFormId.TenderNotice && p.ApplicationId == sectionId).FirstOrDefault();

            // 判断招标公告是否已经存在
            if (tenderNoticeModel == null)
            {
                int formId = (int)EngineeringProjectTypeOfFormId.TenderNotice;
                int workflowId = (int)TypeOfWorkFlowId.Application;

                #region 数据准备

                ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();
                ERPNWorkFlowNodeTransferObject currentNode;
                var keyvaluearry = application.FormValues.Split(Constant.SharpChar);
                string projectestablishtime = keyvaluearry[2].ToString();//申请时间
                model.WenHao = application.WenHao;
                model.BeiYong1 = application.BeiYong1;
                model.WorkFlowID = workflowId;
                model.FormID = formId;
                model.ApplicationId = sectionId;

                //加载表单内容
                var temerpnformmodel = ERPNFormService.GetEntityById(formId).ToViewModel();

                //获取当前表单对应的工作数据列
                string[] formItemArray = temerpnformmodel.ItemsList.Split(Constant.SplitChar);
                string content = temerpnformmodel.ContentStr;
                //if (content.Contains("Text397670573") && content.Contains("Text1900333495"))//项目名称
                //{
                //    string oldvalue1 = "Text397670573";
                //    string newvalue1 = @"Text397670573"" value=""" + name + @"""";
                //    content = content.Replace(oldvalue1, newvalue1);

                //    string oldvalue2 = "Text1900333495";
                //    string newvalue2 = @"Text1900333495"" value=""" + projectname + @"""";
                //    content = content.Replace(oldvalue2, newvalue2);
                //}
                model.FormContent = content;

                //绑定工作名称
                var temperpnworkflowmodel = ERPNWorkFlowService.GetEntityById(workflowId);
                model.WorkName = User.Identity.Name + "--" + temperpnworkflowmodel.WorkFlowName;

                ////绑定工作名称
                var workFlowModel = ERPNWorkFlowService.GetEntityById(workflowId);
                model.WorkName = User.Identity.Name + Constant.DoubleHyphenString + workFlowModel.WorkFlowName;
                //绑定下一节点
                currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == workflowId && p.NodeAddr == Constant.MacroStartString).First();
                model.JieDianID = currentNode.ID;
                model.JieDianName = currentNode.NodeName;

                //批量设置字段的可写、保密属性
                //ViewBag.PiLiangSet = CommonHelper.SetTheWriteAndHiddenField(currentNode.CanWriteSet, currentNode.SecretSet, formItemArray);

                #endregion

                return View(model);
            }
            else
            {
                return View(tenderNoticeModel.ToViewModel());
            }
        }

        /// <summary>
        /// 保存招标公告
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateTenderNotice(string flag, ERPNWorkToDoViewModel model, FormCollection collection)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.UserName = User.Identity.Name;
            model.TimeStr = DateTime.Now;

            var sectionName = MySession[SessionKeyEnum.SectionName].ToString();
            try
            {
                model.StateNow = "正在办理";
            }
            catch
            {
                model.JieDianName = "结束";
                model.StateNow = "强制结束";
            }

            model.OKUserList = "默认";

            int jiedianid = Convert.ToInt32(model.JieDianID);
            var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
            model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));

            //// 更改当前结点id和name
            //var currentNode = ERPNWorkFlowNodeService.GetEntityById(model.JieDianID.Value);
            //string nextNodeSerial = currentNode.NextNode;
            //var nextNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.NodeSerils == nextNodeSerial && p.WorkFlowID == model.WorkFlowID).ToList();
            //model.JieDianID = nextNodeModel.First().ID;
            //model.JieDianName = nextNodeModel.First().NodeName;

            //页面加载后就上报,就添加新上报的数据行
            if (model.NWorkToDoID == 0)
            {
                model.ApplicationStatus = BiddingProjectStatus.TenderNotice.ToString();
                model.ProjectStatus = BiddingProjectStatus.TenderNotice.ToString();
                //上报时设置保存为false、驳回为false、删除为false、冻结为false
                model.IsTemporary = true;
                model.IsLocked = false;
                model.IsDeleted = false;
                model.IsRejected = false;

                int nworktodoid = this.ApplicationService.AddApplication(model.ToDataTransferObjectModel(), model.BidSectionList.Select(x => x.ConvertTo<ProjectBidSection>()).ToList());

                //写系统日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();
                //已办
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.DoSomething = User.Identity.Name + "上报(工程项目申请书)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                // MyRiZhi.NotificationContent = "添加的" + model.WenHao + "已经提交";
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.TenderNotice.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = nworktodoid.ToString();
                MyRiZhi.TimeStr = DateTime.Now;
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                int AddRiZhiSuccess = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //待办
                MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkFlowID), sectionName);

                MyRiZhi1.DoSomething = "需要审批(" + User.Identity.Name + "上报的工程项目申请书)";
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                // MyRiZhi1.NotificationContent = "上报的" + model.WenHao + "需要审批";
                MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.TenderNotice.ToString();
                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.FKApplicationID = nworktodoid.ToString();
                MyRiZhi1.TimeStr = DateTime.Now;
                MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());
                return Json(AddRiZhiSuccess, JsonRequestBehavior.AllowGet);
            }
            //先保存在上报的,就更新保存数据行的IsIsTemporary
            else
            {
                model.ApplicationStatus = BiddingProjectStatus.TenderNotice.ToString();
                model.ProjectStatus = BiddingProjectStatus.TenderNotice.ToString();
                //上报时设置保存为false、驳回为false、删除为false、冻结为false
                model.IsTemporary = true;
                model.IsLocked = false;
                model.IsDeleted = false;
                model.IsRejected = false;

                bool UpdateIstemporySuccess = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());
                //todo:add rizhi

                //更新已办，新增上报，写系统日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();
                //更新已办
                string nworktodoid = model.NWorkToDoID.ToString();
                string formname = EngineeringProjectTypeOfFormId.TenderNotice.ToString();
                MyRiZhi = ERPRiZhiService.GetEntities(p => p.FKApplicationID == nworktodoid && p.FkFormName == formname).FirstOrDefault().ToViewModel();
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.DoSomething = User.Identity.Name + "上报的(工程项目申请书)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                // MyRiZhi.NotificationContent = "添加的" + model.WenHao + "已经提交";
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.TenderNotice.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi.TimeStr = DateTime.Now;
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                bool updateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //新增待办
                MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkFlowID), sectionName);
                MyRiZhi1.DoSomething = "需要审批(" + User.Identity.Name + "上报的工程项目申请书)";
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                // MyRiZhi1.NotificationContent = "上报的" + model.WenHao + "需要审批";
                MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.TenderNotice.ToString();
                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi1.TimeStr = DateTime.Now;
                MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();
                int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                return Json(AddRiZhi1Success, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 保存招标公告
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult TenderNotice(string flag, ERPNWorkToDoViewModel model, FormCollection collection)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.UserName = User.Identity.Name;
            model.TimeStr = DateTime.Now;

            var sectionName = MySession[SessionKeyEnum.SectionName].ToString();
            try
            {
                model.StateNow = "正在办理";
            }
            catch
            {
                model.JieDianName = "结束";
                model.StateNow = "强制结束";
            }

            model.OKUserList = "默认";

            int jiedianid = Convert.ToInt32(model.JieDianID);
            var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
            model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));

            //// 更改当前结点id和name
            //var currentNode = ERPNWorkFlowNodeService.GetEntityById(model.JieDianID.Value);
            //string nextNodeSerial = currentNode.NextNode;
            //var nextNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.NodeSerils == nextNodeSerial && p.WorkFlowID == model.WorkFlowID).ToList();
            //model.JieDianID = nextNodeModel.First().ID;
            //model.JieDianName = nextNodeModel.First().NodeName;

            //页面加载后就上报,就添加新上报的数据行
            if (model.NWorkToDoID == 0)
            {
                model.ApplicationStatus = BiddingProjectStatus.TenderNotice.ToString();
                model.ProjectStatus = BiddingProjectStatus.TenderNotice.ToString();
                //上报时设置保存为false、驳回为false、删除为false、冻结为false
                model.IsTemporary = true;
                model.IsLocked = false;
                model.IsDeleted = false;
                model.IsRejected = false;

                int nworktodoid = this.ApplicationService.AddApplication(model.ToDataTransferObjectModel(), model.BidSectionList.Select(x => x.ConvertTo<ProjectBidSection>()).ToList());

                //写系统日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();
                //已办
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.DoSomething = User.Identity.Name + "上报(工程项目申请书)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                // MyRiZhi.NotificationContent = "添加的" + model.WenHao + "已经提交";
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.TenderNotice.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = nworktodoid.ToString();
                MyRiZhi.TimeStr = DateTime.Now;
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                int AddRiZhiSuccess = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //待办
                MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkFlowID), sectionName);

                MyRiZhi1.DoSomething = "需要审批(" + User.Identity.Name + "上报的工程项目申请书)";
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                // MyRiZhi1.NotificationContent = "上报的" + model.WenHao + "需要审批";
                MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.TenderNotice.ToString();
                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.FKApplicationID = nworktodoid.ToString();
                MyRiZhi1.TimeStr = DateTime.Now;
                MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());
                return Json(AddRiZhiSuccess, JsonRequestBehavior.AllowGet);
            }
            //先保存在上报的,就更新保存数据行的IsIsTemporary
            else
            {
                model.ApplicationStatus = BiddingProjectStatus.TenderNotice.ToString();
                model.ProjectStatus = BiddingProjectStatus.TenderNotice.ToString();
                //上报时设置保存为false、驳回为false、删除为false、冻结为false
                model.IsTemporary = true;
                model.IsLocked = false;
                model.IsDeleted = false;
                model.IsRejected = false;

                bool UpdateIstemporySuccess = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());
                //todo:add rizhi

                //更新已办，新增上报，写系统日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();
                //更新已办
                string nworktodoid = model.NWorkToDoID.ToString();
                string formname = EngineeringProjectTypeOfFormId.TenderNotice.ToString();
                MyRiZhi = ERPRiZhiService.GetEntities(p => p.FKApplicationID == nworktodoid && p.FkFormName == formname).FirstOrDefault().ToViewModel();
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.DoSomething = User.Identity.Name + "上报的(工程项目申请书)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                // MyRiZhi.NotificationContent = "添加的" + model.WenHao + "已经提交";
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.TenderNotice.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi.TimeStr = DateTime.Now;
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                bool updateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //新增待办
                MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkFlowID), sectionName);
                MyRiZhi1.DoSomething = "需要审批(" + User.Identity.Name + "上报的工程项目申请书)";
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                // MyRiZhi1.NotificationContent = "上报的" + model.WenHao + "需要审批";
                MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.TenderNotice.ToString();
                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi1.TimeStr = DateTime.Now;
                MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();
                int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                return Json(AddRiZhi1Success, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 招标公告列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult TenderNoticeList(int id)
        {
            ViewBag.ApplicationId = id;
            ViewBag.Id = id;
            return View();
        }
        /// <summary>
        /// 招标公告列表
        /// </summary>
        /// <returns></returns>
        public ActionResult AllTenderNoticeList()
        {
            ViewBag.Module = "工程项目";
            ViewBag.Title = "招标公告";
            return View();
        }
        
        // 招标公告列表
        public ActionResult GetBidSectionListByApplicationId(int applicationId)
        {
            var application = ApplicationService.GetEntityById(applicationId);

            if (application == null)
            {
                return Content(@"<script type='text/javascript'>alert('项目不存在！'); </script> ");
            }

            var bidSections = ProjectBidSectionService.GetEntities(x => x.ApplicationId == applicationId);

            if (bidSections.Any())
            {
                var result = bidSections.Select(x => x.ConvertTo<ProjectBidSectionViewModel>()).ToList();

                if (result.Any())
                {
                    //序号
                    int number = 1;
                    foreach (var item in result)
                    {
                        item.Number = number;
                        item.ProjectName = application.WenHao;
                        number++;
                    }
                }

                return Json(new { data = result, total = result.Count() }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { data = new List<ProjectBidSectionViewModel> { }, total = 0 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        /// <summary>
        /// 申请书列表查询
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="projectName">项目名称</param>
        /// <param name="state">状态</param>
        /// <param name="freeze">是否冻结</param>
        /// <returns></returns>
        public ActionResult TenderNoticeListStatistics(string projectName, string State, string projectNumber, string startTime, string endTime, int pageIndex, int pageSize)
        {
            DateTime start, end;
            int totalPage = 0;
            if (!string.IsNullOrEmpty(endTime))
            {
                end = Convert.ToDateTime(endTime);
                end = end.AddDays(1);
            }
            else
            {
                end = Constant.MaxTime;
            }

            if (!string.IsNullOrEmpty(startTime))
            {
                start = Convert.ToDateTime(startTime);
            }
            else
            {
                start = Constant.MinTime;
            }

            IEnumerable<ERPNWorkToDoTransferObject> result = SearchProcessingApplicationList(projectName, State, projectNumber, start,
                end, pageSize, pageIndex, ref totalPage);

            bool hasRolesFlag = HasRolesFlag();
            IEnumerable<ERPNWorkToDoTransferObject> resultpage;
            int totalcount = 0;
            //非普通用户
            if (hasRolesFlag)
            {
                resultpage = ApplicationService.GetEntities(p => p.FormID == (int)EngineeringProjectTypeOfFormId.Application
                    && p.TimeStr.Value > start
                    && p.TimeStr.Value < end
                    && ((State == Constant.All) ? true : p.ApplicationStatus == State)
                    && (string.IsNullOrEmpty(projectName) ? true : (p.WenHao.Contains(projectName)))
                    && p.StateNow == "正在办理"
                    && (string.IsNullOrEmpty(projectNumber) ? true : (p.BeiYong1.Contains(projectNumber)))
                    && p.ProjectStatus != ApplicationStatus.BigProjectProcessing.ToString());
                totalcount = resultpage.Count();
            }
            //普通用户
            else
            {
                resultpage = ApplicationService.GetEntities(p => p.FormID == (int)EngineeringProjectTypeOfFormId.Application
                    && p.TimeStr.Value > start
                    && p.TimeStr.Value < end
                    && ((State == Constant.All) ? true : p.ApplicationStatus == State)
                    && (string.IsNullOrEmpty(projectName) ? true : (p.WenHao.Contains(projectName)))
                    && p.StateNow == "正在办理"
                    && p.UserName == User.Identity.Name
                    && (string.IsNullOrEmpty(projectNumber) ? true : (p.BeiYong1.Contains(projectNumber)))
                    && p.ProjectStatus != ApplicationStatus.BigProjectProcessing.ToString());
                totalcount = resultpage.Count();
            }
            return Json(new { data = result, total = totalcount }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 存档资料申请书
        /// </summary>
        /// <returns></returns>

        public ActionResult ArchiveDocument(int applicationId)
        {
            var application = ApplicationService.GetEntityById(applicationId);

            if (application == null)
            {
                return RedirectToAction("Error");
            }

            var tenderNoticeModel = ApplicationService.GetEntities(p => p.FormID == (int)EngineeringProjectTypeOfFormId.ArchiveDocument && p.ApplicationId == applicationId).FirstOrDefault();

            ViewBag.ApplicationId = applicationId;
            ViewBag.Id = applicationId;
            // 判断存档资料是否已经存在
            if (tenderNoticeModel == null)
            {
                int formId = (int)EngineeringProjectTypeOfFormId.ArchiveDocument;
                int workflowId = (int)TypeOfWorkFlowId.Application;

                #region 数据准备

                ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();
                ERPNWorkFlowNodeTransferObject currentNode;
                var keyvaluearry = application.FormValues.Split(Constant.SharpChar);
                string projectestablishtime = keyvaluearry[2].ToString();//申请时间
                model.WenHao = application.WenHao;
                model.BeiYong1 = application.BeiYong1;
                model.WorkFlowID = workflowId;
                model.FormID = formId;
                model.ApplicationId = applicationId;

                //加载表单内容
                var temerpnformmodel = ERPNFormService.GetEntityById(formId).ToViewModel();

                //获取当前表单对应的工作数据列
                string[] formItemArray = temerpnformmodel.ItemsList.Split(Constant.SplitChar);
                string content = temerpnformmodel.ContentStr;
                //if (content.Contains("Text397670573") && content.Contains("Text1900333495"))//项目名称
                //{
                //    string oldvalue1 = "Text397670573";
                //    string newvalue1 = @"Text397670573"" value=""" + name + @"""";
                //    content = content.Replace(oldvalue1, newvalue1);

                //    string oldvalue2 = "Text1900333495";
                //    string newvalue2 = @"Text1900333495"" value=""" + projectname + @"""";
                //    content = content.Replace(oldvalue2, newvalue2);
                //}
                model.FormContent = content;

                //绑定工作名称
                var temperpnworkflowmodel = ERPNWorkFlowService.GetEntityById(workflowId);
                model.WorkName = User.Identity.Name + "--" + temperpnworkflowmodel.WorkFlowName;

                ////绑定工作名称
                var workFlowModel = ERPNWorkFlowService.GetEntityById(workflowId);
                model.WorkName = User.Identity.Name + Constant.DoubleHyphenString + workFlowModel.WorkFlowName;
                //绑定下一节点
                currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == workflowId && p.NodeAddr == Constant.MacroStartString).First();
                model.JieDianID = currentNode.ID;
                model.JieDianName = currentNode.NodeName;

                //批量设置字段的可写、保密属性
                //ViewBag.PiLiangSet = CommonHelper.SetTheWriteAndHiddenField(currentNode.CanWriteSet, currentNode.SecretSet, formItemArray);

                #endregion
                return View(model);
            }
            else
            {
                return View(tenderNoticeModel.ToViewModel());
            }
        }

        /// <summary>
        /// 提交存档资料申请书
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ArchiveDocument(string flag, ERPNWorkToDoViewModel model, FormCollection collection)
        {
            ///验证
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.UserName = User.Identity.Name;
            model.TimeStr = DateTime.Now;

            var sectionName = MySession[SessionKeyEnum.SectionName].ToString();

            //加载表单内容
            var temerpnformmodel = ERPNFormService.GetEntityById((int)EngineeringProjectTypeOfFormId.ArchiveDocument).ToViewModel();

            var itemList = temerpnformmodel.ItemsList;
            string formKeys, formvalues;
            Hepler.GetFormKeyValueAndRemark(collection, itemList, out formKeys, out formvalues);
            model.FormContent = temerpnformmodel.ContentStr;
            model.FormContent = model.FormContent.Replace(Constant.MacroSectionString, sectionName);
            model.FormContent = model.FormContent.Replace(Constant.MacroUserNameString, User.Identity.Name);
            model.FormContent = FormContentReplaceHelper.ReplaceFormContentValue(model.FormContent, collection);

            try
            {
                model.StateNow = "正在办理";
            }
            catch
            {
                model.JieDianName = "结束";
                model.StateNow = "强制结束";
            }

            model.OKUserList = "默认";

            int jiedianid = Convert.ToInt32(model.JieDianID);
            var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
            model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));
            model.FormKeys = formKeys;
            model.FormValues = formvalues;

            //// 更改当前结点id和name
            //var currentNode = ERPNWorkFlowNodeService.GetEntityById(model.JieDianID.Value);
            //string nextNodeSerial = currentNode.NextNode;
            //var nextNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.NodeSerils == nextNodeSerial && p.WorkFlowID == model.WorkFlowID).ToList();
            //model.JieDianID = nextNodeModel.First().ID;
            //model.JieDianName = nextNodeModel.First().NodeName;

            //页面加载后就上报,就添加新上报的数据行
            if (model.NWorkToDoID == 0)
            {
                model.ApplicationStatus = BiddingProjectStatus.Archived.ToString();
                model.ProjectStatus = BiddingProjectStatus.Archived.ToString();
                //上报时设置保存为false、驳回为false、删除为false、冻结为false
                model.IsTemporary = true;
                model.IsLocked = false;
                model.IsDeleted = false;
                model.IsRejected = false;

                int nworktodoid = this.ApplicationService.AddApplication(model.ToDataTransferObjectModel(), model.BidSectionList.Select(x => x.ConvertTo<ProjectBidSection>()).ToList());

                //写系统日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();
                //已办
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.DoSomething = User.Identity.Name + "上报(政府采购申请书)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                // MyRiZhi.NotificationContent = "添加的" + model.WenHao + "已经提交";
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.ArchiveDocument.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = nworktodoid.ToString();
                MyRiZhi.TimeStr = DateTime.Now;
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                int AddRiZhiSuccess = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //待办
                MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkFlowID), sectionName);

                MyRiZhi1.DoSomething = "需要审批(" + User.Identity.Name + "上报的政府采购申请书)";
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                // MyRiZhi1.NotificationContent = "上报的" + model.WenHao + "需要审批";
                MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.ArchiveDocument.ToString();
                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.FKApplicationID = nworktodoid.ToString();
                MyRiZhi1.TimeStr = DateTime.Now;
                MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());
                return Json(AddRiZhiSuccess, JsonRequestBehavior.AllowGet);
            }
            //先保存在上报的,就更新保存数据行的IsIsTemporary
            else
            {
                model.ApplicationStatus = BiddingProjectStatus.Archived.ToString();
                model.ProjectStatus = BiddingProjectStatus.Archived.ToString();
                //上报时设置保存为false、驳回为false、删除为false、冻结为false
                model.IsTemporary = true;
                model.IsLocked = false;
                model.IsDeleted = false;
                model.IsRejected = false;

                bool UpdateIstemporySuccess = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());
                //todo:add rizhi

                //更新已办，新增上报，写系统日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();
                //更新已办
                string nworktodoid = model.NWorkToDoID.ToString();
                string formname = EngineeringProjectTypeOfFormId.ArchiveDocument.ToString();
                MyRiZhi = ERPRiZhiService.GetEntities(p => p.FKApplicationID == nworktodoid && p.FkFormName == formname).FirstOrDefault().ToViewModel();
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.DoSomething = User.Identity.Name + "上报的(政府采购申请书)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                // MyRiZhi.NotificationContent = "添加的" + model.WenHao + "已经提交";
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.ArchiveDocument.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi.TimeStr = DateTime.Now;
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                bool updateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //新增待办
                MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkFlowID), sectionName);
                MyRiZhi1.DoSomething = "需要审批(" + User.Identity.Name + "上报的政府采购申请书)";
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                // MyRiZhi1.NotificationContent = "上报的" + model.WenHao + "需要审批";
                MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.ArchiveDocument.ToString();
                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi1.TimeStr = DateTime.Now;
                MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();
                int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                return Json(AddRiZhi1Success, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 存档资料列表
        /// </summary>
        /// <returns></returns>
        public ActionResult AllArchiveDocumentList()
        {
            ViewBag.Module = "政府采购";
            ViewBag.Title = "存档资料";
            return View();
        }

        [HttpPost]
        /// <summary>
        /// 存档资料列表查询
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="projectName">项目名称</param>
        /// <param name="state">状态</param>
        /// <param name="freeze">是否冻结</param>
        /// <returns></returns>
        public ActionResult ArchiveDocumentListStatistics(string projectName, string State, string projectNumber, string startTime, string endTime, int pageIndex, int pageSize)
        {
            DateTime start, end;
            int totalPage = 0;
            if (!string.IsNullOrEmpty(endTime))
            {
                end = Convert.ToDateTime(endTime);
                end = end.AddDays(1);
            }
            else
            {
                end = Constant.MaxTime;
            }

            if (!string.IsNullOrEmpty(startTime))
            {
                start = Convert.ToDateTime(startTime);
            }
            else
            {
                start = Constant.MinTime;
            }

            IEnumerable<ERPNWorkToDoTransferObject> result = SearchArchiveDocumentList(projectName, State, projectNumber, start,
                end, pageSize, pageIndex, ref totalPage);

            bool hasRolesFlag = HasRolesFlag();
            IEnumerable<ERPNWorkToDoTransferObject> resultpage;
            int totalcount = 0;
            //非普通用户
            if (hasRolesFlag)
            {
                resultpage = ApplicationService.GetEntities(p => p.FormID == (int)EngineeringProjectTypeOfFormId.ArchiveDocument
                    && p.TimeStr.Value > start
                    && p.TimeStr.Value < end
                    && ((State == Constant.All) ? true : p.ApplicationStatus == State)
                    && (string.IsNullOrEmpty(projectName) ? true : (p.WenHao.Contains(projectName)))
                    && p.StateNow == "正在办理"
                    && (string.IsNullOrEmpty(projectNumber) ? true : (p.BeiYong1.Contains(projectNumber)))
                    && p.ProjectStatus != ApplicationStatus.BigProjectProcessing.ToString());
                totalcount = resultpage.Count();
            }
            //普通用户
            else
            {
                resultpage = ApplicationService.GetEntities(p => p.FormID == (int)EngineeringProjectTypeOfFormId.ArchiveDocument
                    && p.TimeStr.Value > start
                    && p.TimeStr.Value < end
                    && ((State == Constant.All) ? true : p.ApplicationStatus == State)
                    && (string.IsNullOrEmpty(projectName) ? true : (p.WenHao.Contains(projectName)))
                    && p.StateNow == "正在办理"
                    && p.UserName == User.Identity.Name
                    && (string.IsNullOrEmpty(projectNumber) ? true : (p.BeiYong1.Contains(projectNumber)))
                    && p.ProjectStatus != ApplicationStatus.BigProjectProcessing.ToString());
                totalcount = resultpage.Count();
            }
            return Json(new { data = result, total = totalcount }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 提交项目前期
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PreSubmitApplication(string flag, ERPNWorkToDoViewModel model, FormCollection collection)
        {
            ///验证
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.UserName = User.Identity.Name;
            model.TimeStr = DateTime.Now;

            var sectionName = MySession[SessionKeyEnum.SectionName].ToString();

            //加载表单内容
            var temerpnformmodel = ERPNFormService.GetEntityById((int)EngineeringProjectTypeOfFormId.PreApplication).ToViewModel();

            var itemList = temerpnformmodel.ItemsList;
            string formKeys, formvalues;
            Hepler.GetFormKeyValueAndRemark(collection, itemList, out formKeys, out formvalues);
            model.FormContent = temerpnformmodel.ContentStr;
            model.FormContent = model.FormContent.Replace(Constant.MacroSectionString, sectionName);
            model.FormContent = model.FormContent.Replace(Constant.MacroUserNameString, User.Identity.Name);
            model.FormContent = FormContentReplaceHelper.ReplaceFormContentValue(model.FormContent, collection);

            int countOfTravelItems;

            if (!string.IsNullOrEmpty(Request["count"]))
            {
                countOfTravelItems = Convert.ToInt32(Request["count"]);
            }
            else
            {
                countOfTravelItems = 0;
            }

            string[] itemNameArray = { "SectionName", "SectionNumber", "CreatedTime" };

            for (int i = 1; i < countOfTravelItems + 1; i++)
            {
                var item = new ProjectBidSectionViewModel();
                item.SectionName = Request["item" + i.ToString() + itemNameArray[0]];
                item.SectionNumber = Request["item" + i.ToString() + itemNameArray[1]];
                item.CreatedTime = Convert.ToDateTime(Request["item" + i.ToString() + itemNameArray[2]]);
                model.BidSectionList.Add(item);
            }

            try
            {
                model.StateNow = "正在办理";
            }
            catch
            {
                model.JieDianName = "结束";
                model.StateNow = "强制结束";
            }

            model.OKUserList = "默认";

            int jiedianid = Convert.ToInt32(model.JieDianID);
            var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
            model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));
            model.FormKeys = formKeys;
            model.FormValues = formvalues;

            if (flag == "Save")//保存
            {
                model.IsTemporary = true;
                model.IsLocked = false;
                model.IsDeleted = false;
                model.IsRejected = false;

                //申请书状态
                model.ApplicationStatus = BiddingProjectStatus.ProjectRegitering.ToString();
                //整个项目进行的状态
                model.ProjectStatus = BiddingProjectStatus.ProjectRegitering.ToString();
                int nworktodoid = this.ApplicationService.AddApplication(model.ToDataTransferObjectModel(), model.BidSectionList.Select(x => x.ConvertTo<ProjectBidSection>()).ToList());
                //todo:日志

                //写系统日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                //已办
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.DoSomething = "保存(政府采购申请书)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.Application.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = nworktodoid.ToString();
                int AddRiZhiSuccess = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                model.NWorkToDoID = nworktodoid;
                /// return RedirectToAction("SubmitApplication", new { id = model.NWorkToDoID });
                return Json(AddRiZhiSuccess, JsonRequestBehavior.AllowGet);
            }
            else//上报
            {
                // 更改当前结点id和name
                var currentNode = ERPNWorkFlowNodeService.GetEntityById(model.JieDianID.Value);
                string nextNodeSerial = currentNode.NextNode;
                var nextNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.NodeSerils == nextNodeSerial && p.WorkFlowID == model.WorkFlowID).ToList();
                model.JieDianID = nextNodeModel.First().ID;
                model.JieDianName = nextNodeModel.First().NodeName;

                //页面加载后就上报,就添加新上报的数据行
                if (model.NWorkToDoID == 0)
                {
                    model.ApplicationStatus = BiddingProjectStatus.ProjectRegitered.ToString();
                    model.ProjectStatus = BiddingProjectStatus.ProjectRegitered.ToString();
                    //上报时设置保存为false、驳回为false、删除为false、冻结为false
                    model.IsTemporary = true;
                    model.IsLocked = false;
                    model.IsDeleted = false;
                    model.IsRejected = false;

                    int nworktodoid = this.ApplicationService.AddApplication(model.ToDataTransferObjectModel(), model.BidSectionList.Select(x => x.ConvertTo<ProjectBidSection>()).ToList());

                    //写系统日志
                    ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                    ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();
                    //已办
                    MyRiZhi.UserName = User.Identity.Name;
                    MyRiZhi.DoSomething = User.Identity.Name + "上报(政府采购申请书)";
                    MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    // MyRiZhi.NotificationContent = "添加的" + model.WenHao + "已经提交";
                    MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.Application.ToString();
                    MyRiZhi.FKAction = "已办";
                    MyRiZhi.FKApplicationID = nworktodoid.ToString();
                    MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                    int AddRiZhiSuccess = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                    //待办
                    MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkFlowID), sectionName);

                    MyRiZhi1.DoSomething = "需要审批(" + User.Identity.Name + "上报的政府采购申请书)";
                    MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    // MyRiZhi1.NotificationContent = "上报的" + model.WenHao + "需要审批";
                    MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.Application.ToString();
                    MyRiZhi1.FKAction = "待办";
                    MyRiZhi1.FKApplicationID = nworktodoid.ToString();
                    MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                    int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());
                    return Json(AddRiZhiSuccess, JsonRequestBehavior.AllowGet);
                }
                //先保存在上报的,就更新保存数据行的IsIsTemporary
                else
                {
                    model.ApplicationStatus = BiddingProjectStatus.ProjectRegitered.ToString();
                    model.ProjectStatus = BiddingProjectStatus.ProjectRegitered.ToString();
                    //上报时设置保存为false、驳回为false、删除为false、冻结为false
                    model.IsTemporary = true;
                    model.IsLocked = false;
                    model.IsDeleted = false;
                    model.IsRejected = false;

                    bool UpdateIstemporySuccess = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());
                    //todo:add rizhi

                    //更新已办，新增上报，写系统日志
                    ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                    ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();
                    //更新已办
                    string nworktodoid = model.NWorkToDoID.ToString();
                    string formname = EngineeringProjectTypeOfFormId.Application.ToString();
                    MyRiZhi = ERPRiZhiService.GetEntities(p => p.FKApplicationID == nworktodoid && p.FkFormName == formname).FirstOrDefault().ToViewModel();
                    MyRiZhi.UserName = User.Identity.Name;
                    MyRiZhi.DoSomething = User.Identity.Name + "上报的(政府采购申请书)";
                    MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    // MyRiZhi.NotificationContent = "添加的" + model.WenHao + "已经提交";
                    MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.Application.ToString();
                    MyRiZhi.FKAction = "已办";
                    MyRiZhi.FKApplicationID = model.NWorkToDoID.ToString();
                    MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                    bool updateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                    //新增待办
                    MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkFlowID), sectionName);
                    MyRiZhi1.DoSomething = "需要审批(" + User.Identity.Name + "上报的政府采购申请书)";
                    MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    // MyRiZhi1.NotificationContent = "上报的" + model.WenHao + "需要审批";
                    MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.Application.ToString();
                    MyRiZhi1.FKAction = "待办";
                    MyRiZhi1.FKApplicationID = model.NWorkToDoID.ToString();
                    MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();
                    int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                    return Json(AddRiZhi1Success, JsonRequestBehavior.AllowGet);
                }
            }
        }

        /// <summary>
        /// 创建项目前期
        /// </summary>
        /// <returns></returns>
        public ActionResult PreSubmitApplication(string id)
        {
            int formId = (int)EngineeringProjectTypeOfFormId.PreApplication;
            int workflowId = (int)TypeOfWorkFlowId.Application;
            ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();
            ERPNWorkFlowNodeTransferObject currentNode;

            //加载表单内容
            var formModel = ERPNFormService.GetEntityById(formId).ToViewModel();

            //获取当前表单对应的工作数据列
            string[] formItemArray = formModel.ItemsList.Split(Constant.SplitChar);

            if (id != null)//保存或上报后的view
            {
                int nworkid = Convert.ToInt32(id);
                model = ApplicationService.GetEntityById(nworkid).ToViewModel();
                currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.ID == model.JieDianID).First();
            }
            else//新建申请书的view
            {
                model.WorkFlowID = workflowId;
                model.FormID = formId;

                //设置上传的附件为空
                // ZWL.Common.PublicMethod.CheckSession();
                // 自动生成 编号
                //model.BeiYong1 = CommonHelper.GenerateProjectNumber(ApplicationType.ScienceResearch);
                string Number = CommonHelper.GenerateProjectNumber(ApplicationType.ScienceResearch);

                string content = formModel.ContentStr;
                if (content.Contains(""))
                {
                    string oldvalue = "Text309803372";
                    string newvalue = @"Text309803372"" value=""" + Number + @"""";
                    content = content.Replace(oldvalue, newvalue);
                }

                //加载表单内容
                model.FormContent = content;
                model.FormContent = model.FormContent.Replace(Constant.MacroSectionString, MySession[SessionKeyEnum.SectionName].ToString());
                model.FormContent = model.FormContent.Replace(Constant.MacroUserNameString, User.Identity.Name);

                ////绑定工作名称
                var workFlowModel = ERPNWorkFlowService.GetEntityById(workflowId);
                model.WorkName = User.Identity.Name + Constant.DoubleHyphenString + workFlowModel.WorkFlowName;

                //绑定下一节点
                currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == workflowId && p.NodeAddr == Constant.MacroStartString).First();
                model.JieDianID = currentNode.ID;
                model.JieDianName = currentNode.NodeName;
            }

            //批量设置字段的可写、保密属性
            ViewBag.PiLiangSet = CommonHelper.SetTheWriteAndHiddenField(currentNode.CanWriteSet, currentNode.SecretSet, formItemArray);

            return View(model);
        }

        /// <summary>
        /// Get://显示审批申请书
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult PreApplicationAgree(string id, string nextaction)
        {
            ERPNWorkToDoViewModel model = ApplicationService.GetEntityById(Convert.ToInt32(id)).ToViewModel();
            //判断驳回或继续审批
            bool isRejected = Convert.ToBoolean(model.IsRejected);

            ERPNWorkFlowNodeTransferObject currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.ID == model.JieDianID).First();

            //加载表单内容
            var formModel = ERPNFormService.GetEntityById(model.FormID.Value);

            //获取当前表单对应的工作数据列
            string[] formItemArray = formModel.ItemsList.Split(Constant.SplitChar);

            ViewBag.ApprovalStep = model.JieDianName;
            ViewBag.Id = id;
            //批量设置字段的可写、保密属性
            ViewBag.PiLiangSet = CommonHelper.SetTheWriteAndHiddenField(currentNode.CanWriteSet, currentNode.SecretSet, formItemArray);

            // 设置审批流程的所有结点和当前结点
            var list = ERPNWorkFlowNodeService.GetEntities(x => x.WorkFlowID == model.WorkFlowID).OrderBy(p => p.NodeSerils).ToList();
            ViewBag.ApprovalWorkflowNode = list;
            ViewBag.CurrentNodeSerils = currentNode.NodeSerils;
            ViewBag.allCount = list.Count;
            bool isRightRoles = IsRightRoles(id);
            ViewBag.isRightPersonApproval = isRightRoles;
            return View(model);
        }

        /// <summary>
        /// 提交审批申请书
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PreApplicationAgree(string flag, ERPNWorkToDoViewModel model, FormCollection collection)
        {
            string nodeSerils;
            string act = string.Empty;
            //审批
            if (flag == "Approval")
            {
                var attachment = model.FuJianList;
                string PiShiStr = model.ShenPiYiJian;

                // TODO: 审批上传附件
                model.ShenPiYiJian = attachment;
                model.OKUserList = model.OKUserList + "," + User.Identity.Name;

                // TODO:
                if (collection["SingleShenPiYiJian"] != "")
                {
                    //审批意见
                    model.ShenPiYiJian = collection["SingleShenPiYiJian"].ToString();
                }
                else
                {
                    model.ShenPiYiJian = PiShiStr;
                }
                //if (collection["SingleShenPiYiJian"] != "")
                //{
                //    //审批意见
                //    model.ShenPiYiJian = PiShiStr + model.JieDianName + ":" + collection["SingleShenPiYiJian"].ToString() + Constant.SplitChar;
                //}
                //else
                //{
                //    model.ShenPiYiJian = PiShiStr;
                //}

                // 更改当前结点id和name
                var erpnrowkflownoderesult1 = ERPNWorkFlowNodeService.GetEntityById(model.JieDianID.Value).ToViewModel();
                string nextNodeSerial = erpnrowkflownoderesult1.NextNode;
                var nextNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.NodeSerils == nextNodeSerial && p.WorkFlowID == model.WorkFlowID).ToList();

                if (nextNodeModel.Count == Constant.ZERO_INT)
                {
                    // 说明审批流程已经完成，审批通过
                    model.JieDianName = "结束";
                    model.ApplicationStatus = ApplicationStatus.ApplicationApproved.ToString();//申请书审批完成
                    model.ProjectStatus = ApplicationStatus.ApplicationApproved.ToString();
                    bool isUpdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());
                    ViewBag.Title = model.JieDianName;

                    nodeSerils = "结束";
                }
                else
                {
                    // 说明审批流程还未完成，正在等待下一级审批
                    model.ApplicationStatus = ApplicationStatus.ApplicationApproving.ToString();//申请书待审批中 
                    model.ProjectStatus = ApplicationStatus.ApplicationApproving.ToString();
                    //根据序号和workflowID获得下一级节点ID
                    model.JieDianID = nextNodeModel.FirstOrDefault().ID;
                    model.JieDianName = nextNodeModel.FirstOrDefault().NodeName;
                    bool isUpdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());
                    ViewBag.Title = model.JieDianName;

                    //下一个节点的NodeSerils
                    nodeSerils = nextNodeModel.FirstOrDefault().NodeSerils;
                }
            }

            //驳回collection["Reject"]
            else
            {
                act = "rejected";
                model.IsRejected = true;
                model.ApplicationStatus = ApplicationStatus.ApplicationRejected.ToString();//申请书审批完成
                model.ProjectStatus = ApplicationStatus.ApplicationRejected.ToString();
                var workFlowNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == model.WorkFlowID).OrderBy(p => p.NodeSerils).Skip(1).Take(1).FirstOrDefault();

                model.JieDianID = workFlowNodeModel.ID;
                model.JieDianName = workFlowNodeModel.NodeName.ToString();

                bool isUpdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());

                nodeSerils = "驳回";

            }

            string formname = EngineeringProjectTypeOfFormId.Application.ToString();
            var rizhiresult = ERPRiZhiService.GetEntities(p => p.FKApplicationID == model.NWorkToDoID.ToString() && p.FkFormName == formname && p.FKAction == "待办").FirstOrDefault();

            IEnumerable<ERPNWorkFlowNodeViewModel> workFlowNodeModelList = new List<ERPNWorkFlowNodeViewModel>();
            //除填写申请书和第二级审批
            workFlowNodeModelList = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == model.WorkFlowID).OrderBy(p => p.NodeSerils).Skip(2).Select(x => x.ToViewModel());
            //将从第三节点（包括第三节点）的NodeSerials全部装在workFlowNodeModelList
            string nodeSerilsList = "";
            foreach (var item in workFlowNodeModelList)
            {
                nodeSerilsList = nodeSerilsList + item.NodeSerils;
            }
            //判断下一节点是否存在
            if (nodeSerilsList.Contains(nodeSerils))
            {
                //日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();

                //待办变成已办
                MyRiZhi.FKAction = "已办";
                MyRiZhi.ID = rizhiresult.ID;
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.DoSomething = "已审批" + model.UserName + "上报的政府采购申请书";
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.Application.ToString();
                MyRiZhi.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //新生成待办，下一审批人的待办
                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.ID = rizhiresult.ID;
                MyRiZhi1.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.DoSomething = "需要审批(" + model.UserName + "上报的政府采购申请书)";
                MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.Application.ToString();
                MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                ERPNWorkFlowNodeViewModel erpnWorkFlowNodeModel = new ERPNWorkFlowNodeViewModel();
                erpnWorkFlowNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == model.WorkFlowID && p.NodeSerils == nodeSerils).FirstOrDefault().ToViewModel();

                MyRiZhi1.UserName = erpnWorkFlowNodeModel.SPDefaultList;
                int returnid = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());
                //继续审批
                return Json(returnid, JsonRequestBehavior.AllowGet);
            }
            else if (nodeSerils == "结束")
            {
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();

                MyRiZhi.FKAction = "已办";
                MyRiZhi.ID = rizhiresult.ID;
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.DoSomething = "已审批" + model.UserName + "上报的政府采购申请书";
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.Application.ToString();
                MyRiZhi.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //新生成新建项目确立的待办，申请人待办
                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.ID = rizhiresult.ID;
                MyRiZhi1.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.UserName = model.UserName;
                MyRiZhi1.DoSomething = model.UserName + "需要添加项目确立";
                MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.Establishment.ToString();
                MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                int returnid = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());
                // 更新成功并且更新后，申请书状态变为 已经审批通过，跳转到项目确立页面
                return Json(returnid, JsonRequestBehavior.AllowGet);
            }
            //toDoAction == "驳回"
            else
            {
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();

                //审批人已办
                MyRiZhi.FKAction = "已办";
                MyRiZhi.ID = rizhiresult.ID;
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.DoSomething = "已驳回" + model.UserName + "上报的政府采购申请书";
                MyRiZhi.FkFormName = EngineeringProjectTypeOfFormId.Application.ToString();
                MyRiZhi.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                bool UpdateRiZhiSuccess1 = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());
                //申请人待办

                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.ID = rizhiresult.ID;
                MyRiZhi1.UserName = model.UserName;
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.DoSomething = "政府采购申请书被驳回，需修改数据";
                MyRiZhi1.FkFormName = EngineeringProjectTypeOfFormId.Application.ToString();
                MyRiZhi1.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.ScienceResearch.ToString();

                int returnid = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                return Json(returnid, JsonRequestBehavior.AllowGet);
            }
        }
    }
}

