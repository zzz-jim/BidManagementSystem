using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScientificResearch.DataTransferModel
{
    public class FundsToDoTransferObject
    {
        public FundsToDoTransferObject()
        {
            FundsList = new List<FundsRecordTransferObject>();
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
        public IList<FundsRecordTransferObject> FundsList { get; set; }

    }
}
