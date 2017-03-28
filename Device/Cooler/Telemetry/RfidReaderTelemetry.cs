using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using PnIotPoc.Device.Cooler.Telemetry.Data;
using PnIotPoc.Device.SimulatorCore.Logging;
using PnIotPoc.Device.SimulatorCore.Telemetry;
using PnIotPoc.WebApi.Common.SampleDataGenerator;

namespace PnIotPoc.Device.Cooler.Telemetry
{
    public class RfidReaderTelemetry : ITelemetry
    {
        private readonly ILogger _logger;
        private readonly string _deviceId;

        private const int ReportFrequencyInSeconds = 5;

        private readonly SampleDataGenerator _rfidTagGenerator;

        public bool TelemetryActive { get; set; }

        public RfidReaderTelemetry(ILogger logger, string deviceId)
        {
            _logger = logger;
            _deviceId = deviceId;

            _rfidTagGenerator = new SampleDataGenerator(20, 50);
        }

        public async Task SendEventsAsync(CancellationToken token, Func<object, Task> sendMessageAsync)
        {
            var monitorData = new RfidReaderTelemetryData();
            while (!token.IsCancellationRequested)
            {
                if (TelemetryActive)
                {
                    monitorData.DeviceId = _deviceId;
                    monitorData.RfidTag = _rfidTagGenerator.GetNextValue().ToString(CultureInfo.InvariantCulture);
                    monitorData.DateTime = DateTime.Now;

                    var messageBody = "DeviceId: " + monitorData.DeviceId + " DateTime: " + monitorData.DateTime + " RfidTag:" + monitorData.RfidTag;

                    _logger.LogInfo("Sending " + messageBody + " for Device: " + _deviceId);

                    await sendMessageAsync(monitorData);
                }
                await Task.Delay(TimeSpan.FromSeconds(ReportFrequencyInSeconds), token);
            }
        }
    }
}
