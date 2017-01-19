using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Auctions.WebUI.Startup))]
namespace Auctions.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
