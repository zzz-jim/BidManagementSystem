using System;
using System.ComponentModel.DataAnnotations;

namespace ScientificResearch.ViewModel
{
   public class ERPNWorkFlowNodeViewModel
    {
        public int ID { get; set; }
        public Nullable<int> WorkFlowID { get; set; }
       [Display(Name="当前节点：")]
        public string NodeSerils { get; set; }
       [Display(Name="节点名称：")]
        public string NodeName { get; set; }
       [Display(Name="节点位置：")]
        public string NodeAddr { get; set; }
       [Display(Name="下一节点：")]
        public string NextNode { get; set; }
        public string IFCanJump { get; set; }
        public string IFCanView { get; set; }
        public string IFCanEdit { get; set; }
        public string IFCanDel { get; set; }
        public Nullable<int> JieShuHours { get; set; }
        public string PSType { get; set; }
        public string SPType { get; set; }
       [Display(Name="审批列表：")]
        public string SPDefaultList { get; set; }
        public string CanWriteSet { get; set; }
        public string SecretSet { get; set; }
        public string ConditionSet { get; set; }
        public string NodeLeft { get; set; }
        public string NodeTop { get; set; }
    }
}
