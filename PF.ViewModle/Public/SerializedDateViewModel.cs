using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PF.ViewModle.Public
{
    [DataContract]
   public class ResultMsgDVM
    {
        [DataMember]
        public bool IsSucceed { get; set; }
        [DataMember]
        public string ErrorMsg { get; set; }
    }
}
