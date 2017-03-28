using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Owin;
using PnIotPoc.WebApi.Common.Configurations;
using PnIotPoc.WebApi.Common.Helpers;
using PnIotPoc.WebApi.Common.Repository;
using PnIotPoc.WebApi.Infrastructure.BusinessLogic;
using PnIotPoc.WebApi.Infrastructure.Repository;
using PnIotPoc.WebApi;

namespace PnIotPoc.WebApi
{
    public partial class Startup
    {
        public void ConfigureAutofac(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            var containerBuilder = new ContainerBuilder();

            // Enable Http routing by attributes
            config.MapHttpAttributeRoutes();

            // register the class that sets up bindings between interfaces and implementation
            containerBuilder.RegisterModule(new WebAutofacModule());

            // register configuration provider
            containerBuilder.RegisterType<ConfigurationProvider>().As<IConfigurationProvider>();

            // register Autofac w/ the WebApi application
            containerBuilder.RegisterControllers(typeof(WebApiApplication).Assembly);

            // Register the WebAPI controllers.
            containerBuilder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            var container = containerBuilder.Build();

            // Setup Autofac dependency resolver for WebAPI
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // Configure Web API with the dependency resolver.
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // 1.  Register the Autofac middleware 
            // 2.  Register Autofac Web API middleware,
            // 3.  Register the standard Web API middleware (this call is made in the Startup.WebApi.cs - Note: No, doesn't work!)
            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);
            app.UseWebApi(config);
        }
    }

    public sealed class WebAutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //Logic
            builder.RegisterType<KeyLogic>().As<IKeyLogic>();
            builder.RegisterType<DeviceLogic>().As<IDeviceLogic>();
            builder.RegisterType<DeviceRulesLogic>().As<IDeviceRulesLogic>();
            builder.RegisterType<DeviceTypeLogic>().As<IDeviceTypeLogic>();
            builder.RegisterType<SecurityKeyGenerator>().As<ISecurityKeyGenerator>();
            builder.RegisterType<ActionMappingLogic>().As<IActionMappingLogic>();
            builder.RegisterType<ActionLogic>().As<IActionLogic>();
            builder.RegisterInstance(CommandParameterTypeLogic.Instance).As<ICommandParameterTypeLogic>();
            builder.RegisterType<DeviceTelemetryLogic>().As<IDeviceTelemetryLogic>();
            builder.RegisterType<AlertsLogic>().As<IAlertsLogic>();

            //Repositories
            builder.RegisterType<IotHubRepository>().As<IIotHubRepository>();
            builder.RegisterType<IoTHubDeviceManager>().As<IIoTHubDeviceManager>();
            builder.RegisterType<DeviceRegistryRepository>().As<IDeviceRegistryListRepository>();
            builder.RegisterType<DeviceRegistryRepository>().As<IDeviceRegistryCrudRepository>();
            builder.RegisterType<DeviceRulesRepository>().As<IDeviceRulesRepository>();
            builder.RegisterType<SampleDeviceTypeRepository>().As<IDeviceTypeRepository>();
            builder.RegisterType<VirtualDeviceTableStorage>().As<IVirtualDeviceStorage>();
            builder.RegisterType<ActionMappingRepository>().As<IActionMappingRepository>();
            builder.RegisterType<ActionRepository>().As<IActionRepository>();
            builder.RegisterType<DeviceTelemetryRepository>().As<IDeviceTelemetryRepository>();
            builder.RegisterType<AlertsRepository>().As<IAlertsRepository>();

            //builder.RegisterType<UserSettingsRepository>().As<IUserSettingsRepository>();
            //builder.RegisterType<ApiRegistrationRepository>().As<IApiRegistrationRepository>();
            //builder.RegisterType<JasperCredentialsProvider>().As<ICredentialProvider>();
            //builder.RegisterType<JasperCellularService>().As<IExternalCellularService>();
            //builder.RegisterType<CellularExtensions>().As<ICellularExtensions>();

            builder.RegisterType<AzureTableStorageClientFactory>().As<IAzureTableStorageClientFactory>();
            builder.RegisterType<BlobStorageClientFactory>().As<IBlobStorageClientFactory>();
            builder.RegisterGeneric(typeof(DocumentDBClient<>)).As(typeof(IDocumentDBClient<>));
        }
    }
}