using System;

namespace ScientificResearch.DataTransferModel
{
    public class ERPBuMenTransferObject
    {
        public int ID { get; set; }
        public string BuMenName { get; set; }
        public string ChargeMan { get; set; }
        public string TelStr { get; set; }
        public string ChuanZhen { get; set; }
        public string BackInfo { get; set; }
        public Nullable<int> DirID { get; set; }
    }
}
