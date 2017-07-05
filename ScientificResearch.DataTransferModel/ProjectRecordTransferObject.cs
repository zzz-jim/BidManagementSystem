using System;

namespace ScientificResearch.DataTransferModel
{
    public class ProjectRecordTransferObject
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
    }
}
