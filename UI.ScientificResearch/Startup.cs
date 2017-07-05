using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UI.ScientificResearch.Startup))]
namespace UI.ScientificResearch
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
