using System.Web.Http;
using Owin;
using PnIotPoc.WebApi.Common.Configurations;

namespace PnIotPoc.WebApi
{
    public partial class Startup
    {
        public static HttpConfiguration HttpConfiguration { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration = new HttpConfiguration();
            var configProvider = new ConfigurationProvider();

            ConfigureAuth(app, configProvider);

            ConfigureAutofac(app);

            // WebAPI call must come after Autofac
            // Autofac hooks into the HttpConfiguration settings
            ConfigureWebApi(app);

            ConfigureJson(app);
        }
    }
}