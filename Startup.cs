using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(McKaren.Startup))]
namespace McKaren
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
