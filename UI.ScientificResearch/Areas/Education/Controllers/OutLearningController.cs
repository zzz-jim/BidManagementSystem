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
    /// 外出进修
    /// </summary>
    public class OutLearningController : Controller
    {
        #region private Service
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

        #region Constructor
		
          public OutLearningController()
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

        public OutLearningController(
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
        /// 外出进修首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Module = "外出进修";
            ViewBag.Title = "简介";

            return View();
        }

        /// <summary>
        /// 申请书
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult SubmitApplication(string id)
        {
            int formId = (int)OutLearningTypeOfFormId.Application;
            int workflowId = (int)OutLearningTypeOfWorkflowId.Application;

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
                model.FormContent = model.FormContent.Replace(Constant.MacroUserNameString, User.Identity.Name);

                // 自动生成 编号
                model.BeiYong1 = CommonHelper.GenerateProjectNumber(ApplicationType.OutLearning);

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
        /// 借款登记
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nextaction"></param>
        /// <returns></returns>
        public ActionResult BorrowRegistration(string id, string nextaction)
        {
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
         
            //点击上报时的view
            else
            {
                int formId = (int)OutLearningTypeOfFormId.BorrowRegistration;
              
                //加载表单内容
                var temerpnformmodel = ERPNFormService.GetEntityById(formId).ToViewModel();

                //获取当前表单对应的工作数据列
                string[] formItemArray = temerpnformmodel.ItemsList.Split(Constant.SplitChar);

                ViewBag.Id = id;
                ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();

                var result = ApplicationService.GetEntities(p => p.ApplicationId == applicaionId && p.FormID == (int)OutLearningTypeOfFormId.BorrowRegistration).ToList();

                if (result.Count > 0)//已经填写了借款登记,显示
                {
                    model = result.FirstOrDefault().ToViewModel();
                }
                else //新建借款登记
                {
                    ViewBag.act = "reported";
                    var appresult = ApplicationService.GetEntities(p => p.ID == applicaionId).ToList();

                    model.FormID = formId;
                    model.ApplicationId = applicaionId;

                    string content = temerpnformmodel.ContentStr;
                    model.FormContent = content;
                    model.FormContent = model.FormContent.Replace(Constant.MacroUserNameString, User.Identity.Name);
                }
                return View(model);
            }
        }

       /// <summary>
       /// 承诺书
       /// </summary>
       /// <param name="id"></param>
       /// <param name="nextaction"></param>
       /// <returns></returns>
        public ActionResult PromiseBook(string id, string nextaction)
        {
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

            //点击上报时的view
            else
            {
                int formId = (int)OutLearningTypeOfFormId.PromiseBook;

                //加载表单内容
                var temerpnformmodel = ERPNFormService.GetEntityById(formId).ToViewModel();

                //获取当前表单对应的工作数据列
                string[] formItemArray = temerpnformmodel.ItemsList.Split(Constant.SplitChar);

                ViewBag.Id = id;
                ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();

                var result = ApplicationService.GetEntities(p => p.ApplicationId == applicaionId && p.FormID == (int)OutLearningTypeOfFormId.PromiseBook).ToList();

                if (result.Count > 0)//已经同意承诺书
                {
                    model = result.FirstOrDefault().ToViewModel();
                }
                else //同意承诺书
                {
                    ViewBag.act = "reported";
                    var appresult = ApplicationService.GetEntities(p => p.ID == applicaionId).ToList();

                    model.FormID = formId;
                    model.ApplicationId = applicaionId;

                    string content = temerpnformmodel.ContentStr;

                    model.FormContent = content;
                    model.FormContent = model.FormContent.Replace(Constant.MacroUserNameString, User.Identity.Name);

                }
                return View(model);
            }
        }

        /// <summary>
        /// 派出证明
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nextaction"></param>
        /// <returns></returns>
        public ActionResult SendProof(string id, string nextaction)
        {
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

            //点击上报时的view
            else
            {
                int formId = (int)OutLearningTypeOfFormId.SendProof;
                int workflowId = (int)OutLearningTypeOfWorkflowId.SendProof;
                ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();
                ERPNWorkFlowNodeTransferObject currentNode;

                //加载表单内容
                var temerpnformmodel = ERPNFormService.GetEntityById(formId).ToViewModel();

                //获取当前表单对应的工作数据列
                string[] formItemArray = temerpnformmodel.ItemsList.Split(Constant.SplitChar);

                ViewBag.Id = id;

                model.WorkFlowID = workflowId;
                model.FormID = formId;
                var result = ApplicationService.GetEntities(p => p.ApplicationId == applicaionId && p.FormID == (int)OutLearningTypeOfFormId.SendProof).ToList();

                if (result.Count > 0)//已经同意承诺书
                {
                    model = result.FirstOrDefault().ToViewModel();
                }
                else //同意承诺书
                {
                    ViewBag.act = "reported";
                    var appresult = ApplicationService.GetEntities(p => p.ID == applicaionId).ToList();

                    model.ApplicationId = applicaionId;

                    string content = temerpnformmodel.ContentStr;

                    model.FormContent = content;
                    model.FormContent = model.FormContent.Replace(Constant.MacroUserNameString, User.Identity.Name);


                    ////绑定工作名称
                    var workFlowModel = ERPNWorkFlowService.GetEntityById(workflowId);
                    model.WorkName = User.Identity.Name + Constant.DoubleHyphenString + workFlowModel.WorkFlowName;
                    //绑定下一节点
                    currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == workflowId && p.NodeAddr == Constant.MacroStartString).First();
                    model.JieDianID = currentNode.ID;
                    model.JieDianName = currentNode.NodeName;

                    //批量设置字段的可写、保密属性
                    ViewBag.PiLiangSet = CommonHelper.SetTheWriteAndHiddenField(currentNode.CanWriteSet, currentNode.SecretSet, formItemArray);

                }
                return View(model);
            }
        }

        /// <summary>
        /// 派出证明审批
        /// </summary>
        /// <returns></returns>
        public ActionResult SendProofAgree(string id, string nextaction)
        {
           // ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();

            ERPNWorkToDoTransferObject model;
            int formId = (int)OutLearningTypeOfFormId.SendProof;
            int id1 = Convert.ToInt32(id);//外出进修申请书的ID

            model = ApplicationService.GetEntities(p=>p.FormID==formId&&p.ApplicationId==id1).First();
            //判断驳回或继续审批

            bool isRejected = Convert.ToBoolean(model.IsRejected);
            bool isTemporary = Convert.ToBoolean(model.IsTemporary);

            //驳回
            if (isRejected || isTemporary)
            {
                if (isTemporary && model.IsRejected == null)
                {
                    return RedirectToAction("SendProof", new { id = id });
                }
                else
                {
                    return RedirectToAction("SendProofReject", new { id = id });
                }
            }
            //审批
            else
            {
                //model.ID = id1;

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
                return View(model.ToViewModel());
            }
        }

        /// <summary>
        /// 派出证明驳回
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult SendProofReject(string id)
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

                //ToDo
                //string projectType = todoModel.FormValues.Split('#')[1];
                string projectType = "国家级";
                ViewBag.act = "save";
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

                model.CreatedTime = DateTime.Now;
                model = FundsRecordService.GetEntityById(applicaionId).ToViewModel();
                return View(model);
            }
            //点击上报时的view
            else
            {
                var todoModel = ApplicationService.GetEntityById(applicaionId);
                
                //ToDo
                //string projectType = todoModel.FormValues.Split('#')[1];
                string projectType = "国家级";
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
               
                int workflowId = (int)OutLearningTypeOfWorkflowId.FeeReimbursement;
                int formId = (int)OutLearningTypeOfFormId.FeeReimbursement; 

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

            //ToDo
            //string projectType = todoModel.FormValues.Split('#')[1];
            string projectType = "国家级";
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
                int workflowId = (int)OutLearningTypeOfWorkflowId.TravelReimbursement;

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
        /// 完成情况
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nextaction"></param>
        /// <returns></returns>
        public ActionResult Completion(string id, string nextaction)
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

            //点击上报时的view
            else
            {
                int formId = (int)OutLearningTypeOfFormId.Completion;
                int workflowId = (int)OutLearningTypeOfWorkflowId.Completion;
                //加载表单内容
                var temerpnformmodel = ERPNFormService.GetEntityById(formId).ToViewModel();

                //获取当前表单对应的工作数据列
                string[] formItemArray = temerpnformmodel.ItemsList.Split(Constant.SplitChar);

                ViewBag.Id = id;
                ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();

                var result = ApplicationService.GetEntities(p => p.ApplicationId == applicaionId && p.FormID == (int)OutLearningTypeOfFormId.Completion).ToList();

                if (result.Count > 0)//已经同意承诺书
                {
                    model = result.FirstOrDefault().ToViewModel();
                }
                else //同意承诺书
                {
                    ViewBag.act = "reported";
                    var appresult = ApplicationService.GetEntities(p => p.ID == applicaionId).ToList();

                    model.FormID = formId;
                    model.ApplicationId = applicaionId;
                    model.WorkFlowID = workflowId;

                    //加载表单内容
                    model.FormContent = temerpnformmodel.ContentStr;
                    model.FormContent = model.FormContent.Replace(Constant.MacroSectionString, MySession[SessionKeyEnum.SectionName].ToString());
                    model.FormContent = model.FormContent.Replace(Constant.MacroUserNameString, User.Identity.Name);

                    ////绑定工作名称
                    var workFlowModel = ERPNWorkFlowService.GetEntityById(workflowId);
                    model.WorkName = User.Identity.Name + Constant.DoubleHyphenString + workFlowModel.WorkFlowName;
                    //绑定下一节点
                    currentNode = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == workflowId && p.NodeAddr == Constant.MacroStartString).First();
                    model.JieDianID = currentNode.ID;
                    model.JieDianName = currentNode.NodeName;

                    //批量设置字段的可写、保密属性
                    ViewBag.PiLiangSet = CommonHelper.SetTheWriteAndHiddenField(currentNode.CanWriteSet, currentNode.SecretSet, formItemArray);

                }
                return View(model);
            }
        }

        /// <summary>
        /// 完成情况审批
        /// </summary>
        /// <returns></returns>
        public ActionResult CompletionAgree(string id, string nextaction)
        {
            ERPNWorkToDoTransferObject model;
            int formId = (int)OutLearningTypeOfFormId.Completion;
            int id1 = Convert.ToInt32(id);//外出进修申请书的ID

            model = ApplicationService.GetEntities(p => p.FormID == formId && p.ApplicationId == id1).First();
          
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
            return View(model.ToViewModel());
        }

        /// <summary>
        /// 完成情况驳回
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CompletionReject(string id)
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
        /// 正在进行的任务记录列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ProcessRecordList(string id)
        {
            
            int applicationid = Convert.ToInt32(id);
            ViewBag.Id = applicationid;
            var result = ApplicationService.GetEntities(p => p.StateNow == "正在办理" && p.FormID == (int)OutLearningTypeOfFormId.TaskTracking && p.ApplicationId == applicationid).OrderByDescending(p => p.TimeStr).ToList();
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
        /// 任务跟踪显示
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

                int formId = (int)OutLearningTypeOfFormId.TaskTracking;

                #region 数据准备

                ERPNWorkToDoViewModel model = new ERPNWorkToDoViewModel();
                model.FormID = formId;
                model.ApplicationId = applicationid;
                //加载表单内容
                var temerpnformmodel = ERPNFormService.GetEntityById(formId).ToViewModel();

                //获取当前表单对应的工作数据列
                string[] formItemArray = temerpnformmodel.ItemsList.Split(Constant.SplitChar);
                string content = temerpnformmodel.ContentStr;
                model.FormContent = content;

                //绑定工作名称
                model.WorkName = User.Identity.Name + "--" + "新建了任务跟踪";

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
        /// 待办事宜(Top 10)
        /// </summary>
        /// <returns></returns>
        public ActionResult Top10WorkToDoList()
        {
            var result = ERPRiZhiService.GetEntities(p => p.FKAction == "待办" && p.ModuleName == ModuleNameOfScienceResearch.OutLearning.ToString()).OrderByDescending(p => p.ID).Take(10);
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
                    && p.FormID == (int)OutLearningTypeOfFormId.Application
                    && p.ProjectStatus == ApplicationStatus.BigProjectProcessing.ToString(), ApplicationSortField.TimeStr_Desc.ToString(), Constant.PageSize, 1, out totalPage);
            }
            else
            {
                result = ApplicationService.GetPageEntities(p => p.StateNow == "正在办理"
                    && p.UserName == User.Identity.Name
                    && p.FormID == (int)OutLearningTypeOfFormId.Application
                    && p.ProjectStatus == ApplicationStatus.BigProjectProcessing.ToString(), ApplicationSortField.TimeStr_Desc.ToString(), Constant.PageSize, 1, out totalPage);
            }

            return View(result.Select(x => x.ToViewModel()));
        }
        

        public ActionResult ProcessRecordDetail(string id)
        {
            int workToDoId = Convert.ToInt32(id);
            var workToDoModel = ApplicationService.GetEntityById(workToDoId).ToViewModel();
            return View(workToDoModel);
        }
        /// <summary>
        /// 资金管理
        /// </summary>
        /// <returns></returns>
        public ActionResult ScienceFundsManageContainer()
        {
            ViewBag.Module = "外出进修";
            ViewBag.Title = "资金管理";
            return View();
        }

        #region 经费管理列表
        
        /// <summary>
        /// 日常经费统计
        /// </summary>
        /// <returns></returns>
        public ActionResult FundsList(string moduleName)
        {
            ViewBag.Module = "外出机修";
            ViewBag.Title = "日常经费记录";

            ViewBag.ModuleName = moduleName;

            return View();
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
      
        #endregion

        #region 申请书列表


        /// <summary>
        /// 申请书列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ApplicationList()
        {
            ViewBag.Module = "外出进修";
            ViewBag.Title = "申请书";
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
        public ActionResult ApplicationListStatistics(string projectName, string State, string Freeze, string startTime, string endTime, int pageIndex, int pageSize)
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
                end, pageSize, pageIndex, ref totalPage);

            bool hasRolesFlag = HasRolesFlag();
            IEnumerable<ERPNWorkToDoTransferObject> resultpage;
            int totalcount=0;
            //非普通用户
            if (hasRolesFlag)
            {
                resultpage = ApplicationService.GetEntities(p => p.FormID == (int)OutLearningTypeOfFormId.Application
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
                resultpage = ApplicationService.GetEntities(p => p.FormID == (int)OutLearningTypeOfFormId.Application
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
                result = ApplicationService.GetPageEntities(p => p.FormID == (int)OutLearningTypeOfFormId.Application
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
                result = ApplicationService.GetPageEntities(p => p.FormID == (int)OutLearningTypeOfFormId.Application
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
        #endregion

        #region 过程记录列表
        /// <summary>
        /// 过程记录列表查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ProcessingProjectList()
        {
            ViewBag.Module = "外出进修";
            ViewBag.Title = "过程记录";
            return View();
        }

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
                resultpage = ApplicationService.GetEntities(p => p.FormID == (int)OutLearningTypeOfFormId.Application
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
                resultpage = ApplicationService.GetEntities(p => p.FormID == (int)OutLearningTypeOfFormId.Application
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
                result = ApplicationService.GetPageEntities(p => p.FormID == (int)OutLearningTypeOfFormId.Application
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
                result = ApplicationService.GetPageEntities(p => p.FormID == (int)OutLearningTypeOfFormId.Application
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
        
        #endregion

        #endregion

        #region Post Action
        /// <summary>
        /// 申请书
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="model"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubmitApplication(string flag, ERPNWorkToDoViewModel model, FormCollection collection)
        {
            ////验证
            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}

            model.UserName = User.Identity.Name;
            model.TimeStr = DateTime.Now;

            var sectionName = MySession[SessionKeyEnum.SectionName].ToString();

            //加载表单内容
            var formModel = ERPNFormService.GetEntityById((int)OutLearningTypeOfFormId.Application).ToViewModel();
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
                    "保存外出进修申请书",// 这种
                    Request.UserHostAddress,
                    OutLearningTypeOfFormId.Application.ToString(),
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
                        User.Identity.Name + "上报外出进修申请书",
                        Request.UserHostAddress,
                        OutLearningTypeOfFormId.Application.ToString(),
                        Constant.Done,
                        ModuleNameOfScienceResearch.OutLearning.ToString(),
                        nworktodoid.ToString(),
                         DateTime.Now
                        );

                    int AddRiZhiSuccess = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                    //待办
                    ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel(
                        FillInRiZhi(Convert.ToInt16(model.WorkFlowID), sectionName),
                        "需要审批('" + User.Identity.Name + "''" + "上报外出进修申请书" + "')",
                        Request.UserHostAddress,
                        OutLearningTypeOfFormId.Application.ToString(),
                        Constant.ToDo,
                        ModuleNameOfScienceResearch.OutLearning.ToString(),
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
                        User.Identity.Name + "上报外出进修申请书",
                        Request.UserHostAddress,
                        OutLearningTypeOfFormId.Application.ToString(),
                        Constant.Done,
                        ModuleNameOfScienceResearch.OutLearning.ToString(),
                        model.NWorkToDoID.ToString(),
                        DateTime.Now
                        );

                    //更新已办
                    string nworktodoid = model.NWorkToDoID.ToString();
                    string formname = OutLearningTypeOfFormId.Application.ToString();
                    MyRiZhi = ERPRiZhiService.GetEntities(p => p.FKApplicationID == nworktodoid && p.FkFormName == formname).FirstOrDefault().ToViewModel();

                    bool updateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                    //新增待办
                    ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel(
                        FillInRiZhi(Convert.ToInt16(model.WorkFlowID), sectionName),
                        "需要审批('" + User.Identity.Name + "''" + "上报外出进修申请书" + "')",
                        Request.UserHostAddress,
                        OutLearningTypeOfFormId.Application.ToString(),
                        Constant.ToDo,
                        ModuleNameOfScienceResearch.OutLearning.ToString(),
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

            string formname = OutLearningTypeOfFormId.Application.ToString();
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
                    "已审批'" + model.UserName + "上报外出进修申请书",
                    Request.UserHostAddress,
                     OutLearningTypeOfFormId.Application.ToString(),
                     Constant.Done,
                     ModuleNameOfScienceResearch.OutLearning.ToString(),
                     model.NWorkToDoID.ToString(),
                     DateTime.Now
                    );
                MyRiZhi.ID = rizhiresult.ID;
                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //新生成待办，下一审批人的待办
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel(
                    rizhiresult.ID,
                    "需要审批('" + model.UserName + "''" + "上报外出进修申请书" + "')",
                    Request.UserHostAddress,
                    OutLearningTypeOfFormId.Application.ToString(),
                    Constant.ToDo,
                    ModuleNameOfScienceResearch.OutLearning.ToString(),
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
                    "已审批'" + model.UserName + "上报外出进修申请书",
                    Request.UserHostAddress,
                    OutLearningTypeOfFormId.Application.ToString(),
                    Constant.Done,
                    ModuleNameOfScienceResearch.OutLearning.ToString(),
                    model.NWorkToDoID.ToString(),
                    DateTime.Now
                    );

                MyRiZhi.ID = rizhiresult.ID;
                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //新生成新建项目确立的待办，申请人待办
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel(
                    model.UserName,
                    model.UserName + "需要添加借款登记",
                    Request.UserHostAddress,
                    OutLearningTypeOfFormId.BorrowRegistration.ToString(),
                    Constant.ToDo,
                    ModuleNameOfScienceResearch.OutLearning.ToString(),
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
                    "已驳回'" + model.UserName +"上报外出进修申请书",
                    Request.UserHostAddress,
                    OutLearningTypeOfFormId.Application.ToString(),
                    Constant.Done,
                    ModuleNameOfScienceResearch.OutLearning.ToString(),
                    model.NWorkToDoID.ToString(),
                    DateTime.Now
                    );

                MyRiZhi.ID = rizhiresult.ID;
                bool UpdateRiZhiSuccess1 = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //申请人待办
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel(
                    model.UserName,
                    "申请书被驳回，请修改",
                    Request.UserHostAddress,
                    OutLearningTypeOfFormId.Application.ToString(),
                    Constant.ToDo,
                    ModuleNameOfScienceResearch.OutLearning.ToString(),
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
            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}

            model.UserName = User.Identity.Name;
            model.TimeStr = DateTime.Now;
            var sectionName = MySession[SessionKeyEnum.SectionName].ToString();

            //加载表单内容
            var temerpnformmodel = ERPNFormService.GetEntityById((int)OutLearningTypeOfFormId.Application).ToViewModel();
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
                    "保存外出进修申请书",
                   Request.UserHostAddress,
                   OutLearningTypeOfFormId.Application.ToString(),
                   Constant.Done,
                   ModuleNameOfScienceResearch.OutLearning.ToString(),
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
                    User.Identity.Name + "上报外出进修申请书",
                    Request.UserHostAddress,
                    OutLearningTypeOfFormId.Application.ToString(),
                    Constant.Done,
                    ModuleNameOfScienceResearch.OutLearning.ToString(),
                    model.NWorkToDoID.ToString(),
                    DateTime.Now
                    );

                //更新已办
                string nworktodoid = model.NWorkToDoID.ToString();
                string formname = OutLearningTypeOfFormId.Application.ToString();
                MyRiZhi = ERPRiZhiService.GetEntities(p => p.FKApplicationID == nworktodoid && p.FKAction == Constant.ToDo).FirstOrDefault().ToViewModel();

                bool updateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());
                //新增待办
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel(
                    FillInRiZhi(Convert.ToInt16(model.WorkFlowID), sectionName),
                    "需要审批('" + User.Identity.Name + "''" + "上报外出进修申请书" + "')",
                    Request.UserHostAddress,
                    OutLearningTypeOfFormId.Application.ToString(),
                    Constant.ToDo,
                    ModuleNameOfScienceResearch.OutLearning.ToString(),
                    model.NWorkToDoID.ToString(),
                    DateTime.Now
                    );

                int UpdateRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                //return RedirectToAction("ApplicationRejected", new { id = model.NWorkToDoID });
                return Json(UpdateRiZhi1Success, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// 借款登记
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult BorrowRegistration(string flag, ERPNWorkToDoViewModel model, FormCollection collection)
        {
            model.UserName = User.Identity.Name;
            model.TimeStr = DateTime.Now;
            var sectionName = MySession[SessionKeyEnum.SectionName].ToString();
            ///加载表单内容
            var temerpnformmodel = ERPNFormService.GetEntityById((int)OutLearningTypeOfFormId.BorrowRegistration).ToViewModel();
            model.FormContent = temerpnformmodel.ContentStr;

            model.FormContent = model.FormContent.Replace(Constant.MacroSectionString, sectionName);
            model.FormContent = model.FormContent.Replace(Constant.MacroUserNameString, User.Identity.Name);

            string formKeys = temerpnformmodel.ItemsList;

            //获取当前表单对应的工作数据列
            string[] formItemArray = temerpnformmodel.ItemsList.Split(Constant.SplitChar);

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

            model.OKUserList = Constant.Default;
            model.ShenPiUserList = Constant.USER_NAME;
            model.FormKeys = formKeys;
            ///保存
            if (flag=="Save")
            {
                ///添加日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                     User.Identity.Name,
                     "保存借款登记",
                     Request.UserHostAddress,
                     OutLearningTypeOfFormId.BorrowRegistration.ToString(),
                     Constant.Done,
                     ModuleNameOfScienceResearch.OutLearning.ToString(),
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

                    erpnworktodomodel.ApplicationStatus = ApplicationStatus.BorrowRegistrationSave.ToString();
                    erpnworktodomodel.ProjectStatus = ApplicationStatus.BorrowRegistrationSave.ToString();

                    bool isUpdateSuccess = ApplicationService.UpdateApplication(erpnworktodomodel.ToDataTransferObjectModel());

                   //return RedirectToAction("BorrowRegistration", new { id = model.ApplicationId, nextaction = act });
                    return Json(returnId, JsonRequestBehavior.AllowGet);
                }
                ///N次保存
                else
                {
                    string act = Constant.Save;
                    model.IsTemporary = true;
                    var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(Convert.ToInt32(model.JieDianID));
                    model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));

                    model.ApplicationStatus = ApplicationStatus.BorrowRegistrationSave.ToString();
                    model.ProjectStatus = ApplicationStatus.BorrowRegistrationSave.ToString();
                    bool isUpdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());
                    return RedirectToAction("BorrowRegistration", new { id = model.NWorkToDoID, nextaction = act });
                }

            }
            else
            {
                ERPNWorkToDoViewModel erpnworktodomodel = new ERPNWorkToDoViewModel();
                erpnworktodomodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();

                ///日志记录 
                ///代办变已办
                string formname = OutLearningTypeOfFormId.BorrowRegistration.ToString();
                var rizhiresult = ERPRiZhiService.GetEntities(p => p.FKApplicationID == model.ApplicationId.ToString() && p.FkFormName == formname && p.FKAction == "待办").FirstOrDefault();
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                    User.Identity.Name,
                    "已填写借款登记",
                    Request.UserHostAddress,
                    OutLearningTypeOfFormId.BorrowRegistration.ToString(),
                     Constant.Done,
                     ModuleNameOfScienceResearch.OutLearning.ToString(),
                     model.ApplicationId.ToString(),
                     DateTime.Now
                    );
                MyRiZhi.ID = rizhiresult.ID;
                bool updateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();
                //新生成承诺书的待办，申请人待办
                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.ID = rizhiresult.ID;
                MyRiZhi1.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.UserName = model.UserName;
                MyRiZhi1.DoSomething = model.UserName + "需要添加承诺书";
                MyRiZhi1.FkFormName = OutLearningTypeOfFormId.PromiseBook.ToString();
                MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.OutLearning.ToString();

                int returnid = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());
               
                //// 更改当前结点id和name
                //var currentNode = ERPNWorkFlowNodeService.GetEntityById(model.JieDianID.Value);
                //string nextNodeSerial = currentNode.NextNode;
                //var nextNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.NodeSerils == nextNodeSerial && p.WorkFlowID == model.WorkFlowID).ToList();
                //model.JieDianID = nextNodeModel.First().ID;
                //model.JieDianName = nextNodeModel.First().NodeName;

                //上报时设置保存为false、驳回为false、删除为false、冻结为false
                model.IsTemporary = false;
                model.IsRejected = false;
                model.IsDeleted = false;
                model.IsLocked = false;
                ////int jiedianid = Convert.ToInt32(model.JieDianID);
                ////var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                //model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));

                //更新申请书中的ApplicationStatus为ProjectProcessing
                erpnworktodomodel.ApplicationStatus = ApplicationStatus.PromiseBookNotAgree.ToString();
                erpnworktodomodel.ProjectStatus = ApplicationStatus.PromiseBookNotAgree.ToString();
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

                ///上报成功标志
                int SucessAdd=1;
                return Json(SucessAdd, JsonRequestBehavior.AllowGet);
                ///return RedirectToAction("BorrowRegistration", new { id = model.ApplicationId });
            }
        }

        /// <summary>
        /// 承诺书
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PromiseBook(ERPNWorkToDoViewModel model, FormCollection collection)
        {
            model.UserName = User.Identity.Name;
            model.TimeStr = DateTime.Now;
            var sectionName = MySession[SessionKeyEnum.SectionName].ToString();
            ///加载表单内容
            var temerpnformmodel = ERPNFormService.GetEntityById((int)OutLearningTypeOfFormId.PromiseBook).ToViewModel();
            model.FormContent = temerpnformmodel.ContentStr;

            model.FormContent = model.FormContent.Replace(Constant.MacroSectionString, sectionName);
            //model.FormContent = model.FormContent.Replace(Constant.MacroUserNameString, User.Identity.Name);
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

            model.OKUserList = Constant.Default;
            model.ShenPiUserList = Constant.USER_NAME;
            model.FormKeys = "";
  
          
                ERPNWorkToDoViewModel erpnworktodomodel = new ERPNWorkToDoViewModel();
                erpnworktodomodel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();

                //写系统日志
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel();
                ///代办变已办
                string formname = OutLearningTypeOfFormId.PromiseBook.ToString();
                var rizhiresult = ERPRiZhiService.GetEntities(p => p.FKApplicationID == model.ApplicationId.ToString() && p.FkFormName == formname && p.FKAction == "待办").FirstOrDefault();
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                    User.Identity.Name,
                    "已填写承诺书",
                    Request.UserHostAddress,
                    OutLearningTypeOfFormId.BorrowRegistration.ToString(),
                     Constant.Done,
                     ModuleNameOfScienceResearch.OutLearning.ToString(),
                     model.ApplicationId.ToString(),
                     DateTime.Now
                    );
                MyRiZhi.ID = rizhiresult.ID;
                bool updateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());


                //待办
                MyRiZhi1.UserName = model.UserName;
                MyRiZhi1.DoSomething = "需要添加派出证明";
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.FkFormName = OutLearningTypeOfFormId.SendProof.ToString();
                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.OutLearning.ToString();


                //上报时设置保存为false、驳回为false、删除为false、冻结为false
                model.IsTemporary = false;
                model.IsRejected = false;
                model.IsDeleted = false;
                model.IsLocked = false;
               
                //更新申请书中的ApplicationStatus为ProjectProcessing
                erpnworktodomodel.ApplicationStatus = ApplicationStatus.SendProof.ToString();
                erpnworktodomodel.ProjectStatus = ApplicationStatus.SendProof.ToString();
                bool isUpdateSuccess = ApplicationService.UpdateApplication(erpnworktodomodel.ToDataTransferObjectModel());
                ///第一次上报
                if (model.NWorkToDoID == 0)
                {

                    int isAddSuccess = this.ApplicationService.AddApplication(model.ToDataTransferObjectModel());

                    MyRiZhi1.FKApplicationID = model.ApplicationId.ToString();
                    int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());
             
                }
                ///先保存后上报
                else
                {
                    bool isUpdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());
                   
                    MyRiZhi1.FKApplicationID = model.NWorkToDoID.ToString();
                    int AddRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());
               
                }

                int SuccessAdd=1;
                return Json(SuccessAdd, JsonRequestBehavior.AllowGet);
                //return RedirectToAction("PromiseBook", new { id = model.ApplicationId });
        }

        /// <summary>
        /// 派出证明
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SendProof(string flag, ERPNWorkToDoViewModel model, FormCollection collection)
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
            var temerpnformmodel = ERPNFormService.GetEntityById((int)OutLearningTypeOfFormId.SendProof).ToViewModel();
            model.FormContent = temerpnformmodel.ContentStr;
            model.FormContent = model.FormContent.Replace(Constant.MacroSectionString, sectionName);
           // model.FormContent = model.FormContent.Replace(Constant.MacroUserNameString, User.Identity.Name);
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

            if (flag == "Save")//保存
            {
                ////日志
                ////添加申请人保存
                //ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                ////已办
                //MyRiZhi.UserName = User.Identity.Name;
                //MyRiZhi.DoSomething = "保存(派出证明)";
                //MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                //MyRiZhi.FkFormName = OutLearningTypeOfFormId.SendProof.ToString();
                //MyRiZhi.FKAction = "已办";
                //MyRiZhi.FKApplicationID = model.ApplicationId.ToString();
                //MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.OutLearning.ToString();

                //int returnRiZhiId = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //第一次保存
               
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
                    erpnworktodomodel.ApplicationStatus = ApplicationStatus.SendProofSave.ToString();
                    erpnworktodomodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    bool isUpdateSuccess = ApplicationService.UpdateApplication(erpnworktodomodel.ToDataTransferObjectModel());

                    return Json(returnid, JsonRequestBehavior.AllowGet);
                    // return RedirectToAction("SignContract", new { id = returnid, nextaction = act });
               
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
                string formname = OutLearningTypeOfFormId.SendProof.ToString();
                var rizhiresult = ERPRiZhiService.GetEntities(p => p.FKApplicationID == model.ApplicationId.ToString() && p.FkFormName == formname && p.FKAction == "待办").FirstOrDefault();
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.ID = rizhiresult.ID;
                MyRiZhi.DoSomething = "已填写派出证明";
                MyRiZhi.FkFormName = OutLearningTypeOfFormId.SendProof.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.OutLearning.ToString();

                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //待办
                MyRiZhi1.UserName = model.UserName;
                MyRiZhi1.DoSomething = "需要审批(派出证明)";
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.FkFormName = OutLearningTypeOfFormId.SendProof.ToString();
                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.TimeStr = DateTime.Now;
                MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.OutLearning.ToString();

                MyRiZhi1.FKApplicationID = model.ApplicationId.ToString();
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

                    int jiedianid = Convert.ToInt32(model.JieDianID);
                    var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                    model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));

                    int isAddSuccess = this.ApplicationService.AddApplication(model.ToDataTransferObjectModel());

                    //更新申请书中的ApplicationStatus为ProjectProcessing
                    erpnworktodomodel.ApplicationStatus = ApplicationStatus.SendProofSubmit.ToString();
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
                    erpnworktodomodel.ApplicationStatus = ApplicationStatus.SendProofSubmit.ToString();
                    erpnworktodomodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    bool isUpdateSuccess = ApplicationService.UpdateApplication(erpnworktodomodel.ToDataTransferObjectModel());

                    ///成功标志
                    int isAddsuccess = 1;
                    return Json(isAddsuccess, JsonRequestBehavior.AllowGet);
                    //return RedirectToAction("ProcessRecordList", new { id = model.ApplicationId });
                }
            }
        }

        /// <summary>
        /// 派出证明审批
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SendProofAgree(string flag, ERPNWorkToDoViewModel model, FormCollection collection)
        {
            string nodeSerils;
            string act = string.Empty;

            ///申请书
            ERPNWorkToDoViewModel ERPModel = new ERPNWorkToDoViewModel();
            ERPModel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();

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
                    ERPModel.ApplicationStatus = ApplicationStatus.ProjectProcessing.ToString();//申请书审批完成
                    ERPModel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    bool isUpdate = this.ApplicationService.UpdateApplication(ERPModel.ToDataTransferObjectModel());
                    ViewBag.Title = model.JieDianName;

                    //toDoAction = "结束";
                    nodeSerils = Constant.End;
                    //nodeSerils=(int)model.JieDianID;
                  
                }
                else
                {
                    // 说明审批流程还未完成，正在等待下一级审批
                    ERPModel.ApplicationStatus = ApplicationStatus.SendProofSubmit.ToString();//申请书待审批中 
                    ERPModel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    //根据序号和workflowID获得下一级节点ID
                    model.JieDianID = nextNodeModel.FirstOrDefault().ID;
                    model.JieDianName = nextNodeModel.FirstOrDefault().NodeName;
                    bool isUpdate = this.ApplicationService.UpdateApplication(ERPModel.ToDataTransferObjectModel());
                    ViewBag.Title = model.JieDianName;

                    // toDoAction = model.JieDianName;
                    //下一个节点的NodeSerils
                    nodeSerils = nextNodeModel.FirstOrDefault().NodeSerils;

                }
               // bool update = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());
            }

            //驳回collection["Reject"]
            else
            {
                act = "rejected";
                model.IsRejected = true;
                ERPModel.ApplicationStatus = ApplicationStatus.SendProofReject.ToString();//申请书审批完成
                ERPModel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                var workFlowNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == model.WorkFlowID).OrderBy(p => p.NodeSerils).Skip(1).Take(1).FirstOrDefault();

                model.JieDianID = workFlowNodeModel.ID;
                model.JieDianName = workFlowNodeModel.NodeName.ToString();

                bool isUpdate = this.ApplicationService.UpdateApplication(ERPModel.ToDataTransferObjectModel());

                //toDoAction = "驳回";
                nodeSerils = Constant.Reject;

            }

            bool update = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());

            string formname = OutLearningTypeOfFormId.SendProof.ToString();
            var rizhiresult = ERPRiZhiService.GetEntities(p => p.FKApplicationID == model.ApplicationId.ToString() && p.FkFormName == formname && p.FKAction == Constant.ToDo).FirstOrDefault();

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
                    "已审批'" + model.UserName + "上报派出证明",
                    Request.UserHostAddress,
                     OutLearningTypeOfFormId.SendProof.ToString(),
                     Constant.Done,
                     ModuleNameOfScienceResearch.OutLearning.ToString(),
                     model.ApplicationId.ToString(),
                     DateTime.Now
                    );
                MyRiZhi.ID = rizhiresult.ID;
                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //新生成待办，下一审批人的待办
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel(
                    rizhiresult.ID,
                    "需要审批('" + model.UserName + "''" + "上报派出证明" + "')",
                    Request.UserHostAddress,
                    OutLearningTypeOfFormId.SendProof.ToString(),
                    Constant.ToDo,
                    ModuleNameOfScienceResearch.OutLearning.ToString(),
                    model.ApplicationId.ToString(),
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
                    "已审批'" + model.UserName + "上报派出证明",
                    Request.UserHostAddress,
                    OutLearningTypeOfFormId.SendProof.ToString(),
                    Constant.Done,
                    ModuleNameOfScienceResearch.OutLearning.ToString(),
                    model.ApplicationId.ToString(),
                    DateTime.Now
                    );

                MyRiZhi.ID = rizhiresult.ID;
                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //新生成新建项目确立的待办，申请人待办
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel(
                    model.UserName,
                    model.UserName + "经费记录",
                    Request.UserHostAddress,
                    OutLearningTypeOfFormId.FeeReimbursement.ToString(),
                    Constant.ToDo,
                    ModuleNameOfScienceResearch.OutLearning.ToString(),
                    model.ApplicationId.ToString(),
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
                    "已驳回'" + model.UserName + "上报派出证明",
                    Request.UserHostAddress,
                    OutLearningTypeOfFormId.SendProof.ToString(),
                    Constant.Done,
                    ModuleNameOfScienceResearch.OutLearning.ToString(),
                    model.ApplicationId.ToString(),
                    DateTime.Now
                    );

                MyRiZhi.ID = rizhiresult.ID;
                bool UpdateRiZhiSuccess1 = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //申请人待办
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel(
                    model.UserName,
                    "派出证明被驳回，请修改",
                    Request.UserHostAddress,
                    OutLearningTypeOfFormId.SendProof.ToString(),
                    Constant.ToDo,
                    ModuleNameOfScienceResearch.OutLearning.ToString(),
                    model.ApplicationId.ToString(),
                    DateTime.Now
                    );

                MyRiZhi1.ID = rizhiresult.ID;
                int returnid = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                // return RedirectToAction("ApplicationRejected", new { id = model.NWorkToDoID });
                return Json(returnid, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 派出证明驳回
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SendProofReject(string flag, ERPNWorkToDoViewModel model, FormCollection collection)
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
            var temerpnformmodel = ERPNFormService.GetEntityById((int)OutLearningTypeOfFormId.SendProof).ToViewModel();
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
                model.ApplicationStatus = ApplicationStatus.SendProofReject.ToString();
                //整个项目进行的状态
                model.ProjectStatus = ApplicationStatus.ProjectProcessing.ToString();
                bool saveUpdateSuccess = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());
                //todo:日志

                //写系统日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                    User.Identity.Name,
                   "保存派出证明申请书",
                   Request.UserHostAddress,
                   OutLearningTypeOfFormId.SendProof.ToString(),
                   Constant.Done,
                   ModuleNameOfScienceResearch.OutLearning.ToString(),
                   model.ApplicationId.ToString(),
                   DateTime.Now);
                //已办
                int UpdateRiZhiSuccess = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                // return RedirectToAction("ApplicationRejected", new { id = model.NWorkToDoID });
                return Json(UpdateRiZhiSuccess, JsonRequestBehavior.AllowGet);
            }
            else//根据已有ID上报更新
            {
                model.ApplicationStatus = ApplicationStatus.SendProofSubmit.ToString();
                model.ProjectStatus = ApplicationStatus.ProjectProcessing.ToString();
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
                    User.Identity.Name + "上报派出证明",
                    Request.UserHostAddress,
                    OutLearningTypeOfFormId.SendProof.ToString(),
                    Constant.Done,
                    ModuleNameOfScienceResearch.OutLearning.ToString(),
                    model.ApplicationId.ToString(),
                    DateTime.Now
                    );

                //更新已办
                string nworktodoid = model.NWorkToDoID.ToString();
                string formname = OutLearningTypeOfFormId.SendProof.ToString();
                MyRiZhi = ERPRiZhiService.GetEntities(p => p.FKApplicationID == nworktodoid && p.FKAction == Constant.ToDo).FirstOrDefault().ToViewModel();

                bool updateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());
                //新增待办
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel(
                   User.Identity.Name,
                    "需要审批('" + User.Identity.Name + "''" +"派出证明" + "')",
                    Request.UserHostAddress,
                    OutLearningTypeOfFormId.SendProof.ToString(),
                    Constant.ToDo,
                    ModuleNameOfScienceResearch.OutLearning.ToString(),
                    model.ApplicationId.ToString(),
                    DateTime.Now
                    );

                int UpdateRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                //return RedirectToAction("ApplicationRejected", new { id = model.NWorkToDoID });
                return Json(UpdateRiZhi1Success, JsonRequestBehavior.AllowGet);
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

            //ToDo
            //string projectType = todoModel.FormValues.Split('#')[1];
            string projectType = "国家级";

            FundsThresholdViewModel fundModel = new FundsThresholdViewModel();
            fundModel = FundsThresholdService.GetEntities(p => p.FundsType == type && p.ModuleName == ModuleNameOfScienceResearch.ScienceResearch.ToString() && p.ProjectType == projectType).FirstOrDefault().ToViewModel();
            //int IsLimited;
            if (priceTotal > fundModel.Threshold)
            {
                ViewBag.limit = "true";
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
                    MyRiZhi.FkFormName =OutLearningTypeOfFormId.FeeReimbursement.ToString();
                    MyRiZhi.FKAction = "已办";
                    MyRiZhi.TimeStr = DateTime.Now;
                    MyRiZhi.ModuleName = ModuleNameOfScienceResearch.OutLearning.ToString();

                    string act = "save";
                    model.IsTemporary = true;
                    model.IsDeleted = false;
                    model.IsLocked = false;

                    //第一次保存
                    if (model.FundsRecordID == 0)
                    {

                        //支出，以后要根据情况修改
                        model.IsIncome = false;

                        model.ModuleName = ApplicationType.OutLearning.ToString();

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
                    MyRiZhi.FkFormName = OutLearningTypeOfFormId.FeeReimbursement.ToString();
                    MyRiZhi.FKAction = "已办";
                    MyRiZhi.TimeStr = DateTime.Now;
                    MyRiZhi.ModuleName = ModuleNameOfScienceResearch.OutLearning.ToString();

                    //待办
                    MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkflowId), sectionName);
                    MyRiZhi1.DoSomething = "需要审批(经费记录)";
                    MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    MyRiZhi1.FkFormName = OutLearningTypeOfFormId.FeeReimbursement.ToString();
                    MyRiZhi1.FKAction = "待办";
                    MyRiZhi1.TimeStr = DateTime.Now;
                    MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.OutLearning.ToString();

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
                        model.ModuleName = ApplicationType.OutLearning.ToString();

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
            if (model.WorkflowId == Convert.ToInt16(OutLearningTypeOfWorkflowId.FeeReimbursement))
            {
                string formname = OutLearningTypeOfFormId.FeeReimbursement.ToString();
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
                    MyRiZhi.FkFormName = OutLearningTypeOfFormId.FeeReimbursement.ToString();
                    //FundsRecord的ID
                    MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi.TimeStr = DateTime.Now;
                    MyRiZhi.ModuleName = ModuleNameOfScienceResearch.OutLearning.ToString();

                    bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                    //新生成待办，下一审批人的待办

                    MyRiZhi1.FKAction = "待办";
                    MyRiZhi1.ID = rizhiresult.ID;
                    //FundsRecord的ID
                    MyRiZhi1.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    MyRiZhi1.DoSomething = "需要审批(" + erpnworktodoModel.UserName + "上报的经费报销单)";
                    MyRiZhi1.FkFormName = OutLearningTypeOfFormId.FeeReimbursement.ToString();
                    MyRiZhi1.TimeStr = DateTime.Now;
                    MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.OutLearning.ToString();

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
                    MyRiZhi.FkFormName = OutLearningTypeOfFormId.FeeReimbursement.ToString();
                    //FundsRecord的ID
                    MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.OutLearning.ToString();

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
                    MyRiZhi.FkFormName = OutLearningTypeOfFormId.FeeReimbursement.ToString();
                    //FundsRecord的ID
                    MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi.TimeStr = DateTime.Now;
                    MyRiZhi.ModuleName = ModuleNameOfScienceResearch.OutLearning.ToString();

                    bool UpdateRiZhiSuccess1 = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());
                    //申请人待办

                    MyRiZhi1.FKAction = "待办";
                    MyRiZhi1.ID = rizhiresult.ID;
                    MyRiZhi1.UserName = erpnworktodoModel.UserName;
                    MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    MyRiZhi1.DoSomething = "经费报销单被驳回，需修改数据";
                    MyRiZhi1.FkFormName = OutLearningTypeOfFormId.FeeReimbursement.ToString();
                    MyRiZhi1.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi1.TimeStr = DateTime.Now;
                    MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.OutLearning.ToString();

                    int returnid = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                    return Json(returnid, JsonRequestBehavior.AllowGet);
                    // return RedirectToAction("ReimburseRejected", new { id = model.FundsRecordID });
                }
            }
            //差旅报销单的日志
            else
            {
                string formname = OutLearningTypeOfFormId.TravelReimbursement.ToString();
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
                    MyRiZhi.FkFormName = OutLearningTypeOfFormId.TravelReimbursement.ToString();
                    //FundsRecord的ID
                    MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi.TimeStr = DateTime.Now;
                    MyRiZhi.ModuleName = ModuleNameOfScienceResearch.OutLearning.ToString();

                    bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                    //新生成待办，下一审批人的待办

                    MyRiZhi1.FKAction = "待办";
                    MyRiZhi1.ID = rizhiresult.ID;
                    //FundsRecord的ID
                    MyRiZhi1.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    MyRiZhi1.DoSomething = "需要审批(" + erpnworktodoModel.UserName + "上报的差旅报销单)";
                    MyRiZhi1.FkFormName = OutLearningTypeOfFormId.TravelReimbursement.ToString();
                    MyRiZhi1.TimeStr = DateTime.Now; MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.OutLearning.ToString();

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
                    MyRiZhi.FkFormName = OutLearningTypeOfFormId.TravelReimbursement.ToString();
                    //FundsRecord的ID
                    MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.OutLearning.ToString();

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
                    MyRiZhi.FkFormName = OutLearningTypeOfFormId.TravelReimbursement.ToString();
                    //FundsRecord的ID
                    MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi.TimeStr = DateTime.Now;
                    MyRiZhi.ModuleName = ModuleNameOfScienceResearch.OutLearning.ToString();

                    bool UpdateRiZhiSuccess1 = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());
                    //申请人待办

                    MyRiZhi1.FKAction = "待办";
                    MyRiZhi1.ID = rizhiresult.ID;
                    MyRiZhi1.UserName = erpnworktodoModel.UserName;
                    MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    MyRiZhi1.DoSomething = "差旅报销单被驳回，需修改数据";
                    MyRiZhi1.FkFormName = OutLearningTypeOfFormId.TravelReimbursement.ToString();
                    //FundsRecord的ID
                    MyRiZhi1.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi1.TimeStr = DateTime.Now;
                    MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.OutLearning.ToString();

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

            //ToDo
            //string projectType = todoModel.FormValues.Split('#')[1];
            string projectType = "国家级";

            FundsThresholdViewModel fundModel = new FundsThresholdViewModel();
            fundModel = FundsThresholdService.GetEntities(p => p.FundsType == type && p.ModuleName == ModuleNameOfScienceResearch.ScienceResearch.ToString() && p.ProjectType == projectType).FirstOrDefault().ToViewModel();
            //int IsLimited;
            if (priceTotal > fundModel.Threshold)
            {

                /// IsLimited =1;
                ViewBag.limit = "true";
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
                    MyRiZhi.FkFormName = OutLearningTypeOfFormId.FeeReimbursement.ToString();
                    MyRiZhi.FKAction = "已办";
                    //FKApplicationID是过程记录的ID
                    MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi.ModuleName = ModuleNameOfScienceResearch.OutLearning.ToString();
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
                    MyRiZhi.FkFormName = OutLearningTypeOfFormId.FeeReimbursement.ToString();
                    MyRiZhi.FKAction = "已办";
                    MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi.TimeStr = DateTime.Now;
                    MyRiZhi.ModuleName = ModuleNameOfScienceResearch.OutLearning.ToString();
                    bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                    //待办
                    MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkflowId), sectionName);
                    MyRiZhi1.DoSomething = "需要审批(经费记录)";
                    MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                    MyRiZhi1.FkFormName = OutLearningTypeOfFormId.FeeReimbursement.ToString();
                    MyRiZhi1.FKAction = "待办";
                    MyRiZhi1.TimeStr = DateTime.Now;
                    MyRiZhi1.FKApplicationID = model.FundsRecordID.ToString();
                    MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.OutLearning.ToString();

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
            model.WorkflowId = (int)OutLearningTypeOfWorkflowId.TravelReimbursement;
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
                MyRiZhi.FkFormName = OutLearningTypeOfFormId.TravelReimbursement.ToString();
                MyRiZhi.FKAction = "已办";
                //FKApplicationID是过程记录的ID
                MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.OutLearning.ToString();
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
                MyRiZhi.FkFormName = OutLearningTypeOfFormId.TravelReimbursement.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = model.FundsRecordID.ToString();
                MyRiZhi.TimeStr = DateTime.Now;
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.OutLearning.ToString();
                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //待办
                MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkflowId), sectionName);
                MyRiZhi1.DoSomething = "需要审批(差旅报销单)";
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.FkFormName = OutLearningTypeOfFormId.TravelReimbursement.ToString();
                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.TimeStr = DateTime.Now;
                MyRiZhi1.FKApplicationID = model.FundsRecordID.ToString();
                MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.OutLearning.ToString();

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
            model.WorkflowId = (int)OutLearningTypeOfWorkflowId.TravelReimbursement;
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
                MyRiZhi.FkFormName = OutLearningTypeOfFormId.TravelReimbursement.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi.TimeStr = DateTime.Now;
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.OutLearning.ToString();

                string act = "save";
                model.IsTemporary = true;
                model.IsDeleted = false;
                //第一次保存
                //注意直接用ID可能出错，页面也未改
                if (model.FundsRecordID == 0)
                {

                    //支出，以后要根据情况修改
                    model.IsIncome = false;

                    model.ModuleName = ApplicationType.OutLearning.ToString();

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
                MyRiZhi.FkFormName = OutLearningTypeOfFormId.TravelReimbursement.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.TimeStr = DateTime.Now;
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.OutLearning.ToString();

                //待办
                MyRiZhi1.UserName = FillInRiZhi(Convert.ToInt16(model.WorkflowId), sectionName);
                MyRiZhi1.DoSomething = "需要审批(差旅报销单)";
                MyRiZhi1.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi1.FkFormName = OutLearningTypeOfFormId.TravelReimbursement.ToString();
                MyRiZhi1.FKAction = "待办";
                MyRiZhi1.TimeStr = DateTime.Now;
                MyRiZhi1.ModuleName = ModuleNameOfScienceResearch.OutLearning.ToString();

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
                    model.ModuleName = ApplicationType.OutLearning.ToString();

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
        /// 完成情况
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Completion(string flag, ERPNWorkToDoViewModel model, FormCollection collection)
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
            var temerpnformmodel = ERPNFormService.GetEntityById((int)OutLearningTypeOfFormId.Completion).ToViewModel();
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

            if (flag == "Save")//保存
            {
                //日志
                //添加申请人保存
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                //已办
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.DoSomething = "保存(派出证明)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.FkFormName = OutLearningTypeOfFormId.Completion.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.OutLearning.ToString();

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
                    erpnworktodomodel.ApplicationStatus = ApplicationStatus.Completing.ToString();
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
                    erpnworktodomodel.ApplicationStatus = ApplicationStatus.Completing.ToString();
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
                //已办
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.DoSomething = User.Identity.Name + "上报完成情况";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                // MyRiZhi.NotificationContent = "添加的" + model.WenHao + "已经提交";
                MyRiZhi.FkFormName =OutLearningTypeOfFormId.Completion.ToString();
                MyRiZhi.FKAction = "待办";
                MyRiZhi.FKApplicationID =model.ApplicationId.ToString();
                MyRiZhi.TimeStr = DateTime.Now; MyRiZhi.ModuleName = ModuleNameOfScienceResearch.OutLearning.ToString();

                int AddRiZhiSuccess = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

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

                    int jiedianid = Convert.ToInt32(model.JieDianID);
                    var temperpworkflownodemodel = ERPNWorkFlowNodeService.GetEntityById(jiedianid);
                    model.LateTime = DateTime.Now.AddHours(double.Parse(temperpworkflownodemodel.JieShuHours.ToString()));

                    int isAddSuccess = this.ApplicationService.AddApplication(model.ToDataTransferObjectModel());

                    //更新申请书中的ApplicationStatus为ProjectProcessing
                    erpnworktodomodel.ApplicationStatus = ApplicationStatus.CompletSubmit.ToString();
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
                    erpnworktodomodel.ApplicationStatus = ApplicationStatus.CompletSubmit.ToString();
                    erpnworktodomodel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    bool isUpdateSuccess = ApplicationService.UpdateApplication(erpnworktodomodel.ToDataTransferObjectModel());

                    ///成功标志
                    int isAddsuccess = 1;
                    return Json(isAddsuccess, JsonRequestBehavior.AllowGet);
                    //return RedirectToAction("ProcessRecordList", new { id = model.ApplicationId });
                }
            }
        }

        /// <summary>
        /// 完成情况审批
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CompletionAgree(string flag, ERPNWorkToDoViewModel model, FormCollection collection)
        {
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

                ///申请书
                ERPNWorkToDoViewModel ERPModel = new ERPNWorkToDoViewModel();
                ERPModel = ApplicationService.GetEntityById(model.ApplicationId).ToViewModel();


                if (nextNodeModel.Count == Constant.ZERO_INT)
                {
                    // 说明审批流程已经完成，审批通过
                    model.JieDianName = Constant.End;
                    ERPModel.ApplicationStatus = ApplicationStatus.Completed.ToString();//申请书审批完成
                    ERPModel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                    bool isUpdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());
                    ViewBag.Title = model.JieDianName;

                    //toDoAction = "结束";
                    nodeSerils = Constant.End;
                    //nodeSerils=(int)model.JieDianID;
                }
                else
                {
                    // 说明审批流程还未完成，正在等待下一级审批
                    ERPModel.ApplicationStatus = ApplicationStatus.CompletSubmit.ToString();//申请书待审批中 
                    ERPModel.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
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
                model.ApplicationStatus = ApplicationStatus.CompletReject.ToString();//申请书审批完成
                model.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                var workFlowNodeModel = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == model.WorkFlowID).OrderBy(p => p.NodeSerils).Skip(1).Take(1).FirstOrDefault();

                model.JieDianID = workFlowNodeModel.ID;
                model.JieDianName = workFlowNodeModel.NodeName.ToString();

                bool isUpdate = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());

                //toDoAction = "驳回";
                nodeSerils = Constant.Reject;

            }

            string formname = OutLearningTypeOfFormId.Completion.ToString();
            var rizhiresult = ERPRiZhiService.GetEntities(p => p.FKApplicationID == model.ApplicationId.ToString() && p.FkFormName == formname && p.FKAction == Constant.ToDo).FirstOrDefault();

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
                    "已审批'" + model.UserName +"上报完成情况",
                    Request.UserHostAddress,
                     OutLearningTypeOfFormId.Completion.ToString(),
                     Constant.Done,
                     ModuleNameOfScienceResearch.OutLearning.ToString(),
                     model.ApplicationId.ToString(),
                     DateTime.Now
                    );
                MyRiZhi.ID = rizhiresult.ID;
                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //新生成待办，下一审批人的待办
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel(
                    rizhiresult.ID,
                    "需要审批('" + model.UserName + "''" + "上报完成情况" + "')",
                    Request.UserHostAddress,
                    OutLearningTypeOfFormId.Completion.ToString(),
                    Constant.ToDo,
                    ModuleNameOfScienceResearch.OutLearning.ToString(),
                    model.ApplicationId.ToString(),
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
                    "已审批'" + model.UserName + "上报完成情况",
                    Request.UserHostAddress,
                    OutLearningTypeOfFormId.Completion.ToString(),
                    Constant.Done,
                    ModuleNameOfScienceResearch.OutLearning.ToString(),
                    model.ApplicationId.ToString(),
                    DateTime.Now
                    );

                MyRiZhi.ID = rizhiresult.ID;
                bool UpdateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

              
                int returnId = 1;
                return Json(returnId, JsonRequestBehavior.AllowGet);
            }
            //toDoAction == "驳回"
            else
            {
                //审批人已办
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                    User.Identity.Name,
                    "已驳回'" + model.UserName + "上报完成情况",
                    Request.UserHostAddress,
                    OutLearningTypeOfFormId.Completion.ToString(),
                    Constant.Done,
                    ModuleNameOfScienceResearch.OutLearning.ToString(),
                    model.ApplicationId.ToString(),
                    DateTime.Now
                    );

                MyRiZhi.ID = rizhiresult.ID;
                bool UpdateRiZhiSuccess1 = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                //申请人待办
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel(
                    model.UserName,
                    "申请书被驳回，请修改",
                    Request.UserHostAddress,
                    OutLearningTypeOfFormId.Completion.ToString(),
                    Constant.ToDo,
                    ModuleNameOfScienceResearch.OutLearning.ToString(),
                    model.ApplicationId.ToString(),
                    DateTime.Now
                    );

                MyRiZhi1.ID = rizhiresult.ID;
                int returnid = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                // return RedirectToAction("ApplicationRejected", new { id = model.NWorkToDoID });
                return Json(returnid, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 完成情况驳回
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CompletionReject(string flag, ERPNWorkToDoViewModel model, FormCollection collection)
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
            var temerpnformmodel = ERPNFormService.GetEntityById((int)OutLearningTypeOfFormId.Completion).ToViewModel();
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
                model.ApplicationStatus = ApplicationStatus.CompletReject.ToString();
                //整个项目进行的状态
                model.ProjectStatus = ApplicationStatus.BigProjectProcessing.ToString();
                bool saveUpdateSuccess = this.ApplicationService.UpdateApplication(model.ToDataTransferObjectModel());
                //todo:日志

                //写系统日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel(
                    User.Identity.Name,
                  "保存完成情况申请",
                   Request.UserHostAddress,
                   OutLearningTypeOfFormId.Completion.ToString(),
                   Constant.Done,
                   ModuleNameOfScienceResearch.OutLearning.ToString(),
                   model.ApplicationId.ToString(),
                   DateTime.Now);
                //已办
                int UpdateRiZhiSuccess = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());

                // return RedirectToAction("ApplicationRejected", new { id = model.NWorkToDoID });
                return Json(UpdateRiZhiSuccess, JsonRequestBehavior.AllowGet);
            }
            else//根据已有ID上报更新
            {
                model.ApplicationStatus = ApplicationStatus.CompletSubmit.ToString();
                model.ProjectStatus = ApplicationStatus.ProjectProcessing.ToString();
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
                    User.Identity.Name + "上报完成情况",
                    Request.UserHostAddress,
                    OutLearningTypeOfFormId.Completion.ToString(),
                    Constant.Done,
                    ModuleNameOfScienceResearch.OutLearning.ToString(),
                    model.ApplicationId.ToString(),
                    DateTime.Now
                    );

                //更新已办
                string nworktodoid = model.ApplicationId.ToString();
                string formname = OutLearningTypeOfFormId.Completion.ToString();
                MyRiZhi = ERPRiZhiService.GetEntities(p => p.FKApplicationID == nworktodoid && p.FKAction == Constant.ToDo).FirstOrDefault().ToViewModel();

                bool updateRiZhiSuccess = ERPRiZhiService.UpdateERPRiZhi(MyRiZhi.ToDataTransferObjectModel());
                //新增待办
                ERPRiZhiViewModel MyRiZhi1 = new ERPRiZhiViewModel(
                    FillInRiZhi(Convert.ToInt16(model.WorkFlowID), sectionName),
                    "需要审批('" + User.Identity.Name + "''" + "上报完成情况" + "')",
                    Request.UserHostAddress,
                    OutLearningTypeOfFormId.Completion.ToString(),
                    Constant.ToDo,
                    ModuleNameOfScienceResearch.OutLearning.ToString(),
                    model.ApplicationId.ToString(),
                    DateTime.Now
                    );

                int UpdateRiZhi1Success = ERPRiZhiService.AddERPRiZhi(MyRiZhi1.ToDataTransferObjectModel());

                //return RedirectToAction("ApplicationRejected", new { id = model.NWorkToDoID });
                return Json(UpdateRiZhi1Success, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        ///任务跟踪提交
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
            var temerpnformmodel = ERPNFormService.GetEntityById((int)OutLearningTypeOfFormId.TaskTracking).ToViewModel();
            model.FormContent = temerpnformmodel.ContentStr;
            model.FormContent = model.FormContent.Replace(Constant.MacroSectionString, sectionName);
            model.FormContent = model.FormContent.Replace(Constant.MacroUserNameString, User.Identity.Name);

            string formKeys = "记录名称#记录类型#记录时间#任务进度#概述";
            //string name = collection["Text397670573"].ToString();
            //string type = collection["Drop46201924"].ToString();
            //string time = collection["Date1476233541"].ToString();
            //string people = collection["Text309804476"].ToString();
            //string formvalues = name + "#" + type + "#" + time + "#" + people;

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
            model.FormKeys = formKeys;
            //model.FormValues = formvalues;

            #endregion
            #region 已办日志

                //写系统日志
                ERPRiZhiViewModel MyRiZhi = new ERPRiZhiViewModel();
                MyRiZhi.UserName = User.Identity.Name;
                MyRiZhi.DoSomething = "添加(任务跟踪记录)";
                MyRiZhi.IpStr = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
                MyRiZhi.FkFormName = OutLearningTypeOfFormId.TaskTracking.ToString();
                MyRiZhi.FKAction = "已办";
                MyRiZhi.FKApplicationID = model.ApplicationId.ToString();
                MyRiZhi.TimeStr = DateTime.Now;
                MyRiZhi.ModuleName = ModuleNameOfScienceResearch.OutLearning.ToString();

                #endregion

                #region 过程记录

                //上报时设置保存为false、驳回为false、删除为false、冻结为false
                model.IsTemporary = false;
                model.IsRejected = false;
                model.IsDeleted = false;
                model.IsLocked = false;

                int nworktodoId = 0;
               
                int returnprocessid = this.ApplicationService.AddApplication(model.ToDataTransferObjectModel());
                nworktodoId = returnprocessid;
                //过程记录的ID
                MyRiZhi.FKApplicationID = returnprocessid.ToString();
                int AddRiZhiSuccess = ERPRiZhiService.AddERPRiZhi(MyRiZhi.ToDataTransferObjectModel());
               
                #endregion
              
                //上报成功的标志
                ViewBag.SendUpSuccess = true;

                ///成功标志
                int isAddsuccess = 1;
                return Json(isAddsuccess, JsonRequestBehavior.AllowGet);
          
        }

        #endregion


        #region Private Method

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

        /// <summary>
        /// 判断首页正在办理前10条申请书状态，返回状态
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
        #endregion
    }
}