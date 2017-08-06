﻿using System;
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
using ScientificResearch.IDataAccess;
using ScientificResearch.DataAccessImplement;
using Microsoft.AspNet.Identity;

namespace UI.ScientificResearch.Controllers
{
    //[CheckLogin]
    //[Authorize(Roles = "普通用户")]
    public class RegisterInfoController : Controller
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
        private IProjectRegistrationRepository ProjectRegistrationService;
        private IProjectBidSectionRepository ProjectBidSectionService;

        private ISession MySession;

        #endregion

        #region Private Field

        private string SearchCriteriaProjectName;
        private string SearchCriteriaProjectStatus;
        private string SearchCriteriaIsLocked;
        private DateTime SearchCriteriaStartTime;
        private DateTime SearchCriteriaEndTime;
        private List<RegisterInfoViewModels> registerInfoViewModelsList = new List<RegisterInfoViewModels>();

        #endregion

        #region Constructor

        public RegisterInfoController()
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
                new ProjectRegistrationRepository(),
                new ProjectBidSectionRepository(),
                new SessionManager()
            )
        {
        }

        public RegisterInfoController(
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
            IProjectRegistrationRepository projectRegistrationService,
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
            this.ProjectRegistrationService = projectRegistrationService;
            this.ProjectBidSectionService = projectBidSectionService;
            this.MySession = session;


            registerInfoViewModelsList.Add(new RegisterInfoViewModels
            {
                Id = "000001",
                CreateTime = DateTime.Today.AddDays(-10),
                BidSection = "2标段",
                CompanyName = "四川科华展示设备有限公司",
                ContactName = "倪志华",
                Email = "2460122779@qq.com",
                IsSubmitRegistrationFee = "已缴纳",
                Phone = "18280290539",
                ProjectId = "000001",
                ProjectName = "2016年绵竹市板桥镇海江村国家农业综合开发高标准农田建设项目 ",
                RegisterTime = DateTime.Now.AddDays(-50).AddMinutes(-564),
                RegistrationFee = 300,
                OperatorId = "123",
                OperatorName = "肖燕",
                Remark = "报名情况"
            });
            registerInfoViewModelsList.Add(new RegisterInfoViewModels
            {
                Id = "000002",
                CreateTime = DateTime.Today.AddDays(-10),
                BidSection = "四川艺术职业学院教学设施维修改造工程",
                CompanyName = "四川省长坤包装制品有限公司",
                ContactName = "旦海燕",
                Email = "410311781@qq.com",
                IsSubmitRegistrationFee = "已缴纳",
                Phone = "13881286994",
                ProjectId = "000001",
                ProjectName = "四川艺术职业学院教学设施维修改造工程",
                RegisterTime = DateTime.Now.AddDays(-50).AddMinutes(-564),
                RegistrationFee = 300,
                OperatorId = "123",
                OperatorName = "肖燕",
                Remark = "报名情况"
            });

            registerInfoViewModelsList.Add(new RegisterInfoViewModels
            {
                Id = "000003",
                CreateTime = DateTime.Today.AddDays(-10),
                BidSection = "1标段",
                CompanyName = "四川邛崃水电工程建设集团有限公司",
                ContactName = "旦海燕",
                Email = "410311781@qq.com",
                IsSubmitRegistrationFee = "已缴纳",
                Phone = "13881286994",
                ProjectId = "000003",
                ProjectName = "四川省烟草公司广元市公司2017年烟叶收购设备采购项目",
                RegisterTime = DateTime.Now.AddDays(-50).AddMinutes(-564),
                RegistrationFee = 300,
                OperatorId = "123",
                OperatorName = "李东",
                Remark = "报名情况"
            });
            registerInfoViewModelsList.Add(new RegisterInfoViewModels
            {
                Id = "000004",
                CreateTime = DateTime.Now.AddDays(-8),
                BidSection = "3标段",
                CompanyName = "四川圆瑞建设工程有限责任公司",
                ContactName = "周慧",
                Email = "410311781@qq.com",
                IsSubmitRegistrationFee = "未缴纳",
                Phone = "13881286994",
                ProjectId = "000004",
                ProjectName = "屏山县太平乡龙山村龙堂村前哨村和新安镇红庙村新民村新春村土地整理 ",
                RegisterTime = DateTime.Now.AddDays(-30).AddMinutes(-624),
                RegistrationFee = 300,
                OperatorId = "123",
                OperatorName = "陈小燕",
                Remark = "报名情况"
            });
        }

        #endregion

        #region Get Action

        /// <summary>
        /// 政府采购导航首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Module = "政府采购";
            ViewBag.Title = "简介";

            return View();
        }

        /// <summary>
        /// 政府采购导航首页待办事宜容器页面
        /// </summary>
        /// <returns></returns>

        /// <summary>
        /// Error action
        /// </summary>
        /// <returns></returns>
        public ActionResult Error()
        {
            ViewBag.Message = "Error.";

            return View();
        }

        public ActionResult Create(int applicationId)
        {
            var bidSections = ProjectBidSectionService.GetEntities(x => x.ApplicationId == applicationId);
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

            return View(new ProjectRegistrationViewModel() { ApplicationId = applicationId });
        }

        [HttpPost]
        public ActionResult Create(ProjectRegistrationViewModel model)
        {
            model.CreatedTime = DateTime.Now;
            model.OperatorName = User.Identity.Name;
            model.OperatorId = User.Identity.GetUserId();
            var bidSectionInfo = ProjectBidSectionService.GetEntityById(model.BidSectionId);
            model.BidSection = bidSectionInfo.SectionName;
            ProjectRegistrationService.AddEntity(model.ConvertTo<ProjectRegistration>());
            //上报成功的标志
            ViewBag.SendUpSuccess = true;
            return View(model);
        }

        public ActionResult List()
        {
            ViewBag.Module = "政府采购";
            ViewBag.Title = "报名情况";

            return View(this.registerInfoViewModelsList);
        }

        public ActionResult ProjectList(int applicationId)
        {
            ViewBag.Module = "政府采购";
            ViewBag.Title = "报名情况";

            var tempModels = ProjectRegistrationService.GetEntities(x => x.ApplicationId == applicationId);
            var models = tempModels.Select(x => x.ConvertTo<ProjectRegistrationViewModel>());
            ViewBag.Id = applicationId;
            return View(models);
        }

        public ActionResult Details(string id)
        {
            var model = this.registerInfoViewModelsList.First(x => x.Id == id);

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var isSuccessful = ProjectRegistrationService.DeleteEntityById(id);

                return Json(
                    new
                    {
                        isSuccessful = isSuccessful,
                    },
                    "text/html", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(
                 new
                 {
                     isSuccessful = false,
                     error = ex.Message
                 },
                 "text/html", JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 添加和修改报名情况
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            var model = ProjectRegistrationService.GetEntityById(id);
            var bidSections = ProjectBidSectionService.GetEntities(x => x.ApplicationId == model.ApplicationId);
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
            return View(model.ConvertTo<ProjectRegistrationViewModel>());
        }

        /// <summary>
        /// 添加和修改报名情况
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(ProjectRegistrationViewModel model)
        {
            model.OperatorName = User.Identity.Name;
            model.OperatorId = User.Identity.GetUserId();
            var bidSectionInfo = ProjectBidSectionService.GetEntityById(model.BidSectionId);
            model.BidSection = bidSectionInfo.SectionName;
            ProjectRegistrationService.UpdateEntity(model.ConvertTo<ProjectRegistration>());
            //上报成功的标志
            ViewBag.SendUpSuccess = true;
            return View(model);
        }

        /// <summary>
        /// 添加和修改保证金情况
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult EditBidBondInfo(int id)
        {
            var model = ProjectRegistrationService.GetEntityById(id);
            var bidSections = ProjectBidSectionService.GetEntities(x => x.ApplicationId == model.ApplicationId);
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
            return View(model.ConvertTo<ProjectRegistrationViewModel>());
        }

        /// <summary>
        /// 添加和修改保证金情况
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditBidBondInfo(ProjectRegistrationViewModel model)
        {
            model.OperatorName = User.Identity.Name;
            model.OperatorId = User.Identity.GetUserId();
            ProjectRegistrationService.UpdateEntity(model.ConvertTo<ProjectRegistration>());
            //上报成功的标志
            ViewBag.SendUpSuccess = true;
            return View(model);
        }

        /// <summary>
        /// 添加和修改中标通知书
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditBidWinnerNotice(int id)
        {
            var model = ProjectRegistrationService.GetEntityById(id);
            var bidSections = ProjectBidSectionService.GetEntities(x => x.ApplicationId == model.ApplicationId);
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
            return View(model.ConvertTo<ProjectRegistrationViewModel>());
        }

        /// <summary>
        /// 添加和修改中标通知书
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CreateBidWinnerNotice(int sectionId)
        {
            // 查出该标段下的所有公司，在页面上编辑中标 得分及名次信息
            var registCompanyList = ProjectRegistrationService.GetEntities(x => x.BidSectionId == sectionId);

            if (!registCompanyList.Any())
            {
                return View(@"<script type='text/javascript'>alert('该标段还没有公司报名！'); </script> ");
            }

            var selectItemList = new List<SelectListItem>()
            {
                //new SelectListItem(){Value="0",Text="全部",Selected=true}
            };

            if (registCompanyList.Any())
            {
                var selectList = new SelectList(registCompanyList, "ID", "CompanyName");
                selectItemList.AddRange(selectList);
            }

            ViewBag.bidCompanyList = selectItemList;

            return View(new ProjectRegistrationViewModel());

        }


        /// <summary>
        /// 添加和修改中标通知书
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateBidWinnerNotice(ProjectRegistrationViewModel model)
        {
            var projectInfo = ProjectRegistrationService.GetEntityById(model.ID);

            if (projectInfo == null)
            {
                return View(@"<script type='text/javascript'>alert('该公司报名信息不正确！'); </script> ");
            }

            projectInfo.IsShow = true;
            projectInfo.Score = model.Score;
            projectInfo.Rank = model.Rank;
            projectInfo.TenderOffer = model.TenderOffer;

            ProjectRegistrationService.UpdateEntity(projectInfo);

            //上报成功的标志
            ViewBag.SendUpSuccess = true;
            return View(model);
        }


        /// <summary>
        /// 添加和修改中标通知书
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetRegisterInfoList(int sectionId)
        {
            // 查出该标段下的所有公司，在页面上编辑中标 得分及名次信息
            var registCompanyList = ProjectRegistrationService.GetEntities(x => x.BidSectionId == sectionId);

            return View(registCompanyList.Select(x => x.ConvertTo<ProjectRegistrationViewModel>()));
        }

        /// <summary>
        /// 添加和修改中标通知书
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditBidWinnerNotice(ProjectRegistrationViewModel model)
        {
            model.OperatorName = User.Identity.Name;
            model.OperatorId = User.Identity.GetUserId();
            ProjectRegistrationService.UpdateEntity(model.ConvertTo<ProjectRegistration>());
            //上报成功的标志
            ViewBag.SendUpSuccess = true;
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public ActionResult ProjectBidWinnerNoticeList(int applicationId)
        {
            ViewBag.Module = "政府采购";
            ViewBag.Title = "报名情况";

            var tempModels = ProjectRegistrationService.GetEntities(x => x.ApplicationId == applicationId);
            var models = tempModels.Select(x => x.ConvertTo<ProjectRegistrationViewModel>());
            ViewBag.Id = applicationId;
            return View(models);
        }

        /// <summary>
        /// 添加和修改中标通知书
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetBidSectionList(int applicationId)
        {
            var application = ApplicationService.GetEntityById(applicationId);

            if (application == null)
            {
                return View(@"<script type='text/javascript'>alert('项目不存在！'); </script> ");
            }

            var bidSections = ProjectBidSectionService.GetEntities(x => x.ApplicationId == applicationId);

            if (bidSections.Any())
            {
                var result = bidSections.Select(x => x.ConvertTo<ProjectBidSectionViewModel>());

                ViewBag.ProjectName = application.WenHao;

                return View(result);
            }

            return View(new List<ProjectBidSectionViewModel> { /*new ProjectBidSectionViewModel() { ApplicationId = applicationId }*/ });
        }


        #endregion
    }
}
