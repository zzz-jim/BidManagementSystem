using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using PF.DomainModel.Identity;

namespace PF.DomainModel.Identity
{
  
        // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

        public class ApplicationUserManager : UserManager<ApplicationUser>
        {
            public ApplicationUserManager(IUserStore<ApplicationUser> store)
                : base(store)
            {
            }

            public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
                IOwinContext context)
            {
                var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
                // Configure validation logic for usernames
                manager.UserValidator = new UserValidator<ApplicationUser>(manager)
                {
                    AllowOnlyAlphanumericUserNames = false,
                    RequireUniqueEmail = true
                };
                //配置验证逻辑和密码选项 
                manager.PasswordValidator = IdentityConfigure.PasswordValidator;
                // Configure user lockout defaults
                manager.UserLockoutEnabledByDefault = IdentityConfigure.UserLockoutEnabledByDefault;
                manager.DefaultAccountLockoutTimeSpan = IdentityConfigure.DefaultAccountLockoutTimeSpan;
                manager.MaxFailedAccessAttemptsBeforeLockout = IdentityConfigure.MaxFailedAccessAttemptsBeforeLockout;
                // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
                // You can write your own provider and plug in here.
                manager.RegisterTwoFactorProvider("PhoneCode", IdentityConfigure.TwoFactorPhoneCode);
                manager.RegisterTwoFactorProvider("EmailCode", IdentityConfigure.TwoFactorSecurityCode);
                manager.EmailService = IdentityConfigure.EmailService;
                manager.SmsService = IdentityConfigure.SmsService;
                var dataProtectionProvider = options.DataProtectionProvider;
                if (dataProtectionProvider != null)
                {
                    manager.UserTokenProvider =
                        new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
                }
                return manager;
            }
        }

        // Configure the RoleManager used in the application. RoleManager is defined in the ASP.NET Identity core assembly
        public class ApplicationRoleManager : RoleManager<IdentityRole>
        {
            public ApplicationRoleManager(IRoleStore<IdentityRole, string> roleStore)
                : base(roleStore)
            {
            }

            public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
            {
                return new ApplicationRoleManager(new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>()));
            }
        }

        public class EmailService : IIdentityMessageService
        {
            public Task SendAsync(IdentityMessage message)
            {
                // Plug in your email service here to send an email.
                return Task.FromResult(0);
            }
        }

        public class SmsService : IIdentityMessageService
        {
            public Task SendAsync(IdentityMessage message)
            {
                // Plug in your sms service here to send a text message.
                return Task.FromResult(0);
            }
        }


        public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
        {
            public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager) :
                base(userManager, authenticationManager) { }

            public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
            {
                return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
            }

            public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
            {
                return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
            }
        }
    }
