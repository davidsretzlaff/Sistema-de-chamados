using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ChamadosPro.Startup))]
namespace ChamadosPro
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
