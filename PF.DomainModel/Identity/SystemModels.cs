using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using PF.Common.Enums.HB.Public;
using PF.Common.Enums.GB;
using System.ComponentModel.DataAnnotations.Schema;

namespace PF.DomainModel.Identity
{
    //接入系统记录
    public class AccessSystem
    {
        [Key,Display(Name="授权号")] //需要给接入商的SysId
        public System.Guid   Id { get; set; }
        [MaxLength(20),Required,Display(Name="接入系统名称")]
        public string AppName { get; set; }
        [MaxLength(20), Required, Display(Name = "接入厂商名称")]
        public string CompanyName { get; set; }
        [Display(Name="接入日期")]
        public DateTime AccessDate { get; set; }
         [Display(Name = "备注")]
        public string Note { get; set; }
    }

   
}
