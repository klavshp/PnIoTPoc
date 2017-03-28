using System;
using System.Threading.Tasks;
using PnIotPoc.Device.SimulatorCore.Devices;
using PnIotPoc.Device.SimulatorCore.Logging;
using PnIotPoc.Device.SimulatorCore.Telemetry;

namespace PnIotPoc.Device.Cooler.Telemetry
{
    public class StartupTelemetry : ITelemetry
    {
        private readonly ILogger _logger;
        private readonly IDevice _device;

        public StartupTelemetry(ILogger logger, IDevice device)
        {
            _logger = logger;
            _device = device;
        }

        public async Task SendEventsAsync(System.Threading.CancellationToken token, Func<object, Task> sendMessageAsync)
        {
            if (!token.IsCancellationRequested)
            {
                _logger.LogInfo("Sending initial data for device {0}", _device.DeviceID);
                await sendMessageAsync(_device.GetDeviceInfo());
            }
        }
    }
}
