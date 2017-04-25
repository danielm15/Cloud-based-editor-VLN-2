using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Cloud_based_editor_VLN_2.Startup))]
namespace Cloud_based_editor_VLN_2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
