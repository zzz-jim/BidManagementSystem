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
    
    public partial class ProjectRecord
    {
        public int ID { get; set; }
        public int ApplicationId { get; set; }
        public int WorkflowId { get; set; }
        public double SuperiorFunds { get; set; }
        public double HospitalFunds { get; set; }
        public System.DateTime FundsTime { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public double Total { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedTime { get; set; }
        public string ProjectLevel { get; set; }
        public Nullable<bool> IsRejected { get; set; }
        public Nullable<bool> IsLocked { get; set; }
        public Nullable<bool> IsTemporary { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string FuJianList { get; set; }
        public Nullable<System.DateTime> SuperiorFundTime { get; set; }
    
        public virtual ERPNWorkFlow ERPNWorkFlow { get; set; }
        public virtual ERPNWorkToDo ERPNWorkToDo { get; set; }
    }
}