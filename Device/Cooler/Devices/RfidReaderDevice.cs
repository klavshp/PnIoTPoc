using PnIotPoc.Device.Cooler.Telemetry;
using PnIotPoc.Device.SimulatorCore.Devices;
using PnIotPoc.Device.SimulatorCore.Logging;
using PnIotPoc.Device.SimulatorCore.Telemetry.Factory;
using PnIotPoc.Device.SimulatorCore.Transport.Factory;
using PnIotPoc.WebApi.Common.Configurations;

namespace PnIotPoc.Device.Cooler.Devices
{
    public class RfidReaderDevice : DeviceBase
    {
        public RfidReaderDevice(ILogger logger, ITransportFactory transportFactory, ITelemetryFactory telemetryFactory, IConfigurationProvider configurationProvider)
            : base(logger, transportFactory, telemetryFactory, configurationProvider) {}

        public void StartTelemetryData()
        {
            var remoteMonitorTelemetry = (RfidReaderTelemetry)_telemetryController;
            remoteMonitorTelemetry.TelemetryActive = true;
            Logger.LogInfo("Device {0}: Telemetry has started", DeviceID);
        }

        public void StopTelemetryData()
        {
            var remoteMonitorTelemetry = (RfidReaderTelemetry)_telemetryController;
            remoteMonitorTelemetry.TelemetryActive = false;
            Logger.LogInfo("Device {0}: Telemetry has stopped", DeviceID);
        }
    }
}
