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
    public class RemoteMonitorTelemetry : ITelemetry
    {
        private readonly ILogger _logger;
        private readonly string _deviceId;

        private const int ReportFrequencyInSeconds = 5;
        private const int PeakFrequencyInSeconds = 90;

        private readonly SampleDataGenerator _temperatureGenerator;
        private readonly SampleDataGenerator _humidityGenerator;
        private readonly SampleDataGenerator _externalTemperatureGenerator;

        public bool ActivateExternalTemperature { get; set; }

        public bool TelemetryActive { get; set; }

        public RemoteMonitorTelemetry(ILogger logger, string deviceId)
        {
            _logger = logger;
            _deviceId = deviceId;

            ActivateExternalTemperature = false;
            TelemetryActive = true;

            int peakFrequencyInTicks = Convert.ToInt32(Math.Ceiling((double)PeakFrequencyInSeconds / ReportFrequencyInSeconds));

            _temperatureGenerator = new SampleDataGenerator(33, 36, 42, peakFrequencyInTicks);
            _humidityGenerator = new SampleDataGenerator(20, 50);
            _externalTemperatureGenerator = new SampleDataGenerator(-20, 120);
        }

        public async Task SendEventsAsync(CancellationToken token, Func<object, Task> sendMessageAsync)
        {
            var monitorData = new RemoteMonitorTelemetryData();
            while (!token.IsCancellationRequested)
            {
                if (TelemetryActive)
                {
                    monitorData.DeviceId = _deviceId;
                    monitorData.RfidTag = _externalTemperatureGenerator.GetNextValue();

                    await sendMessageAsync(monitorData);
                }
                await Task.Delay(TimeSpan.FromSeconds(ReportFrequencyInSeconds), token);
            }
        }

        public void ChangeSetPointTemperature(double newSetPointTemperature)
        {
            _temperatureGenerator.ShiftSubsequentData(newSetPointTemperature);
        }
    }
}
