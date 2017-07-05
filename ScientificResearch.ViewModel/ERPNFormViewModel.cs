using System;
using System.ComponentModel.DataAnnotations;

namespace ScientificResearch.ViewModel
{
    public class ERPNFormViewModel
    {
        
        public int ID { get; set; }
        [Display(Name="审批名称")]
        public string FormName { get; set; }
        public Nullable<int> TypeID { get; set; }
        public string UserListOK { get; set; }
        public string DepListOK { get; set; }
        public string JiaoSeListOK { get; set; }
        public string PaiXuStr { get; set; }
        public string UserName { get; set; }
        public Nullable<System.DateTime> TimeStr { get; set; }
        public string ContentStr { get; set; }
        public string ItemsList { get; set; }
        public string IFOK { get; set; }
        [Display(Name="最后修改时间")]
        public Nullable<System.DateTime> LastModifiedTime { get; set; }
        [Display(Name="修改人")]
        public string ModifiedPerson { get; set; }
        [Display(Name="表单描述")]
        public string ApprovalFlowDescription { get; set; }
        [Display(Name="表单类型")]
        public string FormType { get; set; }
    }
}
