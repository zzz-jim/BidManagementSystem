using ScientificResearch.BusinessLogicImplement;
using ScientificResearch.DataTransferModel;
using ScientificResearch.IBusinessLogic;
using ScientificResearch.Utility.Constants;
using ScientificResearch.Utility.Enums;
using ScientificResearch.Utility.Helper;
using ScientificResearch.ViewModel;
using PF.DomainModel.Identity;
using PF.DomainModel;

using System;
using System.Web.Mvc;
using System.Data;
using System.Collections.Generic;
using System.Linq;

using UI.ScientificResearch.Extensions;
using ScientificResearch.DomainModel;

namespace UI.ScientificResearch.Areas.Education.Controllers
{
    /// <summary>
    /// 继续教育
    /// </summary>
    [CheckLogin]
    [Authorize(Roles = "普通用户")]
    public class ContinuingEducationController : Controller
    {
        #region  private Service
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
        private IContinuingEducationRecordService ContinuingEducationRecordService;

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

        public ContinuingEducationController()
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
                new ContinuingEducationRecordServiceImplement(),
                new SessionManager()
            )
        {
        }

        public ContinuingEducationController(
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
            IContinuingEducationRecordService eContinuingEducationRecordService,
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
            this.ContinuingEducationRecordService = eContinuingEducationRecordService;
            this.MySession = session;

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
        /// 教学管理导航首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Module = "继续教育";
            ViewBag.Title = "简介";

            return View();
        }

        /// <summary>
        /// 教学管理导航首页待办事宜容器页面
        /// </summary>
        /// <returns></returns>
        public ActionResult WorkContainer()
        {
            ViewBag.Module = "教学管理";
            ViewBag.Title = "待办事宜";

            return View();
        }

        /// <summary>
        /// 继教申请书
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult SubmitApplication(string id)
        {
            int formId = (int)ContinuingEducationTypeOfFormId.Application;
            int workflowId = (int)ContinuingEducationTypeOfWorkflowId.Application;

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

                //加载表单内容
                model.FormContent = formModel.ContentStr;
                model.FormContent = model.FormContent.Replace(Constant.MacroSectionString, MySession[SessionKeyEnum.SectionName].ToString());
                // 自动生成 编号
                model.BeiYong1 = CommonHelper.GenerateProjectNumber(ApplicationType.ScienceResearch);

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
        public ActionResult ApplicationAgree(string id, string nextaction)
        {
            var model = ApplicationService.GetEntityById(Convert.ToInt32(id));
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
            var list = ERPNWorkFlowNodeService.GetEntities(x => x.WorkFlowID == model.WorkFlowID).OrderBy(p=>p.NodeSerils).ToList();
            ViewBag.ApprovalWorkflowNode = list;
            ViewBag.CurrentNodeSerils = currentNode.NodeSerils;
            ViewBag.allCount = list.Count;

            bool isRightRoles = IsRightRoles(id);
            ViewBag.isRightPersonApproval = isRightRoles;
            return View(model.ToViewModel());
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
        /// 继教通知
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nextaction"></param>
        /// <returns></returns>
        public ActionResult ContinueNotice(string id, string nextaction)
        {
            int applicationid = Convert.ToInt32(id);
            ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();
            int countOfContinueForm = ApplicationService.GetEntities(p => p.ApplicationId == applicationid && p.FormID == (int)ContinuingEducationTypeOfFormId.Establishment).Count;
            if (countOfContinueForm > 0)
            {
                model = ApplicationService.GetEntities(p => p.ApplicationId == applicationid).FirstOrDefault().ToViewModel();
                model.TimeStr = DateTime.Now;
                ViewBag.Id = id;
                return View(model);
            }

            if (nextaction == Constant.Save)
            {
                model = ApplicationService.GetEntities(p => p.ApplicationId == applicationid).FirstOrDefault().ToViewModel();
                model.TimeStr = DateTime.Now;
                ViewBag.Id = id;
                ViewBag.act = Constant.Save;
                return View(model);

            }
            else
            {
                int formId = (int)ContinuingEducationTypeOfFormId.Establishment;
                int workflowId = (int)ContinuingEducationTypeOfWorkflowId.Establishment;

                ///加载表单内容
                var temerpnformmodel = ERPNFormService.GetEntityById(formId).ToViewModel();

                ///获取当前表单对应的工作数据列
                string[] formItemArray = temerpnformmodel.ItemsList.Split(Constant.SplitChar);

                ViewBag.Id = id;
                ERPNWorkFlowNodeTransferObject currentNode;
                var result = ApplicationService.GetEntities(p => p.ApplicationId == applicationid && p.FormID == (int)ContinuingEducationTypeOfFormId.Establishment).ToList();
                ///已填写了的继教通知
                if (result.Count > 0)
                {
                    model = result.FirstOrDefault().ToViewModel();
                    currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.ID == model.JieDianID).First();

                }
                else
                {
                    var appresult = ApplicationService.GetEntities(p => p.ID == applicationid).ToList();
                    var keyvaluearry = appresult[0].FormValues.Split(Constant.SharpChar);
                    string startTime = keyvaluearry[6].ToString();//举办开始时间
                    string endTime = keyvaluearry[7].ToString();//举办结束时间
                    string projectType = keyvaluearry[2].ToString();//项目类型
                    string projectMan = keyvaluearry[21].ToString();//项目负责人
                    string giveCredit = keyvaluearry[17].ToString();//授予学分级别
                    string credit = keyvaluearry[18].ToString();//学分

                    model.WenHao = appresult[0].WenHao;
                    model.BeiYong1 = appresult[0].BeiYong1;
                    model.WorkFlowID = workflowId;
                    model.FormID = formId;
                    model.ApplicationId = applicationid;

                    string content = temerpnformmodel.ContentStr;
                    if (content.Contains("Text1982658160") && content.Contains("Text746414559") && content.Contains("Text336414556") && content.Contains("Num1486300310") && content.Contains("Date1994547131") && content.Contains("Date72413425"))
                    {
                        string oldvalue = "Date1994547131";
                        string newvalue = @"Date1994547131"" value=""" + startTime + @"""";
                        content = content.Replace(oldvalue, newvalue);

                        string oldvalue1 = "Date72413425";
                        string newvalue1 = @"Date72413425"" value=""" + endTime + @"""";
                        content = content.Replace(oldvalue1, newvalue1);

                        string oldvalue2 = "Text1982658160";
                        string newvalue2 = @"Text1982658160"" value=""" + projectType + @"""";
                        content = content.Replace(oldvalue2, newvalue2);

                        string oldvalue3 = "Text746414559";
                        string newvalue3 = @"Text746414559"" value=""" + projectMan + @"""";
                        content = content.Replace(oldvalue3, newvalue3);

                        string oldvalue4 = "Text336414556";
                        string newvalue4 = @"Text336414556"" value=""" + giveCredit + @"""";
                        content = content.Replace(oldvalue4, newvalue4);

                        string oldvalue5 = "Num1486300310";
                        string newvalue5 = @"Num1486300310"" value=""" + credit + @"""";
                        content = content.Replace(oldvalue5, newvalue5);
                    }
                    model.FormContent = content;

                    ///绑定工作名称
                    var temperpnworkflowmodel = ERPNWorkFlowService.GetEntityById(workflowId);
                    model.WorkName = User.Identity.Name + Constant.DoubleHyphenString + temperpnworkflowmodel.WorkFlowName;
                    ///绑定下一节点
                    currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == workflowId && p.NodeAddr == Constant.MacroStartString).First();
                    model.JieDianID = currentNode.ID;
                    model.JieDianName = currentNode.NodeName;
                }
                ViewBag.PiLiangSet = CommonHelper.SetTheWriteAndHiddenField(currentNode.CanWriteSet, currentNode.SecretSet, formItemArray);
                ViewBag.Id = applicationid;
                return View(model);
            }

        }

        /// <summary>
        /// 人员记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult PersonRecord(string id)
        {
            int NworkToDoId = Convert.ToInt32(id);
            ViewBag.Id = NworkToDoId;
            var result = ContinuingEducationRecordService.GetEntities(p => p.IsProjectCredit == true&&p.NworkToDoId==Convert.ToInt32(id)).OrderByDescending(p => p.Id).ToList();
            IList<ContinuingEducationRecordViewModel> resultlist = new List<ContinuingEducationRecordViewModel>();
            foreach (var item in result)
            {
                resultlist.Add(item.ToViewModel());
            }
            return View(resultlist);
        }

        /// <summary>
        /// 添加人员记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AddPersonRecord(string id)
        {
            var model = ApplicationService.GetEntityById(Convert.ToInt32(id));
            ContinuingEducationRecordViewModel resultmodel = new ContinuingEducationRecordViewModel();
            resultmodel.CreditType = model.FormValues.Split(Constant.SharpChar)[2];
            resultmodel.CreditLevel=model.FormValues.Split(Constant.SharpChar)[17];
            resultmodel.Credit = double.Parse(model.FormValues.Split(Constant.SharpChar)[18]);
            resultmodel.NworkToDoId = model.ID;
            return View(resultmodel);
        }

        /// <summary>
        /// 过程记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nextaction"></param>
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

                int formId = (int)ContinuingEducationTypeOfFormId.ProcessRecord;
                int workflowId = (int)ContinuingEducationTypeOfWorkflowId.ProcessRecord;

                #region 数据准备

                ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();
                ERPNWorkFlowNodeTransferObject currentNode;
                var appresult = ApplicationService.GetEntities(p => p.ID == applicationid).ToList();
                string projectname = workToDoModel.BeiYong1;//课题编号
                string name = workToDoModel.WenHao;//项目名称
                var keyvaluearry = workToDoModel.FormValues.Split(Constant.SharpChar);
                string projectestablishtime = keyvaluearry[4].ToString();//申请时间

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
                    // string newvalue1 = @"" + "Text1009653833" + "\"value=\"" + projectname + @"";
                    string newvalue1 = @"Text397670573"" value=""" + name + @"""";
                    content = content.Replace(oldvalue1, newvalue1);

                    string oldvalue2 = "Text1900333495";
                    // string newvalue2 = "\"" + "Text307349525" + "\"value=\"" + projecttype + "\"";
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
        public ActionResult ProcessAgree(string id, string nextaction)
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

            //model.ApplicationId = Convert.ToInt32(id);
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
        /// 正在审批的过程记录列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ProcessRecordList(string id)
        {
            int applicationid = Convert.ToInt32(id);
            ViewBag.Id = applicationid;
            var result = ApplicationService.GetEntities(p => p.StateNow == Constant.Doing && p.FormID == (int)ContinuingEducationTypeOfFormId.ProcessRecord && p.ApplicationId == applicationid).OrderByDescending(p => p.TimeStr).ToList();

            IList<ERPNWorkToDoViewModel> resultList = new List<ERPNWorkToDoViewModel>();

            foreach (var item in result)
            {
                resultList.Add(item.ToViewModel());
            }

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


            return Json(ReturnReimburseState, JsonRequestBehavior.AllowGet);
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
            if (nextaction == Constant.Save)
            {
                var fundsModel = FundsRecordService.GetAllById(applicaionId);

                var todoModel = ApplicationService.GetEntityById(fundsModel.ApplicationId);

                string projectType = todoModel.FormValues.Split(Constant.SharpChar)[1];
                ViewBag.act = Constant.Save;
                ///获取经费记录类型
                using (var context = new CSPostOAEntities())
                {
                    ViewBag.FundsType = (from h in context.FundsThreshold
                                         where (h.IsDeleted == false && h.ModuleName == ModuleNameOfScienceResearch.ContinuingEducation.ToString() && h.ProjectType == projectType)

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

                string projectType = todoModel.FormValues.Split(Constant.SharpChar)[1];

                ///获取经费记录类型
                using (var context = new CSPostOAEntities())
                {
                    ViewBag.FundsType = (from h in context.FundsThreshold
                                         where (h.IsDeleted == false && h.ModuleName == ModuleNameOfScienceResearch.ContinuingEducation.ToString() && h.ProjectType == projectType)

                                         select new SelectListItem()
                                         {
                                             Text = h.FundsType,
                                             Value = h.FundsType,
                                         }).ToList();
                }
                ERPNWorkFlowNodeTransferObject currentNode;

                int workflowId = (int)ContinuingEducationTypeOfWorkflowId.FeeReimbursement;
                int formId = (int)ContinuingEducationTypeOfFormId.FeeReimbursement;

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
                string[] NextStrList = temperpnworkflownodemodel[0].NextNode.Split(Constant.Comma);
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
        /// </summary>
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

            string projectType = todoModel.FormValues.Split(Constant.SharpChar)[1];
            ///获取经费记录类型
            using (var context = new CSPostOAEntities())
            {
                ViewBag.FundsType = (from h in context.FundsThreshold
                                     where (h.IsDeleted == false && h.ModuleName == ModuleNameOfScienceResearch.ContinuingEducation.ToString() && h.ProjectType == projectType)

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
            // ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();
            TravelFundsRecordViewModel model = new TravelFundsRecordViewModel();
            //string PiLiangSet = "";//批量设置字段的可写、保密属性
            //string Id = "2279";//ERPNWorkToDoViewModel的ID
            // model.GetModel(int.Parse(id));
            int id1 = Convert.ToInt32(id);

            var shenpimodel = FundsRecordService.GetEntityWithTravelDetailListById(id1);
            //var shenpimodel = ApplicationService.GetEntityById(id1);
            //int formId = Convert.ToInt32(shenpimodel.FormID);
            model = shenpimodel.ToViewModel();

            int NowNodeID = Convert.ToInt32(shenpimodel.JieDianID);
            // string NowNodeID = ZWL.DBUtility.DbHelperSQL.GetSHSLInt("select JieDianID from ERPNWorkToDoViewModel where ID=" + id.ToString());
            var MyJieDianNow = ERPNWorkFlowNodeService.GetEntityById(NowNodeID);
            string CanWriteStr = MyJieDianNow.CanWriteSet;
            string SecretStr = MyJieDianNow.SecretSet;

            return View(model);
        }

        /// <summary>
        /// 授予学分
        /// </summary>
        /// <returns></returns>
        public ActionResult GiveCredit(string id)
        {
            int applicationid = Convert.ToInt32(id);
            ViewBag.Id = applicationid;
            var result = ContinuingEducationRecordService.GetEntities(p => p.IsProjectCredit == true && p.NworkToDoId == applicationid).OrderByDescending(p => p.CreatedTime).ToList();

            IList<ContinuingEducationRecordViewModel> resultList = new List<ContinuingEducationRecordViewModel>();

            foreach (var item in result)
            {
                resultList.Add(item.ToViewModel());
            }

            return View(resultList);
        }

        /// <summary>
        /// 学分统计
        /// </summary>
        /// <returns></returns>
        public ActionResult CreditsStatistics()
        {
            ViewBag.Module = "继教申请";
            ViewBag.Title = "学分统计";
            return View();

            //ToDo
            //IList<ContinuingEducationRecordViewModel> resultlist = new List<ContinuingEducationRecordViewModel>();
            //var result = ContinuingEducationRecordService.GetEntities(p => p.Id != null).OrderByDescending(p => p.CreatedTime).ToList();
            //foreach (var item in result)
            //{
            //    resultlist.Add(item.ToViewModel());
            //}
            //return View(resultlist);
        }

        /// <summary>
        /// 奖励设置
        /// </summary>
        /// <returns></returns>
        public ActionResult ProjectBonusCredit()
        {
            var result = ProjectBonusCreditService.GetEntities(p => p.ModuleName == "继续教育").OrderByDescending(p => p.Id);
            IList<ProjectBonusCreditViewModel> resultlist = new List<ProjectBonusCreditViewModel>();
            foreach (var item in result)
            {
                resultlist.Add(item.ToViewModel());
            }

            ViewBag.Module = "继续教育";
            ViewBag.Title = "奖励设置";

            return View(resultlist);
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
        /// 判断已办申请书状态，返回状态
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
        /// 冻结判断
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pagestate"></param>
        /// <param name="random"></param>
        /// <returns></returns>
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
        /// 项目类学分列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ApplicationList()
        {
            ViewBag.Module = "继教申请";
            ViewBag.Title = "申请书";
            return View();
        }

        /// <summary>
        /// 非项目类学分列表
        /// </summary>
        /// <returns></returns>
        public ActionResult NotProjectCreditList()
        {
            //var result = ContinuingEducationRecordService.GetEntities(p => p.IsProjectCredit == false).OrderByDescending(p => p.Id).ToList();
            //IList<ContinuingEducationRecordViewModel> resultlist = new List<ContinuingEducationRecordViewModel>();
            //foreach (var item in result)
            //{
            //    resultlist.Add(item.ToViewModel());
            //}
            //return View(resultlist);

            ViewBag.Module = "继教申请";
            ViewBag.Title = "非项目类学分";
            return View();
        }

        /// <summary>
        /// 添加非项目类学分
        /// </summary>
        /// <returns></returns>
        public ActionResult AddNotProjectCredit()
        {
            ContinuingEducationRecordViewModel model = new ContinuingEducationRecordViewModel();
            return View(model);
        }

        /// <summary>
        /// 非项目类学分详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult NotProjectCreditDetail(int id)
        {
            var model = ContinuingEducationRecordService.GetEntityById(id);
            if (model != null && model.Id != 0)
            {
                return View(model.ToViewModel());
            }
            else
            {
                return View(new ContinuingEducationRecordViewModel());
            }
        }

        /// <summary>
        /// 更新非项目类学分
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult UpdateNotProjectCredit(int id)
        {
            var model = ContinuingEducationRecordService.GetEntityById(id);
            if (model != null && model.Id != 0)
            {
                return View(model.ToViewModel());
            }
            else
            {
                return View(new ContinuingEducationRecordViewModel());
            }
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
        /// 代办事宜
        /// </summary>
        /// <returns></returns>
        public ActionResult AllWorkContainer()
        {
            ViewBag.Module = "全部";
            ViewBag.Title = "待办事宜";

            return View();
        }

        /// <summary>
        /// 待办事宜(Top 10)
        /// </summary>
        /// <returns></returns>
        public ActionResult Top10WorkToDoList()
        {
            var result = ERPRiZhiService.GetEntities(p => p.FKAction == "待办" && p.ModuleName == ModuleNameOfScienceResearch.ContinuingEducation.ToString()).OrderByDescending(p => p.ID).Take(10);
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
                    && p.FormID == (int)ContinuingEducationTypeOfFormId.Application
                    && p.ProjectStatus == ApplicationStatus.ProjectProcessing.ToString(), ApplicationSortField.TimeStr_Desc.ToString(), Constant.PageSize, 1, out totalPage);
            }
            else
            {
                result = ApplicationService.GetPageEntities(p => p.StateNow == "正在办理"
                    && p.UserName == User.Identity.Name
                    && p.FormID == (int)ContinuingEducationTypeOfFormId.Application 
                    && p.ProjectStatus == ApplicationStatus.ProjectProcessing.ToString(), ApplicationSortField.TimeStr_Desc.ToString(), Constant.PageSize, 1, out totalPage);
            }

            return View(result.Select(x => x.ToViewModel()));
        }

        #endregion

        #region  Post Action

        /// <summary>
        /// 填报申请书
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="model"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubmitApplication(string flag, ERPNWorkToDoViewModel model, FormCollection collection)
        {
            ///验证
            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}

            model.UserName = User.Identity.Name;
            model.TimeStr = DateTime.Now;

            var sectionName = MySession[SessionKeyEnum.SectionName].ToString();

            //加载表单内容
            var formModel = ERPNFormService.GetEntityById((int)ContinuingEducationTypeOfFormId.Application).ToViewModel();
            model.FormContent = formModel.ContentStr;
            model.FormContent = model.FormContent.Replace(Constant.MacroSectionString, sectionName);
            model.FormContent = model.FormContent.Replace(Constant.MacroUserNameString, User.Identity.Name);

            string formKeys = formModel.ItemsList;

            //获取当前表单对应的工作数据列
            string[] formItemArray = formModel.ItemsList.Split(Constant.SplitChar);

            ///过滤掉空值
            formItemArray = formItemArray.Where(s => !string.IsNullOrEmpty(s)).ToArray();

            foreach (var item in formItemArray)
            {
                var nameIdArray = item.Split(Constant.UnderlineChar);

                if (nameIdArray.Count() == 2)
                {
                    // nameIdArray[0] id， nameIdArray[1] Chinese name
                    model.FormKeys += nameIdArray[1] + Constant.SharpChar;
                    model.FormValues += collection[nameIdArray[0]].ToString() + Constant.SharpChar;
                }
            }

            /////截取下划线 '_' 前面的字符
            //for (int i = 0; i < formItemArray.Length; i++)
            //{
            //    int index = formItemArray[i].LastIndexOf(Constant.UnderlineChar);
            //    formItemArray[i] = formItemArray[i].Remove(index);
            //}

            model.FormContent = FormContentReplaceHelper.ReplaceFormContentValue(model.FormContent, collection);

            try
            {
                model.StateNow = Constant.Doing;
            }
            catch
            {
                model.JieDianName = Constant.End;
                model.StateNow = Constant.ForcedEnd;
            }

            model.ShenPiUserList = Constant.USER_NAME;
            model.OKUserList = Constant.Default;
            model.BeiYong2 = "##";

            int jiedianid = Convert.ToInt32(model.JieDianID);
            var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
            model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));


            if (flag == Constant.Save)//保存  TODO: "Save"存在常量 Constant中。。。。
            {
                model.IsTemporary = true;
                model.IsLocked = false;
                model.IsDeleted = false;
                model.IsRejected = false;

                //申请书状态
                model.ApplicationStatus = ApplicationStatus.AplicationWriting.ToString();
                //整个项目进行的状态
                model.ProjectStatus = ApplicationStatus.AplicationWriting.ToString();
                int nworktodoid = this.ApplicationService.AddApplication(model.ToDataTransferObjectModel());
                //todo:日志

                //写系统日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                    User.Identity.Name,
                    Constant.SaveSubmitApplication,// 这种
                    Request.UserHostAddress,
                    ContinuingEducationTypeOfFormId.Application.ToString(),
                    Constant.Done, // TODO:存在常量 Constant中。。。。
                    nworktodoid.ToString());
                ////已办
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
                    model.ApplicationStatus = ApplicationStatus.ApplicationApproving.ToString();
                    model.ProjectStatus = ApplicationStatus.ApplicationApproving.ToString();
                    //上报时设置保存为false、驳回为false、删除为false、冻结为false
                    model.IsTemporary = false;
                    model.IsRejected = false;
                    model.IsDeleted = false;
                    model.IsLocked = false;

                    int nworktodoid = this.ApplicationService.AddApplication(model.ToDataTransferObjectModel());

                    //写系统日志
                    //已办
                    ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                        User.Identity.Name,
                        User.Identity.Name + Constant.ReportSubmitAppplication,
                        Request.UserHostAddress,
                        ContinuingEducationTypeOfFormId.Application.ToString(),
                        Constant.Done,
                        ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                        nworktodoid.ToString(),
                         DateTime.Now
                        );

                    int AddRiZhiSuccess = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                    //待办
                    ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel(
                        FillInRiZhi(Convert.ToInt16(model.WorkFlowID), sectionName),
                        "需要审批('" + User.Identity.Name + "''" + Constant.ReportSubmitAppplication + "')",
                        Request.UserHostAddress,
                        ContinuingEducationTypeOfFormId.Application.ToString(),
                        Constant.ToDo,
                        ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                        nworktodoid.ToString(),
                        DateTime.Now
                        );

                    int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());
                    return Json(AddRiZhiSuccess, JsonRequestBehavior.AllowGet);
                    //return RedirectToAction("SubmitApplication", new { id = nworktodoid });
                }
                //先保存在上报的,就更新保存数据行的IsIsTemporary
                else
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
                    ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                        User.Identity.Name,
                        User.Identity.Name + Constant.ReportSubmitAppplication,
                        Request.UserHostAddress,
                        ContinuingEducationTypeOfFormId.Application.ToString(),
                        Constant.Done,
                        ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                        model.NWorkToDoID.ToString(),
                        DateTime.Now
                        );

                    //更新已办
                    string nworktodoid = model.NWorkToDoID.ToString();
                    string formname = ContinuingEducationTypeOfFormId.Application.ToString();
                    MyRiZhi = ERPRiZhiService.GetEntities(p => p.FKApplicationID == nworktodoid && p.FkFormName == formname).FirstOrDefault().ToViewModel();

                    bool updateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                    //新增待办
                    ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel(
                        FillInRiZhi(Convert.ToInt16(model.WorkFlowID),sectionName),
                        "需要审批('" + User.Identity.Name + "''" + Constant.ReportSubmitAppplication + "')",
                        Request.UserHostAddress,
                        ContinuingEducationTypeOfFormId.Application.ToString(),
                        Constant.ToDo,
                        ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                        model.NWorkToDoID.ToString(),
                         DateTime.Now
                        );

                    int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                    //  return RedirectToAction("SubmitApplication", new { id = model.NWorkToDoID });
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
            //action flag
            //string toDoAction;
            string nodeSerils;
            string act = string.Empty;
            //审批
            if (flag == Constant.Process)
            {
                var attachment = model.FuJianList;
                string PiShiStr = model.ShenPiYiJian;

                // TODO: 审批上传附件
                model.ShenPiYiJian = attachment;
                model.OKUserList = model.OKUserList + "," + Constant.USER_NAME;
             
                if (collection["SingleShenPiYiJian"] != "")
                {
                    //审批意见列表
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
                    model.JieDianName = Constant.End;
                    model.ApplicationStatus = ApplicationStatus.ApplicationApproved.ToString();//申请书审批完成
                    model.ProjectStatus = ApplicationStatus.ApplicationApproved.ToString();
                    bool isUpdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());
                    ViewBag.Title = model.JieDianName;

                    //toDoAction = "结束";
                    nodeSerils = Constant.End;
                    //nodeSerils=(int)model.JieDianID;
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

                    // toDoAction = model.JieDianName;
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

                //toDoAction = "驳回";
                nodeSerils = Constant.Reject;

            }

            string formname = ContinuingEducationTypeOfFormId.Application.ToString();
            var rizhiresult = ERPRiZhiService.GetEntities(p => p.FKApplicationID == model.NWorkToDoID.ToString() && p.FkFormName == formname && p.FKAction == Constant.ToDo).FirstOrDefault();

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
                //待办变成已办
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                    User.Identity.Name,
                    "已审批'" + model.UserName + Constant.ReportSubmitAppplication,
                    Request.UserHostAddress,
                     ContinuingEducationTypeOfFormId.Application.ToString(),
                     Constant.Done,
                     ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                     model.NWorkToDoID.ToString(),
                     DateTime.Now
                    );
                MyRiZhi.ID = rizhiresult.ID;
                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //新生成待办，下一审批人的待办
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel(
                    rizhiresult.ID,
                    "需要审批('" + model.UserName + "''" + Constant.ReportSubmitAppplication + "')",
                    Request.UserHostAddress,
                    ContinuingEducationTypeOfFormId.Application.ToString(),
                    Constant.ToDo,
                    ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                    model.NWorkToDoID.ToString(),
                    DateTime.Now
                    );

                ERPNWorkFlowNodeViewModel erpnWorkFlowNodeModel = new ERPNWorkFlowNodeViewModel();
                erpnWorkFlowNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == model.WorkFlowID && p.NodeSerils == nodeSerils).FirstOrDefault().ToViewModel();

                MyRiZhi1.UserName = erpnWorkFlowNodeModel.SPDefaultList;
                int returnid = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());
                //继续审批
                // return RedirectToAction("ApplicationAgree", new { id = model.NWorkToDoID });
                return Json(returnid, JsonRequestBehavior.AllowGet);
            }
            else if (nodeSerils == Constant.End)
            {

                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                    User.Identity.Name,
                    "已审批'" + model.UserName + Constant.ReportSubmitAppplication,
                    Request.UserHostAddress,
                    ContinuingEducationTypeOfFormId.Application.ToString(),
                    Constant.Done,
                    ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                    model.NWorkToDoID.ToString(),
                    DateTime.Now
                    );

                MyRiZhi.ID = rizhiresult.ID;
                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //新生成新建项目确立的待办，申请人待办
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel(
                    model.UserName,
                    model.UserName + Constant.AddContinue,
                    Request.UserHostAddress,
                    ContinuingEducationTypeOfFormId.Establishment.ToString(),
                    Constant.ToDo,
                    ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                    model.NWorkToDoID.ToString(),
                    DateTime.Now
                    );
                MyRiZhi1.ID = rizhiresult.ID;
                int returnid = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());
                // 更新成功并且更新后，申请书状态变为 已经审批通过，跳转到项目确立页面
                //return RedirectToAction("ProjectEstablish", new { id = model.NWorkToDoID });
                return Json(returnid, JsonRequestBehavior.AllowGet);
            }
            //toDoAction == "驳回"
            else
            {
                //审批人已办
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                    User.Identity.Name,
                    "已驳回'" + model.UserName + Constant.ReportSubmitAppplication,
                    Request.UserHostAddress,
                    ContinuingEducationTypeOfFormId.Application.ToString(),
                    Constant.Done,
                    ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                    model.NWorkToDoID.ToString(),
                    DateTime.Now
                    );

                MyRiZhi.ID = rizhiresult.ID;
                bool UpdateRiZhiSuccess1 = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //申请人待办
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel(
                    model.UserName,
                    Constant.RejectContinue,
                    Request.UserHostAddress,
                    ContinuingEducationTypeOfFormId.Application.ToString(),
                    Constant.ToDo,
                    ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                    model.NWorkToDoID.ToString(),
                    DateTime.Now
                    );

                MyRiZhi1.ID = rizhiresult.ID;
                int returnid = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                // return RedirectToAction("ApplicationRejected", new { id = model.NWorkToDoID });
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
            var temerpnformmodel = ERPNFormService.GetEntityById((int)ContinuingEducationTypeOfFormId.Application).ToViewModel();
            model.FormContent = temerpnformmodel.ContentStr;
            model.FormContent = model.FormContent.Replace(Constant.MacroSectionString, sectionName);
            model.FormContent = model.FormContent.Replace(Constant.MacroUserNameString, User.Identity.Name);

            string formKeys = temerpnformmodel.ItemsList;

            string[] formItemArray = temerpnformmodel.ItemsList.Split(Constant.SplitChar);

            ///过滤掉空值
            formItemArray = formItemArray.Where(s => !string.IsNullOrEmpty(s)).ToArray();

            foreach (var item in formItemArray)
            {
                var nameIdArray = item.Split(Constant.UnderlineChar);
                if (nameIdArray.Count() == 2)
                {
                    model.FormKeys += nameIdArray[1] + Constant.SharpChar;
                    model.FormValues += collection[nameIdArray[0]].ToString() + Constant.SharpChar;
                }

            }

            model.FormContent = FormContentReplaceHelper.ReplaceFormContentValue(model.FormContent, collection);

            try
            {
                model.StateNow = Constant.Doing;
            }
            catch
            {
                model.JieDianName = Constant.End;
                model.StateNow = Constant.ForcedEnd;
            }

            model.ShenPiUserList = Constant.USER_NAME;
            model.OKUserList = Constant.Default;

            int jiedianid = Convert.ToInt32(model.JieDianID);
            var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
            model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));


            if (flag == Constant.Save)//根据已有ID保存更新
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
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                    User.Identity.Name,
                   Constant.SaveSubmitApplication,
                   Request.UserHostAddress,
                   ContinuingEducationTypeOfFormId.Application.ToString(),
                   Constant.Done,
                   ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                   model.NWorkToDoID.ToString(),
                   DateTime.Now);
                //已办
                int UpdateRiZhiSuccess = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                // return RedirectToAction("ApplicationRejected", new { id = model.NWorkToDoID });
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
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                    User.Identity.Name,
                    User.Identity.Name + Constant.ReportSubmitAppplication,
                    Request.UserHostAddress,
                    ContinuingEducationTypeOfFormId.Application.ToString(),
                    Constant.Done,
                    ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                    model.NWorkToDoID.ToString(),
                    DateTime.Now
                    );

                //更新已办
                string nworktodoid = model.NWorkToDoID.ToString();
                string formname = ContinuingEducationTypeOfFormId.Application.ToString();
                MyRiZhi = ERPRiZhiService.GetEntities(p => p.FKApplicationID == nworktodoid && p.FKAction == Constant.ToDo).FirstOrDefault().ToViewModel();

                bool updateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());
                //新增待办
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel(
                    FillInRiZhi(Convert.ToInt16(model.WorkFlowID),sectionName),
                    "需要审批('" + User.Identity.Name + "''" + Constant.ReportSubmitAppplication + "')",
                    Request.UserHostAddress,
                    ContinuingEducationTypeOfFormId.Application.ToString(),
                    Constant.ToDo,
                    ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                    model.NWorkToDoID.ToString(),
                    DateTime.Now
                    );

                int UpdateRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                //return RedirectToAction("ApplicationRejected", new { id = model.NWorkToDoID });
                return Json(UpdateRiZhi1Success, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 继教通知
        /// </summary>
        /// <param name="model"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ContinueNotice(ERPNWorkToDoViewModel model, FormCollection collection)
        {
            model.UserName = User.Identity.Name;
            model.TimeStr = DateTime.Now;
            var sectionName = MySession[SessionKeyEnum.SectionName].ToString();
            ///加载表单内容
            var temerpnformmodel = ERPNFormService.GetEntityById((int)ContinuingEducationTypeOfFormId.Establishment).ToViewModel();
            model.FormContent = temerpnformmodel.ContentStr;
            model.FormContent = model.FormContent.Replace(Constant.MacroSectionString, sectionName);
            model.FormContent = model.FormContent.Replace(Constant.MacroUserNameString, User.Identity.Name);
            string formKeys = "交费";
            string PayFee = collection["Num127683006"].ToString();
            model.FormContent = FormContentReplaceHelper.ReplaceFormContentValue(model.FormContent, collection);

            ///添加交费字段
            ERPNWorkToDoViewModel UpdateModel=new ERPNWorkToDoViewModel();
            UpdateModel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
            UpdateModel.BeiYong2 = PayFee+"#";
            bool UpdateSuccess = ApplicationService.UpdateApplication(UpdateModel.ToDataTransferObjectModel());

            try
            {
                model.StateNow = Constant.Doing;
            }
            catch
            {
                model.JieDianName = Constant.End;
                model.StateNow = Constant.ForcedEnd;
            }

            model.OKUserList = Constant.Default;
            model.ShenPiUserList = Constant.USER_NAME;
            model.FormKeys = formKeys;
            model.FormValues = PayFee;
            ///保存
            if (collection[Constant.Save] != null)
            {
                ///添加日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                     User.Identity.Name,
                     Constant.SaveContinue,
                     Request.UserHostAddress,
                     ContinuingEducationTypeOfFormId.Establishment.ToString(),
                     Constant.Done,
                     ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                     model.ApplicationId.ToString(),
                     DateTime.Now
                    );
                int returnRiZhiId = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                ///第一次保存
                if (model.NWorkToDoID == 0)
                {
                    string act = Constant.Save;
                    model.IsTemporary = true;
                    model.IsDeleted = false;
                    var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(Convert.ToInt32(model.JieDianID));
                    model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));

                    int returnId = this.ApplicationService.AddApplication(model.ToDataTransferObjectModel());

                    ///更改申请书状态
                    ERPNWorkToDoViewModel erpnworktodomodel = new ERPNWorkToDoViewModel();
                    erpnworktodomodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();

                    erpnworktodomodel.ApplicationStatus = ApplicationStatus.NoticeNotIssued.ToString();
                    erpnworktodomodel.ProjectStatus = ApplicationStatus.NoticeNotIssued.ToString();

                    bool isUpdateSuccess = ApplicationService.UpdateApplication(erpnworktodomodel.ToDataTransferObjectModel());

                    return RedirectToAction("ContinueNotice", new { id = model.ApplicationId, nextaction = act });
                }
                ///N次保存
                else
                {
                    string act = Constant.Save;
                    model.IsTemporary = true;
                    var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(Convert.ToInt32(model.JieDianID));
                    model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));

                    model.ApplicationStatus = ApplicationStatus.NoticeNotIssued.ToString();
                    model.ProjectStatus = ApplicationStatus.NoticeNotIssued.ToString();
                    bool isUpdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());
                    return RedirectToAction("ContinueNotice", new { id = model.NWorkToDoID, nextaction = act });
                }

            }
            else
            {
                ERPNWorkToDoViewModel erpnworktodomodel = new ERPNWorkToDoViewModel();
                erpnworktodomodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();

                ///日志记录 

                string formname = ContinuingEducationTypeOfFormId.Establishment.ToString();
                var rizhiresult = ERPRiZhiService.GetEntities(p => p.FKApplicationID == model.ApplicationId.ToString() && p.FkFormName == formname && p.FKAction == "待办").FirstOrDefault();
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                    User.Identity.Name,
                    Constant.ReprotContinue,
                    Request.UserHostAddress,
                    ContinuingEducationTypeOfFormId.Establishment.ToString(),
                     Constant.Done,
                     ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                     model.ApplicationId.ToString(),
                     DateTime.Now
                    );
                MyRiZhi.ID = rizhiresult.ID;
                bool updateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                // 更改当前结点id和name
                var currentNode = ERPNWorkFlowNodeService.GetEntityById(model.JieDianID.Value);
                string nextNodeSerial = currentNode.NextNode;
                var nextNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.NodeSerils == nextNodeSerial && p.WorkFlowID == model.WorkFlowID).ToList();
                model.JieDianID = nextNodeModel.First().ID;
                model.JieDianName = nextNodeModel.First().NodeName;

                //上报时设置保存为false、驳回为false、删除为false、冻结为false
                model.IsTemporary = false;
                model.IsRejected = false;
                model.IsDeleted = false;
                model.IsLocked = false;
                int jiedianid = Convert.ToInt32(model.JieDianID);
                var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));

                //更新申请书中的ApplicationStatus为ProjectProcessing
                erpnworktodomodel.ApplicationStatus = ApplicationStatus.SentNotice.ToString();
                erpnworktodomodel.ProjectStatus = ApplicationStatus.SentNotice.ToString();
                bool isUpdateSuccess = ApplicationService.UpdateApplication(erpnworktodomodel.ToDataTransferObjectModel());
                ///第一次上报
                if (model.NWorkToDoID == 0)
                {
                    int isAddSuccess = this.ApplicationService.AddApplication(model.ToDataTransferObjectModel());

                }
                ///先保存后上报
                else
                {
                    bool isUpdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());
                }
                return RedirectToAction("ContinueNotice", new { id = model.ApplicationId });
            }
        }

        /// <summary>
        /// 添加人员记录
        /// </summary>
        /// <param name="model"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddPersonRecord(ContinuingEducationRecordViewModel model,FormCollection collection)
        {
            string UserId = collection["UserId"].ToString();


            ///对选择的人员进行添加
            string[] userIdList = UserId.Split(',');
            string[] userNameList = model.UserName.Split(',');

            ///过滤掉空值
            userIdList = userIdList.Where(s => !string.IsNullOrEmpty(s)).ToArray();
            userNameList = userNameList.Where(s => !string.IsNullOrEmpty(s)).ToArray();
            foreach (var item in userIdList)
            {
                //foreach (var item1 in userNameList)
                // { 
                using (ApplicationDbContext userManager = new ApplicationDbContext())
                {
                    var currentUser = userManager.Users.Where(p => p.Id == item).FirstOrDefault();
                    if (currentUser != null && currentUser.Degree != null && currentUser.Duty != null)
                    {
                        model.UserDegree = currentUser.Degree.ToString();
                        model.UserDuty = currentUser.Duty.ToString();
                    }
                    var currentSection = userManager.Sections.Where(x => x.ApplicationUsers.Where(p => p.ApplicationUserId == item).Count() > 0).FirstOrDefault();
                    model.Department = currentSection.Name.ToString();
                    model.UserName = currentUser.Name.ToString();
                    model.UserId = item;
                }
                model.IsProjectCredit = true;
                model.CreatedTime = DateTime.Now;
                model.CreatedBy = User.Identity.Name;

                int resultId = this.ContinuingEducationRecordService.AddContinuingEducationRecord(model.ToDataTransferObjectModel());
                if (resultId > 0)
                {
                    model.Id = resultId;
                }
                ViewBag.Id = resultId;
                // }
            }

            ///过滤掉UserId中的，
            //int index = UserId.LastIndexOf(',');
            //UserId = UserId.Remove(index);

            //人员记录条数
            int ResultCount = ContinuingEducationRecordService.GetEntities(p => p.NworkToDoId == model.NworkToDoId).Count;

            ///更改申请书状态
            ERPNWorkToDoViewModel erpnworktodomodel = new ERPNWorkToDoViewModel();
            erpnworktodomodel = ApplicationService.GetEntityById(Convert.ToInt32(model.NworkToDoId)).ToViewModel();
            var PayFeemodel = ApplicationService.GetEntities(p => p.ApplicationId == model.NworkToDoId && p.FormID == (int)ContinuingEducationTypeOfFormId.Establishment).FirstOrDefault();

            erpnworktodomodel.ApplicationStatus = ApplicationStatus.AddMeetingPerson.ToString();
            erpnworktodomodel.ProjectStatus = ApplicationStatus.AddMeetingPerson.ToString();
            erpnworktodomodel.BeiYong2 = PayFeemodel.FormValues + "#" + ResultCount;

            bool isUpdateSuccess = ApplicationService.UpdateApplication(erpnworktodomodel.ToDataTransferObjectModel());
            return View(model);
        }

        /// <summary>
        /// 过程记录提交
        /// </summary>
        /// <param name="model"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ProcessRecords(ERPNWorkToDoViewModel model, FormCollection collection)
        {
            model.UserName = User.Identity.Name;
            model.TimeStr = DateTime.Now;
            var sectionName = MySession[SessionKeyEnum.SectionName].ToString();

            ///加载表单
            var temerpnformmodel = ERPNFormService.GetEntityById((int)ContinuingEducationTypeOfFormId.ProcessRecord).ToViewModel();
            model.FormContent = temerpnformmodel.ContentStr;
            model.FormContent = model.FormContent.Replace(Constant.MacroSectionString, sectionName);
            model.FormContent = model.FormContent.Replace(Constant.MacroUserNameString, User.Identity.Name);

            //获取当前表单对应的工作数据列
            string[] formItemArray = temerpnformmodel.ItemsList.Split(Constant.SplitChar);

            string formKeys = "课题名称#记录类型#记录时间#记录人";
            string name = collection["Text397670573"].ToString();
            string type = collection["Drop46201924"].ToString();
            string time = collection["Date1476233541"].ToString();
            string people = collection["Text309804476"].ToString();
            string formvalues = name + "#" + type + "#" + time + "#" + people;

            model.FormContent = FormContentReplaceHelper.ReplaceFormContentValue(model.FormContent, collection);

            try
            {
                model.StateNow = Constant.Doing;
            }
            catch
            {
                model.JieDianName = Constant.End;
                model.StateNow = Constant.ForcedEnd;
            }
            model.ShenPiUserList = Constant.USER_NAME;
            model.OKUserList = Constant.Default;
            model.FormKeys = formKeys;
            model.FormValues = formvalues;

            var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(Convert.ToInt32(model.JieDianID));
            model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));

            if (collection[Constant.Save] != null)
            {
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                    User.Identity.Name,
                    Constant.SaveProcess,
                    Request.UserHostAddress,
                    ContinuingEducationTypeOfFormId.ProcessRecord.ToString(),
                    Constant.Done,
                    ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                    model.ApplicationId.ToString(),
                    DateTime.Now
                    );

                string act = Constant.Save;
                int returnProcessId = 0;
                model.IsTemporary = true;
                model.IsDeleted = false;

                ///第一次保存
                if (model.NWorkToDoID == 0)
                {
                    model.IsLocked = false;
                    returnProcessId = this.ApplicationService.AddApplication(model.ToDataTransferObjectModel());
                    model.NWorkToDoID = returnProcessId;
                    //FKApplicationID是过程记录的ID
                    MyRiZhi.FKApplicationID = returnProcessId.ToString();

                    int returnRiZhiId = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());
                }
                ///N次保存
                else
                {
                    bool isupdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());

                    //FKApplicationID是过程记录的ID
                    MyRiZhi.FKApplicationID = model.NWorkToDoID.ToString();

                    int returnRiZhiId = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());
                }

                ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                tempmodel.ApplicationStatus = ApplicationStatus.ProjectProcessing.ToString();
                tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                bool isMainProjectUpdate = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());
                ViewBag.Id = model.ApplicationId;
                return View(model);

            }
            else
            {
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                    User.Identity.Name,
                    Constant.AddProcess,
                    Request.UserHostAddress,
                    ContinuingEducationTypeOfFormId.ProcessRecord.ToString(),
                    Constant.Done,
                    ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                    model.ApplicationId.ToString(),
                    DateTime.Now
                    );
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel(
                    FillInRiZhi(Convert.ToInt16(model.WorkFlowID), sectionName),
                    Constant.NeedCheck,
                    Request.UserHostAddress,
                    ContinuingEducationTypeOfFormId.ProcessRecord.ToString(),
                    Constant.ToDo,
                    ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                    model.ApplicationId.ToString(),
                    DateTime.Now
                    );

                // 更改当前结点id和name
                var currentNode = ERPNWorkFlowNodeService.GetEntityById(model.JieDianID.Value);
                string nextNodeSerial = currentNode.NextNode;
                var nextNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.NodeSerils == nextNodeSerial && p.WorkFlowID == model.WorkFlowID).ToList();
                model.JieDianID = nextNodeModel.First().ID;
                model.JieDianName = nextNodeModel.First().NodeName;

                //上报时设置保存为false、驳回为false、删除为false、冻结为false
                model.IsTemporary = false;
                model.IsRejected = false;
                model.IsDeleted = false;
                model.IsLocked = false;

                int nworkToDoId;
                if (model.NWorkToDoID == 0)
                {
                    int returnProcessId = this.ApplicationService.AddApplication(model.ToDataTransferObjectModel());
                    nworkToDoId = returnProcessId;
                    ///过程记录ID
                    MyRiZhi.FKApplicationID = returnProcessId.ToString();
                    MyRiZhi1.FKApplicationID = returnProcessId.ToString();
                    ///过程记录添加
                    int AddRiZhiSuccess = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());
                    int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());
                }
                else
                {
                    bool isUpdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());
                    MyRiZhi.FKApplicationID = model.NWorkToDoID.ToString();
                    nworkToDoId = model.NWorkToDoID;
                    MyRiZhi1.FKApplicationID = model.NWorkToDoID.ToString();
                    int AddRiZhiSuccess = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());
                    int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());
                }
                ///更新申请书的状态
                ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                tempmodel.ApplicationStatus = ApplicationStatus.GiveCrediting.ToString();
                tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                bool isMainProjectUpdate = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());
                ERPNWorkToDoViewModel returnmodel = ApplicationService.GetEntityById(nworkToDoId).ToViewModel();
                ///成功的标志
                ViewBag.SendUpSuccess = true;
                return View(returnmodel);
            }
        }

        /// <summary>
        /// 过程记录审批
        /// </summary>
        /// <param name="model"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ProcessAgree(ERPNWorkToDoViewModel model, FormCollection collection)
        {
            string nodeSerils;
            string act;
            if (collection[Constant.Process] != null)
            {
                var attachment = model.FuJianList;
                string PiShiStr = model.ShenPiYiJian;
                model.ShenPiYiJian = attachment;
                model.OKUserList = model.OKUserList + "," + Constant.USER_NAME;
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
                    model.JieDianName = Constant.End;
                    bool isUpdateSuccess = ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());
                    ViewBag.Title = model.JieDianName;
                    nodeSerils = Constant.End;
                }
                ///流程还未完成，待下级审批
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
            //驳回操作
            else
            {
                act = Constant.Reject;
                model.IsRejected = true;
                var workFlowNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == model.WorkFlowID).OrderBy(p => p.NodeSerils).Skip(1).Take(1).FirstOrDefault();

                model.JieDianID = workFlowNodeModel.ID;
                model.JieDianName = workFlowNodeModel.NodeName.ToString();
                bool isUpdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());
                nodeSerils = Constant.Reject;
            }
            string formname = ContinuingEducationTypeOfFormId.ProcessRecord.ToString();
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
            if (nodeSerilsList.Contains(nodeSerils))
            {
                ///待办变成已办
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                    User.Identity.Name,
                    "已审批'" + erpnworktodoModel.UserName + "'上报的过程记录",
                    Request.UserHostAddress,
                     ContinuingEducationTypeOfFormId.ProcessRecord.ToString(),
                    Constant.Done,
                    ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                    model.NWorkToDoID.ToString(),
                    DateTime.Now
                    );
                MyRiZhi.ID = rizhiresult.ID;

                bool UpdateRiZhiSucess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                ///新增下一级待办日志
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel(
                     rizhiresult.ID,
                     "需要审批('" + erpnworktodoModel.UserName + "'上报的过程记录)",
                     Request.UserHostAddress,
                     ContinuingEducationTypeOfFormId.ProcessRecord.ToString(),
                     Constant.ToDo,
                     ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                     model.NWorkToDoID.ToString(),
                     DateTime.Now
                    );
                ERPNWorkFlowNodeViewModel erpnWorkFlowNodeModel = new ERPNWorkFlowNodeViewModel();
                erpnWorkFlowNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == model.WorkFlowID && p.NodeSerils == nodeSerils).FirstOrDefault().ToViewModel();
                MyRiZhi1.UserName = erpnWorkFlowNodeModel.SPDefaultList;

                int returnId = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());
                //继续审批
                return RedirectToAction("ProcessAgree", new { id = model.NWorkToDoID });
            }
            else if (nodeSerils == Constant.End)
            {
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                     User.Identity.Name,
                    "已审批'" + erpnworktodoModel.UserName + "'上报的过程记录",
                    Request.UserHostAddress,
                     ContinuingEducationTypeOfFormId.ProcessRecord.ToString(),
                    Constant.Done,
                    ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                    model.NWorkToDoID.ToString(),
                    DateTime.Now
                    );
                MyRiZhi.ID = rizhiresult.ID;
                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                return RedirectToAction("ProcessAgree", new { id = model.NWorkToDoID });
            }
            ///驳回
            else
            {
                ///审批人驳回
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                     User.Identity.Name,
                    "已驳回'" + erpnworktodoModel.UserName + "'上报的过程记录",
                    Request.UserHostAddress,
                     ContinuingEducationTypeOfFormId.ProcessRecord.ToString(),
                    Constant.Done,
                    ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                    model.NWorkToDoID.ToString(),
                    DateTime.Now
                    );
                MyRiZhi.ID = rizhiresult.ID;
                bool UpdateRiZhiSucces = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());
                ///申请人待办
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel(
                    erpnworktodoModel.UserName,
                    Constant.RejectProcess,
                     Request.UserHostAddress,
                     ContinuingEducationTypeOfFormId.ProcessRecord.ToString(),
                    Constant.ToDo,
                    ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                    model.NWorkToDoID.ToString(),
                    DateTime.Now
                    );
                MyRiZhi1.ID = rizhiresult.ID; ;
                int returnId = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());
                return RedirectToAction("ProcessRecordsRejected", new { id = model.NWorkToDoID });
            }
        }

        /// <summary>
        /// 过程记录审批驳回
        /// </summary>
        /// <param name="model"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ProcessRecordsRejected(ERPNWorkToDoViewModel model, FormCollection collection)
        {
            model.UserName = User.Identity.Name;
            model.TimeStr = DateTime.Now;
            var sectionName = MySession[SessionKeyEnum.SectionName].ToString();

            ///加载表单
            var temerpnformmodel = ERPNFormService.GetEntityById((int)ContinuingEducationTypeOfFormId.ProcessRecord).ToViewModel();
            model.FormContent = temerpnformmodel.ContentStr;
            model.FormContent = model.FormContent.Replace(Constant.MacroSectionString, sectionName);
            model.FormContent = model.FormContent.Replace(Constant.MacroUserNameString, User.Identity.Name);

            //获取当前表单对应的工作数据列
            string[] formItemArray = temerpnformmodel.ItemsList.Split(Constant.SplitChar);

            string formKeys = "课题名称#记录类型#记录时间#记录人";
            string name = collection["Text397670573"].ToString();
            string type = collection["Drop46201924"].ToString();
            string time = collection["Date1476233541"].ToString();
            string people = collection["Text309804476"].ToString();
            string formvalues = name + "#" + type + "#" + time + "#" + people;

            model.FormContent = FormContentReplaceHelper.ReplaceFormContentValue(model.FormContent, collection);

            try
            {
                model.StateNow = Constant.Doing;
            }
            catch
            {
                model.JieDianName = Constant.End;
                model.StateNow = Constant.ForcedEnd;
            }
            model.ShenPiUserList = Constant.USER_NAME;
            model.OKUserList = Constant.Default;
            model.FormKeys = formKeys;
            model.FormValues = formvalues;

            var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(Convert.ToInt32(model.JieDianID));
            model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));

            ///保存
            if (collection[Constant.Save] != null)
            {
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                    User.Identity.Name,
                    Constant.SaveProcess,
                    Request.UserHostAddress,
                    ContinuingEducationTypeOfFormId.ProcessRecord.ToString(),
                    Constant.Done,
                    ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                    model.NWorkToDoID.ToString(),
                    DateTime.Now
                    );
                int returnRiZhiId = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                model.IsTemporary = true;
                model.IsLocked = false;
                model.IsDeleted = false;
                model.IsRejected = false;

                bool isupdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());
                ViewBag.Id = model.ApplicationId;

                return RedirectToAction("ProcessRecordsRejected", new { id = model.NWorkToDoID.ToString() });
            }
            ///上报
            else
            {
                //写系统日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                    User.Identity.Name,
                    Constant.AddProcess,
                    Request.UserHostAddress,
                    ContinuingEducationTypeOfFormId.ProcessRecord.ToString(),
                    Constant.Done,
                    ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                    model.NWorkToDoID.ToString(),
                    DateTime.Now
                    );
                MyRiZhi = ERPRiZhiService.GetEntities(p => p.FKApplicationID == model.NWorkToDoID.ToString() && p.FKAction == Constant.ToDo).FirstOrDefault().ToViewModel();
                bool updateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel(
                   FillInRiZhi(Convert.ToInt16(model.WorkFlowID), sectionName),
                    Constant.NeedCheck,
                    Request.UserHostAddress,
                    ContinuingEducationTypeOfFormId.ProcessRecord.ToString(),
                    Constant.ToDo,
                    ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                    model.NWorkToDoID.ToString(),
                    DateTime.Now
                    );
                int addRiZhiSuccess = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                //上报时设置保存为false、驳回为false、删除为false、冻结为false
                model.IsTemporary = false;
                model.IsRejected = false;
                model.IsDeleted = false;
                model.IsLocked = false;

                bool isUpdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());

                ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                tempmodel.ApplicationStatus = ApplicationStatus.GiveCrediting.ToString();
                tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                bool isMainProjectUpdate = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());

                return RedirectToAction("ProcessRecordsRejected", new { id = model.NWorkToDoID });
            }
        }

        /// <summary>
        /// 经费报销申请单
        /// </summary>
        /// <param name="model"></param>
        /// <param name="collection"></param>
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

            string projectType = todoModel.FormValues.Split(Constant.SharpChar)[1];

            FundsThresholdViewModel fundModel = new FundsThresholdViewModel();
            fundModel = FundsThresholdService.GetEntities(p => p.FundsType == type && p.ModuleName == ModuleNameOfScienceResearch.ContinuingEducation.ToString() && p.ProjectType == projectType).FirstOrDefault().ToViewModel();
            if (priceTotal > fundModel.Threshold)
            {
                ViewBag.limit = "true";
                using (var context = new CSPostOAEntities())
                {
                    ViewBag.FundsType = (from h in context.FundsThreshold
                                         where (h.IsDeleted == false && h.ModuleName == ModuleNameOfScienceResearch.ContinuingEducation.ToString() && h.ProjectType == projectType)

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
                model.CreatedTime = DateTime.Now;
                model.Type = type;
                model.TimeStr = DateTime.Now;
                model.CreatedBy = User.Identity.Name;
                model.UpdatedBy = User.Identity.Name;
                model.Quantity = '1';
                model.Unit = "元";

                try
                {
                    model.StateNow = Constant.Doing;
                }
                catch
                {
                    model.JieDianName = Constant.End;
                    model.StateNow = Constant.ForcedEnd;
                }

                model.ShenPiUserList = Constant.USER_NAME;
                model.OKUserList = Constant.Default;
                ///保存
                if (collection[Constant.Save] != null)
                {
                    ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                    User.Identity.Name,
                    Constant.SaveReiburse,
                    Request.UserHostAddress,
                    ContinuingEducationTypeOfFormId.FeeReimbursement.ToString(),
                    Constant.Done,
                    ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                    DateTime.Now
                    );

                    string act = Constant.Save;
                    model.IsTemporary = true;
                    model.IsDeleted = false;
                    model.IsLocked = false;

                    int jiedianid = Convert.ToInt32(model.JieDianID);
                    var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                    model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));

                    int returnid;
                    //第一次保存
                    if (model.FundsRecordID == 0)
                    {
                        model.IsIncome = false;

                        model.ModuleName = ModuleNameOfScienceResearch.ContinuingEducation.ToString();
                        returnid = this.FundsRecordService.AddFundsRecord(model.ToDataTransferObjectModel());

                        //FKApplicationID是过程记录的ID
                        MyRiZhi.FKApplicationID = returnid.ToString();
                        int returnRiZhiId = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());
                    }
                    else
                    {
                        bool isAddSuccess = this.FundsRecordService.UpdateFundsRecord(model.ToDataTransferObjectModel());
                        //FKApplicationID是过程记录的ID
                        MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                        returnid = this.FundsRecordService.AddFundsRecord(model.ToDataTransferObjectModel());

                    }
                    ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                    tempmodel.ApplicationStatus = ApplicationStatus.ProjectProcessing.ToString();
                    tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    bool isupdate = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());
                    return RedirectToAction("ReimburseProcess", new { id = returnid, nextaction = act });

                }
                ///上报
                else
                {
                    ///申请人已办
                    ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                    User.Identity.Name,
                    Constant.AddReiburse,
                    Request.UserHostAddress,
                    ContinuingEducationTypeOfFormId.FeeReimbursement.ToString(),
                    Constant.Done,
                    ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                    DateTime.Now
                        );

                    //审批人待办
                    ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel(
                        FillInRiZhi(Convert.ToInt16(model.WorkflowId), sectionName),
                        Constant.NeedCkeckReiburse,
                        Request.UserHostAddress,
                        ContinuingEducationTypeOfFormId.FeeReimbursement.ToString(),
                        Constant.ToDo,
                        ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                        DateTime.Now
                        );
                    //上报时设置保存为false、驳回为false、删除为false、冻结为false
                    model.IsTemporary = false;
                    model.IsRejected = false;
                    model.IsDeleted = false;
                    model.IsLocked = false;

                    int jiedianid = Convert.ToInt32(model.JieDianID);
                    var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                    model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));
                    //页面加载后就上报，就添加新的数据
                    int fundsrecordId = 0;
                    if (model.FundsRecordID == 0)
                    {
                        model.IsIncome = false;

                        model.ModuleName = ModuleNameOfScienceResearch.ContinuingEducation.ToString();

                        int isAddSuccess = this.FundsRecordService.AddFundsRecord(model.ToDataTransferObjectModel());
                        fundsrecordId = isAddSuccess;
                        //FundsRecord的ID
                        MyRiZhi.FKApplicationID = isAddSuccess.ToString();
                        int AddRiZhiSuccess = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());
                        //FundsRecord的ID
                        MyRiZhi1.FKApplicationID = isAddSuccess.ToString();
                        int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());
                    }
                    ///先保存再上报
                    else
                    {
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
                    tempmodel.ApplicationStatus = ApplicationStatus.GiveCrediting.ToString();
                    tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    bool isupdate = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());


                    FundsRecordViewModel returnmodel = FundsRecordService.GetEntityById(fundsrecordId).ToViewModel();
                    //上报成功的标志
                    ViewBag.SendUpSuccess = true;
                    ///获取经费记录类型
                    using (var context = new CSPostOAEntities())
                    {
                        ViewBag.FundsType = (from h in context.FundsThreshold
                                             where (h.IsDeleted == false && h.ModuleName == ModuleNameOfScienceResearch.ContinuingEducation.ToString() && h.ProjectType == projectType)

                                             select new SelectListItem()
                                             {
                                                 Text = h.FundsType,
                                                 Value = h.FundsType,
                                             }).ToList();
                    }
                    return View(returnmodel);
                }
            }
        }

        /// <summary>
        /// 费用审批提交
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ReimburseAgree(string flag,FundsRecordViewModel model, FormCollection collection)
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
                model.ShenPiYiJian = attachment;
                model.OKUserList = model.OKUserList + "," + Constant.USER_NAME;
                if (collection["SingleShenPiYiJian"]!= "")
                {
                    //审批意见
                    model.ShenPiYiJian = PiShiStr + model.JieDianName + ":" + collection["SingleShenPiYiJian"].ToString() + Constant.SplitChar;
                }
                else
                {
                    model.ShenPiYiJian = PiShiStr;
                }
                ///更新申请书状态
                ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                tempmodel.ApplicationStatus = ApplicationStatus.ProjectProcessing.ToString();
                tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                bool isupdate1 = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());
                // 更改当前结点id和name
                // 说明过程记录审批流程还未完成，正在等待下一级审批
                var erpnrowkflownoderesult1 = ERPNWorkFlowNodeService.GetEntityById(model.JieDianID.Value).ToViewModel();
                string nextNodeSerial = erpnrowkflownoderesult1.NextNode;
                var nextNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.NodeSerils == nextNodeSerial && p.WorkFlowID == model.WorkflowId).ToList();
                // 说明过程记录审批流程已经完成，审批通过
                if (nextNodeModel.Count == Constant.ZERO_INT)
                {
                    model.JieDianName = Constant.End;
                    bool isUpdateSuccess = FundsRecordService.UpdateFundsRecord(model.ToDataTransferObjectModel());
                    ViewBag.Title = model.JieDianName;

                    nodeSerils = Constant.End;

                }
                // 说明过程记录审批流程还未完成，正在等待下一级审批
                else
                {

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

                nodeSerils = Constant.Reject;
                bool isUpdate = this.FundsRecordService.UpdateFundsRecord(model.ToDataTransferObjectModel());
            }

            //经费报销单的日志
            if (model.WorkflowId == 1031)
            {
                string formname = ScienceResearchTypeOfFormId.FeeReimbursement.ToString();
                var rizhiresult = ERPRiZhiService.GetEntities(p => p.FKApplicationID == model.FundsRecordID.ToString() && p.FkFormName == formname && p.FKAction == Constant.ToDo).FirstOrDefault();
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
                    //日志待办成已办
                    ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                        User.Identity.Name,
                        "已审批'" + erpnworktodoModel.UserName + "'上报的经费报销单",
                        Request.UserHostAddress,
                        ContinuingEducationTypeOfFormId.FeeReimbursement.ToString(),
                        Constant.Done,
                        ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                        model.FundsRecordID.ToString(),
                        DateTime.Now
                        );
                    MyRiZhi.ID = rizhiresult.ID;

                    bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                    //新生成待办，下一审批人的待办
                    ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel(
                        rizhiresult.ID,
                        "需要审批('" + erpnworktodoModel.UserName + "'上报的经费报销单)",
                        Request.UserHostAddress,
                        ContinuingEducationTypeOfFormId.FeeReimbursement.ToString(),
                        Constant.ToDo,
                        ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                        model.FundsRecordID.ToString(),
                        DateTime.Now
                        );

                    ERPNWorkFlowNodeViewModel erpnWorkFlowNodeModel = new ERPNWorkFlowNodeViewModel();
                    erpnWorkFlowNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == model.WorkflowId && p.NodeSerils == nodeSerils).FirstOrDefault().ToViewModel();

                    MyRiZhi1.UserName = erpnWorkFlowNodeModel.SPDefaultList;

                    int returnid = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());
                   
                    //继续审批
                  return Json(returnid, JsonRequestBehavior.AllowGet);
                   // return RedirectToAction("ReimburseAgree", new { id = model.FundsRecordID });
                }
                else if (nodeSerils == Constant.End)
                {
                    ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                        User.Identity.Name,
                        "已审批'" + erpnworktodoModel.UserName + "'上报的经费报销单",
                        Request.UserHostAddress,
                        ContinuingEducationTypeOfFormId.FeeReimbursement.ToString(),
                        Constant.Done,
                        ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                        model.FundsRecordID.ToString(),
                        DateTime.Now
                        );
                    MyRiZhi.ID = rizhiresult.ID;
                    bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                    int UpdateSuccess = 1;
                    return Json(UpdateSuccess, JsonRequestBehavior.AllowGet);
                    // 更新成功并且更新后，申请书状态变为 已经审批通过，跳转到项目确立页面
                   // return RedirectToAction("ReimburseAgree", new { id = model.FundsRecordID });
                }
                //toDoAction == "驳回"
                else
                {
                    //审批人已办
                    ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                          User.Identity.Name,
                       "已驳回'" + erpnworktodoModel.UserName + "'上报的经费报销单",
                        Request.UserHostAddress,
                        ContinuingEducationTypeOfFormId.FeeReimbursement.ToString(),
                        Constant.Done,
                        ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                        model.FundsRecordID.ToString(),
                        DateTime.Now
                        );
                    MyRiZhi.ID = rizhiresult.ID;
                    bool UpdateRiZhiSuccess1 = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                    //申请人待办
                    ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel(
                        erpnworktodoModel.UserName,
                        Constant.RejectReiburse,
                        Request.UserHostAddress,
                        ContinuingEducationTypeOfFormId.FeeReimbursement.ToString(),
                        Constant.ToDo,
                        ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                        model.FundsRecordID.ToString(),
                        DateTime.Now
                        );
                    MyRiZhi1.ID = rizhiresult.ID;
                    int returnid = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                    return Json(returnid, JsonRequestBehavior.AllowGet);
                    //return RedirectToAction("ReimburseRejected", new { id = model.FundsRecordID });
                }
            }
            //差旅报销单的日志
            else
            {
                string formname = ScienceResearchTypeOfFormId.TravelReimbursement.ToString();
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
                    //待办变成已办
                    ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                        User.Identity.Name,
                         "已审批'" + erpnworktodoModel.UserName + "'上报的差旅报销单",
                        Request.UserHostAddress,
                        ContinuingEducationTypeOfFormId.TravelReimbursement.ToString(),
                        Constant.Done,
                        ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                        model.FundsRecordID.ToString(),
                        DateTime.Now
                        );
                    MyRiZhi.ID = rizhiresult.ID;
                    bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                    //新生成待办，下一审批人的待办
                    ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel(
                        rizhiresult.ID,
                        "需要审批('" + erpnworktodoModel.UserName + "'上报的差旅报销单)",
                        Request.UserHostAddress,
                        ContinuingEducationTypeOfFormId.TravelReimbursement.ToString(),
                        Constant.ToDo,
                        ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                        model.FundsRecordID.ToString(),
                        DateTime.Now
                        );

                    ERPNWorkFlowNodeViewModel erpnWorkFlowNodeModel = new ERPNWorkFlowNodeViewModel();
                    erpnWorkFlowNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == model.WorkflowId && p.NodeSerils == nodeSerils).FirstOrDefault().ToViewModel();

                    MyRiZhi1.UserName = erpnWorkFlowNodeModel.SPDefaultList;

                    int returnid = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());
                    //继续审批
                    return Json(returnid, JsonRequestBehavior.AllowGet);
                    //return RedirectToAction("ReimburseAgree", new { id = model.FundsRecordID });
                }
                else if (nodeSerils == Constant.End)
                {
                    ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                         User.Identity.Name,
                        "已审批'" + erpnworktodoModel.UserName + "'上报的差旅报销单",
                        Request.UserHostAddress,
                        ContinuingEducationTypeOfFormId.TravelReimbursement.ToString(),
                        Constant.Done,
                        ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                        model.FundsRecordID.ToString(),
                        DateTime.Now
                        );
                    MyRiZhi.ID = rizhiresult.ID;
                    bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());


                    int UpdateSuccess = 1;
                    return Json(UpdateSuccess, JsonRequestBehavior.AllowGet);
                    // 更新成功并且更新后，申请书状态变为 已经审批通过，跳转到项目确立页面
                    //return RedirectToAction("ReimburseAgree", new { id = model.FundsRecordID });
                }
                //toDoAction == "驳回"
                else
                {
                    //审批人已办
                    ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                          User.Identity.Name,
                       "已驳回'" + erpnworktodoModel.UserName + "'上报的差旅报销单",
                        Request.UserHostAddress,
                        ContinuingEducationTypeOfFormId.TravelReimbursement.ToString(),
                        Constant.Done,
                        ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                        model.FundsRecordID.ToString(),
                        DateTime.Now
                        );

                    MyRiZhi.ID = rizhiresult.ID;
                    bool UpdateRiZhiSuccess1 = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                    //申请人待办
                    ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel(
                         erpnworktodoModel.UserName,
                        Constant.RejectTravel,
                        Request.UserHostAddress,
                        ContinuingEducationTypeOfFormId.TravelReimbursement.ToString(),
                        Constant.ToDo,
                        ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                        model.FundsRecordID.ToString(),
                        DateTime.Now
                        );

                    MyRiZhi1.ID = rizhiresult.ID;
                    int returnid = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                    return Json(returnid, JsonRequestBehavior.AllowGet);
                    //return RedirectToAction("TravelExpensesRejected", new { id = model.FundsRecordID });
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

            string projectType = todoModel.FormValues.Split(Constant.SharpChar)[1];

            FundsThresholdViewModel fundModel = new FundsThresholdViewModel();
            fundModel = FundsThresholdService.GetEntities(p => p.FundsType == type && p.ModuleName == ModuleNameOfScienceResearch.ContinuingEducation.ToString() && p.ProjectType == projectType).FirstOrDefault().ToViewModel();

            if (priceTotal > fundModel.Threshold)
            {

                ViewBag.limit = "true";
                using (var context = new CSPostOAEntities())
                {
                    ViewBag.FundsType = (from h in context.FundsThreshold
                                         where (h.IsDeleted == false && h.ModuleName == ModuleNameOfScienceResearch.ContinuingEducation.ToString() && h.ProjectType == projectType)

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

                model.CreatedTime = DateTime.Now;
                model.Type = type;
                model.TimeStr = DateTime.Now;
                model.CreatedBy = User.Identity.Name;
                model.UpdatedBy = User.Identity.Name;
                model.Quantity = '1';
                model.Unit = "元";

                try
                {
                    model.StateNow = Constant.Doing;
                }
                catch
                {
                    model.JieDianName = Constant.End;
                    model.StateNow = Constant.ForcedEnd;
                }

                model.ShenPiUserList = Constant.USER_NAME;
                model.OKUserList = Constant.Default;

                if (collection[Constant.Save] != null)//保存
                {
                    //已办
                    ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                    User.Identity.Name,
                    Constant.SaveReiburse,
                    Request.UserHostAddress,
                    ContinuingEducationTypeOfFormId.FeeReimbursement.ToString(),
                    Constant.Done,
                    ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                    model.FundsRecordID.ToString(),
                    DateTime.Now
                        );

                    int returnRiZhiId = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                    string act = Constant.Save;
                    model.IsTemporary = true;
                }
                //上报collection["Reported"]
                else
                {
                    //已办
                    ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                          User.Identity.Name,
                        Constant.AddReiburse,
                        Request.UserHostAddress,
                        ContinuingEducationTypeOfFormId.FeeReimbursement.ToString(),
                        Constant.Done,
                        ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                        model.FundsRecordID.ToString(),
                        DateTime.Now
                        );
                    MyRiZhi = ERPRiZhiService.GetEntities(p => p.FKApplicationID == model.FundsRecordID.ToString() && p.FKAction == "待办").FirstOrDefault().ToViewModel();
                    bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                   //待办
                    ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel(
                       FillInRiZhi(Convert.ToInt16(model.WorkflowId), sectionName),
                       Constant.NeedCkeckReiburse,
                       Request.UserHostAddress,
                        ContinuingEducationTypeOfFormId.FeeReimbursement.ToString(),
                        Constant.ToDo,
                        ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                        model.FundsRecordID.ToString(),
                        DateTime.Now
                        );
                    int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                    //上报时设置保存为false、驳回为false、删除为false、冻结为false
                    model.IsTemporary = false;
                    bool isUpdate = this.FundsRecordService.UpdateFundsRecord(model.ToDataTransferObjectModel());
                }
                model.IsLocked = false;
                model.IsDeleted = false;
                model.IsRejected = false;

                int jiedianid = Convert.ToInt32(model.JieDianID);
                var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));

                ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                tempmodel.ApplicationStatus = ApplicationStatus.GiveCrediting.ToString();
                tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                bool isupdate = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());

                bool isAddSuccess = this.FundsRecordService.UpdateFundsRecord(model.ToDataTransferObjectModel());

                return RedirectToAction("ReimburseRejected", new { id = model.FundsRecordID });
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
            model.CreatedBy = Constant.USER_NAME;
            model.UpdatedBy = Constant.USER_NAME;
            model.Quantity = '1';
            model.Unit = "元";
            model.WorkflowId = (int)TypeOfWorkFlowId.TravelReimbursement;
            model.Type = "差旅报销单";

            model.CreatedTime = DateTime.Now;

            try
            {
                model.StateNow = Constant.Doing;
            }
            catch
            {
                model.JieDianName = Constant.End;
                model.StateNow = Constant.ForcedEnd;
            }

            model.ShenPiUserList = Constant.USER_NAME;
            model.OKUserList = Constant.Default;

            int jiedianid = Convert.ToInt32(model.JieDianID);
            var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
            model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));

            model.IsRejected = false;
            model.IsDeleted = false;
            model.IsLocked = false;

            if (collection[Constant.Save] != null)//保存
            {
                //已办
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                    User.Identity.Name,
                    Constant.SaveTravel,
                    Request.UserHostAddress,
                    ContinuingEducationTypeOfFormId.TravelReimbursement.ToString(),
                    Constant.Done,
                    ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                    model.FundsRecordID.ToString(),
                    DateTime.Now
                    );
                int returnRiZhiId = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                string act = Constant.Save;
                model.IsTemporary = true;

                bool isAddSuccess = this.FundsRecordService.UpdateFundsRecord(model.ToDataTransferObjectModel());

            }
            //上报collection["Reported"]
            else
            {
                ///已办
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                    User.Identity.Name,
                    Constant.AddTravel,
                     Request.UserHostAddress,
                    ContinuingEducationTypeOfFormId.TravelReimbursement.ToString(),
                    Constant.Done,
                    ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                    model.FundsRecordID.ToString(),
                    DateTime.Now
                    );

                MyRiZhi = ERPRiZhiService.GetEntities(p => p.FKApplicationID == model.FundsRecordID.ToString() && p.FKAction == Constant.ToDo).FirstOrDefault().ToViewModel();
                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //待办
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel(
                   FillInRiZhi(Convert.ToInt16(model.WorkflowId), sectionName),
                    Constant.NeedCkeckTravel,
                    Request.UserHostAddress,
                    ContinuingEducationTypeOfFormId.TravelReimbursement.ToString(),
                    Constant.ToDo,
                    ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                    model.FundsRecordID.ToString(),
                    DateTime.Now
                    );

                int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                //上报时设置保存为false、驳回为false、删除为false、冻结为false
                model.IsTemporary = false;

                bool isUpdate = this.FundsRecordService.UpdateFundsRecord(model.ToDataTransferObjectModel());
            }

            //更新过程记录相关申请书的applicationstatus
            ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
            tempmodel.ApplicationStatus = ApplicationStatus.ProjectProcessing.ToString();
            tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
            bool isupdate1 = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());
            return RedirectToAction("TravelExpensesRejected", new { id = model.FundsRecordID });
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
            model.CreatedBy = Constant.USER_NAME;
            model.UpdatedBy = Constant.USER_NAME;
            model.Quantity = '1';
            model.Unit = "元";
            model.WorkflowId = (int)TypeOfWorkFlowId.TravelReimbursement;
            model.Type = "差旅报销单";

            model.CreatedTime = DateTime.Now;

            try
            {
                model.StateNow = Constant.Doing;
            }
            catch
            {
                model.JieDianName = Constant.End;
                model.StateNow = Constant.ForcedEnd;
            }

            model.ShenPiUserList = Constant.USER_NAME;
            model.OKUserList = Constant.Default;

            if (collection[Constant.Save] != null)//保存
            {
                //已办
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                    User.Identity.Name,
                    Constant.SaveTravel,
                    Request.UserHostAddress,
                    ContinuingEducationTypeOfFormId.TravelReimbursement.ToString(),
                    Constant.Done,
                    ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                    model.ApplicationId.ToString(),
                    DateTime.Now
                    );

                string act = Constant.Save;
                model.IsTemporary = true;
                model.IsDeleted = false;

                int jiedianid = Convert.ToInt32(model.JieDianID);
                var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));
                //第一次保存
                //注意直接用ID可能出错，页面也未改
                if (model.FundsRecordID == 0)
                {
                    //支出，以后要根据情况修改
                    model.IsIncome = false;

                    model.ModuleName = ApplicationType.ScienceResearch.ToString();

                    int returnid = this.FundsRecordService.AddFundsRecord(model.ToDataTransferObjectModel());

                    //FKApplicationID是过程记录的ID
                    MyRiZhi.FKApplicationID = returnid.ToString();
                    int returnRiZhiId = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                }
                //第二次或第N次保存
                else
                {
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
                //写系统日志,已办
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                    User.Identity.Name,
                    Constant.AddTravel,
                    Request.UserHostAddress,
                    ContinuingEducationTypeOfFormId.TravelReimbursement.ToString(),
                    Constant.Done,
                    ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                    DateTime.Now
                    );

                  //待办
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel(
                    FillInRiZhi(Convert.ToInt16(model.WorkflowId), sectionName),
                    Constant.NeedCkeckTravel,
                     Request.UserHostAddress,
                    ContinuingEducationTypeOfFormId.TravelReimbursement.ToString(),
                    Constant.ToDo,
                    ModuleNameOfScienceResearch.ContinuingEducation.ToString(),
                    DateTime.Now
                    );

                int jiedianid = Convert.ToInt32(model.JieDianID);
                var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));

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
        /// 授予学分更新
        /// </summary>
        /// <param name="model"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GiveCredit(ContinuingEducationRecordViewModel model, FormCollection collection)
        {
            string str = collection["checkItem"];
            string[] strUpdate = str.Split(',');
            foreach (var Id in strUpdate)
            {
                if (Id != "false")
                {
                    ContinuingEducationRecordViewModel UpdateModel = new ContinuingEducationRecordViewModel();
                    UpdateModel = ContinuingEducationRecordService.GetEntities(p => p.Id == Convert.ToInt32(Id)).FirstOrDefault().ToViewModel();
                    UpdateModel.IsGranted = true;
                    bool UpdateSucess = ContinuingEducationRecordService.UpdateContinuingEducationRecord(UpdateModel.ToDataTransferObjectModel());
                }
               
            }

            var result = ContinuingEducationRecordService.GetEntities(p => p.IsProjectCredit == true && p.NworkToDoId == model.Id).OrderByDescending(p => p.CreatedTime).ToList();

            IList<ContinuingEducationRecordViewModel> resultList = new List<ContinuingEducationRecordViewModel>();

            foreach (var item in result)
            {
                resultList.Add(item.ToViewModel());
            }
            ViewBag.Id = model.Id;
            return View(resultList);
        }

        /// <summary>
        /// 奖励设置
        /// </summary>
        /// <param name="model"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ProjectBonusCredit(ProjectBonusCreditViewModel model,FormCollection collection)
        {
            bool isUpdate = this.ProjectBonusCreditService.UpdateProjectBonusCredit(model.ToDataTransferObjectModel());
            return View();
        }

        /// <summary>
        /// 资金管理
        /// </summary>
        /// <returns></returns>
        public ActionResult ScienceFundsManageContainer()
        {
            ViewBag.Module = "继续教育";
            ViewBag.Title = "资金管理";
            return View();
        }

        /// <summary>
        /// 日常经费统计
        /// </summary>
        /// <returns></returns>
        public ActionResult FundsList(string moduleName)
        {
            ViewBag.Module = "继续教育";
            ViewBag.Title = "日常经费记录";

            ViewBag.ModuleName = moduleName;

            return View();
        }

        /// <summary>
        /// fundsInitail the session of this controller
        /// </summary>
        private void FundsInitialSession()
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
        /// Initail the searchCriteria from session of this controller
        /// </summary>
        private void FundsInitialTheSearchCriteria()
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
        /// 资金管理查询
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FundsListStatistics(int page, int pageSize)
        {
            FundsInitialTheSearchCriteria();
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
                    && p.ModuleName == projectType
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
                                && p.ModuleName == projectType
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
                                && p.ModuleName == projectType
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

        /// <summary>
        /// 根据id删除经费记录
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
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


        /// <summary>
        /// 申请书列表查询
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="projectName">项目名称</param>
        /// <param name="state">状态</param>
        /// <param name="freeze">是否冻结</param>
        /// <returns></returns>
          [HttpPost]
        public ActionResult ApplicationListStatistics(string projectName, string State, string Freeze, string startTime, string endTime, int page, int pageSize)
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

            IEnumerable<ERPNWorkToDoTransferObject> result = SearchProcessingApplicationList(projectName, State, Freeze, start,
                end, pageSize, page, ref totalPage);

            return Json(new { data = result, total = totalPage * pageSize }, JsonRequestBehavior.AllowGet);

        }

       /// <summary>
       /// 非项目类学分统计
       /// </summary>
       /// <param name="Name"></param>
       /// <param name="State"></param>
       /// <param name="startTime"></param>
       /// <param name="endTime"></param>
       /// <param name="page"></param>
       /// <param name="pageSize"></param>
       /// <returns></returns>
          public ActionResult NotProjectCreditStatistics(string Name, string State,string startTime, string endTime, int page, int pageSize)
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

              IEnumerable<ContinuingEducationRecordTransferObject> result = SearchCreditStatisticsList(Name, State, start,
                  end, pageSize, page, ref totalPage);

              return Json(new { data = result, total = totalPage * pageSize }, JsonRequestBehavior.AllowGet);

          }


        ///// <summary>
        ///// 非项目类学分授予学分
        ///// </summary>
        ///// <param name="model"></param>
        ///// <param name="collection"></param>
        ///// <returns></returns>
          [HttpPost]
          public ActionResult NotProjectCreditUpdateList(string checkboxId)
          {
              if (checkboxId == null)
              {
                  return Content("请先选择再做操作！");
              }
              string[] strUpdate = checkboxId.Split(',');
              foreach (var Id in strUpdate)
              {
                      ContinuingEducationRecordViewModel UpdateModel = new ContinuingEducationRecordViewModel();
                      UpdateModel = ContinuingEducationRecordService.GetEntities(p => p.Id == Convert.ToInt32(Id)).FirstOrDefault().ToViewModel();
                      UpdateModel.IsGranted = true;
                     bool UpdateSucess = ContinuingEducationRecordService.UpdateContinuingEducationRecord(UpdateModel.ToDataTransferObjectModel());
              }
              var result = ContinuingEducationRecordService.GetEntities(p => p.IsProjectCredit == false).OrderByDescending(p => p.Id).Count();
              //IList<ContinuingEducationRecordViewModel> resultlist = new List<ContinuingEducationRecordViewModel>();
              //foreach (var item in result)
              //{
              //    resultlist.Add(item.ToViewModel());
              //}
              //return View(resultlist);
              return Json(result, JsonRequestBehavior.AllowGet);
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

        /// <summary>
        /// 学分统计
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
       [HttpPost]
        public ActionResult CreditsStatisticsList(string Name, string startTime, string endTime, int page, int pageSize)
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
            IEnumerable<ContinuingEducationRecordTransferObject> result = SearchCreditList(Name, start,
               end, pageSize, page, ref totalPage);

            return Json(new { data = result, total = totalPage * pageSize }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 非项目类学分添加
        /// </summary>
        /// <param name="model"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddNotProjectCredit(ContinuingEducationRecordViewModel model, FormCollection collection)
        {
            
            string UserId = collection["UserId"].ToString();

            ///过滤掉UserId中的，
            int index = UserId.LastIndexOf(',');
            UserId = UserId.Remove(index);
          
            using (ApplicationDbContext userManager = new ApplicationDbContext())
            {
              var currentUser = userManager.Users.Where(p => p.Name == model.UserName&&p.Id==UserId).FirstOrDefault();
                if(currentUser!=null&&currentUser.Degree!=null&&currentUser.Duty!=null)
                {
                    model.UserDegree = currentUser.Degree.ToString();
                    model.UserDuty = currentUser.Duty.ToString();
                }

              var currentSection = userManager.Sections.Where(x => x.ApplicationUsers.Where(p => p.ApplicationUserId == UserId).Count() > 0).FirstOrDefault();
              model.Department = currentSection.Name.ToString();
            }
            model.CreatedTime=DateTime.Now;
            model.CreatedBy=User.Identity.Name;

            int resultId = this.ContinuingEducationRecordService.AddContinuingEducationRecord(model.ToDataTransferObjectModel());

            if (resultId > 0)
            {
                model.Id = resultId;
            }
            return View("NotProjectCreditDetail", model);
        }

        /// <summary>
        /// 更新非项目类学分
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
         [HttpPost]
        public ActionResult UpdateNotProjectCredit(ContinuingEducationRecordViewModel model,FormCollection collection)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string UserId = collection["UserId"].ToString();

            using (ApplicationDbContext userManager = new ApplicationDbContext())
            {
                var currentSection = userManager.Sections.Where(x => x.ApplicationUsers.Where(p => p.ApplicationUserId == UserId).Count() > 0).FirstOrDefault();
                model.Department = currentSection.Name.ToString();
            }

            model.UpdatedBy = User.Identity.Name;
            model.UpdatedTime = DateTime.Now;
            var result = ContinuingEducationRecordService.UpdateContinuingEducationRecord(model.ToDataTransferObjectModel());
            if (result)
            {
                return View("NotProjectCreditDetail", model);
            }
            else
            {
                ViewBag.updateResult = "更新失败,请重试！";
                return View(model);
            }
        }
        #endregion

        public ActionResult SelectUser()
        {
            return View();
        }

        public ActionResult SelectUserData(string Id)
        {
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
                        var sections = userManager.Sections.Where(x => x.DepartmentId == key).ToList();

                        return Json(sections.Select(x => x.ToDataTransferObjectModel()).ToList(), JsonRequestBehavior.AllowGet);
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
        private IEnumerable<ERPNWorkToDoTransferObject> SearchProcessingApplicationList(string projectName, string state, string freeze, DateTime start, DateTime end, int pageSize, int pageIndex, ref int totalPage)
        {
            bool hasRolesFlag = HasRolesFlag();
            IEnumerable<ERPNWorkToDoTransferObject> result;
            //非普通用户
            if (hasRolesFlag) 
            {
            //}
            //if (User.IsInRole(UserRoles.超级管理员.ToString())
            //    || User.IsInRole(UserRoles.科教科科长.ToString())
            //    || User.IsInRole(UserRoles.科教科科员.ToString()))
            //{
                result = ApplicationService.GetPageEntities(p => p.FormID == (int)ContinuingEducationTypeOfFormId.Application
                    && p.TimeStr.Value > start
                    && p.TimeStr.Value < end
                    && ((state == Constant.All) ? true : p.ApplicationStatus == state)
                    && (string.IsNullOrEmpty(projectName) ? true : (p.WenHao.Contains(projectName)))
                    && p.StateNow == "正在办理"
                    //&& p.ShenPiUserList.Contains(Constant.USER_NAME)
                    && ((freeze == Constant.All) ? true : p.IsLocked == (freeze == "冻结"))
                    , ApplicationSortField.TimeStr_Desc.ToString(), pageSize, pageIndex, out totalPage);
            }
            else
            {
                // 普通用户
                            result = ApplicationService.GetPageEntities(p => p.FormID == (int)ContinuingEducationTypeOfFormId.Application
                                && p.TimeStr.Value > start
                                && p.TimeStr.Value < end
                                && ((state == Constant.All) ? true : p.ApplicationStatus == state)
                                && (string.IsNullOrEmpty(projectName) ? true : (p.WenHao.Contains(projectName)))
                                && p.StateNow == "正在办理"
                                && p.UserName==User.Identity.Name
                                && ((freeze == Constant.All) ? true : p.IsLocked == (freeze == "冻结"))
                               , ApplicationSortField.TimeStr_Desc.ToString(), pageSize, pageIndex, out totalPage);
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
            IEnumerable<ERPNWorkToDoTransferObject> result;

            if (User.IsInRole(UserRoles.超级管理员.ToString())
                || User.IsInRole(UserRoles.科教科科长.ToString())
                || User.IsInRole(UserRoles.科教科科员.ToString()))
            {
                result = ApplicationService.GetPageEntities(p => p.FormID == (int)ScienceResearchTypeOfFormId.Application
                    && p.TimeStr.Value > start
                    && p.TimeStr.Value < end
                    && ((state == Constant.All) ? true : p.ApplicationStatus == state)
                    && (string.IsNullOrEmpty(projectName) ? true : (p.WenHao.Contains(projectName)))
                    && p.StateNow == "正在办理"
                    //&& p.ShenPiUserList.Contains(Constant.USER_NAME)
                    && ((freeze == Constant.All) ? true : p.IsLocked == (freeze == "冻结"))

                    && p.ProjectStatus == ApplicationStatus.BigProjectProcessing.ToString(), ApplicationSortField.TimeStr_Desc.ToString(), pageSize, pageIndex, out totalPage);
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

                            result = ApplicationService.GetPageEntities(p => p.FormID == (int)ScienceResearchTypeOfFormId.Application
                                && p.TimeStr.Value > start
                                && p.TimeStr.Value < end
                                && ((state == Constant.All) ? true : p.ApplicationStatus == state)
                                && (string.IsNullOrEmpty(projectName) ? true : (p.WenHao.Contains(projectName)))
                                && p.StateNow == "正在办理"
                                && userNamesOfCurrentSection.Contains(p.UserName)
                                && ((freeze == Constant.All) ? true : p.IsLocked == (freeze == "冻结"))

                                && p.ProjectStatus == ApplicationStatus.BigProjectProcessing.ToString(), ApplicationSortField.TimeStr_Desc.ToString(), pageSize, pageIndex, out totalPage);
                        }
                        else
                        {
                            // 普通员工
                            result = ApplicationService.GetPageEntities(p => p.FormID == (int)ScienceResearchTypeOfFormId.Application
                                && p.TimeStr.Value > start
                                && p.TimeStr.Value < end
                                && ((state == Constant.All) ? true : p.ApplicationStatus == state)
                                && (string.IsNullOrEmpty(projectName) ? true : (p.WenHao.Contains(projectName)))
                                && p.StateNow == "正在办理"
                                && p.UserName == User.Identity.Name
                                && ((freeze == Constant.All) ? true : p.IsLocked == (freeze == "冻结"))

                                && p.ProjectStatus == ApplicationStatus.BigProjectProcessing.ToString(), ApplicationSortField.TimeStr_Desc.ToString(), pageSize, pageIndex, out totalPage);
                        }
                    }
                    else
                    {
                        result = new List<ERPNWorkToDoTransferObject>();
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 非项目类学分查询
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalPage"></param>
        /// <returns></returns>
        private IEnumerable<ContinuingEducationRecordTransferObject> SearchCreditStatisticsList(string Name, string state,DateTime start, DateTime end, int pageSize, int pageIndex, ref int totalPage)
        {
            IEnumerable<ContinuingEducationRecordTransferObject> result;

            if (User.IsInRole(UserRoles.超级管理员.ToString())
                || User.IsInRole(UserRoles.科教科科长.ToString())
                || User.IsInRole(UserRoles.科教科科员.ToString()))
            {

                result = ContinuingEducationRecordService.GetPageNotProjectEntities(p => p.CreatedTime.Value > start
                    && p.CreatedTime.Value < end
                    && (string.IsNullOrEmpty(Name) ? true : (p.UserName.Contains(Name)))
                    && ((state == Constant.All) ? true : p.IsGranted ==(state=="false"))
                    &&(p.IsProjectCredit==false)
                    , ContinuingRecordSortField.CreatedTime_Desc.ToString(), pageSize, pageIndex, out totalPage);
            }
            else
            {
                result = new List<ContinuingEducationRecordTransferObject>();
            }
            return result;
        }

       /// <summary>
       /// 学分统计查询
       /// </summary>
       /// <param name="Name"></param>
       /// <param name="start"></param>
       /// <param name="end"></param>
       /// <param name="pageSize"></param>
       /// <param name="pageIndex"></param>
       /// <param name="totalPage"></param>
       /// <returns></returns>
        private IEnumerable<ContinuingEducationRecordTransferObject> SearchCreditList(string Name, DateTime start, DateTime end, int pageSize, int pageIndex, ref int totalPage)
        {
            IEnumerable<ContinuingEducationRecordTransferObject> result;

            if (User.IsInRole(UserRoles.超级管理员.ToString())
                || User.IsInRole(UserRoles.科教科科长.ToString())
                || User.IsInRole(UserRoles.科教科科员.ToString()))
            {

                result = ContinuingEducationRecordService.GetPageEntities(p => p.CreatedTime.Value > start
                    && p.CreatedTime.Value < end
                    && (string.IsNullOrEmpty(Name) ? true : (p.UserName.Contains(Name)))
                    , ContinuingRecordSortField.CreatedTime_Desc.ToString(), pageSize, pageIndex, out totalPage);
            }
            else
            {
                result = new List<ContinuingEducationRecordTransferObject>();
            }
            return result;
        }

        /// <summary>
        /// 判断登录者角色是否有权限查看申请书列表内容
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
        /// 判断经费申请书节点的角色和登录人角色是否对应
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
    }
}