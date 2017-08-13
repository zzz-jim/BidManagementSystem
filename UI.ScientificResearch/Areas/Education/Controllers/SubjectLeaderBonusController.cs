using System;
using System.Web.Mvc;

using ScientificResearch.BusinessLogicImplement;
using ScientificResearch.IBusinessLogic;
using ScientificResearch.ViewModel;
using UI.ScientificResearch.Extensions;

namespace UI.ScientificResearch.Areas.Education.Controllers
{
    [CheckLogin]
    [Authorize(Roles = "普通用户")]
    public class SubjectLeaderBonusController : Controller
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

        //private ISession session;

        #endregion

        #region Constructor

        public SubjectLeaderBonusController()
            : this(
                new ERPNFormServiceImplement(),
                new ERPBuMenServiceImplement(),
                new ERPRiZhiServiceImplement(),
                new ApplicationServiceImplement(),
                new ERPNWorkFlowServiceImplement(),
                new ERPNWorkFlowNodeServiceImplement(),
                new FundsRecordServiceImplement(),
                new ProjectRecordServiceImplement(),
                new ProjectBonusCreditServiceImplement()
            )//, new SessionManager())
        {
        }

        public SubjectLeaderBonusController(
            IERPNFormService eRPNFormService,
            IERPBuMenService eRPBuMenService,
            IERPRiZhiService eRPRiZhiService,
            IApplicationService applicationService,
            IERPNWorkFlowService eRPNWorkFlowService,
            IERPNWorkFlowNodeService eRPNWorkFlowNodeService,
            IFundsRecordService eFundsRecordService,
            IProjectRecordService eProjectRecordService,
            IProjectBonusCreditService eProjectBonusCreditService
            )
        //, ISession session)
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
            // this.session = session;
        }

        #endregion

        /// <summary>
        /// 添加奖励设置
        /// </summary>
        /// <returns></returns>
        public ActionResult ProjectBonusCreditAdd()
        {
            return View(new ProjectBonusCreditViewModel());
        }

        /// <summary>
        /// 添加奖励设置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ProjectBonusCreditAdd(ProjectBonusCreditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.CreatedBy = User.Identity.Name;
            model.CreateTime = DateTime.Now;

            int resultId = ProjectBonusCreditService.AddProjectBonusCredit(model.ToDataTransferObjectModel());

            if (resultId > 0)
            {
                model.Id = resultId;
            }

            return View("ProjectBonusCreditDetail", model);
        }

        /// <summary>
        /// 更新奖励设置
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public ActionResult ProjectBonusCreditUpdate(int id)
        {
            var model = ProjectBonusCreditService.GetEntityById(id);

            if (model != null && model.Id != 0)
            {
                return View(model.ToViewModel());
            }
            else
            {
                return View(new ProjectBonusCreditViewModel());
            }
        }

        /// <summary>
        /// 更新奖励设置
        /// </summary>
        /// <param name="model">奖励model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ProjectBonusCreditUpdate(ProjectBonusCreditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.UpdateBy = User.Identity.Name;
            model.UpdateTime = DateTime.Now;

            var result = ProjectBonusCreditService.UpdateProjectBonusCredit(model.ToDataTransferObjectModel());

            if (result)
            {
                return View("ProjectBonusCreditDetail", model);
            }
            else
            {
                ViewBag.UpdateResult = "更新失败，请重试！！";

                return View(model);
            }
        }

        /// <summary>
        /// 奖励设置详情
        /// </summary>
        /// <returns></returns>
        public ActionResult ProjectBonusCreditDetail(int id)
        {
            var model = ProjectBonusCreditService.GetEntityById(id);

            if (model != null && model.Id != 0)
            {
                return View(model.ToViewModel());
            }
            else
            {
                return View(new ProjectBonusCreditViewModel());
            }
        }
    }
}