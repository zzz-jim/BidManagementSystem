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
using ScientificResearch.DomainModel;
using System.Data.Entity;
using System.Collections.Specialized;
using System.Data;

namespace UI.ScientificResearch.Areas.Education.Controllers
{
    [CheckLogin]
    [Authorize(Roles = "普通用户")]
    public class PaperPublishController : Controller
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
        private IProjectBonusCreditService ProjectBonusCreditService;
        private ITravelFundsDetailService TravelFundsDetailService;
        private IFundsThresholdService FundsThresholdService;
        private IStatisticService StatisticService;


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

        public PaperPublishController()
            : this(
                new ERPNFormServiceImplement(),
                new ERPBuMenServiceImplement(),
                new ERPRiZhiServiceImplement(),
                new ApplicationServiceImplement(),
                new ERPNWorkFlowServiceImplement(),
                new ERPNWorkFlowNodeServiceImplement(),
                new FundsRecordServiceImplement(),
                new ProjectRecordServiceImplement(),
                new ProjectBonusCreditServiceImplement(),
                new TravelFundsDetailServiceImplement(),
                new FundsThresholdServiceImplement(),
                new SessionManager(),
                new StatisticServiceImplement()
            )
        {
        }

        public PaperPublishController(
            IERPNFormService eRPNFormService,
            IERPBuMenService eRPBuMenService,
            IERPRiZhiService eRPRiZhiService,
            IApplicationService applicationService,
            IERPNWorkFlowService eRPNWorkFlowService,
            IERPNWorkFlowNodeService eRPNWorkFlowNodeService,
            IFundsRecordService eFundsRecordService,
            IProjectRecordService eProjectRecordService,
            IProjectBonusCreditService eProjectBonusCreditService,
            ITravelFundsDetailService eTravelFundsDetailService,
            IFundsThresholdService eFundsThresholdService,
            ISession session,
            IStatisticService statisticService
            )
        {

            this.ERPNFormService = eRPNFormService;
            this.ERPBuMenService = eRPBuMenService;
            this.ERPRiZhiService = eRPRiZhiService;
            this.ApplicationService = applicationService;
            this.ERPNWorkFlowService = eRPNWorkFlowService;
            this.ERPNWorkFlowNodeService = eRPNWorkFlowNodeService;
            this.FundsRecordService = eFundsRecordService;
            this.ProjectRecordService = eProjectRecordService;
            this.ProjectBonusCreditService = eProjectBonusCreditService;
            this.TravelFundsDetailService = eTravelFundsDetailService;
            this.FundsThresholdService = eFundsThresholdService;
            this.MySession = session;
            this.StatisticService = statisticService;

            //// TODO: delete the debug code..
            //MySession[SessionKeyEnum.SectionName] = "Nurse";
        }

        #endregion

        #region Get Action

        /// <summary>
        /// 论文发表导航首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Module = "论文发表";
            ViewBag.Title = "简介";

            return View();
        }

        /// <summary>
        /// 论文发表导航首页待办事宜容器页面
        /// </summary>
        /// <returns></returns>
        public ActionResult WorkContainer()
        {
            ViewBag.Module = "论文发表";
            ViewBag.Title = "待办事宜";

            return View();
        }

        /// <summary>
        /// 论文发表导航首页资金管理容器页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ScienceFundsManageContainer()
        {
            ViewBag.Module = "论文发表";
            ViewBag.Title = "资金管理";

            return View();
        }

        /// <summary>
        /// 论文发表导航首页论文配置容器页面
        /// </summary>
        /// <returns></returns>
        public ActionResult PaperConfigManageContainer()
        {
            ViewBag.Module = "论文发表";
            ViewBag.Title = "论文配置";

            return View();
        }

        /// <summary>
        /// 论文发表导航首页学分配置容器页面
        /// </summary>
        /// <returns></returns>
        public ActionResult PaperCreditManageContainer()
        {
            ViewBag.Module = "论文发表";
            ViewBag.Title = "学分配置";

            return View();
        }

        /// <summary>
        /// 创建申请书
        /// </summary>
        /// <returns></returns>
        public ActionResult SubmitApplication(string id)
        {
            int formId = (int)PaperPublishTypeOfFormId.Application;
            int workflowId = (int)PaperPublishTypeOfWorkflowId.Application;
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

                // 自动生成 编号
               // model.BeiYong1 = CommonHelper.GenerateProjectNumber(ApplicationType.ScienceResearch);
                string Number = CommonHelper.GenerateProjectNumber(ApplicationType.PublishPaper);

                string content = formModel.ContentStr;
                if (content.Contains(""))
                {
                    string oldvalue = "Text569823366";
                    string newvalue = @"Text569823366"" value=""" + Number + @"""";
                    content = content.Replace(oldvalue, newvalue);
                }


                //批量设置字段的可写、保密属性
                //设置上传的附件为空
                model.FormContent = content;
                model.FormContent = model.FormContent.Replace(Constant.MacroSectionString, MySession[SessionKeyEnum.SectionName].ToString());
                model.FormContent = model.FormContent.Replace(Constant.MacroUserNameString, User.Identity.Name);

                ////绑定工作名称
                var workFlowModel = ERPNWorkFlowService.GetEntityById(workflowId);
                model.WorkName = User.Identity.Name + Constant.DoubleHyphenString + workFlowModel.WorkFlowName;

                //绑定下一节点
                string jiedianid = string.Empty;
                currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == workflowId && p.NodeAddr == Constant.MacroStartString).First();
                model.JieDianID = currentNode.ID;
                model.JieDianName = currentNode.NodeName;
            }

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
            string[] formItemValue = model.FormValues.Split(Constant.SharpChar);
            ViewBag.ApprovalStep = model.JieDianName;
            ViewBag.Id = id;
            //页面上刊物名称的value
            ViewBag.PublicationName = formItemValue[0].ToString();

            //批量设置字段的可写、保密属性
            ViewBag.PiLiangSet = CommonHelper.SetTheWriteAndHiddenField(currentNode.CanWriteSet, currentNode.SecretSet, formItemArray);

            // 设置审批流程的所有结点和当前结点
            var list = ERPNWorkFlowNodeService.GetEntities(x => x.WorkFlowID == model.WorkFlowID).OrderBy(p => p.NodeSerils).ToList();
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
        /// 打印生成介紹信
        /// </summary>
        /// <returns></returns>
        public ActionResult ProjectEstablish(string id, string nextaction)
        {
            int applicaionId = Convert.ToInt32(id);
            int formId = (int)PaperPublishTypeOfFormId.Establishment;
            int workflowId = (int)PaperPublishTypeOfWorkflowId.Establishment;
            ERPNWorkFlowNodeTransferObject currentNode;

            var resultOfContract = ApplicationService.GetEntities(p => p.ApplicationId == applicaionId && p.FormID == formId);

            if (resultOfContract.Count > 0)
            {
                ViewBag.Id = id;

                return View("ProjectEstablishDetail", resultOfContract.FirstOrDefault().ToViewModel());
            }

            //点击保存后的view
            if (nextaction == "save")
            {
                ViewBag.Id = id;
                ViewBag.act = "save";
                ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();
                model = ApplicationService.GetEntities(p => p.ID == applicaionId).FirstOrDefault().ToViewModel();

                return View(model);
            }
            //点击修改数据后的view
            else if (nextaction == "updatedata")
            {
                ViewBag.Id = id;
                ViewBag.act = "updatedata";

                ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();
                model = ApplicationService.GetEntities(p => p.ID == applicaionId).FirstOrDefault().ToViewModel();
                return View(model);
            }
            //点击上报时的view
            else
            {
                ViewBag.Id = id;
                ViewBag.act = "reported";

                var result = ApplicationService.GetEntities(p => p.ApplicationId == applicaionId && p.FormID == (int)formId).ToList();

                if (result.Count > 0)//已经填写了合同记录,显示改条合同记录
                {
                    ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();
                    model = result.FirstOrDefault().ToViewModel();

                    return View(model);
                }
                else //新建合同记录
                {
                    ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();
                    var appresult = ApplicationService.GetEntities(p => p.ID == applicaionId).FirstOrDefault();
                    string paperName = appresult.WenHao;//论文名称
                    var keyValuerArray = appresult.FormValues.Split(Constant.SharpChar);

                    #region

                    string magzineName = keyValuerArray[0].ToString();//刊物名称
                    string mainAuthor = keyValuerArray[1].ToString();//第一作者
                    string paperStartDate = keyValuerArray[2].ToString();// 投稿开始时间
                    string paperEndDate = keyValuerArray[3].ToString();// 投稿结束时间
                    string applyDate = keyValuerArray[4].ToString();
                    string department = keyValuerArray[6].ToString();//科室
                    string investor = keyValuerArray[7].ToString();// 基金资助方
                    string summarize = keyValuerArray[8].ToString();//概述

                    //string birthday = keyvaluearry[0].ToString();//出生日期
                    //string title = keyvaluearry[0].ToString();//职称
                    //string jobName = keyvaluearry[0].ToString();//职务
                    //string sex = keyvaluearry[0].ToString();//性别
                    //string degree = keyvaluearry[0].ToString();// 学历
                    //string otherAuthor = keyvaluearry[0].ToString();// 其他作者
                    //string approveBy = keyvaluearry[0].ToString();// 审批人

                    #endregion

                    model.WorkFlowID = workflowId;
                    model.FormID = formId;
                    model.ApplicationId = applicaionId;
                  
                    //加载表单内容
                    var temerpnformmodel = ERPNFormService.GetEntityById(formId).ToViewModel();

                    string content = temerpnformmodel.ContentStr;

                    IList<string> keyList = new List<string>();
                    keyList.Add("Text1978713851");
                    keyList.Add("Text1783445882");
                    keyList.Add("Text435761615");
                    keyList.Add("Text289827346");
                    keyList.Add("Text309804476");
                    keyList.Add("TextArea683159807");
                    keyList.Add("Date1362114152");
                    keyList.Add("Date256038949");
                    keyList.Add("Date442495555");

                    string oldValue;
                    string newValue;

                    if (content.Contains("Text1086959023"))
                    {
                        oldValue = "Text1086959023";
                        newValue = @"Text1086959023"" value=""" + paperName + @"""";
                        content = content.Replace(oldValue, newValue);
                    }

                    if (content.Contains("Text1978713851") && content.Contains("Text1783445882") //&& content.Contains("Text435761615")
                        && content.Contains("Text289827346") && content.Contains("Text309804476"))//&& content.Contains("TextArea683159807"))//项目名称
                    {
                        oldValue = "Text1978713851";
                        newValue = @"Text1978713851"" value=""" + magzineName + @"""";
                        content = content.Replace(oldValue, newValue);

                        oldValue = "Text1783445882";
                        newValue = @"Text1783445882"" value=""" + mainAuthor + @"""";
                        content = content.Replace(oldValue, newValue);

                        oldValue = "Text289827346";
                        newValue = @"Text289827346"" value=""" + department + @"""";
                        content = content.Replace(oldValue, newValue);

                        oldValue = "Text309804476";
                        newValue = @"Text309804476"" value=""" + investor + @"""";
                        content = content.Replace(oldValue, newValue);
                    }

                    if (content.Contains("Date1362114152") && content.Contains("Date256038949") && content.Contains("Date442495555"))
                    {
                        oldValue = "Date1362114152Y";
                        newValue = @"Date1362114152Y"" value=""" + Convert.ToDateTime(paperStartDate).Year + @"""";
                        content = content.Replace(oldValue, newValue);

                        oldValue = "Date1362114152M";
                        newValue = @"Date1362114152M"" value=""" + Convert.ToDateTime(paperStartDate).Month + @"""";
                        content = content.Replace(oldValue, newValue);

                        oldValue = "Date1362114152D";
                        newValue = @"Date1362114152D"" value=""" + Convert.ToDateTime(paperStartDate).Day + @"""";
                        content = content.Replace(oldValue, newValue);

                        oldValue = "Date256038949Y";
                        newValue = @"Date256038949Y"" value=""" + Convert.ToDateTime(paperEndDate).Year + @"""";
                        content = content.Replace(oldValue, newValue);

                        oldValue = "Date256038949M";
                        newValue = @"Date256038949M"" value=""" + Convert.ToDateTime(paperEndDate).Month + @"""";
                        content = content.Replace(oldValue, newValue);

                        oldValue = "Date256038949D";
                        newValue = @"Date256038949D"" value=""" + Convert.ToDateTime(paperEndDate).Day + @"""";
                        content = content.Replace(oldValue, newValue);


                        oldValue = "Date442495555Y";
                        newValue = @"Date442495555Y"" value=""" + Convert.ToDateTime(applyDate).Year + @"""";
                        content = content.Replace(oldValue, newValue);

                        oldValue = "Date442495555M";
                        newValue = @"Date442495555M"" value=""" + Convert.ToDateTime(applyDate).Month + @"""";
                        content = content.Replace(oldValue, newValue);

                        oldValue = "Date442495555D";
                        newValue = @"Date442495555D"" value=""" + Convert.ToDateTime(applyDate).Day + @"""";
                        content = content.Replace(oldValue, newValue);
                    }

                    model.FormContent = content;

                    var temperpnworkflowmodel = ERPNWorkFlowService.GetEntityById(workflowId);
                    model.WorkName = "admin" + "--" + temperpnworkflowmodel.WorkFlowName;
                    
                    //获取当前表单对应的工作数据列
                    string[] formItemArray = temerpnformmodel.ItemsList.Split(Constant.SplitChar);
                    //绑定下一节点
                    currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == workflowId && p.NodeAddr == Constant.MacroStartString).First();
                    model.JieDianID = currentNode.ID;
                    model.JieDianName = currentNode.NodeName;

                    //批量设置字段的可写、保密属性
                    ViewBag.PiLiangSet = CommonHelper.SetTheWriteAndHiddenField(currentNode.CanWriteSet, currentNode.SecretSet, formItemArray);
                    return View(model);
                }
            }
        }

        /// <summary>
        /// 费用报销单显示
        /// </summary>
        /// <returns></returns>
        public ActionResult ReimburseProcess(string id, string nextaction)
        {
            int applicaionId = Convert.ToInt32(id);
            ViewBag.Id = id;
            string k;
            //点击保存后的view
            if (nextaction == "save")
            {
                var fundsModel = FundsRecordService.GetAllById(applicaionId);

                var todoModel = ApplicationService.GetEntityById(fundsModel.ApplicationId);

                string projectType = todoModel.FormValues.Split('#')[0];

                using (var context = new CSPostOAEntities())
                {
                    var result = context.PaperMagazine.Where(x => x.IsDeleted == false && x.Name == projectType).FirstOrDefault();
                    k = result.Level;
                }

                ///获取经费记录类型
                using (var context = new CSPostOAEntities())
                {
                    ViewBag.FundsType = (from h in context.FundsThreshold
                                         where (h.IsDeleted == false && h.ModuleName == ModuleNameOfScienceResearch.PaperPublish.ToString() && h.ProjectType == k)

                                         select new SelectListItem()
                                         {
                                             Text = h.FundsType,
                                             Value = h.FundsType,
                                         }).ToList();
                }

                ViewBag.act = "save";
                FundsRecordViewModel model = new FundsRecordViewModel();
                model.CreatedTime = DateTime.Now;
                model = FundsRecordService.GetEntityById(applicaionId).ToViewModel();
                return View(model);
            }
            //点击上报时的view
            else
            {
                var todoModel = ApplicationService.GetEntityById(applicaionId);

                string projectType = todoModel.FormValues.Split('#')[0];

                using (var context = new CSPostOAEntities())
                {
                    var result = context.PaperMagazine.Where(x => x.IsDeleted == false && x.Name == projectType).FirstOrDefault();
                    k = result.Level;
                }

                ///获取经费记录类型
                using (var context = new CSPostOAEntities())
                {
                    ViewBag.FundsType = (from h in context.FundsThreshold
                                         where (h.IsDeleted == false && h.ModuleName == ModuleNameOfScienceResearch.PaperPublish.ToString() && h.ProjectType == k)

                                         select new SelectListItem()
                                         {
                                             Text = h.FundsType,
                                             Value = h.FundsType,
                                         }).ToList();
                }

                FundsRecordViewModel model = new FundsRecordViewModel();

                int workflowId = (int)PaperPublishTypeOfWorkflowId.FeeReimbursement;
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
                string SecretStr = MyJieDianNow.SecretSet;
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
            string k;
            var fundsModel = FundsRecordService.GetAllById(Convert.ToInt32(id));

            var todoModel = ApplicationService.GetEntityById(fundsModel.ApplicationId);

            string projectType = todoModel.FormValues.Split('#')[0];

            using (var context = new CSPostOAEntities())
            {
                var result = context.PaperMagazine.Where(x => x.IsDeleted == false && x.Name == projectType).FirstOrDefault();
                k = result.Level;
            }
            ///获取经费记录类型
            using (var context = new CSPostOAEntities())
            {
                ViewBag.FundsType = (from h in context.FundsThreshold
                                     where (h.IsDeleted == false && h.ModuleName == ModuleNameOfScienceResearch.PaperPublish.ToString() && h.ProjectType == k)

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
                int workflowId = (int)PaperPublishTypeOfWorkflowId.TravelReimbursement;

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
        /// 论文登记审批单
        /// </summary>
        /// <returns></returns>
        public ActionResult ConclusionsAgree(string id, string nextaction)
        {
            int id1 = Convert.ToInt32(id);//applicationid
            ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();
            model = ApplicationService.GetEntities(p => p.ApplicationId == id1 && p.FormID == Convert.ToInt32(PaperPublishTypeOfFormId.Conclusion)).FirstOrDefault().ToViewModel();

            //继续审批

            bool isRejected = Convert.ToBoolean(model.IsRejected);
            bool isTemporary = Convert.ToBoolean(model.IsTemporary);

            ERPNWorkFlowNodeTransferObject currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.ID == model.JieDianID).First();

            model.ApplicationId = id1;
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
        public ActionResult ConclusionsRejected(string id)
        {
            int id1 = Convert.ToInt32(id);//applicationid
            ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();
            model = ApplicationService.GetEntities(p => p.ApplicationId == id1 && p.FormID == Convert.ToInt32(PaperPublishTypeOfFormId.Conclusion)).FirstOrDefault().ToViewModel();
            //继续审批

            bool isRejected = Convert.ToBoolean(model.IsRejected);
            bool isTemporary = Convert.ToBoolean(model.IsTemporary);

            ERPNWorkFlowNodeTransferObject currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.ID == model.JieDianID).First();

            model.ApplicationId = id1;
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
        /// 论文登记显示
        /// </summary>
        /// <returns></returns>
        public ActionResult Conclusions(string id, string nextaction)
        {
            int applicationid = Convert.ToInt32(id);//applicationid
            int formId = (int)PaperPublishTypeOfFormId.Conclusion;
            int workflowId = (int)PaperPublishTypeOfWorkflowId.Conclusion;
          //  ViewBag.Id = id;

            var countOfConlusion = ApplicationService.GetEntities(p => p.ApplicationId == applicationid && p.FormID == formId).Count;

            if (countOfConlusion > 0)
            {
                ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();
                var appresult = ApplicationService.GetEntities(p => p.ID == applicationid).FirstOrDefault();
            
                model = ApplicationService.GetEntities(p => p.ApplicationId == applicationid && p.FormID == Convert.ToInt32(PaperPublishTypeOfFormId.Conclusion)).FirstOrDefault().ToViewModel();
                model.WenHao = appresult.WenHao;//论文名称
                // 将申请书的form values 暂时赋给当前model,供前台展示申请书的内容。
                model.FormValues = appresult.FormValues;
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

            ////判断是否已经进入到项目结题审批中
            //var applicaitonodelcount = ApplicationService.GetEntities(p => p.ApplicationId == applicationid && p.FormID == formId).Count();

            //if (applicaitonodelcount > 0)
            //{
            //    ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();
            //    model = ApplicationService.GetEntities(p => p.ApplicationId == applicationid && p.FormID ==formId).FirstOrDefault().ToViewModel();
            //    if (model.IsTemporary == true)
            //    {
            //        ViewBag.Id = model.ApplicationId;
            //        ViewBag.act = "save";
            //        return View(model);
            //    }
            //    else
            //    {
            //        return RedirectToAction("ConclusionsAgree", new { id = id });
            //    }
            //}

            //点击保存后的view  //点击修改数据后的view
            if (nextaction == "save")
            {
              
                ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();
                var appresult = ApplicationService.GetEntities(p => p.ID == applicationid).FirstOrDefault();
                model = ApplicationService.GetEntities(p => p.FormID == formId && p.ApplicationId == applicationid).FirstOrDefault().ToViewModel();
                model.WenHao = appresult.WenHao;
                model.FormValues = appresult.FormValues;
                ViewBag.Id = model.ApplicationId;
                ViewBag.act = "save";
                return View(model);
            }
            //else if (nextaction == "updatedata")
            //{
            //    ViewBag.Id = id;
            //    ViewBag.act = "updatedata";
            //    ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();
            //    var appresult = ApplicationService.GetEntities(p => p.ID == applicationid).FirstOrDefault();
            //    model = ApplicationService.GetEntities(p => p.FormID == formId && p.ApplicationId == applicationid).FirstOrDefault().ToViewModel();
            //    model.WenHao = appresult.WenHao;
            //    model.FormValues = appresult.FormValues;
            //    return View(model);
            //}
            //点击上报时的view
            else
            {
                ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();
                ERPNWorkFlowNodeTransferObject currentNode;

                var appresult = ApplicationService.GetEntities(p => p.ID == applicationid).FirstOrDefault();
                model.WenHao = appresult.WenHao;//论文名称

                // 将申请书的form values 暂时赋给当前model,供前台展示申请书的内容。
                model.FormValues = appresult.FormValues;
                model.WorkFlowID = workflowId;
                model.FormID = formId;

                //加载表单内容
                var temerpnformmodel = ERPNFormService.GetEntityById(formId).ToViewModel();
                //获取当前表单对应的工作数据列
                string[] formItemArray = temerpnformmodel.ItemsList.Split(Constant.SplitChar);

                model.FormContent = temerpnformmodel.ContentStr;

                var formValues = appresult.FormValues.Split(Constant.SharpChar);

                if (formValues.Count() > 7)
                {
                    NameValueCollection collection = new NameValueCollection();

                    collection.Add("Text1978713851", formValues[0].ToString()); // magzineName
                    collection.Add("Text1783445882", formValues[1].ToString()); // mainAuthor
                    collection.Add("Date1362114152", formValues[2].ToString()); // paperStartDate
                    collection.Add("Date256038949", formValues[3].ToString()); // paperEndDate
                    collection.Add("Date442495555", formValues[4].ToString()); // applyDate
                    collection.Add("Text435761615", formValues[5].ToString()); // applyMan
                    collection.Add("Text289827346", formValues[6].ToString()); // department
                    collection.Add("Text309804476", formValues[7].ToString()); // investor

                    model.FormContent = FormContentReplaceHelper.ReplaceFormContentValue(model.FormContent, collection);
                }

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
        /// 判断申请书状态，返回状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ReturnStateValue(string id)
        {
            int id1 = Convert.ToInt32(id);//ERPNWorkToDo的ID

            var result = ApplicationService.GetEntityById(id1);
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
            if (result.WorkflowId == 1075)
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

        public ActionResult RetrunFreezeFlag(string id, string pagestate, string k)
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
                WorkName = "论文发表申请书",
            });
            return View(listResult);
        }

        /// <summary>
        /// 待办事宜
        /// </summary>
        /// <returns></returns>
        public ActionResult WorkToDoList()
        {
            var result = ERPRiZhiService.GetEntities(p => p.FKAction == "待办" && p.ModuleName == ModuleNameOfScienceResearch.PaperPublish.ToString()).OrderByDescending(p => p.ID).Take(20);
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
            var result = ERPRiZhiService.GetEntities(p => p.FKAction == "已办" && p.ModuleName == ModuleNameOfScienceResearch.PaperPublish.ToString()).OrderByDescending(p => p.ID).Take(20);
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
            var result = ERPRiZhiService.GetEntities(p => p.FKAction == "待办" && p.ModuleName == ModuleNameOfScienceResearch.PaperPublish.ToString()).OrderByDescending(p => p.ID).Take(10);
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
                    && p.FormID == (int)PaperPublishTypeOfFormId.Application
                    && p.ProjectStatus == ApplicationStatus.BigProjectProcessing.ToString(), ApplicationSortField.TimeStr_Desc.ToString(), Constant.PageSize, 1, out totalPage);
            }
            else
            {
                result = ApplicationService.GetPageEntities(p => p.StateNow == "正在办理"
                    && p.UserName == User.Identity.Name
                    && p.FormID == (int)PaperPublishTypeOfFormId.Application
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
            var result = ProjectBonusCreditService.GetEntities(p => p.ModuleName == "论文发表").OrderByDescending(p => p.Id);
            IList<ProjectBonusCreditViewModel> resultlist = new List<ProjectBonusCreditViewModel>();
            foreach (var item in result)
            {
                resultlist.Add(item.ToViewModel());
            }

            ViewBag.Module = "论文发表";
            ViewBag.Title = "资金管理";

            return View(resultlist);
        }

        /// <summary>
        /// 正在审批中的申请书列表，还未项目确立的申请书
        /// </summary>
        /// <returns></returns>
        public ActionResult ApplicationList()
        {

            ViewBag.Module = "论文发表";
            ViewBag.Title = "申请书";

            return View();
        }

        /// <summary>
        /// 过程记录列表查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ProcessingProjectList()
        {

            ViewBag.Module = "论文发表";
            ViewBag.Title = "过程记录";

            return View();
        }

        public ActionResult MagazineList()
        {
            using (var context = new CSPostOAEntities())
            {
                var result = context.PaperMagazine.Where(x => x.IsDeleted == false);

                return View(result.ToList<PaperMagazine>());
            }
        }


        public ActionResult LoadMagazineNameList()
        {
            using (var context = new CSPostOAEntities())
            {
                var result = context.PaperMagazine.Where(x => x.IsDeleted == false);

                return View(result.ToList<PaperMagazine>());
            }
        }

        public ActionResult PaperMagazineLevelList()
        {
            using (var context = new CSPostOAEntities())
            {
                var result = context.PaperMagazineLevel.Where(x => x.IsDeleted == false);

                return View(result.ToList<PaperMagazineLevel>());
            }
        }

        public ActionResult PaperMagazineTypeList()
        {
            using (var context = new CSPostOAEntities())
            {
                var result = context.PaperMagazineType.Where(x => x.IsDeleted == false);

                return View(result.ToList<PaperMagazineType>());
            }
        }

        public ActionResult CreateMagazine()
        {
            using (var context = new CSPostOAEntities())
            {
                ViewBag.PaperMagazineLevel = (from h in context.PaperMagazineLevel
                                              where h.IsDeleted == false
                                              select new SelectListItem()
                                              {
                                                  Text = h.LevelName,
                                                  Value = h.Id.ToString(),
                                              }
                                               ).ToList();

                ViewBag.PaperMagazineType = (from h in context.PaperMagazineType
                                             where h.IsDeleted == false
                                             select new SelectListItem()
                                             {
                                                 Text = h.TypeName,
                                                 Value = h.Id.ToString(),
                                             }).ToList();
                return View();
            }
        }

        public ActionResult EditMagazine(int id)
        {
            using (var context = new CSPostOAEntities())
            {
                var entity = context.PaperMagazine.Find(id);

                ViewBag.PaperMagazineLevel = (from h in context.PaperMagazineLevel
                                              where h.IsDeleted == false
                                              select new SelectListItem()
                                              {
                                                  Text = h.LevelName,
                                                  Value = h.Id.ToString(),
                                                  Selected = h.Id == entity.LevelId
                                              }
                                              ).ToList();

                ViewBag.PaperMagazineType = (from h in context.PaperMagazineType
                                             where h.IsDeleted == false
                                             select new SelectListItem()
                                             {
                                                 Text = h.TypeName,
                                                 Value = h.Id.ToString(),
                                                 Selected = h.Id == entity.TypeId
                                             }).ToList();

                return View(entity);
            }
        }

        public ActionResult CreateMagazineLevel()
        {
            return View(new PaperMagazineLevel());
        }

        public ActionResult EditMagazineLevel(int id)
        {
            using (var context = new CSPostOAEntities())
            {
                var entity = context.PaperMagazineLevel.Find(id);

                return View(entity);
            }
        }

        public ActionResult CreateMagazineType()
        {
            return View(new PaperMagazineType());
        }

        public ActionResult EditMagazineType(int id)
        {
            using (var context = new CSPostOAEntities())
            {
                var entity = context.PaperMagazineType.Find(id);

                return View(entity);
            }
        }

        public ActionResult DeleteMagazineLevel(int id)
        {
            bool result = false;

            using (var context = new CSPostOAEntities())
            {
                var model = context.PaperMagazineLevel.Find(id);
                model.IsDeleted = true;
                context.PaperMagazineLevel.Attach(model);
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

            return RedirectToAction("PaperMagazineLevelList");
        }

        public ActionResult DeleteMagazineType(int id)
        {
            bool result = false;

            using (var context = new CSPostOAEntities())
            {
                var model = context.PaperMagazineType.Find(id);
                model.IsDeleted = true;
                context.PaperMagazineType.Attach(model);
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

            return RedirectToAction("PaperMagazineTypeList");
        }

        public ActionResult DeleteMagazine(int id)
        {
            string deleteSuccess = "";

            using (var context = new CSPostOAEntities())
            {
                var model = context.PaperMagazine.Find(id);
                model.IsDeleted = true;
                context.PaperMagazine.Attach(model);
                context.Entry(model).State = EntityState.Modified;

                if (1 == context.SaveChanges())
                {
                    deleteSuccess = "Yes";
                }
                else
                {
                    deleteSuccess = "No";
                }
            }

            return Json(deleteSuccess,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 学分奖励配置列表
        /// </summary>
        /// <returns></returns>
        public ActionResult PaperBonusCreditList()
        {
            using (var context = new CSPostOAEntities())
            {
                var result = context.PaperBonusCredit.Where(x => x.IsDeleted == false);

                ViewBag.Module = "论文发表";
                ViewBag.Title = "学分配置";

                return View(result.ToList<PaperBonusCredit>());
            }
        }

        /// <summary>
        /// 新建学分奖励配置
        /// </summary>
        /// <returns></returns>
        public ActionResult CreatePaperBonusCredit()
        {
            using (var context = new CSPostOAEntities())
            {
                ViewBag.PaperMagazineLevel = (from h in context.PaperMagazineLevel
                                              where h.IsDeleted == false
                                              select new SelectListItem()
                                              {
                                                  Text = h.LevelName,
                                                  Value = h.Id.ToString(),
                                              }
                                               ).ToList();

                ViewBag.PaperMagazineType = (from h in context.PaperMagazineType
                                             where h.IsDeleted == false
                                             select new SelectListItem()
                                             {
                                                 Text = h.TypeName,
                                                 Value = h.Id.ToString(),
                                             }).ToList();
                return View();
            }
        }

        /// <summary>
        /// 新建学分奖励配置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreatePaperBonusCredit(PaperBonusCredit model)
        {
            int result = 0;

            model.IsDeleted = false;
            model.CreatedBy = User.Identity.Name;
            model.CreateTime = DateTime.Now;
            model.UpdateBy = User.Identity.Name;
            model.UpdateTime = DateTime.Now;

            using (var context = new CSPostOAEntities())
            {
                model.Level = context.PaperMagazineLevel.Find(model.LevelId).LevelName;
                model.Type = context.PaperMagazineType.Find(model.TypeId).TypeName;

                model.Name = model.Level + "--" + model.Type + "--";// +model.AuthorLevel;

                context.PaperBonusCredit.Add(model);

                // 已写入基础数据库的对象的数目
                if (1 == context.SaveChanges())
                {
                    result = model.Id;
                }
                else
                {
                    result = 0;
                }
            }

            if (result == 0)
            {
                return View(model);
            }
            else
            {
                return View("PaperBonusCreditDetail", model);
            }
        }

        /// <summary>
        /// 修改学分奖励配置
        /// </summary>
        /// <returns></returns>
        public ActionResult EditPaperBonusCredit(int id)
        {
            using (var context = new CSPostOAEntities())
            {
                var entity = context.PaperBonusCredit.Find(id);

                ViewBag.PaperMagazineLevel = (from h in context.PaperMagazineLevel
                                              where h.IsDeleted == false
                                              select new SelectListItem()
                                              {
                                                  Text = h.LevelName,
                                                  Value = h.Id.ToString(),
                                                  Selected = h.Id == entity.LevelId
                                              }
                                              ).ToList();

                ViewBag.PaperMagazineType = (from h in context.PaperMagazineType
                                             where h.IsDeleted == false
                                             select new SelectListItem()
                                             {
                                                 Text = h.TypeName,
                                                 Value = h.Id.ToString(),
                                                 Selected = h.Id == entity.TypeId
                                             }).ToList();

                return View(entity);
            }
        }

        /// <summary>
        /// 修改学分奖励配置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditPaperBonusCredit(PaperBonusCredit model)
        {
            bool result = false;

            model.UpdateBy = User.Identity.Name;
            model.UpdateTime = DateTime.Now;

            using (var context = new CSPostOAEntities())
            {
                model.Level = context.PaperMagazineLevel.Find(model.LevelId).LevelName;
                model.Type = context.PaperMagazineType.Find(model.TypeId).TypeName;

                context.PaperBonusCredit.Attach(model);
                context.Entry(model).State = EntityState.Modified;

                if (1 == context.SaveChanges())
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
                int id = model.Id;
                var entity = context.PaperBonusCredit.Find(id);

                ViewBag.PaperMagazineLevel = (from h in context.PaperMagazineLevel
                                              where h.IsDeleted == false
                                              select new SelectListItem()
                                              {
                                                  Text = h.LevelName,
                                                  Value = h.Id.ToString(),
                                                  Selected = h.Id == entity.LevelId
                                              }
                                              ).ToList();

                ViewBag.PaperMagazineType = (from h in context.PaperMagazineType
                                             where h.IsDeleted == false
                                             select new SelectListItem()
                                             {
                                                 Text = h.TypeName,
                                                 Value = h.Id.ToString(),
                                                 Selected = h.Id == entity.TypeId
                                             }).ToList();
            }
            ViewBag.updateSuccess = "true";
            return View(model);
        }

        /// <summary>
        /// 删除学分奖励配置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeletePaperBonusCredit(int id)
        {
            string deleteSuccess = "";

            using (var context = new CSPostOAEntities())
            {
                var model = context.PaperBonusCredit.Find(id);
                model.IsDeleted = true;
                context.PaperBonusCredit.Attach(model);
                context.Entry(model).State = EntityState.Modified;

                if (1 == context.SaveChanges())
                {
                    deleteSuccess = "Yes";
                }
                else
                {
                    deleteSuccess = "No";
                }
            }

            return Json(deleteSuccess, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateMagazine(PaperMagazine model)
        {
            ///验证
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (var context = new CSPostOAEntities())
            {
                ViewBag.PaperMagazineLevel = (from h in context.PaperMagazineLevel
                                              where h.IsDeleted == false
                                              select new SelectListItem()
                                              {
                                                  Text = h.LevelName,
                                                  Value = h.Id.ToString(),
                                              }
                                               ).ToList();

                ViewBag.PaperMagazineType = (from h in context.PaperMagazineType
                                             where h.IsDeleted == false
                                             select new SelectListItem()
                                             {
                                                 Text = h.TypeName,
                                                 Value = h.Id.ToString(),
                                             }).ToList();
            }

            int result = 0;

            model.IsDeleted = false;
            model.CreatedBy = User.Identity.Name;
            model.CreateTime = DateTime.Now;
            model.UpdateBy = User.Identity.Name;
            model.UpdateTime = DateTime.Now;

            using (var context = new CSPostOAEntities())
            {
                model.Level = context.PaperMagazineLevel.Find(model.LevelId).LevelName;
                model.Type = context.PaperMagazineType.Find(model.TypeId).TypeName;

                context.PaperMagazine.Add(model);

                // 已写入基础数据库的对象的数目
                if (1 == context.SaveChanges())
                {
                    result = model.Id;
                    ViewBag.createSuccessFlag = "Yes";
                }
                else
                {
                    result = 0;
                    ViewBag.createSuccessFlag = "No";
                }
            }
            
            return View( model);
        }

        [HttpPost]
        public ActionResult EditMagazine(PaperMagazine model)
        {
            bool result = false;

            model.UpdateBy = User.Identity.Name;
            model.UpdateTime = DateTime.Now;

            using (var context = new CSPostOAEntities())
            {
                model.Level = context.PaperMagazineLevel.Find(model.LevelId).LevelName;
                model.Type = context.PaperMagazineType.Find(model.TypeId).TypeName;

                context.PaperMagazine.Attach(model);
                context.Entry(model).State = EntityState.Modified;

                if (1 == context.SaveChanges())
                {
                    result = true;
                }
                else
                {
                    result = false;
                   // ViewBag.result = false;
                }
                int id = model.Id;
                var entity = context.PaperMagazine.Find(id);
                ViewBag.PaperMagazineLevel = (from h in context.PaperMagazineLevel
                                              where h.IsDeleted == false
                                              select new SelectListItem()
                                              {
                                                  Text = h.LevelName,
                                                  Value = h.Id.ToString(),
                                                  Selected = h.Id == entity.LevelId
                                              }
                                              ).ToList();

                ViewBag.PaperMagazineType = (from h in context.PaperMagazineType
                                             where h.IsDeleted == false
                                             select new SelectListItem()
                                             {
                                                 Text = h.TypeName,
                                                 Value = h.Id.ToString(),
                                                 Selected = h.Id == entity.TypeId
                                             }).ToList();
            }
            ViewBag.Isuccess = "true";
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateMagazineLevel(PaperMagazineLevel model)
        {
            int result = 0;

            model.IsDeleted = false;
            model.CreatedBy = User.Identity.Name;
            model.CreateTime = DateTime.Now;
            model.UpdateBy = User.Identity.Name;
            model.UpdateTime = DateTime.Now;

            using (var context = new CSPostOAEntities())
            {
                context.PaperMagazineLevel.Add(model);

                // 已写入基础数据库的对象的数目
                if (1 == context.SaveChanges())
                {
                    result = model.Id;
                }
                else
                {
                    result = 0;
                }
            }

            return View("MagazineLevelDetail", model);
        }

        [HttpPost]
        public ActionResult EditMagazineLevel(PaperMagazineLevel model)
        {
            bool result = false;

            model.UpdateBy = User.Identity.Name;
            model.UpdateTime = DateTime.Now;

            using (var context = new CSPostOAEntities())
            {
                context.PaperMagazineLevel.Attach(model);
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

            return View(result);
        }

        [HttpPost]
        public ActionResult CreateMagazineType(PaperMagazineType model)
        {
            int result = 0;

            model.IsDeleted = false;
            model.CreatedBy = User.Identity.Name;
            model.CreateTime = DateTime.Now;
            model.UpdateBy = User.Identity.Name;
            model.UpdateTime = DateTime.Now;

            using (var context = new CSPostOAEntities())
            {
                context.PaperMagazineType.Add(model);

                // 已写入基础数据库的对象的数目
                if (1 == context.SaveChanges())
                {
                    result = model.Id;
                }
                else
                {
                    result = 0;
                }
            }

            return View("MagazineTypeDetail", model);
        }

        [HttpPost]
        public ActionResult EditMagazineType(PaperMagazineType model)
        {
            bool result = false;

            model.UpdateBy = User.Identity.Name;
            model.UpdateTime = DateTime.Now;

            using (var context = new CSPostOAEntities())
            {
                context.PaperMagazineType.Attach(model);
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

            return View(result);
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
            ///验证
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.UserName = User.Identity.Name;
            model.TimeStr = DateTime.Now;

            var sectionName = MySession[SessionKeyEnum.SectionName].ToString();

            //加载表单内容
            var temerpnformmodel = ERPNFormService.GetEntityById((int)PaperPublishTypeOfFormId.Application).ToViewModel();
            model.FormContent = temerpnformmodel.ContentStr;
            model.FormContent = model.FormContent.Replace(Constant.MacroSectionString, sectionName);
            model.FormContent = model.FormContent.Replace(Constant.MacroUserNameString, User.Identity.Name);

            string formKeys = "刊物名称#第一作者#投稿开始时间#投稿结束时间#申请时间#申请人#科室#基金资助方#通讯作者#概述";
            string magzineName = collection["Drop1978713851"].ToString();
            string mainAuthor = collection["Text1783445882"].ToString();
            string paperStartDate = collection["Date1362114152"].ToString();
            string paperEndDate = collection["Date256038949"].ToString();
            string applyDate = collection["Date442495555"].ToString();
            string applyMan = collection["Text435761615"].ToString();
            string department = collection["Text289827346"].ToString();
            string investor = collection["Text309804476"].ToString();
            string communicationAuthor = collection["Text666"].ToString();
            string summarize = collection["TextArea683159807"].ToString();
            string formvalues = magzineName + "#" + mainAuthor + "#" + paperStartDate + "#" + paperEndDate + "#" + applyDate + "#" + applyMan + "#" + department + "#" + investor + "#" + communicationAuthor + "#" + summarize;

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

            if (flag=="Save")//保存
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
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                //已办
                MyRiZhi.UserName = User.Identity.Name; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();
                MyRiZhi.DoSomething = "保存(论文发表申请书)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.FkFormName = PaperPublishTypeOfFormId.Application.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = nworktodoid.ToString();
                int AddRiZhiSuccess = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                model.NWorkToDoID = nworktodoid;
                return Json(AddRiZhiSuccess, JsonRequestBehavior.AllowGet);
                //return RedirectToAction("SubmitApplication", new { id = model.NWorkToDoID });
            }
            else//上报
            {
                // 更改当前结点id和name
                var erpnrowkflownoderesult1 = ERPNWorkFlowNodeService.GetEntityById(model.JieDianID.Value).ToViewModel();
                string nextNodeSerial = erpnrowkflownoderesult1.NextNode;
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
                    ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                    ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();
                    //已办
                    MyRiZhi.UserName = User.Identity.Name; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();
                    MyRiZhi.DoSomething = User.Identity.Name + "上报(论文发表申请书)";
                    MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    // MyRiZhi.NotificationContent = "添加的" + model.WenHao + "已经提交";
                    MyRiZhi.FkFormName = PaperPublishTypeOfFormId.Application.ToString();
                    MyRiZhi.FKAction = "已办";
                    MyRiZhi.FKApplicationID = nworktodoid.ToString();
                    MyRiZhi.TimeStr = DateTime.Now;

                    int AddRiZhiSuccess = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                    //待办
                    MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkFlowID), sectionName);

                    MyRiZhi1.DoSomething = "需要审批(" + User.Identity.Name + "上报的论文发表申请书)";
                    MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    MyRiZhi1.FkFormName = PaperPublishTypeOfFormId.Application.ToString();
                    MyRiZhi1.FKAction = "待办";
                    MyRiZhi1.FKApplicationID = nworktodoid.ToString();
                    MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();

                    int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                    //return RedirectToAction("SubmitApplication", new { id = nworktodoid });
                    return Json(AddRiZhiSuccess, JsonRequestBehavior.AllowGet);
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
                    ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                    ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();
                    //更新已办
                    string nworktodoid = model.NWorkToDoID.ToString();
                    string formname = PaperPublishTypeOfFormId.Application.ToString();
                    MyRiZhi = ERPRiZhiService.GetEntities(p => p.FKApplicationID == nworktodoid && p.FkFormName == formname).FirstOrDefault().ToViewModel();
                    MyRiZhi.UserName = User.Identity.Name; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();
                    MyRiZhi.DoSomething = User.Identity.Name + "上报的(论文发表申请书)";
                    MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    MyRiZhi.FkFormName = PaperPublishTypeOfFormId.Application.ToString();
                    MyRiZhi.FKAction = "已办";
                    MyRiZhi.FKApplicationID = model.NWorkToDoID.ToString();
                    MyRiZhi.TimeStr = DateTime.Now;

                    bool updateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                    //新增待办
                    MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkFlowID), sectionName);
                    MyRiZhi1.DoSomething = "需要审批(" + User.Identity.Name + "上报的论文发表申请书)";
                    MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    MyRiZhi1.FkFormName = PaperPublishTypeOfFormId.Application.ToString();
                    MyRiZhi1.FKAction = "待办";
                    MyRiZhi1.FKApplicationID = model.NWorkToDoID.ToString();
                    MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();
                    int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                    //return RedirectToAction("SubmitApplication", new { id = model.NWorkToDoID });
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
            if (flag =="Approval")
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

                // TODO: jim 
                // 将当前结点改为开始结点

                model.ApplicationStatus = ApplicationStatus.ApplicationRejected.ToString();//申请书审批完成
                model.ProjectStatus = ApplicationStatus.ApplicationRejected.ToString();
                var workFlowNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == model.WorkFlowID).OrderBy(p => p.NodeSerils).Skip(1).Take(1).FirstOrDefault();

                model.JieDianID = workFlowNodeModel.ID;
                model.JieDianName = workFlowNodeModel.NodeName.ToString();
                bool isUpdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());

                nodeSerils = "驳回";
            }

            string formname = PaperPublishTypeOfFormId.Application.ToString();
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
                MyRiZhi.UserName = User.Identity.Name; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.DoSomething = "已审批" + model.UserName + "上报的论文发表申请书";
                MyRiZhi.FkFormName = PaperPublishTypeOfFormId.Application.ToString();
                MyRiZhi.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi.TimeStr = DateTime.Now;

                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //新生成待办，下一审批人的待办
                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.ID = rizhiresult.ID;
                MyRiZhi1.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.DoSomething = "需要审批(" + model.UserName + "上报的论文发表申请书)";
                MyRiZhi1.FkFormName = PaperPublishTypeOfFormId.Application.ToString();
                MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();

                ERPNWorkFlowNodeViewModel erpnWorkFlowNodeModel = new ERPNWorkFlowNodeViewModel();
                erpnWorkFlowNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == model.WorkFlowID && p.NodeSerils == nodeSerils).FirstOrDefault().ToViewModel();

                MyRiZhi1.UserName = erpnWorkFlowNodeModel.SPDefaultList;

                int returnid = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());
                //继续审批
                //return RedirectToAction("ApplicationAgree", new { id = model.NWorkToDoID });
                return Json(returnid, JsonRequestBehavior.AllowGet);
            }
            else if (nodeSerils == "结束")
            {
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();

                MyRiZhi.FKAction = "已办";
                MyRiZhi.ID = rizhiresult.ID;
                MyRiZhi.UserName = User.Identity.Name; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.DoSomething = "已审批" + model.UserName + "上报的论文发表申请书";
                MyRiZhi.FkFormName = PaperPublishTypeOfFormId.Application.ToString();
                MyRiZhi.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi.TimeStr = DateTime.Now;

                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //新生成新建项目确立的待办，申请人待办
                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.ID = rizhiresult.ID;
                MyRiZhi1.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.UserName = model.UserName;
                MyRiZhi1.DoSomething = model.UserName + "需要生成论文介绍信";
                MyRiZhi1.FkFormName = PaperPublishTypeOfFormId.Establishment.ToString();
                MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();

                int returnid = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());
                // 更新成功并且更新后，申请书状态变为 已经审批通过，跳转到项目确立页面
                //return RedirectToAction("ProjectEstablish", new { id = model.NWorkToDoID });
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
                MyRiZhi.UserName = User.Identity.Name; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.DoSomething = "已驳回" + model.UserName + "上报的论文发表申请书";
                MyRiZhi.FkFormName = PaperPublishTypeOfFormId.Application.ToString();
                MyRiZhi.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi.TimeStr = DateTime.Now;

                bool UpdateRiZhiSuccess1 = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());
                //申请人待办

                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.ID = rizhiresult.ID;
                MyRiZhi1.UserName = model.UserName;
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.DoSomething = "论文发表申请书被驳回，需修改数据";
                MyRiZhi1.FkFormName = PaperPublishTypeOfFormId.Application.ToString();
                MyRiZhi1.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();

                int returnid = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                //return RedirectToAction("ApplicationRejected", new { id = model.NWorkToDoID });
                return Json(returnid, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 申请书被驳回
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ApplicationRejected(string flag,ERPNWorkToDoViewModel model, FormCollection collection)
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
            var temerpnformmodel = ERPNFormService.GetEntityById((int)PaperPublishTypeOfFormId.Application).ToViewModel();
            model.FormContent = temerpnformmodel.ContentStr;
            model.FormContent = model.FormContent.Replace(Constant.MacroSectionString, sectionName);
            model.FormContent = model.FormContent.Replace(Constant.MacroUserNameString, User.Identity.Name);

            string formKeys = "刊物名称#第一作者#投稿开始时间#投稿结束时间#申请时间#申请人#科室#基金资助方#通讯作者#概述";
            string magzineName = collection["Drop1978713851"].ToString();
            string mainAuthor = collection["Text1783445882"].ToString();
            string paperStartDate = collection["Date1362114152"].ToString();
            string paperEndDate = collection["Date256038949"].ToString();
            string applyDate = collection["Date442495555"].ToString();
            string applyMan = collection["Text435761615"].ToString();
            string department = collection["Text289827346"].ToString();
            string investor = collection["Text309804476"].ToString();
            string communicationAuthor = collection["Text666"].ToString();
            string summarize = collection["TextArea683159807"].ToString();
            string formvalues = magzineName + "#" + mainAuthor + "#" + paperStartDate + "#" + paperEndDate + "#" + applyDate + "#" + applyMan + "#" + department + "#" + investor + "#" + communicationAuthor + "#" + summarize;

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
                MyRiZhi.DoSomething = "保存(论文发表申请书)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.FkFormName = PaperPublishTypeOfFormId.Application.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.TimeStr = DateTime.Now;
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();
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
                string formname = PaperPublishTypeOfFormId.Application.ToString();
                MyRiZhi = ERPRiZhiService.GetEntities(p => p.FKApplicationID == nworktodoid && p.FKAction == "待办").FirstOrDefault().ToViewModel();
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.DoSomething = User.Identity.Name + "上报的(论文发表申请书)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.FkFormName = PaperPublishTypeOfFormId.Application.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi.TimeStr = DateTime.Now;
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();

                bool updateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //新增待办
                MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkFlowID), sectionName);
                MyRiZhi1.DoSomething = "需要审批(" + User.Identity.Name + "上报的论文发表申请书)";
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.FkFormName = PaperPublishTypeOfFormId.Application.ToString();
                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.FKApplicationID = model.NWorkToDoID.ToString();
                MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();
                int UpdateRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                return Json(UpdateRiZhi1Success, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 打印生成介紹信
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ProjectEstablish(ERPNWorkToDoViewModel model, FormCollection collection)
        {
            int formId = (int)PaperPublishTypeOfFormId.Establishment;

            model.UserName = User.Identity.Name;
            model.TimeStr = DateTime.Now;
            var sectionName = MySession[SessionKeyEnum.SectionName].ToString();

            //加载表单内容
            var temerpnformmodel = ERPNFormService.GetEntityById(formId).ToViewModel();
            model.FormContent = temerpnformmodel.ContentStr;

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

            if (collection["Save"] != null)//保存
            {
                //日志
                //添加申请人保存
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                //已办
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();
                MyRiZhi.DoSomething = "保存(合同记录)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.FkFormName = PaperPublishTypeOfFormId.Establishment.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi.TimeStr = DateTime.Now;

                int returnRiZhiId = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //第一次保存
                if (model.NWorkToDoID == 0)
                {
                    string act = "save";
                    model.IsTemporary = true;
                    model.IsDeleted = false;

                    int jiedianid = Convert.ToInt32(model.JieDianID);
                    var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                    model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));

                    int returnid = this.ApplicationService.AddApplication(model.ToDataTransferObjectModel());

                    //更新申请书中的ApplicationStatus为ProjectEstablishing
                    ERPNWorkToDoViewModel erpnworktodomodel = new ERPNWorkToDoViewModel();
                    erpnworktodomodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                    erpnworktodomodel.ApplicationStatus = ApplicationStatus.ProjectEstablishing.ToString();
                    erpnworktodomodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    bool isUpdateSuccess = ApplicationService.UpdateApplication(erpnworktodomodel.ToDataTransferObjectModel());

                    return RedirectToAction("ProjectEstablish", new { id = returnid, nextaction = act });
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

                    //更新申请书中的ApplicationStatus为ProjectEstablishing
                    ERPNWorkToDoViewModel erpnworktodomodel = new ERPNWorkToDoViewModel();
                    erpnworktodomodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                    erpnworktodomodel.ApplicationStatus = ApplicationStatus.ProjectEstablishing.ToString();
                    erpnworktodomodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    bool isUpdateSuccess = ApplicationService.UpdateApplication(erpnworktodomodel.ToDataTransferObjectModel());

                    return RedirectToAction("ProjectEstablish", new { id = model.NWorkToDoID, nextaction = act });
                }
            }
            //上报
            else if (collection["Reported"] != null)
            {
                ERPNWorkToDoViewModel erpnworktodomodel = new ERPNWorkToDoViewModel();
                erpnworktodomodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                //写系统日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();

                //待办变成已办
                string formname = formId.ToString();
                var rizhiresult = ERPRiZhiService.GetEntities(p => p.FKApplicationID == model.ApplicationId.ToString() && p.FkFormName == PaperPublishTypeOfFormId.Establishment.ToString() && p.FKAction == "待办").FirstOrDefault();
                MyRiZhi.UserName = User.Identity.Name; 
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.ID = rizhiresult.ID;
                MyRiZhi.DoSomething = "已生成论文介绍信";
                MyRiZhi.FkFormName = PaperPublishTypeOfFormId.Establishment.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi.TimeStr = DateTime.Now;

                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //页面加载后就上报，就添加新的数据
                if (model.NWorkToDoID == 0)
                {
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
                    erpnworktodomodel.ApplicationStatus = ApplicationStatus.ProjectEstablished.ToString();
                    erpnworktodomodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    bool isUpdateSuccess = ApplicationService.UpdateApplication(erpnworktodomodel.ToDataTransferObjectModel());

                    return RedirectToAction("Conclusions", new { id = model.ApplicationId });
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
                    erpnworktodomodel.ApplicationStatus = ApplicationStatus.ProjectEstablished.ToString();
                    erpnworktodomodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    bool isUpdateSuccess = ApplicationService.UpdateApplication(erpnworktodomodel.ToDataTransferObjectModel());
                    return RedirectToAction("Conclusions", new { id = model.ApplicationId });
                }
            }
            //修改数
            else
            {
                string act = "updatedata";
                return RedirectToAction("ProjectEstablish", new { id = model.NWorkToDoID, nextaction = act });
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
            string k;
            string type = collection["TypeId"].ToString();
            double priceTotal = Convert.ToDouble(collection["TotalPrice"].ToString());

            var todoModel = ApplicationService.GetEntityById(Convert.ToInt32(model.ApplicationId));

            string projectType = todoModel.FormValues.Split('#')[0];

            using (var context = new CSPostOAEntities())
            {
                var result = context.PaperMagazine.Where(x => x.IsDeleted == false && x.Name == projectType).FirstOrDefault();
                k = result.Level;
            }

            FundsThresholdViewModel fundModel = new FundsThresholdViewModel();
            fundModel = FundsThresholdService.GetEntities(p => p.FundsType == type && p.ModuleName == ModuleNameOfScienceResearch.PaperPublish.ToString() && p.ProjectType == k).FirstOrDefault().ToViewModel();
            if (priceTotal > fundModel.Threshold)
            {
                ViewBag.limit = "true";
                using (var context = new CSPostOAEntities())
                {
                    ViewBag.FundsType = (from h in context.FundsThreshold
                                         where (h.IsDeleted == false && h.ModuleName == ModuleNameOfScienceResearch.PaperPublish.ToString() && h.ProjectType == k)

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
                    MyRiZhi.FkFormName = PaperPublishTypeOfFormId.FeeReimbursement.ToString();
                    MyRiZhi.FKAction = "已办";
                    MyRiZhi.TimeStr = DateTime.Now;
                    MyRiZhi.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();

                    string act = "save";
                    model.IsTemporary = true;
                    model.IsDeleted = false;
                    model.IsLocked = false;

                    if (model.FundsRecordID == 0)
                    {
                        //支出，以后要根据情况修改
                        model.IsIncome = false;
                        model.ModuleName = ApplicationType.PublishPaper.ToString();

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
                    MyRiZhi.FkFormName = PaperPublishTypeOfFormId.FeeReimbursement.ToString();
                    MyRiZhi.FKAction = "已办";
                    MyRiZhi.TimeStr = DateTime.Now;
                    MyRiZhi.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();

                    //待办
                    MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkflowId), sectionName);
                    MyRiZhi1.DoSomething = "需要审批(经费记录)";
                    MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    MyRiZhi1.FkFormName = PaperPublishTypeOfFormId.FeeReimbursement.ToString();
                    MyRiZhi1.FKAction = "待办";
                    MyRiZhi1.TimeStr = DateTime.Now;
                    MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();

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
                        model.ModuleName = ApplicationType.PublishPaper.ToString();

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
                                             where (h.IsDeleted == false && h.ModuleName == ModuleNameOfScienceResearch.ScienceResearch.ToString() && h.ProjectType == projectType)

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
        public ActionResult ReimburseAgree(string flag, FundsRecordViewModel model, FormCollection collection)
        {
            ///验证
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.CreatedTime = DateTime.Now;

            //action flag
            //string toDoAction;
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
                //act = "rejected";
                model.IsRejected = true;
                var workFlowNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == model.WorkflowId).OrderBy(p => p.NodeSerils).Skip(1).Take(1).FirstOrDefault();

                model.JieDianID = workFlowNodeModel.ID;
                model.JieDianName = workFlowNodeModel.NodeName.ToString();
                nodeSerils = "驳回";
                bool isUpdate = this.FundsRecordService.UpdateFundsRecord(model.ToDataTransferObjectModel());
            }

            //经费报销单的日志
            if (model.WorkflowId == Convert.ToInt16(PaperPublishTypeOfWorkflowId.FeeReimbursement))
            {
                string formname = PaperPublishTypeOfFormId.FeeReimbursement.ToString();
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
                    MyRiZhi.FkFormName = PaperPublishTypeOfFormId.FeeReimbursement.ToString();
                    //FundsRecord的ID
                    MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi.TimeStr = DateTime.Now;
                    MyRiZhi.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();

                    bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                    //新生成待办，下一审批人的待办

                    MyRiZhi1.FKAction = "待办";
                    MyRiZhi1.ID = rizhiresult.ID;
                    //FundsRecord的ID
                    MyRiZhi1.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    MyRiZhi1.DoSomething = "需要审批(" + erpnworktodoModel.UserName + "上报的经费报销单)";
                    MyRiZhi1.FkFormName = PaperPublishTypeOfFormId.FeeReimbursement.ToString();
                    MyRiZhi1.TimeStr = DateTime.Now;
                    MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();

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
                    MyRiZhi.DoSomething = "已审批" + erpnworktodoModel.UserName + "上报的经费报销单";
                    MyRiZhi.FkFormName = PaperPublishTypeOfFormId.FeeReimbursement.ToString();
                    //FundsRecord的ID
                    MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();

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
                    MyRiZhi.FkFormName = PaperPublishTypeOfFormId.FeeReimbursement.ToString();
                    //FundsRecord的ID
                    MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi.TimeStr = DateTime.Now;
                    MyRiZhi.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();

                    bool UpdateRiZhiSuccess1 = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());
                    //申请人待办

                    MyRiZhi1.FKAction = "待办";
                    MyRiZhi1.ID = rizhiresult.ID;
                    MyRiZhi1.UserName = erpnworktodoModel.UserName;
                    MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    MyRiZhi1.DoSomething = "经费报销单被驳回，需修改数据";
                    MyRiZhi1.FkFormName = PaperPublishTypeOfFormId.FeeReimbursement.ToString();
                    MyRiZhi1.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi1.TimeStr = DateTime.Now;
                    MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();

                    int returnid = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                    return Json(returnid, JsonRequestBehavior.AllowGet);
                   
                    //return RedirectToAction("ReimburseRejected", new { id = model.FundsRecordID });
                }
            }
            //差旅报销单的日志
            else
            {
                string formname = PaperPublishTypeOfFormId.TravelReimbursement.ToString();
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
                    MyRiZhi.FkFormName = PaperPublishTypeOfFormId.TravelReimbursement.ToString();
                    //FundsRecord的ID
                    MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi.TimeStr = DateTime.Now;
                    MyRiZhi.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();

                    bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                    //新生成待办，下一审批人的待办

                    MyRiZhi1.FKAction = "待办";
                    MyRiZhi1.ID = rizhiresult.ID;
                    //FundsRecord的ID
                    MyRiZhi1.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    MyRiZhi1.DoSomething = "需要审批(" + erpnworktodoModel.UserName + "上报的差旅报销单)";
                    MyRiZhi1.FkFormName = PaperPublishTypeOfFormId.TravelReimbursement.ToString();
                    MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();

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
                    MyRiZhi.FkFormName = PaperPublishTypeOfFormId.TravelReimbursement.ToString();
                    //FundsRecord的ID
                    MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();

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
                    MyRiZhi.DoSomething = "已驳回" + erpnworktodoModel.UserName + "上报的差旅报销单";
                    MyRiZhi.FkFormName = PaperPublishTypeOfFormId.TravelReimbursement.ToString();
                    //FundsRecord的ID
                    MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi.TimeStr = DateTime.Now;
                    MyRiZhi.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();

                    bool UpdateRiZhiSuccess1 = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());
                    //申请人待办

                    MyRiZhi1.FKAction = "待办";
                    MyRiZhi1.ID = rizhiresult.ID;
                    MyRiZhi1.UserName = erpnworktodoModel.UserName;
                    MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    MyRiZhi1.DoSomething = "差旅报销单被驳回，需修改数据";
                    MyRiZhi1.FkFormName = PaperPublishTypeOfFormId.TravelReimbursement.ToString();
                    //FundsRecord的ID
                    MyRiZhi1.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi1.TimeStr = DateTime.Now;
                    MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();

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

            string k;
            string type = collection["TypeId"].ToString();
            double priceTotal = Convert.ToDouble(collection["TotalPrice"].ToString());

            var todoModel = ApplicationService.GetEntityById(Convert.ToInt32(model.ApplicationId));

            string projectType = todoModel.FormValues.Split('#')[0];

            using (var context = new CSPostOAEntities())
            {
                var result = context.PaperMagazine.Where(x => x.IsDeleted == false && x.Name == projectType).FirstOrDefault();
                k = result.Level;
            }

            FundsThresholdViewModel fundModel = new FundsThresholdViewModel();
            fundModel = FundsThresholdService.GetEntities(p => p.FundsType == type && p.ModuleName == ModuleNameOfScienceResearch.PaperPublish.ToString() && p.ProjectType == k).FirstOrDefault().ToViewModel();
            if (priceTotal > fundModel.Threshold)
            {
                ViewBag.limit = "true";
                using (var context = new CSPostOAEntities())
                {
                    ViewBag.FundsType = (from h in context.FundsThreshold
                                         where (h.IsDeleted == false && h.ModuleName == ModuleNameOfScienceResearch.PaperPublish.ToString() && h.ProjectType == k)

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
                    MyRiZhi.FkFormName = PaperPublishTypeOfFormId.FeeReimbursement.ToString();
                    MyRiZhi.FKAction = "已办";
                    //FKApplicationID是过程记录的ID
                    MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();
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
                    MyRiZhi.FkFormName = PaperPublishTypeOfFormId.FeeReimbursement.ToString();
                    MyRiZhi.FKAction = "已办";
                    MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi.TimeStr = DateTime.Now;
                    MyRiZhi.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();
                    bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                    //待办
                    MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkflowId), sectionName);
                    MyRiZhi1.DoSomething = "需要审批(经费记录)";
                    MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    MyRiZhi1.FkFormName = PaperPublishTypeOfFormId.FeeReimbursement.ToString();
                    MyRiZhi1.FKAction = "待办";
                    MyRiZhi1.TimeStr = DateTime.Now;
                    MyRiZhi1.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();

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
            model.WorkflowId = (int)PaperPublishTypeOfWorkflowId.TravelReimbursement;
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
                MyRiZhi.FkFormName = PaperPublishTypeOfFormId.TravelReimbursement.ToString();
                MyRiZhi.FKAction = "已办";
                //FKApplicationID是过程记录的ID
                MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();
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
                MyRiZhi.FkFormName = PaperPublishTypeOfFormId.TravelReimbursement.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                MyRiZhi.TimeStr = DateTime.Now;
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();
                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //待办
                MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkflowId), sectionName);
                MyRiZhi1.DoSomething = "需要审批(差旅报销单)";
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.FkFormName = PaperPublishTypeOfFormId.TravelReimbursement.ToString();
                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.TimeStr = DateTime.Now;
                MyRiZhi1.FKApplicationID = model.FundsRecordID.ToString();
                MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();

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
            model.WorkflowId = (int)PaperPublishTypeOfWorkflowId.TravelReimbursement;
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
                MyRiZhi.FkFormName = PaperPublishTypeOfFormId.TravelReimbursement.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi.TimeStr = DateTime.Now;
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();

                string act = "save";
                model.IsTemporary = true;
                model.IsDeleted = false;
                //第一次保存
                //注意直接用ID可能出错，页面也未改
                if (model.FundsRecordID == 0)
                {

                    //支出，以后要根据情况修改
                    model.IsIncome = false;

                    model.ModuleName = ApplicationType.PublishPaper.ToString();

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
                MyRiZhi.FkFormName = PaperPublishTypeOfFormId.TravelReimbursement.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.TimeStr = DateTime.Now;
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();

                //待办
                MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkflowId), sectionName);
                MyRiZhi1.DoSomething = "需要审批(差旅报销单)";
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.FkFormName = PaperPublishTypeOfFormId.TravelReimbursement.ToString();
                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.TimeStr = DateTime.Now;
                MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();

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
        /// 论文登记提交
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
            var temerpnformmodel = ERPNFormService.GetEntityById((int)PaperPublishTypeOfFormId.Conclusion).ToViewModel();
            model.FormContent = temerpnformmodel.ContentStr;
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
                MyRiZhi.DoSomething = "保存(论文登记)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.FkFormName = PaperPublishTypeOfFormId.Conclusion.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = model.ApplicationId.ToString();

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
                    //return RedirectToAction("Conclusions", new { id = model.ApplicationId, nextaction = act });
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
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();
                MyRiZhi.DoSomething = "添加(论文登记)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.FkFormName = PaperPublishTypeOfFormId.Conclusion.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi.TimeStr = DateTime.Now;

                int AddRiZhiSuccess = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //待办
                MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkFlowID), sectionName);
                MyRiZhi1.DoSomething = "需要审批(论文登记)";
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.FkFormName = PaperPublishTypeOfFormId.Conclusion.ToString();
                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();

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
                    //return RedirectToAction("ConclusionsAgree", new { id = model.ApplicationId });
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
                    //return RedirectToAction("ConclusionsAgree", new { id = model.ApplicationId });
                }
            }
        }

        /// <summary>
        /// 论文登记审批提交
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ConclusionsAgree(string flag, ERPNWorkToDoViewModel model, FormCollection collection)
        {
            string nodeSerils;
            string act = string.Empty;
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
            //驳回
            else
            {
                act = "rejected";
                model.IsRejected = true;

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

            string formname = PaperPublishTypeOfFormId.Conclusion.ToString();
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
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.DoSomething = "已审批'" + erpnworktodoModel.UserName + "'上报的论文登记";
                MyRiZhi.FkFormName = PaperPublishTypeOfFormId.Conclusion.ToString();
                MyRiZhi.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi.TimeStr = DateTime.Now;

                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //新生成待办，下一审批人的待办

                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.ID = rizhiresult.ID;
                MyRiZhi1.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.DoSomething = "需要审批(" + erpnworktodoModel.UserName + "上报的论文登记)";
                MyRiZhi1.FkFormName = PaperPublishTypeOfFormId.Conclusion.ToString();
                MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();

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
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.DoSomething = "已审批" + erpnworktodoModel.UserName + "上报的论文登记";
                MyRiZhi.FkFormName = PaperPublishTypeOfFormId.Conclusion.ToString();
                MyRiZhi.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi.TimeStr = DateTime.Now;

                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                ///成功标准
                int UpdateSuccess = 1;
                return Json(UpdateSuccess, JsonRequestBehavior.AllowGet);
                
                // 更新成功并且更新后，申请书状态变为 已经审批通过，跳转到项目确立页面
                //return RedirectToAction("ConclusionsAgree", new { id = model.ApplicationId });
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
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.DoSomething = "已驳回" + erpnworktodoModel.UserName + "上报的论文登记";
                MyRiZhi.FkFormName = PaperPublishTypeOfFormId.Conclusion.ToString();
                MyRiZhi.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi.TimeStr = DateTime.Now;

                bool UpdateRiZhiSuccess1 = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());
                //申请人待办

                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.ID = rizhiresult.ID;
                MyRiZhi1.UserName = erpnworktodoModel.UserName;
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.DoSomething = "论文登记被驳回，需修改数据";
                MyRiZhi1.FkFormName = PaperPublishTypeOfFormId.Conclusion.ToString();
                MyRiZhi1.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();

                int returnid = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                return Json(returnid, JsonRequestBehavior.AllowGet);
                //return RedirectToAction("ConclusionsRejected", new { id = model.ApplicationId, nextaction = act });
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ConclusionsRejected(ERPNWorkToDoViewModel model, FormCollection collection)
        {
            model.UserName = User.Identity.Name;
            model.TimeStr = DateTime.Now;
            var sectionName = MySession[SessionKeyEnum.SectionName].ToString();

            //加载表单内容
            var temerpnformmodel = ERPNFormService.GetEntityById((int)PaperPublishTypeOfFormId.Conclusion).ToViewModel();
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
                MyRiZhi.FkFormName = PaperPublishTypeOfFormId.Conclusion.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();
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
                tempmodel.ApplicationStatus = ApplicationStatus.ConcludeUnSubmit.ToString();
                tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                bool isupdate = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());

                int jiedianid = Convert.ToInt32(model.JieDianID);
                var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));
                bool isUpdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());

                return RedirectToAction("ConclusionsRejected", new { id = model.ApplicationId });

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
                MyRiZhi.FkFormName = ApplicationStatus.ConcludeUnSubmit.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();

                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //待办
                MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkFlowID), sectionName);
                MyRiZhi1.DoSomething = "需要审批(项目延期)";
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.FkFormName = PaperPublishTypeOfFormId.Conclusion.ToString();
                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi1.TimeStr = DateTime.Now;
                MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.PaperPublish.ToString();

                int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                //上报时设置保存为false、驳回为false、删除为false、冻结为false
                model.IsTemporary = false;
                model.IsRejected = false;
                model.IsDeleted = false;
                model.IsLocked = false;

                ERPNWorkToDoViewModel tempmodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();
                tempmodel.ApplicationStatus = ApplicationStatus.ProjectConcluding.ToString();
                tempmodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                bool isupdate1 = ApplicationService.UpdateApplication(tempmodel.ToDataTransferObjectModel());

                int jiedianid = Convert.ToInt32(model.JieDianID);
                var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));
                bool isupdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());

                return RedirectToAction("ConclusionsRejected", new { id = model.ApplicationId });
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
        public ActionResult ApplicationListStatistics(string startTime, string endTime, string projectName, string State, string Freeze, int page, int pageSize)
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

            IEnumerable<ERPNWorkToDoTransferObject> result = SearchProcessingApplicationList(projectName, State, Freeze, SearchCriteriaStartTime,
                end, pageSize, page, ref totalPage);

            bool hasRolesFlag = HasRolesFlag();
            IEnumerable<ERPNWorkToDoTransferObject> resultpage;
            int totalcount = 0;
            //非普通用户
            if (hasRolesFlag)
            {
                resultpage = ApplicationService.GetEntities(p => p.FormID == (int)PaperPublishTypeOfFormId.Application
                    && p.TimeStr.Value > start
                    && p.TimeStr.Value < end
                    && ((State == Constant.All) ? true : p.ApplicationStatus == State)
                    && (string.IsNullOrEmpty(projectName) ? true : (p.WenHao.Contains(projectName)))
                    && p.StateNow == "正在办理"
                    && ((Freeze == Constant.All) ? true : p.IsLocked == (Freeze == "冻结"))
                    && p.ProjectStatus != ApplicationStatus.BigProjectProcessing.ToString());
                totalcount = resultpage.Count();
            }
            //普通用户
            else
            {
                resultpage = ApplicationService.GetEntities(p => p.FormID == (int)PaperPublishTypeOfFormId.Application
                    && p.TimeStr.Value > start
                    && p.TimeStr.Value < end
                    && ((State == Constant.All) ? true : p.ApplicationStatus == State)
                    && (string.IsNullOrEmpty(projectName) ? true : (p.WenHao.Contains(projectName)))
                    && p.StateNow == "正在办理"
                    && p.UserName == User.Identity.Name
                    && ((Freeze == Constant.All) ? true : p.IsLocked == (Freeze == "冻结"))
                    && p.ProjectStatus != ApplicationStatus.BigProjectProcessing.ToString());
                totalcount = resultpage.Count();
            }

            return Json(new { data = result, total = totalcount }, JsonRequestBehavior.AllowGet);
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
        /// <summary>
        /// 过程记录列表查询
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="projectName">项目名称</param>
        /// <param name="state">状态</param>
        /// <param name="Freeze">是否冻结</param>
        /// <returns></returns>
        [HttpPost]
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
            int totalcount = 0;
            //非普通用户
            if (hasRolesFlag)
            {
                resultpage = ApplicationService.GetEntities(p => p.FormID == (int)PaperPublishTypeOfFormId.Application
                    && p.TimeStr.Value > start
                    && p.TimeStr.Value < end
                    && ((State == Constant.All) ? true : p.ApplicationStatus == State)
                    && (string.IsNullOrEmpty(projectName) ? true : (p.WenHao.Contains(projectName)))
                    && p.StateNow == "正在办理"
                    && ((Freeze == Constant.All) ? true : p.IsLocked == (Freeze == "冻结"))
                    && p.ProjectStatus == ApplicationStatus.BigProjectProcessing.ToString());
                totalcount = resultpage.Count();
            }
            //普通用户
            else
            {
                resultpage = ApplicationService.GetEntities(p => p.FormID == (int)PaperPublishTypeOfFormId.Application
                    && p.TimeStr.Value > start
                    && p.TimeStr.Value < end
                    && ((State == Constant.All) ? true : p.ApplicationStatus == State)
                    && (string.IsNullOrEmpty(projectName) ? true : (p.WenHao.Contains(projectName)))
                    && p.StateNow == "正在办理"
                    && p.UserName == User.Identity.Name
                    && ((Freeze == Constant.All) ? true : p.IsLocked == (Freeze == "冻结"))
                    && p.ProjectStatus == ApplicationStatus.BigProjectProcessing.ToString());
                totalcount = resultpage.Count();
            }

            return Json(new { data = result, total = totalcount }, JsonRequestBehavior.AllowGet);
        }

        #endregion


        /// <summary>
        /// 论文管理导航首页统计分析容器页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ScienceStatisticContainer()
        {
            ViewBag.Module = "论文管理";
            ViewBag.Title = "统计分析";

            return View();
        }

        /// <summary>
        /// 论文统计分析
        /// </summary>
        /// <returns></returns>
        public ActionResult PaperPublishStatisticsAnalysis()
        {
            IList<PaperPublishStatisticsViewModel> resultList;
            var result = StatisticService.GetPaperPublishStatistics(x => true);

            if (result.Count != 0)
            {
                resultList = result.Select(x => x.ToViewModel()).ToList();
            }
            else
            {
                resultList = new List<PaperPublishStatisticsViewModel>();
            }

            return View(resultList);
        }

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
                        //var key = arrayList[1];
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
        [HttpPost]
        /// <summary>
        /// 论文统计分析导出excel
        /// </summary>
        /// <returns></returns>
        public ActionResult PaperPublishStatisticsAnalysis(FormCollection collection)
        {
            if (collection["OutPaperPublishStatistics"] != null)
            {
                IList<PaperPublishStatisticsViewModel> resultList;
                var result = StatisticService.GetPaperPublishStatistics(x => true);

                if (result.Count != 0)
                {
                    resultList = result.Select(x => x.ToViewModel()).ToList();
                }
                else
                {
                    resultList = new List<PaperPublishStatisticsViewModel>();
                }

                DataTable dt = new DataTable();
                string excelName = "论文管理-论文管理统计分析列表";
                string fileName = DateTime.Now.ToString("yyyyMMddhhmmss");
                string path = System.Web.HttpContext.Current.Server.MapPath("~/UploadFiles/论文管理-论文管理统计分析-" + fileName + ".xls");
                //Excel表头
                dt.Columns.Add("年份");
                dt.Columns.Add("特I类期刊");
                dt.Columns.Add("I类期刊");
                dt.Columns.Add("II类期刊");
                dt.Columns.Add("III类期刊");
                dt.Columns.Add("合计");

                //往datatable中填入内容
                foreach (var item in resultList)
                {

                    DataRow row = dt.NewRow();
                    row["年份"] = item.Year;
                    row["特I类期刊"] = item.Superjournal;
                    row["I类期刊"] = item.Onejournal;
                    row["II类期刊"] = item.Twojournal;
                    row["III类期刊"] = item.Threejournal;
                    row["合计"] = item.TotalCount;
                    dt.Rows.Add(row);
                }
                DataRow totalrow = dt.NewRow();
                totalrow["年份"] = "合计";
                totalrow["特I类期刊"] = resultList.Sum(x => x.Superjournal);
                totalrow["I类期刊"] = resultList.Sum(x => x.Onejournal);
                totalrow["II类期刊"] = resultList.Sum(x => x.Twojournal);
                totalrow["III类期刊"] = resultList.Sum(x => x.Threejournal);
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
                return this.File(path, "application/octet-stream", "论文管理-论文管理统计分析-" + fileName + ".xls");
            }
            else
            {
                return PartialView();
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
                result = ApplicationService.GetPageEntities(p => p.FormID == (int)PaperPublishTypeOfFormId.Application
                    && p.TimeStr.Value > start
                    && p.TimeStr.Value < end
                    && ((state == Constant.All) ? true : p.ApplicationStatus == state)
                    && (string.IsNullOrEmpty(projectName) ? true : (p.WenHao.Contains(projectName)))
                    && p.StateNow == "正在办理"
                    && ((freeze == Constant.All) ? true : p.IsLocked == (freeze == "冻结"))
                    && p.ProjectStatus != ApplicationStatus.BigProjectProcessing.ToString(), ApplicationSortField.TimeStr_Desc.ToString(), pageSize, pageIndex, out totalPage);
            }
            //普通用户
          else
          {
              result = ApplicationService.GetPageEntities(p => p.FormID == (int)PaperPublishTypeOfFormId.Application
                  && p.TimeStr.Value > start
                  && p.TimeStr.Value < end
                  && ((state == Constant.All) ? true : p.ApplicationStatus == state)
                  && (string.IsNullOrEmpty(projectName) ? true : (p.WenHao.Contains(projectName)))
                  && p.StateNow == "正在办理"
                  && p.UserName == User.Identity.Name
                  && ((freeze == Constant.All) ? true : p.IsLocked == (freeze == "冻结"))
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
                result = ApplicationService.GetPageEntities(p => p.FormID == (int)PaperPublishTypeOfFormId.Application
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
                result = ApplicationService.GetPageEntities(p => p.FormID == (int)PaperPublishTypeOfFormId.Application
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
    }
}
