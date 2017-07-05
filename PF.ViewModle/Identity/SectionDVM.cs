using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace PF.ViewModle.Identity
{
       [DataContract]
    public class SectionDVM
    {
     [DataMember(IsRequired = true, Name = "科室代码")]
        [Display(Name = "科室代码")]
        public string Id { get; set; }
            [DataMember]
        [Display(Name = "科室名称")]
        public string Name { get; set; }
            [DataMember]
        [Display(Name = "拼音码")]
        public string PinYin { get; set; }
            [DataMember]
        [Display(Name = "部门代码")]
        public string DepartmentId { get; set; }
            [DataMember]
        [Display(Name = "医保代码")]
        [MaxLength(256)]
        public string InsuranceCode { get; set; }
    

    }
}
