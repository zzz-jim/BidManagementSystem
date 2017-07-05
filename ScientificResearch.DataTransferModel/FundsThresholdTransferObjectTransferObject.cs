using System;

namespace ScientificResearch.DataTransferModel
{
    public class FundsThresholdTransferObjectTransferObject
    {
        public int Id { get; set; }
        public string ModuleName { get; set; }
        public string ProjectType { get; set; }
        public string FundsType { get; set; }
        public double Threshold { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedTime { get; set; }

        public int NWorkToDoID { get; set; }
    }
}
