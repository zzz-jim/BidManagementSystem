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
using ScientificResearch.IDataAccess;
using ScientificResearch.DataAccessImplement;
using Microsoft.AspNet.Identity;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace UI.ScientificResearch.Controllers
{
    [CheckLogin]
    [Authorize(Roles = "普通用户")]
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
        private IProjectFileRepository FileService;

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
                new ProjectFileRepository(),
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
            IProjectFileRepository fileService,
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
            this.FileService = fileService;
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
            if (!User.IsInRole(UserRoles.超级管理员.ToString()))
            {
                return Content(@"<script type='text/javascript'>alert('无权限！'); </script> ");
            }

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
            if (!User.IsInRole(UserRoles.超级管理员.ToString()))
            {
                return Content(@"<script type='text/javascript'>alert('无权限！'); </script> ");
            }

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
            if (User.IsInRole(UserRoles.超级管理员.ToString()))
            {
                ViewBag.Module = "政府采购";
                ViewBag.Title = "报名情况";

                var tempModels = ProjectRegistrationService.GetEntities(x => x.ApplicationId == applicationId);
                var models = tempModels.Select(x => x.ConvertTo<ProjectRegistrationViewModel>());
                ViewBag.Id = applicationId;
                return View(models);
            }

            return Content(@"<script type='text/javascript'>alert('无权限！'); </script> ");
            //return Content(@"<script type='text/javascript'>alert('无权限！'); </script> ");
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
            if (!User.IsInRole(UserRoles.超级管理员.ToString()))
            {
                return Content(@"<script type='text/javascript'>alert('无权限！'); </script> ");
            }
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
            if (!User.IsInRole(UserRoles.超级管理员.ToString()))
            {
                return Content(@"<script type='text/javascript'>alert('无权限！'); </script> ");
            }
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
            if (!User.IsInRole(UserRoles.超级管理员.ToString()))
            {
                return Content(@"<script type='text/javascript'>alert('无权限！'); </script> ");
            }
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
            if (!User.IsInRole(UserRoles.超级管理员.ToString()))
            {
                return Content(@"<script type='text/javascript'>alert('无权限！'); </script> ");
            }
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
            var result = new BidWinnerNoticeViewModel();

            // 查出该标段下的所有公司，在页面上编辑中标 得分及名次信息
            var registCompanyList = ProjectRegistrationService.GetEntities(x => x.BidSectionId == sectionId);

            if (!registCompanyList.Any())
            {
                return Content(@"<script type='text/javascript'>alert('该标段还没有公司报名！'); </script> ");
            }

            var application = ApplicationService.GetEntityById(registCompanyList.First().ApplicationId);
            var section = ProjectBidSectionService.GetEntityById(sectionId);

            result.ApplicationId = application.ID;
            result.ProjectName = application.WenHao;
            result.ProjectNumber = application.BeiYong1;
            result.BidSectionId = sectionId;
            result.SectionName = section.SectionName;
            result.SectionNumber = section.SectionNumber;

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
            ViewBag.ApplicationId = application.ID;
            // 查出该标段下的所有公司的 前三甲，在页面上编辑中标 得分及名次信息
            var rankedCompanyList = registCompanyList.Where(x => x.IsShow);
            if (rankedCompanyList.Any())
            {
                foreach (var item in rankedCompanyList)
                {
                    if (item.Rank == 1)
                    {
                        result.CompanyId1 = item.ID;
                        result.CompanyName1 = item.CompanyName;
                        result.Score1 = item.Score.Value;
                        result.TenderOffer1 = item.TenderOffer.Value;
                        result.Address1 = item.Address;
                    }
                    else
                    if (item.Rank == 2)
                    {
                        result.CompanyId2 = item.ID;
                        result.CompanyName2 = item.CompanyName;
                        result.Score2 = item.Score.Value;
                        result.TenderOffer2 = item.TenderOffer.Value;
                        result.Address2 = item.Address;
                    }
                    else
                    if (item.Rank == 3)
                    {
                        result.CompanyId3 = item.ID;
                        result.CompanyName3 = item.CompanyName;
                        result.Score3 = item.Score.Value;
                        result.TenderOffer3 = item.TenderOffer.Value;
                        result.Address3 = item.Address;
                    }
                    else { }
                }
            }

            return View(result);

        }


        /// <summary>
        /// 添加和修改中标通知书
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateBidWinnerNotice(BidWinnerNoticeViewModel model)
        {
            var companyList = ProjectRegistrationService.GetEntities(x => x.ID == model.CompanyId1 || x.ID == model.CompanyId2 || x.ID == model.CompanyId3 || x.IsShow).ToList();

            if (!companyList.Any())
            {
                return Content(@"<script type='text/javascript'>alert('该公司报名信息不正确！'); </script> ");
            }

            foreach (var item in companyList)
            {
                if (item.ID == model.CompanyId1)
                {
                    item.IsShow = true;
                    item.Score = model.Score1;
                    item.TenderOffer = model.TenderOffer1;
                    item.Rank = 1;
                }
                else
                if (item.ID == model.CompanyId2)
                {
                    item.IsShow = true;
                    item.Score = model.Score2;
                    item.TenderOffer = model.TenderOffer2;
                    item.Rank = 2;
                }
                else
                if (item.ID == model.CompanyId3)
                {
                    item.IsShow = true;
                    item.Score = model.Score3;
                    item.TenderOffer = model.TenderOffer3;
                    item.Rank = 3;
                }
                else

                    item.IsShow = false;

                ProjectRegistrationService.UpdateEntity(item);
            }

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
                return Content(@"<script type='text/javascript'>alert('项目不存在！'); </script> ");
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

        /// <summary>
        /// 添加和修改中标通知书
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        public ActionResult GetBidSectionListByApplicationId2(int applicationId)
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

                return Json(result, JsonRequestBehavior.AllowGet);
            }

            return Json(new List<ProjectBidSectionViewModel> { }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查出该标段下的所有公司
        /// </summary>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        public ActionResult GetRegisterInfoListBySectionId(int sectionId)
        {
            // 查出该标段下的所有公司的 前三甲，在页面上编辑中标 得分及名次信息
            var registCompanyList = ProjectRegistrationService.GetEntities(x => x.BidSectionId == sectionId && x.IsShow);
            if (registCompanyList.Any())
            {
                var result = registCompanyList.Select(x => x.ConvertTo<ProjectRegistrationViewModel>()).OrderBy(x => x.Rank).ToList();

                //序号
                int number = 1;
                foreach (var item in result)
                {
                    item.Number = number;
                    item.RankDescription = "第" + item.Rank + "名";
                    number++;
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            return Json(new List<ProjectRegistrationViewModel> { }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 中标通知书列表
        /// </summary>
        /// <returns></returns>
        public ActionResult BidWinnerNoticeList(int applicationId)
        {
            ViewBag.ApplicationId = applicationId;
            ViewBag.Id = applicationId;
            return View();
        }

        public ActionResult SectionProjectRegisterInfoList(int applicationId)
        {
            ViewBag.Module = "政府采购";
            ViewBag.Title = "报名情况";

            var tempModels = ProjectRegistrationService.GetEntities(x => x.ApplicationId == applicationId);
            var models = tempModels.Select(x => x.ConvertTo<ProjectRegistrationViewModel>()).ToList();
            ViewBag.Id = applicationId;
            int number = 1;
            foreach (var item in models)
            {
                item.Number = number;
                number++;
            }

            return Json(models, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AllProjectRegisterInfoList()
        {
            ViewBag.Module = "政府采购";
            ViewBag.Title = "报名情况";

            return View();
        }

        public ActionResult AllProjectBidBondInfoList()
        {
            ViewBag.Module = "政府采购";
            ViewBag.Title = "保证金情况";

            return View();
        }

        public ActionResult ALlProjectBidWinnerNoticeList()
        {
            ViewBag.Module = "政府采购";
            ViewBag.Title = "中标通知书";

            return View();
        }

        /// <summary>
        /// 发送招标文件给对应标段的公司
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public ActionResult SendBidDocumentToCompany(int applicationId)
        {
            try
            {
                bool isSuccessful = false;
                var application = ApplicationService.GetEntityById(applicationId);
                var valueArray = application.FormValues.Split(Constant.SharpChar);
                var agent = application.FormValues.Split(Constant.SharpChar)[19];// 代理公司 鑫森 等等公司
                if (string.IsNullOrEmpty(agent) || string.IsNullOrWhiteSpace(agent))
                {
                    return Json(
                     new
                     {
                         isSuccessful = isSuccessful,
                     }, JsonRequestBehavior.AllowGet);
                }

                var emailConfig = ConfigurationManager.AppSettings[agent];
                var emailConfigArray = emailConfig.Split(Constant.SpaceChar);
                string fromEmailAddress = string.Empty; // 发件箱 邮箱地址
                string toEmailAddress = string.Empty;
                string fromEmailAddressPwd = string.Empty; // 发件箱 邮箱密码
                string fromEmailServer = string.Empty;// 邮件服务器地址

                if (emailConfigArray.Count() == 2)
                {
                    fromEmailAddress = emailConfigArray.First();
                    fromEmailAddressPwd = emailConfigArray.Last();
                }

                fromEmailServer = fromEmailAddress.Split(Constant.AtChar).LastOrDefault();
                if (!MvcApplication.EmailServerConfig.ContainsKey(fromEmailServer))
                {
                    throw new Exception($"邮件服务器{fromEmailServer}的服务器地址未配置，请管理员配置");
                }

                // 查询收件箱列表  获取项目所有标段的报名公司 
                var companyList = ProjectRegistrationService.GetEntities(x => x.ApplicationId == applicationId).ToList();//.GroupBy(x => x.BidSectionId);

                // 查询邮件附件内容
                var fileList = FileService.GetEntities(x => x.ApplicationId == applicationId && x.Remark == BiddingDocumentType.终稿文件.ToString() && x.FileType == (int)UploadFilePageType.招标文件).ToList();
                var mailClassList = new List<MailClass>();
                foreach (var item in companyList)
                {

                    var fileAddress = string.Empty;
                    var fileModel = fileList.FirstOrDefault(x => x.SectionId == item.BidSectionId);
                    if (fileModel == null)
                    {
                        throw new Exception($"标段{item.BidSection}(id{item.BidSectionId})的终稿文件还未上传");
                    }
                    var mailItem = new MailClass()
                    {
                        Attachment = fileModel.FileAddress,
                        MailCharset = "utf-8",
                        MailTo = item.Email,
                        MailFrom = fromEmailAddress,
                        MailFromDisplayName = agent,
                        MailUserPassword = fromEmailAddressPwd,
                        MailUserName = fromEmailAddress,
                        MailSubject = "招标文件",
                        // EmailContent 此字段暂时没用
                        MailServer = MvcApplication.EmailServerConfig[fromEmailServer],
                    };

                    mailClassList.Add(mailItem);
                }

                foreach (var item in mailClassList)
                {
                    MailTest.SendMailMethod(item);// 发送邮件
                }

                //return Json(new { ReturnState, applicationId }, JsonRequestBehavior.AllowGet);
                isSuccessful = true;

                return Json(
                    new
                    {
                        isSuccessful = isSuccessful,
                    }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(
                 new
                 {
                     isSuccessful = false,
                     error = ex.Message
                 }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 发送招标文件给对应标段的公司
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public ActionResult SendBidDocumentToCompanyById(int companyId)
        {
            try
            {
                bool isSuccessful = false;

                // 查询收件箱列表  获取项目所有标段的报名公司 
                var company = ProjectRegistrationService.GetEntityById(companyId);
                var application = ApplicationService.GetEntityById(company.ApplicationId);
                var valueArray = application.FormValues.Split(Constant.SharpChar);
                var agent = application.FormValues.Split(Constant.SharpChar)[19];// 代理公司 鑫森 等等公司
                if (string.IsNullOrEmpty(agent) || string.IsNullOrWhiteSpace(agent))
                {
                    return Json(
                     new
                     {
                         isSuccessful = isSuccessful,
                     }, JsonRequestBehavior.AllowGet);
                }

                var emailConfig = ConfigurationManager.AppSettings[agent];
                var emailConfigArray = emailConfig.Split(Constant.SpaceChar);
                string fromEmailAddress = string.Empty; // 发件箱 邮箱地址
                string toEmailAddress = string.Empty;
                string fromEmailAddressPwd = string.Empty; // 发件箱 邮箱密码
                string fromEmailServer = string.Empty;// 邮件服务器地址

                if (emailConfigArray.Count() == 2)
                {
                    fromEmailAddress = emailConfigArray.First();
                    fromEmailAddressPwd = emailConfigArray.Last();
                }

                fromEmailServer = fromEmailAddress.Split(Constant.AtChar).LastOrDefault();
                if (!MvcApplication.EmailServerConfig.ContainsKey(fromEmailServer))
                {
                    throw new Exception($"邮件服务器{fromEmailServer}的服务器地址未配置，请管理员配置");
                }


                // 查询邮件附件内容
                var fileList = FileService.GetEntities(x => x.ApplicationId == company.ApplicationId && x.SectionId == company.BidSectionId && x.Remark == BiddingDocumentType.终稿文件.ToString() && x.FileType == (int)UploadFilePageType.招标文件).ToList();
                var mailClassList = new List<MailClass>();

                var fileAddress = string.Empty;
                var fileModel = fileList.FirstOrDefault(x => x.SectionId == company.BidSectionId);
                if (fileModel == null)
                {
                    throw new Exception($"标段{company.BidSection}(id{company.BidSectionId})的终稿文件还未上传");
                }
                var mailItem = new MailClass()
                {
                    Attachment = fileModel.FileAddress,
                    MailCharset = "utf-8",
                    MailTo = company.Email,
                    MailFrom = fromEmailAddress,
                    MailFromDisplayName = agent,
                    MailUserPassword = fromEmailAddressPwd,
                    MailUserName = fromEmailAddress,
                    MailSubject = "招标文件",
                    // EmailContent 此字段暂时没用
                    MailServer = MvcApplication.EmailServerConfig[fromEmailServer],
                };

                MailTest.SendMailMethod(mailItem);// 发送邮件

                // 更新发送状态
                company.IsSentEmail = true;

                ProjectRegistrationService.UpdateEntity(company);

                isSuccessful = true;

                return Json(
                    new
                    {
                        isSuccessful = isSuccessful,
                    }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(
                 new
                 {
                     isSuccessful = false,
                     error = ex.Message
                 }, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public void ExportExcel(int applicationId)
        {

            //var tempModels = ProjectRegistrationService.GetEntities(x => x.ApplicationId == applicationId);
            var dataTable = ProjectRegistrationService.GetListByApplicationIdAsync(applicationId);

            CreateExcel(dataTable, "application/ms-excel", "Excel" + DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ".xls");//调用函数  
            //return Json(
            //         new
            //         {
            //             isSuccessful = true,
            //         }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>  
        /// DataTable导出到Excel  
        /// </summary>  
        /// <param name="dt">DataTable类型的数据源</param>  
        /// <param name="FileType">文件类型</param>  
        ///// <param name="FileName">文件名</param>  
        //public void CreateExcel2(IList<ProjectRegistration> dt, string FileType, string FileName)
        //{
        //    Response.Clear();
        //    Response.Charset = "UTF-8";
        //    Response.Buffer = true;
        //    Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        //    Response.AppendHeader("Content-Disposition", "attachment;filename=\"" + System.Web.HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8) + ".xls\"");
        //    Response.ContentType = FileType;
        //    string colHeaders = string.Empty;
        //    string ls_item = string.Empty;
        //    DataRow[] myRow = dt.Select();
        //    int i = 0;
        //    int cl = dt.Columns.Count;
        //    for (int j = 0; j < dt.Columns.Count; j++)
        //    {
        //        ls_item += dt.Columns[j].ColumnName + "\t"; //栏位：自动跳到下一单元格  
        //    }
        //    ls_item = ls_item.Substring(0, ls_item.Length - 1) + "\n";
        //    foreach (var row in dt)
        //    {
        //        for (i = 0; i < cl; i++)
        //        {
        //            if (i == (cl - 1))
        //            {
        //                ls_item += row[i].ToString() + "\n";
        //            }
        //            else
        //            {
        //                ls_item += row[i].ToString() + "\t";
        //            }
        //        }
        //        Response.Output.Write(ls_item);
        //        ls_item = string.Empty;
        //    }
        //    Response.Output.Flush();
        //    Response.End();
        //}
        /// <summary>  
        /// DataTable导出到Excel  
        /// </summary>  
        /// <param name="dt">DataTable类型的数据源</param>  
        /// <param name="FileType">文件类型</param>  
        /// <param name="FileName">文件名</param>  
        public void CreateExcel(DataTable dt, string FileType, string FileName)
        {
            Response.Clear();
            Response.Charset = "UTF-8";
            Response.Buffer = true;
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            Response.AppendHeader("Content-Disposition", "attachment;filename=\"" + System.Web.HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8) + ".xls\"");
            Response.ContentType = FileType;
            string colHeaders = string.Empty;
            string ls_item = string.Empty;
            DataRow[] myRow = dt.Select();
            int i = 0;
            int cl = dt.Columns.Count;
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                ls_item += dt.Columns[j].ColumnName + "\t"; //栏位：自动跳到下一单元格  
            }
            ls_item = ls_item.Substring(0, ls_item.Length - 1) + "\n";
            foreach (DataRow row in myRow)
            {
                for (i = 0; i < cl; i++)
                {
                    if (i == (cl - 1))
                    {
                        ls_item += row[i].ToString() + "\n";
                    }
                    else
                    {
                        ls_item += row[i].ToString() + "\t";
                    }
                }
                Response.Output.Write(ls_item);
                ls_item = string.Empty;
            }
            Response.Output.Flush();
            Response.End();
        }
        #endregion
    }
}
