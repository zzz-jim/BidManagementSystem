//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ScientificResearch.DomainModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class ERPReport
    {
        public int ID { get; set; }
        public string ReportName { get; set; }
        public string ReportSql { get; set; }
        public string BackInfo { get; set; }
        public Nullable<int> TypeID { get; set; }
        public string UserListOK { get; set; }
        public string DepListOK { get; set; }
        public string JiaoSeListOK { get; set; }
        public string PaiXuStr { get; set; }
        public string UserName { get; set; }
        public Nullable<System.DateTime> TimeStr { get; set; }
    }
}
