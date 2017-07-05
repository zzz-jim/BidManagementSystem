using System;
using System.Collections.Generic;

namespace ScientificResearch.DataTransferModel
{
    public class ERPNWorkToDoFundsTransferObject
    {
        public ERPNWorkToDoFundsTransferObject()
        {
            NWorkToDoFundsList = new List<FundsThresholdTransferObjectTransferObject>();
        }

        public int ID { get; set; }
        public string WorkName { get; set; }

        /// <summary>
        /// 文号
        /// </summary>
        public string WenHao { get; set; }
        public Nullable<int> FormID { get; set; }
        public Nullable<int> WorkFlowID { get; set; }
        public string UserName { get; set; }
        public Nullable<DateTime> TimeStr { get; set; }
        public string FormContent { get; set; }
        public string FuJianList { get; set; }
        public string ShenPiYiJian { get; set; }
        public Nullable<int> JieDianID { get; set; }
        public string JieDianName { get; set; }
        public string ShenPiUserList { get; set; }
        public string OKUserList { get; set; }
        public string StateNow { get; set; }
        public Nullable<DateTime> LateTime { get; set; }
        public string BeiYong1 { get; set; }
        public string BeiYong2 { get; set; }
        public string ApplicationStatus { get; set; }
        public int ApplicationId { get; set; }
        public string FormKeys { get; set; }
        public string FormValues { get; set; }
        public Nullable<bool> IsRejected { get; set; }
        public Nullable<bool> IsLocked { get; set; }
        public Nullable<bool> IsTemporary { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string ProjectStatus { get; set; }
       // public Nullable<DateTime> ProjectEstablishTime { get; set; }

        public IList<FundsThresholdTransferObjectTransferObject> NWorkToDoFundsList { get; set; }
    }
}
