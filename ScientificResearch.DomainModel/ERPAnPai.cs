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
    
    public partial class ERPAnPai
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string TitleStr { get; set; }
        public string ContentStr { get; set; }
        public Nullable<System.DateTime> TimeStart { get; set; }
        public Nullable<System.DateTime> TimeEnd { get; set; }
        public Nullable<System.DateTime> TimeTiXing { get; set; }
        public string TypeStr { get; set; }
        public Nullable<System.DateTime> TimeStr { get; set; }
        public string GongXiangWho { get; set; }
    }
}
