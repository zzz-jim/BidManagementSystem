//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ScientificResearch.DomainModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class ERPFileList
    {
        public int ID { get; set; }
        public string FileName { get; set; }
        public string BianHao { get; set; }
        public string BackInfo { get; set; }
        public Nullable<int> DaXiao { get; set; }
        public string FileType { get; set; }
        public Nullable<int> DirID { get; set; }
        public Nullable<System.DateTime> ShangChuanTime { get; set; }
        public string FilePath { get; set; }
        public string UserName { get; set; }
        public string IFDel { get; set; }
        public string TypeName { get; set; }
        public string IfShare { get; set; }
        public Nullable<int> DirOrFile { get; set; }
        public string CanAdd { get; set; }
        public string CanMod { get; set; }
        public string CanDel { get; set; }
        public string CanView { get; set; }
        public Nullable<System.DateTime> DaoQiShijian { get; set; }
        public byte ShiFouTiXing { get; set; }
    }
}
