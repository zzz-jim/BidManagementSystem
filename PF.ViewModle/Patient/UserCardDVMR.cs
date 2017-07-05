using PF.ViewModle.Public;
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
   public class UserCardDVMR
    {
        [DataMember]
        public ResultMsgDVM ResultMsg { get; set; }
        [DataMember]
        public List<UserCardDVM> UserCards { get; set; }

        public Guid patientId;

        public UserCardDVMR()
        {
            this.ResultMsg = new ViewModle.Public.ResultMsgDVM()
            {
                IsSucceed = false,
                ErrorMsg = "传入的参数都不能为空"
            };
        }
        public UserCardDVMR(string sysId, string userId, string patientId)
        {
            this.ResultMsg = new ViewModle.Public.ResultMsgDVM()
                 {
                     IsSucceed = true,
                     ErrorMsg = string.Empty
                 };
           
           
            if (!Guid.TryParse(patientId.Trim(), out this.patientId))
            {
                this.ResultMsg.IsSucceed = false;
                this.ResultMsg.ErrorMsg = "传入的患者ID不合法";
            }
            
          
        }
    }
}
