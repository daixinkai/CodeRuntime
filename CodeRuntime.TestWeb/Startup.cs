using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CodeRuntime.TestWeb.Startup))]
namespace CodeRuntime.TestWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
