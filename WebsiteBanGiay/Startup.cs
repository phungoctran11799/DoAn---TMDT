using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebsiteBanGiay.Startup))]
namespace WebsiteBanGiay
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
