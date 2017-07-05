using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScientificResearch.DataTransferModel
{
   public class ScienceResearchOfCapitalStatisticTransferObject
    {
       /// <summary>
       /// 模块
       /// </summary>
       public string Modeule { get; set; }
       /// <summary>
       /// 项目名称
       /// </summary>
       public string ProjectName { get; set; }
       /// <summary>
       /// 负责人
       /// </summary>
       public string PrincipalMan { get; set; }
       /// <summary>
       /// 拨款时间，及项目确立时间
       /// </summary>
       public DateTime AllocationOfTime { get; set; }
       /// <summary>
       /// 拨款金额
       /// </summary>
       public double AppropriationMoney { get; set; }
       /// <summary>
       /// 报销金额
       /// </summary>
       public double ReimbursementAmount { get; set; }
    }
}
