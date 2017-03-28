﻿using System;
using System.Globalization;
using PnIotPoc.Device.SimulatorCore.Devices;
using PnIotPoc.Device.SimulatorCore.Logging;

namespace PnIotPoc.Device.SimulatorCore.Telemetry.Factory
{
    public class GenericConcreteTelemetryFactory
    {
        private readonly ILogger _logger;

        public GenericConcreteTelemetryFactory(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Populates devices with sample static events (events that fire the same data each time)
        /// Events are "injected" into a device, so a wide variety of scenarios can be supported
        /// with each device having its own set of events to send to IoT Hub
        /// </summary>
        /// <param name="device">The device to add the events to</param>
        public void PopulateDeviceWithTelemetryEvents(IDevice device)
        {
            var eg1 = new ConcreteTelemetry(_logger)
            {
                DelayBefore = new TimeSpan(0, 0, 0, 0, 1000),
                MessageBody = string.Format(CultureInfo.CurrentCulture, "Device {0} - event A!", device.DeviceID),
                RepeatCount = 5
            };

            device.TelemetryEvents.Add(eg1);

            var eg2 = new ConcreteTelemetry(_logger)
            {
                DelayBefore = new TimeSpan(0, 0, 0, 0, 1000),
                MessageBody = string.Format(CultureInfo.CurrentCulture, "Device {0} - event B!", device.DeviceID),
                RepeatCount = 5
            };

            device.TelemetryEvents.Add(eg2);
        }
    }
}
