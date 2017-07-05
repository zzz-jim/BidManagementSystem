using System;
using System.Collections.Generic;

namespace ScientificResearch.DataTransferModel
{
    public class TravelFundsRecordTransferObject
    {
        public TravelFundsRecordTransferObject()
        {
            TravelFundsList = new List<TravelFundsDetailTransferObject>();
        }

        public int FundsRecordID { get; set; }
        public int ApplicationId { get; set; }
        public int WorkflowId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string ProjectName { get; set; }
        public int CountOfBill { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        public double TotalPrice { get; set; }
        public Nullable<bool> IsIncome { get; set; }
        public bool IsPrint { get; set; }
        public Nullable<System.DateTime> LastPrintTime { get; set; }
        public string UserName { get; set; }
        public Nullable<System.DateTime> TimeStr { get; set; }
        public string FuJianList { get; set; }
        public string ShenPiYiJian { get; set; }
        public Nullable<int> JieDianID { get; set; }
        public string JieDianName { get; set; }
        public string OKUserList { get; set; }
        public string ShenPiUserList { get; set; }
        public string StateNow { get; set; }
        public Nullable<System.DateTime> LateTime { get; set; }
        public string Comment { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedTime { get; set; }
        public Nullable<bool> IsRejected { get; set; }
        public Nullable<bool> IsLocked { get; set; }
        public Nullable<bool> IsTemporary { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string ModuleName { get; set; }
        public IList<TravelFundsDetailTransferObject> TravelFundsList { get; set; }
    }
}
