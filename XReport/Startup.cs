using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(XReport.Startup))]
namespace XReport
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
