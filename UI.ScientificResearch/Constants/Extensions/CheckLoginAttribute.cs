using PF.DomainModel.Identity;
using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq;
using System.Security.Principal;


namespace UI.ScientificResearch.Extensions
{
    /// <summary>
    /// Check用户是否登录
    /// </summary>
    public sealed class CheckLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session != null)
            {
                if (filterContext.HttpContext.Session.IsNewSession)
                {
                    var sessionCookie = filterContext.HttpContext.Request.Headers["Cookie"];
                    if ((sessionCookie != null) && (sessionCookie.IndexOf("ASP.NET_SessionId", StringComparison.OrdinalIgnoreCase) >= 0))
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Account", Action = "Login", Area = "ApplicationIdentity", }));
                    }
                }
            }
        }
    }
    //public sealed class NotCommonUserAttribute : ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        //判断登录者是否有除普通用户的权限，还有其他更高级的权限
    //        string allUserRoles = filterContext.HttpContext.Session[SessionKeyEnum.UserRoles.ToString()].ToString();
    //        string[] roles = allUserRoles.Split(',');
    //        if (roles.Length > 2 || allUserRoles.Contains("超级管理员"))
    //        {

    //        }
    //        else
    //        {
    //            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Account", Action = "Login", Area = "ApplicationIdentity", }));
    //        }
    //    }
    //}
}