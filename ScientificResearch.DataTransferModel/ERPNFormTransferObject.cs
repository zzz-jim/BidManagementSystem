using System;

namespace ScientificResearch.DataTransferModel
{
    public class ERPNFormTransferObject
    {
        public int ID { get; set; }
        public string FormName { get; set; }
        public Nullable<int> TypeID { get; set; }
        public string UserListOK { get; set; }
        public string DepListOK { get; set; }
        public string JiaoSeListOK { get; set; }
        public string PaiXuStr { get; set; }
        public string UserName { get; set; }
        public Nullable<DateTime> TimeStr { get; set; }
        public string ContentStr { get; set; }
        public string ItemsList { get; set; }
        public string IFOK { get; set; }
        public Nullable<DateTime> LastModifiedTime { get; set; }
        public string ModifiedPerson { get; set; }
        public string ApprovalFlowDescription { get; set; }
        public string FormType { get; set; }
    }
}
