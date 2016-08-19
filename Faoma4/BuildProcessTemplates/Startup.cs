using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Faoma4.Startup))]
namespace Faoma4
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
