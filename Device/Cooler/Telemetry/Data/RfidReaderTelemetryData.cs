using System;

namespace PnIotPoc.Device.Cooler.Telemetry.Data
{
    public class RfidReaderTelemetryData
    {
        public string DeviceId { get; set; }
        public string RfidTag { get; set; }
        public DateTime DateTime { get; set; }
    }
}
