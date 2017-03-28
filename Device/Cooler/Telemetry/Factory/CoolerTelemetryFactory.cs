using PnIotPoc.Device.SimulatorCore.Devices;
using PnIotPoc.Device.SimulatorCore.Logging;
using PnIotPoc.Device.SimulatorCore.Telemetry;
using PnIotPoc.Device.SimulatorCore.Telemetry.Factory;

namespace PnIotPoc.Device.Cooler.Telemetry.Factory
{
    public class CoolerTelemetryFactory : ITelemetryFactory
    {
        private readonly ILogger _logger;


        public CoolerTelemetryFactory(ILogger logger)
        {
            _logger = logger;
        }

        public object PopulateDeviceWithTelemetryEvents(IDevice device)
        {
            var startupTelemetry = new StartupTelemetry(_logger, device);
            device.TelemetryEvents.Add(startupTelemetry);

            var monitorTelemetry = new RemoteMonitorTelemetry(_logger, device.DeviceID);
            device.TelemetryEvents.Add(monitorTelemetry);

            return monitorTelemetry;
        }
    }
}
