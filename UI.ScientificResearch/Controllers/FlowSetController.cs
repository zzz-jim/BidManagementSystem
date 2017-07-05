using PF.DomainModel.Identity;
using ScientificResearch.BusinessLogicImplement;
using ScientificResearch.DataTransferModel;
using ScientificResearch.IBusinessLogic;
using ScientificResearch.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UI.ScientificResearch.Extensions;

namespace UI.ScientificResearch.Controllers
{
    [CheckLogin]
    [Authorize(Roles = "普通用户")]
    public class FlowSetController : Controller
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

        public FlowSetController()
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
                new SessionManager()
            )
        {
        }

        public FlowSetController(
            IERPNFormService eRPNFormService,
            IERPBuMenService eRPBuMenService,
            IERPRiZhiService eRPRiZhiService,
            IApplicationService applicationService,
            IERPNWorkFlowService eRPNWorkFlowService,
            IERPNWorkFlowNodeService eRPNWorkFlowNodeService,
            IFundsRecordService eFundsRecordService,
            IProjectRecordService eProjectRecordService,
            IProjectBonusCreditService eProjectBonusCreditService,
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
            this.ProjectRecordService = eProjectRecordService;
            this.ProjectBonusCreditService = eProjectBonusCreditService;
            this.MySession = session;
        }

        #endregion

        //
        // GET: /FlowSet/
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 审批设置列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ApprovalSetList()
        { 
        int typeID=11;
        IEnumerable<ERPNFormViewModel> erpnFormList = ERPNFormService.GetEntities(p => p.TypeID == typeID).Select(x=>x.ToViewModel()).ToList();

        ViewBag.Module = "审批设置";
        ViewBag.Title = "审批流程列表";

        return View(erpnFormList);
        }
        /// <summary>
        /// 修改流程
        /// </summary>
        /// <returns></returns>
        public ActionResult ModifiedFlowList(string formid)
        {
            ViewBag.formid = formid;
            return View();
        }
        public ActionResult ModifiedFlow(string formid)
        {
            var workFlowModel = ERPNWorkFlowService.GetEntities(p => p.FormID == Convert.ToInt16(formid)).FirstOrDefault();
            int wokFlowId = workFlowModel.ID;
            IEnumerable<ERPNWorkFlowNodeViewModel> workFlowNodeList = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == wokFlowId && p.NodeAddr != "开始").Select(x => x.ToViewModel()).OrderBy(p => Convert.ToInt16(p.NodeSerils));

            return Json(new { data = workFlowNodeList, total = workFlowNodeList.Count() }, JsonRequestBehavior.AllowGet);

        }
       
        [HttpPost]
        public ActionResult DeleteThisRowByFormId(string modelId)
        {
            //是否是第一个或者最后一个节点
            bool isfirstorlast=false;
            //是否删除成功
            bool deletedataScucess = false;
            //ERPNWorkToDo是否存在与此节点相关数据
            bool hasRelationData = false;
            int workflownodeid = Convert.ToInt16(modelId);
            var currentModel = ERPNWorkFlowNodeService.GetEntityById(workflownodeid);
            int workFlowId=(int)currentModel.WorkFlowID;
            string currentNode = currentModel.NodeSerils;
            string nextNode = currentModel.NextNode;
            string upNode = (Convert.ToInt16(currentNode) - 1).ToString();
            //判断是否有是一个节点
            int hasUpNodeModelCount = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == workFlowId && p.NodeSerils == upNode).Count;
            //判断是不是最后一个或者是第一个节点
            if (nextNode == "" || hasUpNodeModelCount == 0)
            {
                deletedataScucess = false;
                isfirstorlast = true;
            }
            else
            {
              
               int hasCountInErpnworkToDo  = ApplicationService.GetEntities(p => p.JieDianID == workflownodeid).Count;
                //判断ERPNWorkToDo是否有与此相关的节点数据
               if (hasCountInErpnworkToDo <=0)
                {
                    ERPNWorkFlowNodeViewModel model = new ERPNWorkFlowNodeViewModel();
                    bool deleteSuccess = ERPNWorkFlowNodeService.DeleteEntityById(workflownodeid);
                    isfirstorlast = false;
                    if (deleteSuccess)
                    {
                        deletedataScucess = true;
                        //对其他数据行进行重排序,更新删除行的后面几行
                        //上几行
                        IEnumerable<ERPNWorkFlowNodeViewModel> nextNodeModelList = new List<ERPNWorkFlowNodeViewModel>();
                        nextNodeModelList = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == workFlowId && Convert.ToInt16(p.NodeSerils) > Convert.ToInt16(currentNode)).Select(x => x.ToViewModel());
                        foreach (var item in nextNodeModelList)
                        {
                            ERPNWorkFlowNodeViewModel TempModel = new ERPNWorkFlowNodeViewModel();
                            TempModel = ERPNWorkFlowNodeService.GetEntities(p => p.ID == item.ID).FirstOrDefault().ToViewModel();
                            TempModel.NodeSerils = (int.Parse(item.NodeSerils) - 1).ToString();
                            if (item.NextNode != "")
                            {
                                TempModel.NextNode = (int.Parse(item.NextNode) - 1).ToString();
                            }
                            else
                            {

                            }
                            //更新单行内容
                            bool updateSuccess = ERPNWorkFlowNodeService.UpdateERPNWorkFlowNode(TempModel.ToDataTransferObjectModel());
                        }
                    }
                    else
                    {
                        deletedataScucess = false;
                    }
                    hasRelationData = false;
                }
                else
                {
                    deletedataScucess = false;
                    hasRelationData = true;
                }
            }
            //返回删除成功的标志
            return Json(new { deletedataScucess, isfirstorlast, hasRelationData }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult CreateThisRowByFormId()
        {
            ERPNWorkFlowNodeViewModel erpnWorkFlwoNodeModel = new ERPNWorkFlowNodeViewModel();
            erpnWorkFlwoNodeModel.NodeSerils = "2";
            erpnWorkFlwoNodeModel.NodeAddr = "中间段";
            erpnWorkFlwoNodeModel.CanWriteSet = "";
            erpnWorkFlwoNodeModel.SecretSet = "";
            erpnWorkFlwoNodeModel.NextNode = "3";
            erpnWorkFlwoNodeModel.JieShuHours = 72;
            ViewData["spdefalutlist"] = GetSpItemList();
            return View(erpnWorkFlwoNodeModel);
        }
        [HttpPost]
        public ActionResult CreateThisRowByFormId(string formId, ERPNWorkFlowNodeViewModel model)
        {
            ERPNWorkFlowViewModel erpnWorkFlowModel = ERPNWorkFlowService.GetEntities(p => p.FormID ==Convert.ToInt16(formId)).FirstOrDefault().ToViewModel();
            bool addSuccess = false;
            
            if (erpnWorkFlowModel != null)
            {
                //新增的行
                ERPNWorkFlowNodeViewModel erpnWorkFlowNodeModel = new ERPNWorkFlowNodeViewModel();
                erpnWorkFlowNodeModel.WorkFlowID = erpnWorkFlowModel.ID;
                erpnWorkFlowNodeModel.NodeSerils ="2";
                erpnWorkFlowNodeModel.NodeAddr = "中间段";
                erpnWorkFlowNodeModel.CanWriteSet = "";
                erpnWorkFlowNodeModel.SecretSet = "";
                erpnWorkFlowNodeModel.NextNode = "3";
                erpnWorkFlowNodeModel.JieShuHours = 72;
                erpnWorkFlowNodeModel.NodeName = model.NodeName;
                int returnId = ERPNWorkFlowNodeService.AddERPNWorkFlowNode(erpnWorkFlowNodeModel.ToDataTransferObjectModel());
               
                //数据库中已经存在的行
                IEnumerable<ERPNWorkFlowNodeViewModel> erpnWorkFlowNodeModelList = new List<ERPNWorkFlowNodeViewModel>();
                erpnWorkFlowNodeModelList = ERPNWorkFlowNodeService.GetEntities(p => p.WorkFlowID == erpnWorkFlowModel.ID && p.ID != returnId&&p.NodeAddr!="开始").Select(x => x.ToViewModel());
                foreach (var item in erpnWorkFlowNodeModelList)
                {
                    ERPNWorkFlowNodeViewModel TempModel = new ERPNWorkFlowNodeViewModel();
                    TempModel = ERPNWorkFlowNodeService.GetEntities(p=>p.ID==item.ID).FirstOrDefault().ToViewModel();
                    TempModel.NodeSerils = (int.Parse(item.NodeSerils) + 1).ToString();
                    if (item.NextNode != "")
                    {
                        TempModel.NextNode = (int.Parse(item.NextNode) + 1).ToString();
                    }
                    else
                    { 
                    
                    }
                    
                    //更新单行内容
                    bool updateSuccess = ERPNWorkFlowNodeService.UpdateERPNWorkFlowNode(TempModel.ToDataTransferObjectModel());
                }

                    addSuccess = true;
            }
            else
            {
                addSuccess = false;
            }

            ViewData["spdefalutlist"] = GetSpItemList();
            return View(model);
        }
        public ActionResult EidtFlwswet(string workflownodeid)
        {
            ERPNWorkFlowNodeViewModel erpnWorkFloweNodeModel = new ERPNWorkFlowNodeViewModel();
           
            erpnWorkFloweNodeModel = ERPNWorkFlowNodeService.GetEntityById(Convert.ToInt16(workflownodeid)).ToViewModel();
           
            ViewData["spdefalutlist"] = GetSpItemList();
            return View(erpnWorkFloweNodeModel);
        }
        [HttpPost]
        public ActionResult EidtFlwswet(ERPNWorkFlowNodeViewModel model)
        {
            model.CanWriteSet = "";
            model.SecretSet = "";
            bool updateSuccess = ERPNWorkFlowNodeService.UpdateERPNWorkFlowNode(model.ToDataTransferObjectModel());
           
            //审批人下拉列表
            ViewData["spdefalutlist"] = GetSpItemList();
            //修改人和最后修改时间(ERPNForm表中)

            ERPNWorkFlowViewModel erpnWorkFlwoModel=new ERPNWorkFlowViewModel ();
            int workflowId=Convert.ToInt16( model.WorkFlowID);
            erpnWorkFlwoModel=ERPNWorkFlowService.GetEntityById(workflowId).ToViewModel();

            ERPNFormViewModel erpnFormModel = new ERPNFormViewModel();
            erpnFormModel = ERPNFormService.GetEntityById(Convert.ToInt16(erpnWorkFlwoModel.FormID)).ToViewModel();
            erpnFormModel.ModifiedPerson = User.Identity.Name;
            erpnFormModel.LastModifiedTime = System.DateTime.Now;
            bool updateerpnFormModelSucces = ERPNFormService.UpdateERPNForm(erpnFormModel.ToDataTransferObjectModel());

            return View(model);
        }
        //审批人下拉列表
        public List<SelectListItem> GetSpItemList()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var list = context.Roles.ToList();
            List<SelectListItem> spdefalutlist = new List<SelectListItem>();

            foreach (var item in list)
            {
                spdefalutlist.Add(new SelectListItem { Text = item.Name.ToString(), Value = item.Name.ToString() });
            }
            return spdefalutlist;
        }

	}
}