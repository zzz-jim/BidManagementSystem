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
    
    public partial class ContinuingEducationRecord
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public Nullable<int> NworkToDoId { get; set; }
        public string UserName { get; set; }
        public string Department { get; set; }
        public Nullable<double> AccountCredit { get; set; }
        public double Credit { get; set; }
        public string CreditLevel { get; set; }
        public string CreditType { get; set; }
        public bool IsProjectCredit { get; set; }
        public bool IsGranted { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedTime { get; set; }
        public string UserDegree { get; set; }
        public string UserDuty { get; set; }
    }
}
