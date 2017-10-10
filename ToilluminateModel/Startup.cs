using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ToilluminateModel.Startup))]
namespace ToilluminateModel
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
