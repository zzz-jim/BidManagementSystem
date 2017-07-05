using System;
using System.Configuration;
using System.IO;
using System.Web.Mvc;

using ScientificResearch.BusinessLogicImplement;
using ScientificResearch.IBusinessLogic;
using ScientificResearch.Utility.Constants;
using ScientificResearch.Utility;
using UI.ScientificResearch.Extensions;

namespace UI.ScientificResearch.Controllers
{
    [CheckLogin]
    [Authorize(Roles = "普通用户")]
    public class FileServiceController : Controller
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

        private IFundsManageProgramStatisticsService FundsManageProgramStatisticsService;
        //private ISession session;

        #endregion

        #region Constructor

        public FileServiceController()
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
                new FundsManageProgramStatisticsImplement()
            )//, new SessionManager())
        {
        }

        public FileServiceController(
            IERPNFormService eRPNFormService,
            IERPBuMenService eRPBuMenService,
            IERPRiZhiService eRPRiZhiService,
            IApplicationService applicationService,
            IERPNWorkFlowService eRPNWorkFlowService,
            IERPNWorkFlowNodeService eRPNWorkFlowNodeService,
            IFundsRecordService eFundsRecordService,
            IProjectRecordService eProjectRecordService,
            IStatisticService statisticService,
            IFundsManageProgramStatisticsService fundsManageProgramStatisticsService
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
            // this.session = session;
            this.StatisticService = statisticService;
            this.FundsManageProgramStatisticsService = fundsManageProgramStatisticsService;
        }

        #endregion

        /// <summary>
        /// 上传
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            string exceptionMsg = string.Empty;
            var formContent = Request.Form;
            var uploadedFiles = Request.Files;
            string fileName = string.Empty;
            bool isSuccessful = false;
            string userName = Constant.USER_NAME;
            string Date = DateTime.Now.ToString();
            string newFileName = string.Empty;

            if (uploadedFiles.Count > 0)
            {
                foreach (string file in uploadedFiles)
                {
                    try
                    {
                        var currentFile = Request.Files[file];
                        fileName = Path.GetFileName(currentFile.FileName);
                        string serverPath = Server.MapPath(ConfigurationManager.AppSettings[Constant.AttachmentPathKey]);
                        string newServerPath = serverPath + "\\" + userName;
                        if (!Directory.Exists(newServerPath))
                        {
                            Directory.CreateDirectory(newServerPath);
                        }
                        //获取后缀名(包括“.”例 “.txt”)
                        string extension = System.IO.Path.GetExtension(fileName);
                        //获取前缀
                        string prefix = System.IO.Path.GetFileNameWithoutExtension(fileName);
                        //文件名加入时间
                        newFileName = DateTime.Now.ToString() + "-" + prefix + extension;
                        //去掉文件名中的非法字符
                        newFileName = ReplaceBadCharOfFileName.ReplaceBadCharOfFileNames(newFileName);
                        string newFilePath = newServerPath + "\\" + newFileName;
                        string filePath = Path.Combine(newFilePath);
                        currentFile.SaveAs(filePath);

                        if (System.IO.File.Exists(filePath))
                        {
                            isSuccessful = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        isSuccessful = false;
                        exceptionMsg += ex.Message;
                    }
                }
            }

            //注意要写好后面的第二第三个参数
            return Json(
                new
                {
                    isSuccessful = isSuccessful,
                    //id = formContent.Get("Id"),
                    //文件名
                    name = newFileName,
                    error = exceptionMsg,
                    //用户名
                    Name = userName
                },
                "text/html", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public ActionResult Download(string fileName)
        {
            string userName = Constant.USER_NAME;
            try
            {
                string filePath = Server.MapPath(ConfigurationManager.AppSettings[Constant.AttachmentPathKey] + "\\" + userName + "\\" + fileName);

                FileStream fs = new FileStream(filePath, FileMode.Open);
                byte[] bytes = new byte[(int)fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                fs.Close();

                Response.Charset = "UTF-8";
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                Response.ContentType = "application/octet-stream";

                Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(fileName));
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();
            }
            catch (Exception e)
            {
                return View();
            }
            return View();
        }
        public ActionResult Download2(string Path)
        {
            string fileName = string.Empty;
            string userName = string.Empty;
            //获取路径中的用户名
            userName = Path.Substring(0, Path.LastIndexOf("\\"));
            //获取路径中的文件名
            fileName = Path.Substring(Path.LastIndexOf("\\") + 1);
            try
            {
                string filePath = Server.MapPath(ConfigurationManager.AppSettings[Constant.AttachmentPathKey] + "\\" + userName + "\\" + fileName);

                FileStream fs = new FileStream(filePath, FileMode.Open);
                byte[] bytes = new byte[(int)fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                fs.Close();

                Response.Charset = "UTF-8";
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                Response.ContentType = "application/octet-stream";

                Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(fileName));
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();
            }
            catch (Exception e)
            {
                return View();
            }
            return View();
        }
    }
}