using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PnIotPoc.Device.SimulatorCore.Devices;
using PnIotPoc.Device.SimulatorCore.Devices.Factory;
using PnIotPoc.Device.SimulatorCore.Logging;
using PnIotPoc.Device.SimulatorCore.Telemetry.Factory;
using PnIotPoc.Device.SimulatorCore.Transport.Factory;
using PnIotPoc.WebApi.Common.Configurations;
using PnIotPoc.WebApi.Common.Models;

namespace PnIotPoc.Device.Cooler.Devices.Factory
{
    public class CoolerDeviceFactory : IDeviceFactory
    {
        public IDevice CreateDevice(ILogger logger, ITransportFactory transportFactory,
            ITelemetryFactory telemetryFactory, IConfigurationProvider configurationProvider, InitialDeviceConfig config)
        {
            var device = new CoolerDevice(logger, transportFactory, telemetryFactory, configurationProvider);
            device.Init(config);
            return device;
        }
    }
}
