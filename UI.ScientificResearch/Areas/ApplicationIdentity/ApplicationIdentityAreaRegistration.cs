using System.Web.Mvc;

namespace UI.ScientificResearch.Areas.ApplicationIdentity
{
    public class ApplicationIdentityAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ApplicationIdentity";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ApplicationIdentity_default",
                "ApplicationIdentity/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}