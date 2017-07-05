using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PF.DomainModel.Identity
{
   public static class IdentityConfigure
    {

                  //配置验证逻辑和密码选项 
       public static PasswordValidator PasswordValidator = new PasswordValidator
           {
               RequiredLength = 1,
               RequireNonLetterOrDigit = false,//要求特殊字符
               RequireDigit = false,//要求数字
               RequireLowercase = false,//要求小写
               RequireUppercase = false,//要求大写
           };
           // Configure user lockout defaults
       public static bool UserLockoutEnabledByDefault = true;
       public static TimeSpan DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
       public static int MaxFailedAccessAttemptsBeforeLockout = 5;

       public static EmailService EmailService = new EmailService();
       public static SmsService SmsService = new SmsService();

       public static PhoneNumberTokenProvider<ApplicationUser> TwoFactorPhoneCode = new PhoneNumberTokenProvider<ApplicationUser>
               {
                   MessageFormat = "Your security code is: {0}"
               };
       public static EmailTokenProvider<ApplicationUser> TwoFactorSecurityCode = new EmailTokenProvider<ApplicationUser>
                {
                    Subject = "SecurityCode",
                    BodyFormat = "Your security code is {0}"
                };
      

        
    }
}
