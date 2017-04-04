using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RecipeConverterApp.Startup))]
namespace RecipeConverterApp
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
