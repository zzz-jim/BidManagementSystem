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
    
    public partial class ERPTaskFP
    {
        public int ID { get; set; }
        public string TaskTitle { get; set; }
        public string TaskContent { get; set; }
        public string FromUser { get; set; }
        public string ToUserList { get; set; }
        public string XinXiGouTong { get; set; }
        public Nullable<decimal> JinDu { get; set; }
        public string WanCheng { get; set; }
        public string NowState { get; set; }
        public Nullable<System.DateTime> TimeStr { get; set; }
        public Nullable<System.DateTime> KSSJ { get; set; }
        public Nullable<System.DateTime> JSSJ { get; set; }
        public string SFFK { get; set; }
        public Nullable<System.DateTime> FKSJ { get; set; }
    }
}
