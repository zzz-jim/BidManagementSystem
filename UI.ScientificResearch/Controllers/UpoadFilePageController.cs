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

namespace UI.ScientificResearch.Controllers
{
    [CheckLogin]
    [Authorize(Roles = "普通用户")]
    public class UploadFilePageController : Controller
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
        private IProjectFileRepository FileService;

        private ISession MySession;

        #endregion

        #region Private Field

        private string SearchCriteriaProjectName;
        private string SearchCriteriaProjectStatus;
        private string SearchCriteriaIsLocked;
        private DateTime SearchCriteriaStartTime;
        private DateTime SearchCriteriaEndTime;
        private List<FileUploadViewModels> fileUploadViewModelsList = new List<FileUploadViewModels>();

        #endregion

        #region Constructor

        public UploadFilePageController()
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
                new ProjectFileRepository(),
                new ProjectBidSectionRepository(),
                new SessionManager()
            )
        {
        }

        public UploadFilePageController(
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
            IProjectFileRepository fileService,
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
            this.FileService = fileService;
            this.ProjectBidSectionService = projectBidSectionService;
            this.MySession = session;

            fileUploadViewModelsList.Add(new FileUploadViewModels
            {
                //ID = "000001",
                CreatedTime = DateTime.Today.AddDays(-10),
                FileAddress = "",
                FileName = "川国土资【2016】89号2016年度第一批省投资高标准基本农田建设项目开评标相关资料-新安、太平.pdf",

                FileSize = "5735 KB ",
                FileType = UploadFilePageType.开评标相关资料,
                Number = 1,
                OperatorId = "123",
                OperatorName = "肖燕",
                Remark = "开评标相关资料"
            });
            fileUploadViewModelsList.Add(new FileUploadViewModels
            {
                //ID = "000002",
                CreatedTime = DateTime.Now.AddDays(-8),
                FileAddress = "",
                FileName = "川国土资【2016】 开评标相关资料-新安、太平.pdf",

                FileSize = "6422 KB ",
                FileType = UploadFilePageType.开评标相关资料,
                Number = 2,
                OperatorId = "123",
                OperatorName = "肖燕",
                Remark = "开评标相关资料"
            });


            fileUploadViewModelsList.Add(new FileUploadViewModels
            {
                //ID = "000003",
                CreatedTime = DateTime.Today.AddDays(-10),
                FileAddress = "",
                FileName = "川国土资【2016】89号2016年度第一批省投资高标准基本农田建设项目立项的合同-新安、太平.pdf",

                FileSize = "5735 KB ",
                FileType = UploadFilePageType.合同资料,
                Number = 1,
                OperatorId = "123",
                OperatorName = "李东",
                Remark = "招标代理合同"
            });
            fileUploadViewModelsList.Add(new FileUploadViewModels
            {
                //ID = "000004",
                CreatedTime = DateTime.Now.AddDays(-8),
                FileAddress = "",
                FileName = "川国土资【2016】90号2016年度第一批省投资土地开发整理项目立项的合同-新安、太平.pdf",

                FileSize = "6422 KB ",
                FileType = UploadFilePageType.合同资料,
                Number = 2,
                OperatorId = "123",
                OperatorName = "陈小燕",
                Remark = "招标代理合同"
            });


            fileUploadViewModelsList.Add(new FileUploadViewModels
            {
                //ID = "000005",
                CreatedTime = DateTime.Today.AddDays(-10),
                FileAddress = "",
                FileName = "川国土资【2016】89号2016年度第一批省投资高标准基本农田建设项目立项的合同-新安、太平.pdf",

                FileSize = "5735 KB ",
                FileType = UploadFilePageType.招标文件,
                Number = 1,
                OperatorId = "123",
                OperatorName = "肖燕",
                Remark = "招标文件"
            });
            fileUploadViewModelsList.Add(new FileUploadViewModels
            {
                //ID = "000006",
                CreatedTime = DateTime.Now.AddDays(-8),
                FileAddress = "",
                FileName = " 屏山县太平乡龙山村龙堂村前哨村和新安镇红庙村新民村新春村土地整理项目12.5(1).doc",

                FileSize = "6422 KB ",
                FileType = UploadFilePageType.招标文件,
                Number = 2,
                OperatorId = "123",
                OperatorName = "肖燕",
                Remark = "招标文件"
            });

            fileUploadViewModelsList.Add(new FileUploadViewModels
            {
                //ID = "000007",
                CreatedTime = DateTime.Today.AddDays(-10),
                FileAddress = "",
                FileName = " 屏山县太平乡龙山村龙堂村前哨村和新安镇红庙村新民村新春村土地整理项目12.5(1).doc",

                FileSize = "5735 KB ",
                FileType = UploadFilePageType.项目概况相关资料,
                Number = 1,
                OperatorId = "123",
                OperatorName = "李小明",
                Remark = "项目概况相关资料"
            });
            fileUploadViewModelsList.Add(new FileUploadViewModels
            {
                //ID = "000008",
                CreatedTime = DateTime.Now.AddDays(-8),
                FileAddress = "",
                FileName = "四川省烟草公司达州市公司二○一五年度烟叶生产基础设施建设项目设计（第二次）.doc",

                FileSize = "6422 KB ",
                FileType = UploadFilePageType.项目概况相关资料,
                Number = 2,
                OperatorId = "123",
                OperatorName = "张梅",
                Remark = "项目概况相关资料"
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

        public ActionResult Create(UploadFilePageType type)
        {

            return View();
        }

        public ActionResult List(UploadFilePageType type)
        {
            ViewBag.UploadFilePageType = (int)type;


            return View(this.fileUploadViewModelsList.Where(x => x.FileType == type));
        }

        /// <summary>
        /// 招标代理合同
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult ContractList(int applicationId)
        {
            ViewBag.Module = "政府采购";
            ViewBag.Title = UploadFilePageType.合同资料.ToString();

            UploadFilePageType type = UploadFilePageType.合同资料;
            ViewBag.UploadFilePageType = (int)type;
            ViewBag.Id = applicationId;

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
            var tempResult = FileService.GetEntities(x => x.FileType == (int)type && x.ApplicationId == applicationId).Select(x => x.ConvertTo<FileUploadViewModels>()).OrderBy(x => x.SectionId).ThenBy(x => x.Remark).ThenByDescending(x => x.CreatedTime).ToList();

            foreach (var item in tempResult)
            {
                var section = bidSections.FirstOrDefault(x => x.ID == item.SectionId);
                if (section != null)
                {
                    item.SectionName = section.SectionName;
                }
            }
            return View("List", tempResult);
        }

        /// <summary>
        /// 招标代理合同
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult SectionContractList(int applicationId)
        {
            ViewBag.Module = "政府采购";
            ViewBag.Title = UploadFilePageType.合同资料.ToString();

            UploadFilePageType type = UploadFilePageType.合同资料;
            ViewBag.UploadFilePageType = (int)type;
            ViewBag.Id = applicationId;

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
            var result = FileService.GetEntities(x => x.FileType == (int)type && x.ApplicationId == applicationId).Select(x => x.ConvertTo<FileUploadViewModels>()).OrderBy(x => x.SectionId).ThenBy(x => x.Remark).ThenByDescending(x=>x.CreatedTime).ToList();
            int number = 1;
            foreach (var item in result)
            {
                item.Number = number;
                number++;
            }

            return Json(result, JsonRequestBehavior.AllowGet);    //return View("List", );//.Where(x => x.FileType == type));
        }

        /// <summary>
        /// 开评标相关资料
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult OpenBidsDocumentList(int applicationId)
        {
            ViewBag.Module = "政府采购";
            ViewBag.Title = "开评标相关资料";

            UploadFilePageType type = UploadFilePageType.开评标相关资料;
            ViewBag.UploadFilePageType = (int)type;
            ViewBag.Id = applicationId;
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
            var tempResult = FileService.GetEntities(x => x.FileType == (int)type && x.ApplicationId == applicationId).Select(x => x.ConvertTo<FileUploadViewModels>()).OrderBy(x => x.SectionId).ThenBy(x => x.Remark).ThenByDescending(x => x.CreatedTime).ToList();

            foreach (var item in tempResult)
            {
                var section = bidSections.FirstOrDefault(x => x.ID == item.SectionId);
                if (section != null)
                {
                    item.SectionName = section.SectionName;
                }
            }
            return View("List", tempResult);
        }

        /// <summary>
        /// 开评标相关资料
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult SectionOpenBidsDocumentList(int applicationId)
        {
            ViewBag.Module = "政府采购";
            ViewBag.Title = "开评标相关资料";

            UploadFilePageType type = UploadFilePageType.开评标相关资料;
            ViewBag.UploadFilePageType = (int)type;
            ViewBag.Id = applicationId;
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

            var result = FileService.GetEntities(x => x.FileType == (int)type && x.ApplicationId == applicationId).Select(x => x.ConvertTo<FileUploadViewModels>()).ToList();
            int number = 1;
            foreach (var item in result)
            {
                item.Number = number;
                number++;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 招标文件
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult BiddingDocumentList(int applicationId)
        {
            ViewBag.Module = "政府采购";
            ViewBag.Title = "招标文件";

            UploadFilePageType type = UploadFilePageType.招标文件;
            ViewBag.UploadFilePageType = (int)type;
            ViewBag.Id = applicationId;
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
            var tempResult = FileService.GetEntities(x => x.FileType == (int)type && x.ApplicationId == applicationId).Select(x => x.ConvertTo<FileUploadViewModels>()).ToList();

            foreach (var item in tempResult)
            {
                var section = bidSections.FirstOrDefault(x => x.ID == item.SectionId);
                if (section != null)
                {
                    item.SectionName = section.SectionName;
                }
            }
            return View(tempResult);
        }

        /// <summary>
        /// 招标文件
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult SectionBiddingDocumentList(int applicationId)
        {
            ViewBag.Module = "政府采购";
            ViewBag.Title = "招标文件";

            UploadFilePageType type = UploadFilePageType.招标文件;
            ViewBag.UploadFilePageType = (int)type;
            ViewBag.Id = applicationId;
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
            var result = FileService.GetEntities(x => x.FileType == (int)type && x.ApplicationId == applicationId).Select(x => x.ConvertTo<FileUploadViewModels>()).ToList();
            int number = 1;
            foreach (var item in result)
            {
                item.Number = number;
                number++;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(int id)
        {
            var model = this.fileUploadViewModelsList.First(x => x.ID == id);

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var isSuccessful = this.FileService.DeleteEntityById(id);

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
        /// 正在审批中的申请书列表，还未项目确立的申请书
        /// </summary>
        /// <returns></returns>
        public ActionResult AllContractList()
        {
            ViewBag.Module = "政府采购";
            ViewBag.Title = "合同资料";
            return View();
        }

        public ActionResult AllOpenBidsDocumentList()
        {
            ViewBag.Module = "政府采购";
            ViewBag.Title = "开评标相关资料";
            return View();
        }

        public ActionResult AllBiddingDocumentList()
        {
            ViewBag.Module = "政府采购";
            ViewBag.Title = "招标文件";
            return View();
        }

        /// <summary>
        /// 中标通知书文件
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult BidWinnerNoticeDocumentList(int applicationId)
        {
            ViewBag.Module = "政府采购";
            ViewBag.Title = UploadFilePageType.中标通知书资料.ToString();

            UploadFilePageType type = UploadFilePageType.中标通知书资料;
            ViewBag.UploadFilePageType = (int)type;
            ViewBag.Id = applicationId;

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
            var tempResult = FileService.GetEntities(x => x.FileType == (int)type && x.ApplicationId == applicationId).Select(x => x.ConvertTo<FileUploadViewModels>()).OrderBy(x => x.SectionId).ThenBy(x => x.Remark).ThenByDescending(x => x.CreatedTime).ToList();

            foreach (var item in tempResult)
            {
                var section = bidSections.FirstOrDefault(x => x.ID == item.SectionId);
                if (section != null)
                {
                    item.SectionName = section.SectionName;
                }
            }
            return View("List", tempResult);
        }

        #endregion
    }
}
