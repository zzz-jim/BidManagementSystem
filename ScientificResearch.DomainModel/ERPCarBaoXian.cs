//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ScientificResearch.DomainModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class ERPCarBaoXian
    {
        public int ID { get; set; }
        public string CarName { get; set; }
        public string FeiYongName { get; set; }
        public string ProjectName { get; set; }
        public string BaoXianPrice { get; set; }
        public string BaoXianDate { get; set; }
        public string TiXingDate { get; set; }
        public string UserName { get; set; }
        public Nullable<System.DateTime> TimeStr { get; set; }
        public string BackInfo { get; set; }
    }
}
