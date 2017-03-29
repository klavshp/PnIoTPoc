using PnIotPoc.Device.SimulatorCore.Devices;
using PnIotPoc.Device.SimulatorCore.Devices.Factory;
using PnIotPoc.Device.SimulatorCore.Logging;
using PnIotPoc.Device.SimulatorCore.Telemetry.Factory;
using PnIotPoc.Device.SimulatorCore.Transport.Factory;
using PnIotPoc.WebApi.Common.Configurations;
using PnIotPoc.WebApi.Common.Models;

namespace PnIotPoc.Device.Cooler.Devices.Factory
{
    public class RfidReaderDeviceFactory : IDeviceFactory
    {
        public IDevice CreateDevice(ILogger logger, ITransportFactory transportFactory, ITelemetryFactory telemetryFactory, IConfigurationProvider configurationProvider, InitialDeviceConfig config)
        {
            var device = new RfidReaderDevice(logger, transportFactory, telemetryFactory, configurationProvider);
            device.Init(config, "rfid");
            return device;
        }
    }
}
