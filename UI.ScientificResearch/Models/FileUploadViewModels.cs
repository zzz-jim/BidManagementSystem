using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using ScientificResearch.Utility.Enums;

namespace UI.ScientificResearch.Models
{
    /// <summary>
    /// 文件资料上传
    /// </summary>
    public class FileUploadViewModels
    {
        [Required]
        [Display(Name = "Id")]
        public int ID { get; set; }

        [Required]
        [Display(Name = "序号")]
        public int Number { get; set; }

        [Required]
        [Display(Name = "文件种类")]
        public UploadFilePageType FileType { get; set; }

        [Required]
        [Display(Name = "备注说明")]
        public string Remark { get; set; }

        [Required]
        [Display(Name = "上传人")]
        public string OperatorName { get; set; }

        [Required]
        [Display(Name = "上传人ID")]
        public string OperatorId { get; set; }

        [Display(Name = "上传时间")]
        public DateTime CreatedTime { get; set; }

        [Display(Name = "文件名称")]
        public string FileName { get; set; }

        [Display(Name = "文件地址")]
        public string FileAddress { get; set; }

        [Display(Name = "文件大小")]
        public string FileSize { get; set; }
    }
}