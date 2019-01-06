using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FitFlexApparel.Startup))]
namespace FitFlexApparel
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
