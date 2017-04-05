using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using PnIotPoc.Device.Cooler.Devices.Factory;
using PnIotPoc.Device.Cooler.Telemetry.Factory;
using PnIotPoc.Device.DataInitialization;
using PnIotPoc.Device.SimulatorCore.Logging;
using PnIotPoc.Device.SimulatorCore.Repository;
using PnIotPoc.Device.SimulatorCore.Transport.Factory;
using PnIotPoc.WebApi.Common.Configurations;
using PnIotPoc.WebApi.Common.Helpers;
using PnIotPoc.WebApi.Common.Repository;

namespace PnIotPoc.Device
{
    internal class Program
    {
        static readonly CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();
        static IContainer _simulatorContainer;
        private static Timer _timer;

        static void Main(string[] args)
        {
            Console.WriteLine("Starting device simulation...");

            BuildContainer();

            StartDataInitializationAsNeeded();

            StartSimulator();

            RunAsync().Wait();


            Console.ReadLine();
        }

        private static void BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new SimulatorModule());
            _simulatorContainer = builder.Build();
        }

        private static void CreateInitialDataAsNeeded(object state)
        {
            if (!CancellationTokenSource.Token.IsCancellationRequested)
            {
                Trace.TraceInformation("Preparing to add initial data");
                var creator = _simulatorContainer.Resolve<IDataInitializer>();
                creator.CreateInitialDataIfNeeded();
            }
        }

        private static void StartDataInitializationAsNeeded()
        {
            //We have observed that Azure reliably starts the web job twice on a fresh deploy. The second start
            //is reliably about 7 seconds after the first start (under current conditions -- this is admittedly
            //not a perfect solution, but absent visibility into the black box of Azure this is what works at
            //the time) with a shutdown command being received on the current instance in the interim. We want
            //to further bolster our guard against starting a data initialization process that may be aborted
            //in the middle of its work. So we want to delay the data initialization for about 10 seconds to
            //give ourselves the best chance of receiving the shutdown command if it is going to come in. After
            //this delay there is an extremely good chance that we are on a stable start that will remain in place.
            _timer = new Timer(CreateInitialDataAsNeeded, null, 10000, Timeout.Infinite);
        }

        private static void StartSimulator()
        {
            // Dependencies to inject into the Bulk Device Tester
            var logger = new TraceLogger();
            var configProvider = new ConfigurationProvider();
            var tableStorageClientFactory = new AzureTableStorageClientFactory();

            var telemetryFactory = new CoolerTelemetryFactory(logger);
            var deviceFactory = new CoolerDeviceFactory();
            var transportFactory = new IotHubTransportFactory(logger, configProvider);

            IVirtualDeviceStorage deviceStorage;
            var useConfigforDeviceList = Convert.ToBoolean(configProvider.GetConfigurationSettingValueOrDefault("UseConfigForDeviceList", "False"), CultureInfo.InvariantCulture);

            if (useConfigforDeviceList)
            {
                deviceStorage = new AppConfigRepository(configProvider, logger);
            }
            else
            {
                deviceStorage = new VirtualDeviceTableStorage(configProvider, tableStorageClientFactory);
            }

            // Start Simulator
            Trace.TraceInformation("Starting Simulator");
            var tester = new BulkDeviceTester(transportFactory, logger, configProvider, telemetryFactory, deviceFactory, deviceStorage);
            Task.Run(() => tester.ProcessDevicesAsync(CancellationTokenSource.Token), CancellationTokenSource.Token);
        }

        private static async Task RunAsync()
        {
            while (CancellationTokenSource.Token.IsCancellationRequested)
            {
                Trace.TraceInformation("Running");
                try
                {
                    await Task.Delay(TimeSpan.FromMinutes(5), CancellationTokenSource.Token);
                }
                catch (TaskCanceledException) { }
            }
        }
    }
}
