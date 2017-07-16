using System;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using System.Linq;

using ScientificResearch.BusinessLogicImplement;
using ScientificResearch.IBusinessLogic;
using ScientificResearch.Utility.Constants;
using ScientificResearch.Utility;
using UI.ScientificResearch.Extensions;
using ScientificResearch.Utility.Enums;
using ScientificResearch.IDataAccess;
using ScientificResearch.DataAccessImplement;
using ScientificResearch.DomainModel;
using Microsoft.AspNet.Identity;

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
        private IProjectFileRepository FileService;

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
                new FundsManageProgramStatisticsImplement(),
                new ProjectFileRepository()
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
            IFundsManageProgramStatisticsService fundsManageProgramStatisticsService,
         IProjectFileRepository fileService
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
            this.FileService = fileService;
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
            int applicationId;// 项目的Id
            UploadFilePageType fileType;// 文件类型
            string remark = string.Empty;

            try
            {
                applicationId = Convert.ToInt32(formContent["application_id"]);
                fileType = (UploadFilePageType)Convert.ToInt32(formContent["file_type"]);
                remark = formContent["remark"];
            }
            catch (Exception ex)
            {
                return Json(
                    new
                    {
                        isSuccessful = false,
                        error = "项目的Id或文件类型不正确",
                    },
                    "text/html", JsonRequestBehavior.AllowGet);
            }

            var uploadedFiles = Request.Files;
            string fileName = string.Empty;
            bool isSuccessful = false;
            string userName = Constant.USER_NAME;
            string Date = DateTime.Now.ToString();
            string newFileName = string.Empty;
            string filePath = string.Empty;
            int fileSize = 0;
            // step1，先保存到服务器中
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
                        filePath = Path.Combine(newFilePath);
                        currentFile.SaveAs(filePath);
                        fileSize = currentFile.ContentLength;
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

            if (isSuccessful)
            {
                try
                {
                    // step2，记录相关信息到数据库中的文件上传记录表中 
                    FileService.AddEntity(new ProjectFile
                    {
                        ApplicationId = applicationId,
                        CreatedTime = DateTime.Now,
                        FileName = fileName,
                        FileType = (int)fileType,
                        FileAddress = filePath,
                        FileSize = (fileSize / 1000).ToString() + " KB",
                        OperatorName = User.Identity.GetUserName(),
                        OperatorId = User.Identity.GetUserId(),
                        Remark=remark
                    });

                }
                catch (Exception ex)
                {
                    // 记录日志

                    return Json(
                        new
                        {
                            isSuccessful = false,
                            error = "文件上传失败，请重新上传",
                        },
                        "text/html", JsonRequestBehavior.AllowGet);
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