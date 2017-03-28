using PnIotPoc.Device.SimulatorCore.Telemetry.Factory;
using PnIotPoc.Device.SimulatorCore.Transport.Factory;
using PnIotPoc.WebApi.Common.Configurations;
using PnIotPoc.WebApi.Common.Models;

namespace PnIotPoc.Device.SimulatorCore.Devices.Factory
{
    public interface IDeviceFactory
    {
        IDevice CreateDevice(Logging.ILogger logger, ITransportFactory transportFactory,
            ITelemetryFactory telemetryFactory, IConfigurationProvider configurationProvider, InitialDeviceConfig config);
    }
}