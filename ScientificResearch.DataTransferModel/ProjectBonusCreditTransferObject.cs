using System;

namespace ScientificResearch.DataTransferModel
{
  public  class ProjectBonusCreditTransferObject
    {
        public int Id { get; set; }
        public string ModuleName { get; set; }
        public string ProjectType { get; set; }
        public string ProjectLevel { get; set; }
        public double Credit { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
    }
}
