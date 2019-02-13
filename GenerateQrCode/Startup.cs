using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GenerateQrCode.Startup))]
namespace GenerateQrCode
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
