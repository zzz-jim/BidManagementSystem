using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace PF.ViewModle.Patient
{
    //用户绑定的可作为身份标识的卡（帐号）
//    名称	说明	数据类型	类属性
//卡号		String	CardNum
//卡类型	见平台支持的卡列表	int	CardType
//卡状态	0 有效1 注销2 挂失	Int	CardStatus
    //绑定日期		DateTime	BindDate
//备注		String	Note
    [DataContract]
   public class UserCardDVM
    {
     [DataMember]
       public string CardNum { get; set; }
        [DataMember]
       public PF.Common.Enums.HB.Public.SupportCard CardType { get; set; }
        [DataMember]
       public PF.Common.Enums.HB.Public.UserCardState CardStatus { get; set; }
        [DataMember]
       public DateTime BindDate { get; set; }
      // public string Note { get; set; }
    }
}
