using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace UI.ScientificResearch
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static Dictionary<string, string> EmailServerConfig = new Dictionary<string, string>
        {
            { "sina.cn","smtp.sina.com.cn"},
            { "sina.com","smtp.sina.com"},
            { "sinaVIP","smtp.vip.sina.com"},
            { "sohu.com","smtp.sohu.com"},
            { "126.com","smtp.126.com"},
            { "163.com","smtp.163.com"},
            { "qq.com","smtp.qq.com"},
            { "139.com","smtp.139.com"},
        };

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
