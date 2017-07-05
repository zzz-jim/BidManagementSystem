using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScientificResearch.ViewModel
{
   public class FundsThresholdViewModel
    {
        public int Id { get; set; }
       [Display(Name = "项目类型")]
        public string ModuleName { get; set; }

       [Display(Name = "级别类型")]
        public string ProjectType { get; set; }

       [Display(Name = "经费类型")]
        public string FundsType { get; set; }

       [Display(Name = "限制金额")]
        public double Threshold { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedTime { get; set; }

        public int NWorkToDoID { get; set; }
    }
}
