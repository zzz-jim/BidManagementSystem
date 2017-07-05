using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PF.ViewModle.Patient
{
    //病人基础信息
    //    名称	说明	数据类型	类属性
    //病人ID	病人在平台系统中的唯一标识，用于调用其它相关接口的输入参数	String	PatientId
    //病人姓名		String	Name
    //性别	GB/T 2261.1-2003	Int	Gender
    //出生日期		DateTime	BirthDate
    [DataContract]
   public class PatinetInfoDVM
    {
        [DataMember]
       public string PatientId { get; set; }
        [DataMember]
       public string Name { get; set; }
        [DataMember]
       public DateTime BirthDate { get; set; }
    }


}
