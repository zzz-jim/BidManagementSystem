using System;

namespace ScientificResearch.ViewModel
{
   public class ERPNWorkFlowViewModel
    {
        public int ID { get; set; }
        public string WorkFlowName { get; set; }
        public Nullable<int> FormID { get; set; }
        public string UserListOK { get; set; }
        public string DepListOK { get; set; }
        public string JiaoSeListOK { get; set; }
        public string PaiXuStr { get; set; }
        public string UserName { get; set; }
        public Nullable<System.DateTime> TimeStr { get; set; }
        public string BackInfo { get; set; }
        public string IFOK { get; set; }
    }
}
