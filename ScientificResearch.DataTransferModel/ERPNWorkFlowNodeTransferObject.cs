using System;

namespace ScientificResearch.DataTransferModel
{
    public class ERPNWorkFlowNodeTransferObject
    {
        public int ID { get; set; }
        public Nullable<int> WorkFlowID { get; set; }
        public string NodeSerils { get; set; }
        public string NodeName { get; set; }
        public string NodeAddr { get; set; }
        public string NextNode { get; set; }
        public string IFCanJump { get; set; }
        public string IFCanView { get; set; }
        public string IFCanEdit { get; set; }
        public string IFCanDel { get; set; }
        public Nullable<int> JieShuHours { get; set; }
        public string PSType { get; set; }
        public string SPType { get; set; }
        public string SPDefaultList { get; set; }
        public string CanWriteSet { get; set; }
        public string SecretSet { get; set; }
        public string ConditionSet { get; set; }
        public string NodeLeft { get; set; }
        public string NodeTop { get; set; }
    }
}
