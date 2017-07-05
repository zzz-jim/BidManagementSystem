using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScientificResearch.DataTransferModel
{
    public class LatedApprovalTransferObject
    {
        public int ApplicationId { get; set; }
        public string ApplicationManName { get; set; }
        public string Department { get; set; }
        public string TimeDifference { get; set; }
        public string ApprovalManName { get; set; }
    }
}
