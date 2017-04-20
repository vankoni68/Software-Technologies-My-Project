using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Software_Technologies_Project.Startup))]
namespace Software_Technologies_Project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
