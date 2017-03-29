using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using PnIotPoc.WebApi.Common.Exceptions;
using PnIotPoc.WebApi.Common.Helpers;
using PnIotPoc.WebApi.Common.Models;
using PnIotPoc.WebApi.Common.Models.Commands;
using PnIotPoc.WebApi.Infrastructure.Models;

namespace PnIotPoc.WebApi.Common.Factory
{
    public static class SampleDeviceFactory
    {
        public const string ObjectTypeDeviceInfo = "DeviceInfo";
        public const string Version10 = "1.0";
        private const int MaxCommandsSupported = 6;
        private const bool IsSimulatedDevice = true;
        private static readonly Random Rand = new Random();
        private static readonly List<string> DefaultDeviceNames = new List<string>{
            "SampleDevice001",
            "SampleDevice002",
            "SampleDevice003",
            "SampleDevice004"
        };

        private class Location
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }

            public Location(double latitude, double longitude)
            {
                Latitude = latitude;
                Longitude = longitude;
            }
        }

        private static readonly List<Location> PossibleDeviceLocations = new List<Location>
        {
            new Location(55.7359677, 12.388040600000068),           // Lautruphøj 10
            new Location(55.73669599999999, 12.388249999999971),    // Lautruphøj 12
            new Location(55.738291, 12.39358059999995),             // Lautrupparken 42
            new Location(55.7352032, 12.390869399999929),           // Lautrupparken 46
            new Location(55.735744, 12.382654000000002)             // Borupgaard gymnasium
        };

        public static DeviceModel GetSampleSimulatedDevice(string deviceId, string key)
        {
            var device = DeviceCreatorHelper.BuildDeviceStructure(deviceId, true, null);

            AssignDeviceProperties(device);
            device.ObjectType = ObjectTypeDeviceInfo;
            device.Version = Version10;
            device.IsSimulatedDevice = IsSimulatedDevice;

            AssignTelemetry(device);
            AssignCommands(device);

            return device;
        }

        public static DeviceModel GetSampleDevice(Random randomNumber, SecurityKeys keys)
        {
            var deviceId = string.Format(
                    CultureInfo.InvariantCulture,
                    "00000-DEV-{0}C-{1}LK-{2}D-{3}",
                    MaxCommandsSupported,
                    randomNumber.Next(99999),
                    randomNumber.Next(99999),
                    randomNumber.Next(99999));

            var device = DeviceCreatorHelper.BuildDeviceStructure(deviceId, false, null);
            device.ObjectName = "IoT Device Description";

            AssignDeviceProperties(device);
            AssignTelemetry(device);
            AssignCommands(device);

            return device;
        }

        private static void AssignDeviceProperties(DeviceModel device)
        {
            int randomId = Rand.Next(0, PossibleDeviceLocations.Count - 1);
            if (device?.DeviceProperties == null)
            {
                throw new DeviceRequiredPropertyNotFoundException("Required DeviceProperties not found");
            }

            device.DeviceProperties.HubEnabledState = true;
            device.DeviceProperties.Manufacturer = "CGI Danmark";
            device.DeviceProperties.ModelNumber = "MD-" + randomId;
            device.DeviceProperties.SerialNumber = "SER" + randomId;
            device.DeviceProperties.FirmwareVersion = "1." + randomId;
            device.DeviceProperties.Platform = "Plat-" + randomId;
            device.DeviceProperties.Processor = "i3-" + randomId;
            device.DeviceProperties.InstalledRAM = randomId + " MB";

            // Choose a location among the 16 above and set Lat and Long for device properties
            device.DeviceProperties.Latitude = PossibleDeviceLocations[randomId].Latitude;
            device.DeviceProperties.Longitude = PossibleDeviceLocations[randomId].Longitude;
        }

        private static void AssignTelemetry(DeviceModel device)
        {
            device.Telemetry.Add(new Telemetry("RfidTag", "RfidTag", "double"));
        }

        private static void AssignCommands(DeviceModel device)
        {
            device.Commands.Add(new Command("PingDevice"));
            device.Commands.Add(new Command("StartTelemetry"));
            device.Commands.Add(new Command("StopTelemetry"));
            device.Commands.Add(new Command("ChangeSetPointTemp", new[] { new Parameter("SetPointTemp", "double") }));
            device.Commands.Add(new Command("DiagnosticTelemetry", new[] { new Parameter("Active", "boolean") }));
            device.Commands.Add(new Command("ChangeDeviceState", new[] { new Parameter("DeviceState", "string") }));
        }

        public static List<string> GetDefaultDeviceNames()
        {
            long milliTime = DateTime.Now.Millisecond;
            return DefaultDeviceNames.Select(r => string.Concat(r, "_" + milliTime)).ToList();
        }
    }
}