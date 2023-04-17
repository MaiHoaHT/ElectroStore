using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ElectroWeb.Startup))]
namespace ElectroWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
