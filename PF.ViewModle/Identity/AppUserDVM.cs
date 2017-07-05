using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PF.ViewModle.Identity
{
    [DataContract]
    public class AppUserDVM
    {
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Name { get; set; }
          [DataMember]
        public string WorkId { get; set; }
          [DataMember]
          public string Email { get; set; }
         [DataMember]
        public string PinYin { get; set; }
          [DataMember]
        public string Qualification { get; set; }
       [DataMember]
        public string Degree { get; set; }
          [DataMember]
        public string Special { get; set; }
          [DataMember]
        public string TechnicalTitle { get; set; }
           [DataMember]
        public string Duty { get; set; }

          [DataMember]
        public DateTime LastUpdateDate { get; set; }

          //[DataMember]
          //public string[] SectionIds { get; set; }
          [DataMember]
          public string SectionId { get; set; }
    }
}
