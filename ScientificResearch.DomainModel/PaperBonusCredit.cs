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
    
    public partial class PaperBonusCredit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> BonusCredit { get; set; }
        public string Type { get; set; }
        public int TypeId { get; set; }
        public string Level { get; set; }
        public int LevelId { get; set; }
        public Nullable<int> AuthorLevelId { get; set; }
        public string AuthorLevel { get; set; }
        public string CreditType { get; set; }
        public Nullable<decimal> Author1 { get; set; }
        public Nullable<decimal> Author2 { get; set; }
        public Nullable<decimal> Author3 { get; set; }
        public Nullable<decimal> Author4 { get; set; }
        public Nullable<decimal> Author5 { get; set; }
        public Nullable<decimal> Author6 { get; set; }
        public Nullable<decimal> Author7 { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
    }
}