using PF.Common.Enums.HB.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UI.ScientificResearch.Areas.ApplicationIdentity.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        //[Required]
        //[Display(Name = "Email")]
        //[EmailAddress]
        //public string Email { get; set; }

        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "下次记住我?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        //[Display(Name = "用户头像")]
        //public string AccountPicture { get; set; }
         [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "{0}的长度至少需要 {2} 位.", MinimumLength = 1)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认")]
        [Compare("Password", ErrorMessage = "两次密码输入不相同.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "姓名")]
        public string Name { get; set; }
        [Display(Name = "工号")]
        public string WorkerId { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "从业资格")]
        public string Qualification { get; set; }
        [Display(Name = "学历")]
        public string Degree { get; set; }
        [Display(Name = "所学专业")]
        public string Special { get; set; }
        [Display(Name = "职称等级")]
        public string TechnicalTitle { get; set; }
        [Display(Name = "职务")]
        public string Duty { get; set; }
        [Display(Name = "类别")]
        //[Range(0, 6)]
       // [EnumDataType(typeof(PF.Common.Enums.HB.Identity.UserCategory))]
        public UserCategory Category { get; set; }
    
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}