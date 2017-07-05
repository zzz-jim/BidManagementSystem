using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PF.Common.Enums.HB.Public
{
        //支持绑定的可作为身份标识的卡（号）的列表 
       public enum SupportCard
       {
           院内就诊卡=0,
           身份证号=1,
           居民健康卡=2,
           病案号=3,
           医保卡=4,
           银行卡=5,
           微信号=6,
           QQ号=7,
           支付宝账号=8,
           财富通帐号=9,
           百度钱包=10,
           手机号=11,

       }
       //已绑定的卡/号的状态
       public enum UserCardState
       {
           有效=0,
           注销=1,
           挂失=2
       }
   
}
